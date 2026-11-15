using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    public int damage;
    public float timer;

    private GameObject boss;
    private GameObject character;
    private int clawNum;
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
            boss.GetComponent<BossEnemy>().ClawEnded(-1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject == character)
        {
            MeleeHit(character);
            gameObject.SetActive(false);
        }
    }

    void MeleeHit(GameObject cha)
    {
        Debug.Log("Boss Claw " + clawNum + " has hit");
        cha.GetComponent<Character>().TakeDamage(damage);
        boss.GetComponent<BossEnemy>().ClawEnded(1);
    }

    public void MeleeAttack(GameObject b, int num)
    {
        boss = b;
        clawNum = num;
    }
}
