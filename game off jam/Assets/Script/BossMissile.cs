using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class BossMissile : MonoBehaviour, IDamageAble
{
    private GameObject player;

    [SerializeField]
    private GameObject Mesh;
    [SerializeField]
    private BoxCollider hitBoxCollider;
    [SerializeField]
    private BoxCollider explodeHitBoxCollider;
    [SerializeField]
    private VisualEffect effect;
    [SerializeField]
    private LayerMask layerHit;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float knockBackForce;

    private bool isAlreadyExplode;
    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        effect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardPlayer();
        MoveForward();
        CheckForHit();
    }

    private void TurnTowardPlayer()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.LookRotation(player.transform.position - transform.position), 
            turnSpeed * Time.deltaTime);
    }
    private void MoveForward()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }

    private void Explode()
    {
        if (isAlreadyExplode) return;

        AudioManager.PlaySound(AudioManager.SoundName.ExplodeSmall, transform.position);

        isAlreadyExplode = true;
        effect.Play();
        moveSpeed = 0f;
        turnSpeed = 0f;

        Collider[] overlapCollider = Physics.OverlapBox(explodeHitBoxCollider.bounds.center, explodeHitBoxCollider.bounds.extents, Quaternion.identity);

        for (int i = 0; i < overlapCollider.Length; i++)
        {
            IDamageAble iDamageAble = overlapCollider[i].gameObject.GetComponentExtend<IDamageAble>();
            Player playerClass = overlapCollider[i].gameObject.GetComponentExtend<Player>();

            if (iDamageAble != null)
            {
                iDamageAble.GotHit(gameObject, damage);
            }
            if(playerClass != null)
            {
                Vector3 knockBackDir = player.transform.position - transform.position;

                playerClass.KnockBack(knockBackForce, knockBackDir);
            }
        }

        Destroy(Mesh);
        Destroy(gameObject, 5f);
        Destroy(effect, 1f);
    }

    private void CheckForHit()
    {
        if (isAlreadyExplode) return;

        Collider[] overlapCollider = Physics.OverlapBox(hitBoxCollider.bounds.center, hitBoxCollider.bounds.extents, Quaternion.identity, layerHit);

        if (overlapCollider.Length > 0f)
        {
            Explode();
        }
    }

    void IDamageAble.GotHit(GameObject gameObject, int damage)
    {
        Explode();
    }
}
