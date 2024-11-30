using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : PlayerMovementBase
{
    [SerializeField]
    private float jumpBuffer;
    [SerializeField]
    private float coyoteTime;

    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float maxAirJump;

    [SerializeField]
    private float jumpLeft;

    private bool isLeaveGround;
    private bool isLanding;

    private bool canJump;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LeaveGroundTrigger();
        LandingTrigger();
    }

    public void Jump(InputAction.CallbackContext context = default)
    {
        StartCoroutine(JumpBuffer());

        IEnumerator JumpBuffer()
        {
            float jumpBufferCounter;
            jumpBufferCounter = 0;

            while (jumpBufferCounter < jumpBuffer)
            {
                jumpBufferCounter += Time.deltaTime;

                if (canJump)
                {
                    GroundJump();

                    yield break;
                }
                else if (!canJump && jumpLeft > 0)
                {
                    AirJump();

                    yield break;
                }
                yield return null;
            }
        }
    }
    private void GroundJump()
    {
        canJump = false;

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }
        rb.linearVelocity += new Vector3(0, jumpHeight, 0);
    }

    private void AirJump()
    {
        jumpLeft--;

        if(rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        rb.linearVelocity += new Vector3(0, jumpHeight * 0.75f, 0);
    }

    private void LeaveGroundTrigger()
    {

        if (!isLeaveGround && !playerGroundCheck.IsGround())
        {
            isLeaveGround = true;
            StartCoroutine(CoyoteTime());
        }
        else if (playerGroundCheck.IsGround())
        {
            isLeaveGround = false;
        }

        IEnumerator CoyoteTime()
        {
            yield return new WaitForSeconds(coyoteTime);
            canJump = false;
        }
    }

    private void LandingTrigger()
    {
        if (!isLanding && playerGroundCheck.IsGround())
        {
            isLanding = true;
            canJump = true;

            jumpLeft = maxAirJump;
        }
        else if (playerGroundCheck.IsGround())
        {
            isLanding = false;
        }
    }
}