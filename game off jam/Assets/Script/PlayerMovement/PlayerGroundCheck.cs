using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField]
    private BoxCollider groundCheckCollider;
    [SerializeField]
    private LayerMask groundLayer;

    public bool IsGround()
    {
        Collider[] overlap = Physics.OverlapBox(groundCheckCollider.bounds.center, groundCheckCollider.bounds.extents, Quaternion.identity, groundLayer);

        if(overlap.Length > 0 )
        {
            return true;
        }else
        {
            return false;
        }
    }
}
