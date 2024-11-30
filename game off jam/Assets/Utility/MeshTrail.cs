using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public Material material;
    public string shaderAlphaRef = "_Alpha";

    public SkinnedMeshRenderer[] skinnedMeshRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ActivateTrail(GameObject _gameObject, float _activeDuration = 2f, float _meshRefreshRate = 0.1f, float _meshDestroyDelay = 3f)
    {
        MeshTrail instance = _gameObject.GetComponent<MeshTrail>();
        if(instance == null)
        {
            instance = _gameObject.AddComponent<MeshTrail>();
        }
        instance.material = GameAssets.i.characterTrailMaterial;

        instance.StartCoroutine(ActivateTrailCoroutine(instance, _gameObject, _activeDuration, _meshRefreshRate, _meshDestroyDelay));
    }

    static IEnumerator ActivateTrailCoroutine(MeshTrail instance,GameObject _gameObject, float _activeDuration = 2f, float _meshRefreshRate = 0.1f, float _meshDestroyDelay = 3f)
    {
        while(_activeDuration > 0f)
        {
            _activeDuration -= _meshRefreshRate;

            if (instance.skinnedMeshRenderer == null)
            {
                instance.skinnedMeshRenderer = _gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for (int i = 0; i < instance.skinnedMeshRenderer.Length; i++)
            {
                GameObject gameObj = new GameObject();
                gameObj.transform.SetPositionAndRotation(instance.skinnedMeshRenderer[i].transform.position, instance.skinnedMeshRenderer[i].transform.rotation);

                MeshRenderer meshRenderer = gameObj.AddComponent<MeshRenderer>();
                MeshFilter meshFilter = gameObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                instance.skinnedMeshRenderer[i].BakeMesh(mesh);

                meshFilter.mesh = mesh;
                meshRenderer.material = instance.material;

                instance.StartCoroutine(AnimateMaterialAlpha(instance ,meshRenderer.material, _meshDestroyDelay));

                Destroy(gameObj, _meshDestroyDelay);
            }

            yield return new WaitForSeconds(_meshRefreshRate);
        }
    }

    static IEnumerator AnimateMaterialAlpha(MeshTrail instance, Material material, float duration)
    {
        float start = material.GetFloat(instance.shaderAlphaRef);
        float end = 0f;

        float timeElapse = 0f;

        while (timeElapse < duration) 
        {
            float t = timeElapse / duration;

            timeElapse += Time.deltaTime;

            material.SetFloat(instance.shaderAlphaRef, math.lerp(start, end, t));

            yield return null;
        }
    }

}
