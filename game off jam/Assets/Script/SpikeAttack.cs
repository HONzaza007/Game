using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float knockbackForce = 20f;
    public float duration;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(CloseTimer());
    }

    public void OnHit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponentExtend<Rigidbody>();
            IDamageAble iDamageAble = other.gameObject.GetComponentExtend<IDamageAble>();

            rb.AddForce(transform.up * knockbackForce, ForceMode.VelocityChange);

            iDamageAble.GotHit(gameObject, damage);
        }
    }


    IEnumerator CloseTimer()
    {
        yield return new WaitForSeconds(duration);
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
