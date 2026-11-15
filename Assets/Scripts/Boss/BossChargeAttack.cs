using UnityEngine;

public class BossChargeAttack : MonoBehaviour
{
    public int damage;

    private GameObject boss;
    private GameObject character;

    void Awake()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        character = temp[0];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject == character)
        {
            ChompHit(character);
            gameObject.SetActive(false);
        }
    }

    void ChompHit(GameObject cha)
    {
        Debug.Log("Chomp has hit");
        cha.GetComponent<Character>().TakeDamage(damage);
        boss.GetComponent<BossEnemy>().ChompHasHit();
    }

    public void ChompAttack(GameObject b)
    {
        boss = b;
    }
}
