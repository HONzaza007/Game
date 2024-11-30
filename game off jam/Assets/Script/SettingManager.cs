using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    private Slider sensSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sensSlider.value = PlayerLook.sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerLook.sensitivity = sensSlider.value;
    }
}
