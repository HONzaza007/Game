using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private GameObject playerGameObject;
    private EnemyManager enemyManager;
    [SerializeField]
    private LayerMask bossLayer;
    [SerializeField]
    private Slider PlayerHpSlider;
    [SerializeField]
    private Slider bossHpSlider;
    [SerializeField]
    private Image playerGotHitImage;
    [SerializeField]
    private GameObject pauseGameObj;
    [SerializeField]
    private GameObject deadUiGameObj;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }

        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        GameManager.onPause += PauseUi;
        GameManager.onUnPause += UnPauseUi;
        Player.onPlayerDeath += DeadUi;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BossDetect();
        HpBoss();
        HpPlayer();
    }

    private void PauseUi()
    {
        pauseGameObj.SetActive(true);
    }
    private void UnPauseUi()
    {
        pauseGameObj.SetActive(false);
    }

    private void BossDetect()
    {
        Collider[] colliderOverlap = Physics.OverlapSphere(playerGameObject.transform.position, 65f,bossLayer);

        if (colliderOverlap.Length == 0)
        {
            bossHpSlider.gameObject.SetActive(false);
            enemyManager = null;
            return;
        }else
        {
            bossHpSlider.gameObject.SetActive(true);
        }
        for (int i = 0; i < colliderOverlap.Length; i++)
        {
            enemyManager = colliderOverlap[i].gameObject.GetComponentExtend<EnemyManager>();
        }
    }

    private void HpBoss()
    {
        if (enemyManager != null)
        {
            bossHpSlider.maxValue = enemyManager.maxHp;
            bossHpSlider.value = enemyManager.hp;
        }
    }

    private void HpPlayer()
    {
        Player player = playerGameObject.GetComponent<Player>();

        PlayerHpSlider.maxValue = player.maxHp;
        PlayerHpSlider.value = player.hp;
    }

    public void PlayerGotHitScreen()
    {
        StartCoroutine(gotHitCoroutine());

        IEnumerator gotHitCoroutine()
        {
            Debug.Log("asd");

            float start = 0.5f;
            float end = 0;

            float timeElapse = 0f;
            float duration = 0.25f;
            while (timeElapse < duration)
            {
                yield return null;

                float t = timeElapse/ duration;
                playerGotHitImage.color = new Color(1, 0, 0, math.lerp(start, end, t));

                timeElapse += Time.deltaTime;
            }
            playerGotHitImage.color = new Color(1, 0, 0, end);
        }
    }

    private void DeadUi()
    {
        Debug.Log("asddd");
        deadUiGameObj.SetActive(true);
    }
}
