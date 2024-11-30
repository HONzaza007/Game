using System.Collections;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public bool isUseDrag = true;
    public bool isUseGravity = true;
    public bool isUseFastFall = true;

    [SerializeField]
    private AnimationCurve fastFallCurve;
    [SerializeField]
    private float fastFallDuration;
    private bool isAlreadyFastFall;

    [SerializeField]
    private Rigidbody rb;

    public float defaultDragScale { get; private set; }
    public float currentDragScale;
    public float defaultGravityScale { get; private set; }
    public float currentGravityScale;

    private void Awake()
    {
        rb = gameObject.GetComponentExtend<Rigidbody>();

        defaultDragScale = currentDragScale;
        defaultGravityScale = currentGravityScale;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Gravity();
        Drag();
        FastFall();
    }

    private void Gravity()
    {
        if (!isUseGravity) return;

        rb.AddForceToMax(-transform.up, currentGravityScale, currentGravityScale);
    }

    private void Drag()
    {
        if(!isUseDrag) return;

        Vector3 dragDir = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized;
        rb.AddForceToMax(dragDir, -currentDragScale, 0);
    }


    private void FastFall()
    {
        if(!isUseFastFall) return;

        if (rb.linearVelocity.y < 0 && isAlreadyFastFall == false)
        {
            StartFastFall();

        }else if(rb.linearVelocity.y >= 0)
        {
            StopFastFall();
        }

        void StartFastFall()
        {
            isAlreadyFastFall = true;
            StartCoroutine(FastFallCoroutine());
        }

        void StopFastFall()
        {
            isAlreadyFastFall = false;
            currentGravityScale = defaultGravityScale;
        }

        IEnumerator FastFallCoroutine()
        {
            float start = defaultGravityScale;
            float end = defaultGravityScale * 20;

            float timeElapse = 0;
            float duration = fastFallDuration;

            while(timeElapse < duration)
            {
                float t = timeElapse / duration;
                t = fastFallCurve.Evaluate(t);

                currentGravityScale = Mathf.Lerp(start, end, t);

                timeElapse += Time.deltaTime;

                if(rb.linearVelocity.y >= 0)
                {
                    StopFastFall();
                    yield break;
                }
                yield return null;
            }

            StopFastFall();

            currentGravityScale = start;
        }
    }
}
