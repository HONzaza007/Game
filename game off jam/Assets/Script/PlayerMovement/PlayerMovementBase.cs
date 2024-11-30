using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBase : MonoBehaviour
{
    public enum MovementState
    {
        Walk,
        WallRun
    }

    protected PlayerInputAction playerInput;

    protected Rigidbody rb;

    protected PlayerWalk playerWalk;
    protected PlayerJump playerJump;
    protected PlayerGroundCheck playerGroundCheck;
    protected PlayerMovementManager playerMovementManger; 

    void Awake()
    {
        playerInput = new PlayerInputAction();
        playerInput.Player.Enable();

        rb = gameObject.GetComponentExtend<Rigidbody>();

        playerWalk = GetComponent<PlayerWalk>();
        playerJump = GetComponent<PlayerJump>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerMovementManger = GetComponent<PlayerMovementManager>();

        OnAwake();
    }

    public virtual void OnAwake()
    {

    }
}

