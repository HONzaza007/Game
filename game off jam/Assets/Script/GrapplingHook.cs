using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    private PlayerInputAction inputAction;
    [SerializeField]
    private GameObject grapplingHeadPrefab;
    private GameObject grapplingHead;
    private GrapplingHead grapplingHeadScript;
    [SerializeField]
    private PlayerPhysics playerPhysics;

    [Header("Swinging")]
    private Vector3 swingPoint;
    private Vector3 directionToPoint;
    private float distanceFromPoint;
    private SpringJoint joint;

    [Header("OdmGear")]
    public Rigidbody rb;
    public float forwardThrustForce;

    [Header("Cooldown")]
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float samuraiCooldown;
    private float cooldownTimer;
    private bool canShoot = true;
    private bool alreadyCooldown;

    public bool isHitSamurai;



    private void Awake()
    {
        inputAction = new PlayerInputAction();
        inputAction.Player.Enable();

    }

    private void Update()
    {
        GrapplingHookPhysicsController();
        StartCooldown();
    }

    private void FixedUpdate()
    {
        if (joint != null) MoveTowardPoint();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    public void ShootGrapplingHook(InputAction.CallbackContext context = default)
    {
        if (grapplingHead != null || !canShoot) return;

        AudioManager.PlaySound(AudioManager.SoundName.GrapplingHook);

        canShoot = false;

        Debug.Log("shoot");

        Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, whatIsGrappleable);

        if(hit.point == Vector3.zero) return;

        lr.positionCount = 2;

        grapplingHead = Instantiate(grapplingHeadPrefab);

        grapplingHeadScript = grapplingHead.GetComponent<GrapplingHead>();
        grapplingHeadScript.grapplingHook = this;

        grapplingHead.transform.position = gunTip.transform.position;
        grapplingHead.transform.LookAt(hit.point);
    }

    public void StartSwing(Vector3 grappleHead = default)
    {
        if (grappleHead == null) return;

        swingPoint = grappleHead;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = 0f;

        // customize values as you like
        joint.spring = 3.5f;
        joint.damper = 7f;
        joint.massScale = 7.5f;

        directionToPoint = (swingPoint - player.transform.position).normalized;
        rb.AddForce(directionToPoint.normalized * forwardThrustForce * 1.5f, ForceMode.VelocityChange);

        currentGrapplePosition = gunTip.position;
    }

    public void StopSwing(InputAction.CallbackContext context = default)
    {
        lr.positionCount = 0;

        Destroy(joint);
        Destroy(grapplingHead);
    }

    private void MoveTowardPoint()
    {
        distanceFromPoint = Vector3.Distance(player.position, swingPoint);



        directionToPoint = (swingPoint - player.transform.position).normalized;
        Vector3 forceToAdd = directionToPoint.normalized * forwardThrustForce;
        rb.AddForce(forceToAdd, ForceMode.Acceleration);

        joint.maxDistance = distanceFromPoint * 0.8f;
    }

    private Vector3 currentGrapplePosition;

    private void DrawRope()
    {
        // if not grappling, don't draw rope
        if (grapplingHead == null) {
            lr.positionCount = 0;
            return;
        } 

        currentGrapplePosition = grapplingHeadScript.grapplingPoint.position;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void GrapplingHookPhysicsController()
    {
        if (joint != null)
        {
            playerPhysics.isUseGravity = false;
            playerPhysics.isUseDrag = false;
        }
        else
        {
            playerPhysics.isUseGravity = true;
            playerPhysics.isUseDrag = true;
        }
    }

    private void StartCooldown()
    {
        if (alreadyCooldown) return;

        if (grapplingHead == null && !canShoot && isHitSamurai)
        {
            StartCoroutine(CooldownTimer(samuraiCooldown));
        }else if (grapplingHead == null && !canShoot)
        {
            StartCoroutine(CooldownTimer(cooldown));
        }
        IEnumerator CooldownTimer(float _cooldown)
        {
            alreadyCooldown = true;
            yield return new WaitForSeconds(_cooldown);
            canShoot = true;
            alreadyCooldown = false;

        }
    }
}
