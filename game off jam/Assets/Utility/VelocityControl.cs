using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VelocityControl
{
    public static void AddForceToMax(this Rigidbody rb, Vector3 forceDir, float speed, float maxSpeed)
    {
        forceDir = forceDir.normalized;

        float dirMagnitude = Vector3.Dot(rb.linearVelocity, forceDir);
        float speedDiff = maxSpeed - dirMagnitude;
        float forceToAdd;


        if (speed > 0)
        {
            if (speedDiff < 0) return;

            speed = Mathf.Clamp(speed, 0, maxSpeed);
            forceToAdd = Mathf.Clamp(speed, 0, speedDiff);
        }
        else
        {
            forceToAdd = Mathf.Clamp(speed, speedDiff, 0);
        }

        rb.linearVelocity += forceDir * forceToAdd;
    }
}
