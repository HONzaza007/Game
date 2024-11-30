using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour, IDamageAble
{
    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;
    public delegate void OnPlayerRespawn();
    public static OnPlayerRespawn onPlayerRespawn;

    public int maxHp;
    public int hp;

    [SerializeField]
    private float gotHitCooldown;
    [SerializeField]
    private float hpRegenCooldown;
    [SerializeField]
    private float hpRegenTimer;

    private Rigidbody rb;
    private bool canGotHit = true;


    private void Awake()
    {
        rb = gameObject.GetComponentExtend<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RegenHp();
        if (hp <= 0) PlayerDeath();
        
    }

    public void GotHit(GameObject gameObject, int damage)
    {
        if (!canGotHit) return;
        AudioManager.PlaySound(AudioManager.SoundName.Impack1);
        hp -= damage;
        StartCoroutine(GotHitCooldown());
        UIManager.Instance.PlayerGotHitScreen();
    }

    private IEnumerator GotHitCooldown()
    {
        canGotHit = false;
        yield return new WaitForSeconds(gotHitCooldown);
        canGotHit = true;
    }

    public void KnockBack(float force, Vector3 dir)
    {
        rb.AddForce(dir.normalized * force, ForceMode.VelocityChange);
    }

    private void RegenHp()
    {
        if (hp >= maxHp) return;

        hpRegenTimer += Time.deltaTime;

        if(hpRegenTimer >= hpRegenCooldown)
        {
            hpRegenTimer = 0f;
            hp++;
        }
    }

    private void PlayerDeath()
    {
        MusicManager.PlayMusic(MusicManager.MusicName.general);
        onPlayerDeath.Invoke();
    }
    public void Respawn()
    {
        hp = maxHp;
        transform.position = new Vector3(0, 0, 0);
        onPlayerRespawn.Invoke();
    }
}

public interface IDamageAble
{
    public void GotHit(GameObject targetGameObject = default, int Damage = 0)
    {

    }
}
