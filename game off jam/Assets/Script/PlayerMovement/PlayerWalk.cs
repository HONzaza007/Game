using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerMovementBase
{
    public float baseMoveSpeed;
    public float currentMoveSpeed;

    public float maxMoveSpeed;

    public Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        currentMoveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        if(!playerGroundCheck.IsGround())
        {
            currentMoveSpeed = baseMoveSpeed * 0.5f;
        }else
        {
            currentMoveSpeed = baseMoveSpeed;
        }
    }

    public void Walk(Vector2 inputDir)
    {
        moveDir = transform.TransformDirection(new Vector3(inputDir.x, 0, inputDir.y));
        rb.AddForceToMax(moveDir, currentMoveSpeed, maxMoveSpeed);
    }
}
