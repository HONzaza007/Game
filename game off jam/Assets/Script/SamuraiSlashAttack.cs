using UnityEngine;

public class SamuraiSlashAttack : MonoBehaviour
{
    bool isAlreadyHit = false;

    [SerializeField]
    private int damgage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAlreadyHit)
        {
            isAlreadyHit = true;
            IDamageAble iDamageAble = other.gameObject.GetComponentExtend<IDamageAble>();
            iDamageAble.GotHit(gameObject, damgage);
        }
    }
}
