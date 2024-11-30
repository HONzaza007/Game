using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicManager.PlayMusic(MusicManager.MusicName.general);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }
}
