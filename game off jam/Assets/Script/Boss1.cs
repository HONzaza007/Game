using System.Collections;
using UnityEditor;
using UnityEngine;

public class Boss1 : MonoBehaviour , IDamageAble
{
    private EnemyManager enemyManager;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject missilePrefab;
    [SerializeField]
    private GameObject floorAttackPrefab;
    [SerializeField]
    private GameObject enerySlashPrefab;
    [SerializeField]
    private GameObject bossOutSide;
    

    [SerializeField]
    private float missileAttackDelay;
    [SerializeField]
    private float missileAttackDuration;
    [SerializeField]
    private float floorAttackDuration;
    [SerializeField]
    private bool canFloorAttack = true;
    [SerializeField]
    private float attackDelay;
    private float attackTimer;
    private bool canShootMissile = true;
    private GameObject missile;
    private int previousAttack = -1;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyManager.isActive) 
        {
            enemyManager.hp = enemyManager.maxHp;

            return;
        }

        bossOutSide.transform.LookAt(enemyManager.player.transform.position);
        AttackStateHanderler();

        if (enemyManager.hp <= 0 && !enemyManager.isAlreadyDeath)
        {
            enemyManager.isAlreadyDeath = true;
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Player player = enemyManager.player.GetComponent<Player>();
        player.hp = player.maxHp;
        MusicManager.PlayMusic(MusicManager.MusicName.general);
        AudioManager.PlaySound(AudioManager.SoundName.ExplodeBig, transform.position);
        GameManager.bossDeadCount++;
        enemyManager.Death(gameObject);
        enemyManager.Death(bossOutSide);
    }

    private void AttackStateHanderler()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;

            int randomNum = Random.Range(0, 4);

            while (randomNum == previousAttack || (randomNum == 3 && !canFloorAttack))
            {
                randomNum = Random.Range(0, 4);
            }
            previousAttack = randomNum;


            if (randomNum == 0)
            {
                StartMissileAttack();
            }
            if (randomNum == 1)
            {
                animator.SetTrigger("horizontalAttack");
            }
            if (randomNum == 2)
            {
                animator.SetTrigger("verticleAttack");
            }
            if (randomNum == 3)
            {
                canFloorAttack = false;
                StartFloorAttack();
            }
        }

        attackTimer += Time.deltaTime;
    }

    private void StartMissileAttack()
    {
        animator.SetBool("open",true);

        StartCoroutine(MissileAttackTimer());

        IEnumerator MissileAttackTimer()
        {
            float timeElapse = 0f;

            while(timeElapse < missileAttackDuration)
            {
                MissileAttack();
                timeElapse += Time.deltaTime;

                yield return null;
            }
            animator.SetBool("open", false);
        }
    }

    private void StartFloorAttack()
    {
        AudioManager.PlaySound(AudioManager.SoundName.EnergySlash, transform.position);

        animator.SetBool("open", true);

        StartCoroutine(MissileAttackTimer());
        
        IEnumerator MissileAttackTimer()
        {
            yield return new WaitForSeconds(1);

            GameObject floorAttack = Instantiate(floorAttackPrefab);
            floorAttack.transform.position = new Vector3(transform.position.x, -5, transform.position.z);

            yield return new WaitForSeconds( -1f);

            animator.SetBool("open", false);

            yield return new WaitForSeconds(floorAttackDuration);

            canFloorAttack = true;
        }
    }

    private void MissileAttack()
    {
        if (!canShootMissile) return;

        AudioManager.PlaySound(AudioManager.SoundName.Missile, transform.position);

        float randomNumberX = Random.Range(-0.25f, 0.25f);
        float randomNumberZ = Random.Range(-0.25f, 0.25f);

        Vector3 launceDir = new Vector3(randomNumberX, 1f, randomNumberZ).normalized;

        missile = Instantiate(missilePrefab);
        missile.transform.LookAt(launceDir);
        missile.transform.position = transform.position;

        StartCoroutine(MissileCoolDown());

        IEnumerator MissileCoolDown()
        {
            canShootMissile = false;
            yield return new WaitForSeconds(missileAttackDelay);
            canShootMissile = true;
        }
    }

    public void VerticleAttack()
    {
        AudioManager.PlaySound(AudioManager.SoundName.EnergySlash, transform.position);

        GameObject enerySlash = Instantiate(enerySlashPrefab);
        enerySlash.transform.position = gameObject.transform.position;
        enerySlash.transform.LookAt(enemyManager.player.transform.position);

        enerySlash.transform.Rotate(0, 0, 90);

    }
    public void HorizontalAttack()
    {
        AudioManager.PlaySound(AudioManager.SoundName.EnergySlash, transform.position);

        GameObject enerySlash = Instantiate(enerySlashPrefab);
        enerySlash.transform.position = gameObject.transform.position;
        enerySlash.transform.LookAt(enemyManager.player.transform.position);
    }

    void IDamageAble.GotHit(UnityEngine.GameObject _gameObject, int Damage)
    {
        enemyManager.hp -= Damage;
        AudioManager.PlaySound(AudioManager.SoundName.ImpackMetal, transform.position);
        HitFlash.Flash(gameObject);
        HitFlash.Flash(bossOutSide);
    }

    
}
