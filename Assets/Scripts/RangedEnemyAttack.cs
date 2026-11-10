using UnityEngine;
using System.Collections;

public class RangedEnemyAttack : MonoBehaviour
{
    [Tooltip("Projectile damage")]
    public int damage;
    [Tooltip("Projectile speed")]
    public float speed;
    [Tooltip("Does projectile follow player")]
    public bool trackPlayer;
    [Tooltip("Delay for projectile to read player movement")]
    public float moveDelay;

    private GameObject character;
    private Rigidbody2D rb;
    private float distance;
    private float timer;

    void Awake()
    {
        //GameObject enemyObject = transform.parent.gameObject;
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        character = temp[0];
        rb = GetComponent<Rigidbody2D>();
        //enemy = enemyObject.GetComponent<Enemy>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(character.transform.position, transform.position);
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else if (trackPlayer == true)
        {
            Vector3 direction = (character.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            timer = moveDelay;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == character)
        {
            RangedAttackHit(character);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    void RangedAttackHit(GameObject cha) // When projectile hits player, give them damage
    {
        Debug.Log("Attack has hit");
        cha.GetComponent<Character>().TakeDamage(damage);
    }

    public void Shoot(Vector3 direction) // Shoots projectile in direction of player
    {
        rb.linearVelocity = direction * speed;
    }
    
}
