using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public float speed; //Speed of enemy movement
    public float attackSpeed; // How long the enemy will wait to attack after attacking or moving
    [Tooltip("Distance enemy can attack player from using chomp sprint")]
    public float chompAttackDistance; // Distance from player for enemy to attack
    [Tooltip("Delay at start of scene for enemy actions")]
    public float startDelay; // How long boss waits before moving at start of round
    public int health; // Health of enemy
    public int damage; // How much damage each enemy attack does
    [Tooltip("Enemy chomp attack hitbox")]
    public GameObject chompHitbox;
    [Tooltip("Enemy claw sensor hitbox")]
    public GameObject clawSensor;
    [Tooltip("Array of enemy claw hitboxes")]
    public GameObject[] clawHitboxes;
    [Tooltip("Enemy howl hitbox")]
    public GameObject howlHitbox;


    [Tooltip("Recharge time for a sprinter's charge attack")]
    public float sprintRecharge; // Recharge time for their charge attack
    [Tooltip("How long the sprinter will telegraph its charge attack")]
    public float attackChargingTime; // How long the boss will telegraph its attack
    [Tooltip("How far the sprinter will go on its charge attack")]
    public float sprintChargeDistance; // How far the sprint will go on the attack before they stop
    [Tooltip("Delay for enemy to read player movement")]
    public float moveDelay; // Enemy movement delay
    

    [Tooltip("Color that object should be when not attacking/recharging/stunned")]
    public Color baseColor;
    [Tooltip("Color that enemy will be to telegraph")]
    public Color attackColor;
    [Tooltip("Color that enemy will be on recharge")]
    public Color rechargeColor;
    [Tooltip("Color that enemy will be when stunned")]
    public Color stunColor;

    private Rigidbody2D rb; // Rigidbody of enemy
    private GameObject character; // Target object of enemy, generally player
    private Vector3 direction; // Direction for enemy to head in
    private float distance; // Distance between enemy and player
    private Vector3 sprintOldPos; // Old position of sprinter before attack; for measuring distance of charge
    private float sprintDistance; // Distance between sprinter old and current positions

    private float timer; // How long enemy waits between actions
    private float attackTimer; // How long it takes in between attacks
    private float attackChargeTimer; // Timer for sprint attacks to charge up
    private float sprintRechargeTimer; // Timer for sprinter recharge time
    private float stunTimer; // Timer for stunned enemies
    private Vector3 oldTargetDirection = new Vector3(0, 0, 1); // Old position of target for sprinters
    private int attackChargeFlag = 0;// Flag for attacks charging; when enemy is in range of player they stop moving to charge their attack, this flag prevents it from moving whilst charging up.
    private int sprintingFlag = 0; // Flag for whether or not sprinter is sprinting
    private Renderer rend;
    private int howlFlag = 0;

    private int attackCounter; // Counter for what attack is up next in the sequence
    private float clawAttackDistance;
    private float howlAttackDistance;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = this.GetComponent<Renderer>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        if (temp.Length != 0)
        {
            character = temp[0]; // Find player character in scene and then target them
        }
        else
        {
            Debug.LogError("Enemy cannot find player");
        }

        if (clawSensor != null)
        {
            clawAttackDistance = clawSensor.transform.localScale.x + gameObject.transform.localScale.x;
        }
        if (howlHitbox != null)
        {
            howlAttackDistance = howlHitbox.transform.localScale.x;
        }

        if(clawSensor == null)
        {
            Debug.LogWarning("Claw Sensor Not Detected");
        }
        if (chompHitbox == null)
        {
            Debug.LogWarning("Chomp Hitbox Not Detected");
        }
        if (howlHitbox == null)
        {
            Debug.LogWarning("Howl Hitbox Not Detected");
        }
        for(int i = 0; i < clawHitboxes.Length; i++)
        {
            if(clawHitboxes[i] == null)
            {
                Debug.LogWarning("Claw Hitbox for Claw #" + i + " not detected");
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackTimer = attackSpeed;
        rb.freezeRotation = true;
        direction = (character.transform.position - transform.position).normalized;
        distance = Vector3.Distance(character.transform.position, transform.position);
        timer = moveDelay;

    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Collider2D thisCollider = gameObject.GetComponent<Collider2D>();
        Collider2D playerCollider = character.GetComponent<Collider2D>();
        distance = (thisCollider.Distance(playerCollider)).distance;

        if (sprintingFlag == 1)
        {
            sprintDistance = Vector3.Distance(sprintOldPos, transform.position);
        }

        if (startDelay > 0f)
        {
            startDelay -= Time.deltaTime;
        }
        else if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else if (sprintingFlag == 1)
        {
            if(sprintDistance >= sprintChargeDistance)
            {
                rb.linearVelocity = new Vector2(0f, 0f);
                sprintingFlag = 0;
                Debug.Log("Done Sprinting");
                sprintRechargeTimer = sprintRecharge;
                chompHitbox.SetActive(false);
                attackCounter = (attackCounter + 1) % 4;
                rend.material.SetColor("_Color", rechargeColor);
            }
        }
        else if (sprintRechargeTimer > 0f)
        {
            sprintRechargeTimer -= Time.deltaTime;
        }
        else if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
        }
        else if (attackChargeTimer > 0f)
        {
            attackChargeTimer -= Time.deltaTime;
        }
        else
        {
            timer = moveDelay;
            Movement();
        }
    }

    void Movement()
    {
        direction = (character.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.linearVelocity = direction * speed;

        if (attackCounter == 2)
        {
            Howl();
        }
        else if(attackCounter == 3 && (distance <= chompAttackDistance || attackChargeFlag == 1))
        {
            Chomp();
        }
        else if ((distance <= clawAttackDistance || attackChargeFlag == 1))
        {
            if (attackCounter == 0)
            {
                ClawAttack(attackCounter);
            }
            else if (attackCounter == 1)
            {
                ClawAttack(attackCounter);
            }
        }

        if (attackTimer <= 0 && attackChargeTimer <= 0)
        {
            rend.material.SetColor("_Color", baseColor);
        }
    }

    void Howl()
    {
        rb.linearVelocity = new Vector2(0f, 0f);
        if (attackChargeFlag == 0)
        {
            Debug.Log("Howl charging");
            rend.material.SetColor("_Color", attackColor);
            attackChargeTimer = attackChargingTime;
            attackChargeFlag = 1;
        }
        if(attackChargeTimer <= 0)
        {
            if (howlFlag == 0)
            {
                howlHitbox.SetActive(true);
                BossHowlAttack temp = howlHitbox.GetComponent<BossHowlAttack>();
                temp.HowlAttack(gameObject);
                howlFlag = 1;
            }
        }
    }

    void Chomp()
    {
        if (oldTargetDirection == new Vector3(0, 0, 1))
        {
            oldTargetDirection = direction;
            attackChargeTimer = attackChargingTime;
            rend.material.SetColor("_Color", attackColor);
        }
        rb.linearVelocity = new Vector2(0f, 0f);
        attackChargeFlag = 1;

        if (attackChargeTimer <= 0)
        {
            chompHitbox.SetActive(true);
            BossChargeAttack temp = chompHitbox.GetComponent<BossChargeAttack>();
            temp.ChompAttack(gameObject);

            sprintOldPos = gameObject.transform.position;
            rb.linearVelocity = oldTargetDirection * attackSpeed;
            sprintRechargeTimer = sprintRecharge;
            attackChargeFlag = 0;
            sprintingFlag = 1;
            oldTargetDirection = new Vector3(0, 0, 1);
            Debug.Log("Chomp Attack");
        }
        else
        {
            Debug.Log("Chomp charging");
        }
    }

    void ClawAttack(int num)
    {
        if(attackChargeFlag == 0 && attackTimer <= 0)
        {
            attackChargeFlag = 1;
            attackChargeTimer = attackChargingTime;
            rend.material.SetColor("_Color", attackColor);
        }
        float reducedSpeed = speed / 2;
        rb.linearVelocity = direction * reducedSpeed;
        if(attackTimer <= 0 && attackChargeTimer <= 0)
        {
            attackChargeFlag = 0;
            rend.material.SetColor("_Color", rechargeColor);
            clawHitboxes[num].SetActive(true);
            BossMeleeAttack temp = clawHitboxes[num].GetComponent<BossMeleeAttack>();
            temp.MeleeAttack(gameObject, num);
            attackTimer = attackSpeed;
        }
    }

    public void ChompHasHit()
    {
        rb.linearVelocity = new Vector2(0f, 0f);
        sprintingFlag = 0;
        Debug.Log("Done sprinting because it hit");
        attackCounter = (attackCounter + 1) % 4;
        rend.material.SetColor("_Color", rechargeColor);
    }

    public void HowlEnded()
    {
        Debug.Log("Done Howling");
        attackCounter = (attackCounter + 1) % 4;
        howlFlag = 0;
        attackChargeFlag = 0;
        rend.material.SetColor("_Color", rechargeColor);
    }

    public void ClawEnded(int flag)
    {
        if(flag == 1)
        {
            Debug.Log("Claw hit player");
        }
        else
        {
            Debug.Log("Claw didn't hit");
        }
        attackCounter = (attackCounter + 1) % 4;
        attackChargeFlag = 0;
        rend.material.SetColor("_Color", rechargeColor);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walls") && attackCounter == 3)
        {
            Stunned();
        }
    }

    void Stunned()
    {
        stunTimer = sprintRecharge * 2f;
        rb.linearVelocity = new Vector2(0f, 0f);
        sprintingFlag = 0;
        rend.material.SetColor("_Color", stunColor);
        Debug.Log("Stunned");
    }
}
