using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : PlayerMovementBase
{
    [SerializeField]
    private Vector3 velocity;

    public override void OnAwake()
    {
        playerInput.Player.Jump.performed += playerJump.Jump;
    }
    private void OnDestroy()
    {
        playerInput.Player.Jump.performed -= playerJump.Jump;
        playerInput.Player.Disable();
    }

    private void FixedUpdate()
    {
        playerWalk.Walk(playerInput.Player.Move.ReadValue<Vector2>());
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.linearVelocity;

    }

    public void test(InputAction.CallbackContext context)
    {
        Debug.Log("test");
    }
}
