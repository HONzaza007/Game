using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Unity.Mathematics;

public class AnimationManager : MonoBehaviour
{
    [System.Serializable]
    public class AnimationProperties
    {
        public AnimationClip animationClip;       // Animation clip
        public AnimationCurve timeStretchCurve;   // Time remapping curve
        public float speed = 1f;                  // Playback speed (default 1.0)

        private AnimationClip timeStretchedClip;  // Cached time-stretched clip

        // Create or retrieve the time-stretched version of the animation clip
        public AnimationClip GetTimeStretchedClip()
        {
            if (timeStretchedClip == null)
            {
                timeStretchedClip = Object.Instantiate(animationClip);
                timeStretchedClip.wrapMode = animationClip.wrapMode;
            }
            return timeStretchedClip;
        }

        // Remove events from the original animation clip
        public void RemoveRegularEvents()
        {
            animationClip.events = new AnimationEvent[0];
        }
    }


    public AnimationProperties[] animations; // Array of animation properties

    private PlayableGraph playableGraph;
    private AnimationMixerPlayable mixerPlayable;
    private Animator animator;

    private int currentClipIndex = -1;
    private float elapsedTime;

    void Start()
    {
        

        animator = GetComponent<Animator>();

        // Create the PlayableGraph
        playableGraph = PlayableGraph.Create("AnimationManagerGraph");

        // Create an output connected to the Animator
        var output = AnimationPlayableOutput.Create(playableGraph, "AnimationOutput", animator);

        // Create a mixer to manage multiple animations
        mixerPlayable = AnimationMixerPlayable.Create(playableGraph, animations.Length);
        output.SetSourcePlayable(mixerPlayable);

        // Add animation clips to the mixer
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].animationClip != null)
            {
                // Use a time-stretched version of the animation clip
                AnimationClip timeStretchedClip = animations[i].GetTimeStretchedClip();

                var clipPlayable = AnimationClipPlayable.Create(playableGraph, timeStretchedClip);
                playableGraph.Connect(clipPlayable, 0, mixerPlayable, i);
                mixerPlayable.SetInputWeight(i, 0); // Disable all initially

                // Remove events from the original clip to avoid duplicates
                animations[i].RemoveRegularEvents();
            }
        }

        // Start the graph
        playableGraph.Play();
    }

    void Update()
    {
        if (animations.Length == 0 || animator == null)
            return;

        // Get the current animation state
        int matchingClipIndex = GetMatchingClipIndex(animator.GetCurrentAnimatorStateInfo(0));

        if (matchingClipIndex != -1)
        {
            ApplyTimeStretch(matchingClipIndex);
        }
        else
        {
            ResetAnimationWeights();
        }
    }

    private int GetMatchingClipIndex(AnimatorStateInfo stateInfo)
    {
        // Match the animation state to its corresponding clip
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].animationClip != null &&
                stateInfo.IsName(animations[i].animationClip.name))
            {
                return i;
            }
        }
        return -1; // No match found
    }

    private void ApplyTimeStretch(int clipIndex)
    {
        var clipPlayable = (AnimationClipPlayable)mixerPlayable.GetInput(clipIndex);

        if (currentClipIndex != clipIndex)
        {
            clipPlayable.SetTime(0f);

            // Switch to the new clip
            elapsedTime = 0f;
            currentClipIndex = clipIndex;
        }

        elapsedTime += Time.deltaTime;

        // Calculate normalized time with speed adjustment
        AnimationProperties properties = animations[clipIndex];
        float normalizedTime = elapsedTime * properties.speed / properties.animationClip.length;

        if (properties.animationClip.isLooping)
        {
            if (normalizedTime > 1f) {
                ResetAnimationWeights();
                normalizedTime = 0; // Wrap around for looping
                clipPlayable.SetTime(0f);
            }
        }
        else
        {
            normalizedTime = Mathf.Clamp01(normalizedTime); // Clamp for non-looping
        }

        // Remap time using the curve
        float remappedTime = properties.timeStretchCurve != null
            ? properties.timeStretchCurve.Evaluate(normalizedTime)
            : normalizedTime;

        // Set the playable time
        clipPlayable.SetTime(remappedTime * properties.animationClip.length);

        // Update mixer weights
        SetMixerWeights(clipIndex);
    }

    private void ResetAnimationWeights()
    {
        currentClipIndex = -1;
        for (int i = 0; i < animations.Length; i++)
        {
            mixerPlayable.SetInputWeight(i, 0);
        }
    }

    private void SetMixerWeights(int activeIndex)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            mixerPlayable.SetInputWeight(i, i == activeIndex ? 1f : 0f);
        }
    }

    private void OnDestroy()
    {
        // Clean up the graph when done
        playableGraph.Destroy();
    }
}
