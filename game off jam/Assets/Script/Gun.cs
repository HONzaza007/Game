using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.VFX;

[RequireComponent (typeof(Animator))]
public class Gun : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private VisualEffect muzzleFlashVFXPrefab;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private TrailRenderer bulletTrail;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private LayerMask aimRaycastLayer;

    [Header("GunStat")]
    [SerializeField]
    private int gunDamage;
    [SerializeField]
    private float shootCoolDown = 0.5f;

    private bool canShoot = true;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(InputAction.CallbackContext context = default)
    {
        if (!canShoot || GameManager.isPause) return;

        AudioManager.PlaySound(AudioManager.SoundName.GunShoot);
        animator.SetTrigger("Shoot");
        MuzzleFlash();
        StartCoroutine(ShootCooldown());

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, float.MaxValue, aimRaycastLayer))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));

            IDamageAble iDamageAble = hit.collider.gameObject.GetComponentExtend<IDamageAble>();

            if (iDamageAble != null)
            {
                iDamageAble.GotHit(gameObject ,gunDamage);
            }
            
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;

        Vector3 startPosition = trail.transform.position;

        while (time < 1) 
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);

            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;

        Destroy(trail.gameObject, trail.time);
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCoolDown);
        canShoot = true;
    }

    private void MuzzleFlash()
    {
        VisualEffect muzzleFlashVFX = Instantiate(muzzleFlashVFXPrefab);

        muzzleFlashVFX.transform.position = bulletSpawnPoint.position;
        muzzleFlashVFX.transform.rotation = Quaternion.LookRotation(-gameObject.transform.forward);

        Destroy(muzzleFlashVFX.gameObject, 0.25f);
    }
}
