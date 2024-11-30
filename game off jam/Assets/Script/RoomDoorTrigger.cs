using UnityEngine;

public class RoomDoorTrigger : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private EnemyManager enemyManager;


    [SerializeField]
    private BoxCollider triggerCollder;
    [SerializeField]
    private GameObject door;


    private void Awake()
    {
        Player.onPlayerDeath += OnDoorOpen;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnDoorClose();
        if (enemyManager.isAlreadyDeath)
        {
            OnDoorOpen();
        }
    }

    private void OnDoorClose()
    {
        Collider[] colliderOverlap = Physics.OverlapBox(triggerCollder.bounds.center, triggerCollder.bounds.extents, Quaternion.identity, playerLayer);

        if (!enemyManager.isAlreadyDeath)
        {
            if (colliderOverlap.Length != 0)
            {
                MusicManager.PlayMusic(MusicManager.MusicName.battle);
                door.SetActive(true);
                enemyManager.Active();
            }
        }
    }
    private void OnDoorOpen()
    {
        door.SetActive(false);
    }
}
