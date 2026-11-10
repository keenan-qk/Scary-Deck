using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("How long enemy hitbox will remain active for upon attack")]
    public float time;

    private GameObject character;
    private int damage;
    private float timer;

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
        if(timer > 0f)
        {
            timer -= Time.deltaTime; 
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == character)
        {
            AttackHit(character);
            gameObject.SetActive(false);
        }
        
    }

    void AttackHit(GameObject cha)
    {
        Debug.Log("Attack has hit");
        cha.GetComponent<Character>().TakeDamage(damage);
    }

    public void MeleeAttack(int dam)
    {
        damage = dam;
        gameObject.SetActive(true);
        timer = time;
    }
}
