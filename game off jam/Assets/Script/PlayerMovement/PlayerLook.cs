using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public static float sensitivity = 1f;

    [SerializeField]
    private CinemachineInputAxisController inputAxitConTroller;

    public Camera mainCamera;
    [SerializeField]
    private GameObject playerGameobject;
    [SerializeField]
    private GameObject weaponGameobject;
    private void Awake()
    {
        
    }

    private void Start()
    {
        weaponGameobject.transform.position = mainCamera.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        inputAxitConTroller.Controllers[0].Input.Gain = sensitivity;
        inputAxitConTroller.Controllers[1].Input.Gain = -sensitivity;
    }
    private void FixedUpdate()
    {
        if (GameManager.isPause)
        {
            UnLook();
        }
        else
        {
            Look();
        }
        
    }
    void LateUpdate()
    {
        if (GameManager.isPause)
        {
            UnLook();
        }
        else
        {
            Look();
        }
    }

    private void Look()
    {
        inputAxitConTroller.Controllers[0].Enabled = true;
        inputAxitConTroller.Controllers[1].Enabled = true;

        playerGameobject.transform.rotation = Quaternion.Euler(0, mainCamera.transform.localRotation.eulerAngles.y, 0);
        weaponGameobject.transform.rotation = mainCamera.transform.rotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void UnLook()
    {
        inputAxitConTroller.Controllers[0].Enabled = false;
        inputAxitConTroller.Controllers[1].Enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
}
