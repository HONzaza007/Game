using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    public GameObject player;

    public bool isActive;

    public bool isAlreadyDeath;

    public float maxHp;
    public float hp;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Player.onPlayerDeath += DeActive;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Death(GameObject _gameObject)
    {
        StartCoroutine(DeadCourutine());
        IEnumerator DeadCourutine()
        {
            Vector3 start = _gameObject.transform.localScale;
            Vector3 end = Vector3.zero;
            float timeElapse = 0;
            float duration = 0.5f;

            while (timeElapse < duration)
            {
                yield return null;

                float t = timeElapse / duration;
                _gameObject.transform.localScale = Vector3.Lerp(start, end, t);
                timeElapse += Time.deltaTime;
            }
            Destroy( _gameObject );
        }
    }

    public void Active()
    {
        isActive = true;
    }
    private void DeActive()
    {
        isActive = false;
    }
}
