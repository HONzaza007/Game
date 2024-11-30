using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    private PlayerInputAction inputAction;
    private Gun gun;
    private GrapplingHook grapplingHook;

    private void Awake()
    {
        inputAction = new PlayerInputAction();

        inputAction.Player.Enable();

        gun = GetComponent<Gun>();
        grapplingHook = GetComponent<GrapplingHook>();

        inputAction.Player.Shoot.performed += gun.Shoot;
        inputAction.Player.Grappling.performed += grapplingHook.ShootGrapplingHook;
        inputAction.Player.Grappling.canceled += grapplingHook.StopSwing;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AimAt()
    {
        
    }
}
