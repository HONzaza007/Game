using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetect : MonoBehaviour
{
    // Collision Events
    [SerializeField] private CollisionEvent onCollisionEnter;
    [SerializeField] private CollisionEvent onCollisionStay;
    [SerializeField] private CollisionEvent onCollisionExit;

    // Trigger Events
    [SerializeField] private TriggerEvent onTriggerEnter;
    [SerializeField] private TriggerEvent onTriggerStay;
    [SerializeField] private TriggerEvent onTriggerExit;

    // Called when the collider enters a collision
    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke(collision);
    }

    // Called every frame while the collider is in collision
    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay.Invoke(collision);
    }

    // Called when the collider exits a collision
    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit.Invoke(collision);
    }

    // Called when the collider enters a trigger
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(other);
    }

    // Called every frame while the collider is within a trigger
    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke(other);
    }

    // Called when the collider exits a trigger
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(other);
    }

    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }

    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }
}
