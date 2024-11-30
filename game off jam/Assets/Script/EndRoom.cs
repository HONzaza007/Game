using UnityEngine;

public class EndRoom : MonoBehaviour
{

    [SerializeField] GameObject endDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.bossDeadCount == 2)
        {
            endDoor.SetActive(false);
        }
    }


}
