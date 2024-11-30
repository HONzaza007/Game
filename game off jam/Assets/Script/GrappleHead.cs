using System.Collections;
using UnityEngine;

public class GrapplingHead : MonoBehaviour
{
    public GrapplingHook grapplingHook;

    public Transform grapplingPoint;

    private bool isAlreadyHit;

    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float destroyTime;



    private void Start()
    {
        StartCoroutine(DestroyTimer());
        grapplingHook.isHitSamurai = false;
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        CheckGrapplingHit();
    }

    private void Move()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void CheckGrapplingHit()
    {
        if (isAlreadyHit) return;

        Collider[] colliderOverlap;

        colliderOverlap = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents, Quaternion.identity,
            grapplingHook.whatIsGrappleable);

        if (colliderOverlap.Length > 0)
        {
            moveSpeed = 0f;
            grapplingHook.StartSwing(grapplingPoint.position);

            isAlreadyHit = true;
        }
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTime);
        if (!isAlreadyHit)
        {
            Destroy(gameObject);
        }
    }

    public void hitSamurai(Vector3 position)
    {
        isAlreadyHit = true;
        moveSpeed = 0f;
        gameObject.transform.position = position;
        grapplingHook.isHitSamurai = true;
    }
}
