using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    private float timer;

    void Start()
    {
        Destroy(gameObject, timer);
    }
}
