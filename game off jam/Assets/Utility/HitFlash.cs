using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    private Renderer[] meshRenderer;

    private List<Color[]> OGColor = new List<Color[]>();
    private List<Material[]> OGMaterial = new List<Material[]>();

    private void Awake()
    {
        meshRenderer = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in meshRenderer)
        {
            OGMaterial.Add(renderer.materials);
            List<Color> colorList = new List<Color>();

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                if (renderer.materials[i].HasProperty("_Color"))
                {
                    colorList.Add(renderer.materials[i].color);
                }
                else
                {
                    colorList.Add(Color.black);
                }
            }
            OGColor.Add(colorList.ToArray());
        }
    }

    public static void Flash(GameObject _gameObject, float flashDuration = 0.5f, Color flashColor = default)
    {
        HitFlash instance = _gameObject.GetComponent<HitFlash>();

        if (instance == null)
        {
            instance = _gameObject.AddComponent<HitFlash>();
        }

        if (flashColor == default)
        {
            flashColor = Color.white;
        }

        flashColor = flashColor * 2f;

        instance.StartCoroutine(FlashTimer());

        IEnumerator FlashTimer()
        {
            float duration = flashDuration;

            float timeElapse = 0;

            Color start = flashColor;


            while (timeElapse < duration)
            {
                timeElapse += Time.deltaTime;

                float t = timeElapse / duration;

                for (int i = 0;i < instance.meshRenderer.Length;i++)
                {
                    for (int j = 0; j < instance.meshRenderer[i].materials.Length; j++)
                    {
                        if (instance.meshRenderer[i].materials[j].HasProperty("_Color"))
                        {
                            

                            instance.meshRenderer[i].materials[j].color = Color.Lerp(start, instance.OGColor[i][j], t);
                        }
                    }
                }
                
                yield return null;
            }
            for (int i = 0; i < instance.meshRenderer.Length; i++)
            {
                instance.meshRenderer[i].materials = instance.OGMaterial[i];
            }

        }
    }
}
