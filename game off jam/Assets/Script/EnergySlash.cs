using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class EnergySlash : MonoBehaviour
{
    [SerializeField]
    private GameObject mesh;
    [SerializeField]
    private VisualEffect effect;
    [SerializeField]
    private BoxCollider hitBoxCollider;
    [SerializeField]
    private LayerMask hitLayer;
    [SerializeField]
    private int damage;
    public float speed;

    [SerializeField]
    IDamageAble iDamageAble;

    private bool isAlreadyHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyCountDown());


    }

    // Update is called once per frame
    void Update()
    {
        CheckHit();
        MoveForward();
    }

    private void DestroySlash()
    {
        Destroy(mesh);
        Destroy(gameObject, 5f);
        effect.Stop();
        isAlreadyHit = true;
    }

    private void CheckHit()
    {
        if (isAlreadyHit) return;

        Collider[] overlapCollider = Physics.OverlapBox(hitBoxCollider.bounds.center,
            hitBoxCollider.bounds.extents, hitBoxCollider.transform.localRotation, hitLayer);
        if (overlapCollider.Length != 0)
        {
            for (int i = 0; i < overlapCollider.Length; i++)
            {
                IDamageAble iDamageAble = overlapCollider[i].gameObject.GetComponentExtend<IDamageAble>();

                if (iDamageAble != null)
                {
                    iDamageAble.GotHit(gameObject, damage);

                    DestroySlash();
                }
            }
        }
    }

    private void MoveForward() 
    {
        transform.position += transform.forward * speed * Time.deltaTime; 
    }

    IEnumerator DestroyCountDown()
    {
        yield return new WaitForSeconds(10f);
        DestroySlash();
    }
}
