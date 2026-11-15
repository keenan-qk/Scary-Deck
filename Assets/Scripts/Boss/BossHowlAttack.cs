using UnityEngine;

public class BossHowlAttack : MonoBehaviour
{
    public int damage;
    public float timer;
    public float stunTime;

    private GameObject character;
    private GameObject boss;
    private float realTimer;


    void Awake()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        character = temp[0];
        realTimer = timer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(realTimer > 0f)
        {
            realTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
            boss.GetComponent<BossEnemy>().HowlEnded();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == character)
        {
            HowlHit(character);
            gameObject.SetActive(false);
        }
    }

    void HowlHit(GameObject cha)
    {
        Debug.Log("Howl has hit player");
        boss.GetComponent<BossEnemy>().HowlEnded();
        cha.GetComponent<Character>().Stun(stunTime);
    }

    public void HowlAttack(GameObject b)
    {
        boss = b;
    }
}
