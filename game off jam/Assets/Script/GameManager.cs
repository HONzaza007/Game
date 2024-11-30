using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int bossDeadCount;

    public static bool isPause;

    private bool canPause = true;

    public PlayerInputAction action;

    public delegate void OnPause();
    public static OnPause onPause;

    public delegate void OnUnPause();
    public static OnUnPause onUnPause;

    private void Awake()
    {
        action = new PlayerInputAction();
        action.Player.Enable();

        action.Player.Pause.performed += Pause;
        Player.onPlayerDeath += PlayerDeath;
        Player.onPlayerRespawn += PlayerRespawn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!canPause) return;
        if (isPause)
        {
            UnPause();
        }else{
            isPause = true;
            Time.timeScale = 0.0f;
            onPause.Invoke();
        }
    }
    public void UnPause()
    {
        if (!canPause) return;
        isPause = false;
        Time.timeScale = 1f;
        onUnPause.Invoke();
    }
    private void PlayerDeath()
    {
        canPause = false;
        isPause = true;
        Time.timeScale = 0.0f;
    }
    private void PlayerRespawn()
    {
        canPause = true;
        isPause = false;
        Time.timeScale = 1f;
    }

    public void OnEndTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.PlayMusic(MusicManager.MusicName.general);
            SceneManager.LoadScene(2);
        }
    }
}
