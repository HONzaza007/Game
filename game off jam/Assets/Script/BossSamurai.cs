using System.Collections;
using UnityEngine;

public class BossSamurai : MonoBehaviour, IDamageAble
{

    [SerializeField]
    private GameObject slashProjectilePrefab;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject bigSlashPrefab;
    [SerializeField]
    private GameObject slashVfxPrefab;
    [SerializeField]
    private BoxCollider hitBoxCollider;
    [SerializeField]
    private LayerMask grapplingHeadLayer;
    [SerializeField]
    private Transform grapplingHeadPoint;
    private EnemyManager enemyManager;
    private Rigidbody rb;

    [SerializeField]
    private float stundDuration = 3f;
    [SerializeField]
    private float walkSpeed = 1f;
    [SerializeField]
    private float sideDashStrengt = 10f;
    [SerializeField]
    private float attackDelay;
    private float attackTimer;
    private int previousAttack = -1;

    [Header("projectile")]
    [SerializeField]
    private float slashProjectileDelay;
    [SerializeField]
    private int slashProjectileMaxShoot;
    [Header("shotgunProjectile")]
    [SerializeField]
    private float shotgunMaxShoot = 5;
    [SerializeField]
    private float shotgunAngle = 60f;
    [SerializeField]
    private bool isStund;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Vector3.zero, transform.localPosition) >= 35f)
        {
            transform.localPosition = Vector3.zero;
        }

        if (!enemyManager.isActive)
        {
            enemyManager.hp = enemyManager.maxHp;
            animator.SetBool("IsActive", false);
            return;
        }
        else
        {
            animator.SetBool("IsActive", true);
        }
        if (!isStund)
        {
            AttackStateHanderler();
            BossLook();
        }
        StundHandler();


        if (enemyManager.hp <= 0 && !enemyManager.isAlreadyDeath)
        {
            enemyManager.isAlreadyDeath = true;
            OnDeath();
        }
    }
    private void FixedUpdate()
    {

        if (enemyManager.isActive && !isStund && animator.GetCurrentAnimatorStateInfo(0).IsName("SamuraiWalk"))
        {
            Walk();
        }
    }

    private void OnDeath()
    {
        Player player = enemyManager.player.GetComponent<Player>();
        player.hp = player.maxHp;
        MusicManager.PlayMusic(MusicManager.MusicName.general);
        GameManager.bossDeadCount++;
        enemyManager.Death(gameObject);
    }
    private void AttackStateHanderler()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            int randomNum = Random.Range(0, 4);

            while (randomNum == previousAttack){
                randomNum = Random.Range(0, 4);
            }

            previousAttack = randomNum;

            if (randomNum == 0)
            {
                StartProjectileAttack();
            }
            if (randomNum == 1)
            {
                SideDash();
            }
            if (randomNum == 2)
            {
                StartBigSlash();
            }
            if (randomNum == 3)
            {
                StartShotGunProjectileAttack();
            }

        }

        attackTimer += Time.deltaTime;
    }

    private void Walk()
    {
        Vector3 walkDir = (enemyManager.player.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, enemyManager.player.transform.position);
        if (distance > 5)
        {
            rb.AddForceToMax(walkDir.normalized, walkSpeed, walkSpeed);
        }
    }


    private void SideDash()
    {
        int randomNum = Random.Range(0, 2);
        Vector3 forceDir;
        if (randomNum == 0)
        {
            forceDir = transform.right;
        }
        else
        {
            forceDir = -transform.right;
        }

        AudioManager.PlaySound(AudioManager.SoundName.SwooshSmall, transform.position);

        MeshTrail.ActivateTrail(gameObject, 0.25f, 0.025f, 2f);

        StartCoroutine(SideDashLerp(forceDir));

        attackTimer = attackDelay / 2;

        IEnumerator SideDashLerp(Vector3 dir)
        {
            Vector3 start = transform.position;
            Vector3 end = transform.position + (dir * sideDashStrengt);

            float timeElapse = 0f;
            float duration = 0.25f;

            while (timeElapse < duration)
            {
                timeElapse += Time.deltaTime;
                float t = timeElapse / duration;
                transform.position = Vector3.Lerp(start, end, t);

                yield return null;
            }

        }
    }

    private void StartBigSlash()
    {
        if (isStund) return;

        animator.SetTrigger("BigSlash");

        MeshTrail.ActivateTrail(gameObject, 0.5f, 0.025f, 2f);

        StartCoroutine(MoveTowardPlayer());

        IEnumerator MoveTowardPlayer()
        {
            Vector3 start = transform.position;

            float timeElapse = 0f;
            float duration = 0.5f;

            while (timeElapse < duration)
            {
                Vector3 moveToPosition = (transform.position - enemyManager.player.transform.position).normalized;
                moveToPosition = enemyManager.player.transform.position + (moveToPosition * 5f);
                moveToPosition = new Vector3(moveToPosition.x, transform.position.y, moveToPosition.z);

                Vector3 end = moveToPosition;

                timeElapse += Time.deltaTime;
                float t = timeElapse / duration;

                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
        }
    }
    public void OnBigSlash()
    {
        if (isStund) return;

        AudioManager.PlaySound(AudioManager.SoundName.Slash, transform.position);
        AudioManager.PlaySound(AudioManager.SoundName.SwooshBig, transform.position);

        Debug.Log("asd");

        GameObject bigSlash = Instantiate(bigSlashPrefab);
        bigSlash.transform.position = transform.position;
        bigSlash.transform.rotation = Quaternion.LookRotation(transform.forward);
    }
    private void StartProjectileAttack()
    {
        animator.SetBool("ProjectileSlash", true);

        StartCoroutine(ProjectileTimer());
    }
    IEnumerator ProjectileTimer()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < slashProjectileMaxShoot; i++)
        {
            Vector3 dir = (enemyManager.player.transform.position - transform.position).normalized;

            ShootSlashProjectile(dir, 150);
            yield return new WaitForSeconds(slashProjectileDelay);
        }
        animator.SetBool("ProjectileSlash", false);
    }

    private void StartShotGunProjectileAttack()
    {
        animator.SetBool("ProjectileSlash", true);

        StartCoroutine(ShotGunTimer());
    }
    IEnumerator ShotGunTimer()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < shotgunMaxShoot; i++)
        {
            float angle = ((shotgunAngle / (shotgunMaxShoot - 1)) * (i)) - (shotgunAngle / 2);
            Quaternion rotate = Quaternion.AngleAxis(angle, transform.up);
            Vector3 playerDir = (enemyManager.player.transform.position - transform.position).normalized;
            Vector3 dir = rotate * playerDir;

            ShootSlashProjectile(dir, 50f, 90f);

            Debug.Log(angle);
        }
        animator.SetBool("ProjectileSlash", false);
    }

    private void ShootSlashProjectile(Vector3 dir, float _speed = 100f, float rotationZ = default)
    {
        AudioManager.PlaySound(AudioManager.SoundName.Slash, transform.position);

        GameObject slashVfx = Instantiate(slashVfxPrefab);
        GameObject slash = Instantiate(slashProjectilePrefab);
        EnergySlash slashScript = slash.GetComponent<EnergySlash>();

        slashScript.speed = _speed;

        slash.transform.rotation = Quaternion.LookRotation(dir);

        if (rotationZ == default)
        {
            rotationZ = Random.Range(0, 360);
        }

        slash.transform.position = transform.position;
        slash.transform.localRotation = Quaternion.Euler(slash.transform.localRotation.eulerAngles.x, slash.transform.localRotation.eulerAngles.y, rotationZ);
        slashVfx.transform.position = slash.transform.position;
        slashVfx.transform.rotation = slash.transform.rotation;


    }
    private void BossLook()
    {
        Vector3 lookDir = (enemyManager.player.transform.position - transform.position).normalized;
        lookDir = new Vector3(lookDir.x, 0, lookDir.z);

        transform.rotation = Quaternion.LookRotation(lookDir);
    }
    void IDamageAble.GotHit(UnityEngine.GameObject targetGameObject, int Damage)
    {
        if (!isStund)
        {
            AudioManager.PlaySound(AudioManager.SoundName.SlashHit, transform.position);

            Vector3 lookDir = (enemyManager.player.transform.position - transform.position).normalized;
            float randomZ = Random.Range(0, 360);
            GameObject slashVfx = Instantiate(slashVfxPrefab);

            slashVfx.transform.rotation = Quaternion.LookRotation(lookDir);
            slashVfx.transform.localRotation = Quaternion.Euler(slashVfx.transform.localRotation.eulerAngles.x, slashVfx.transform.localRotation.eulerAngles.y, randomZ);
            slashVfx.transform.position = transform.position;
        }
        else
        {
            AudioManager.PlaySound(AudioManager.SoundName.Impack2, transform.position);
            enemyManager.hp -= Damage;
            HitFlash.Flash(gameObject);
        }
    }

    

    private void StundHandler()
    {
        if (isStund) return;

        Collider[] overlapCollider = Physics.OverlapBox(hitBoxCollider.bounds.center, hitBoxCollider.bounds.extents, hitBoxCollider.transform.rotation, grapplingHeadLayer);

        if (overlapCollider.Length > 0)
        {
            for (int i = 0; i < overlapCollider.Length; i++)
            {
                GrapplingHead grapplingHead = overlapCollider[i].gameObject.GetComponentExtend<GrapplingHead>();

                if (grapplingHead != null)
                {
                    StartStund(grapplingHead);
                }
            }
        }
    }

    private void StartStund(GrapplingHead grapplingHead)
    {
        AudioManager.PlaySound(AudioManager.SoundName.SlashHit, transform.position);

        isStund = true;
        grapplingHead.hitSamurai(grapplingHeadPoint.position);
        StopAllCoroutines();
        animator.SetBool("ProjectileSlash", false);
        StartCoroutine(stundTimer());

        IEnumerator stundTimer()
        {
            animator.SetBool("stund", true);

            GameObject grapplingHeadGameObj = grapplingHead.gameObject;
            float timeCount = 0f;

            while (timeCount < stundDuration)
            {
                timeCount += Time.deltaTime;

                if (grapplingHeadGameObj == null)
                {
                    stopStund(grapplingHeadGameObj);
                    yield break;
                }
                grapplingHead.hitSamurai(grapplingHeadPoint.position);
                yield return null;
            }
            stopStund(grapplingHeadGameObj);
        }
    }
    private void stopStund(GameObject grapplingHeadGameObj)
    {
        if (grapplingHeadGameObj != null)
        {
            Destroy(grapplingHeadGameObj);
        }
        isStund = false;
        animator.SetBool("stund", false);
        
    }
}
