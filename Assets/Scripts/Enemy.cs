using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed; //Speed of enemy movement
    public float attackSpeed; // How long the enemy will wait to attack after attacking or moving
    [Tooltip("Distance enemy can attack player from")]
    public float attackDistance; // Distance from player for enemy to attack, also used inversely for ranged enemies
    [Tooltip("How close a player can get before active patrol follows")]
    public float playerCloseness; //How close is the player from enemy. For active patrollers
    //[Tooltip("Where enemy will target to follow")]
    //public Transform targetDestination; //Where the enemy will target/follow if it is meant to, generally the player
    [Tooltip("Delay at start of scene for enemy actions")]
    public float startDelay; // How long enemies wait before moving at start of round

    [Tooltip("What type of patrol the enemy will take")]
    public string patrolType; // Patrol types for patroller enemies
    public int health; // Health of enemy
    public int damage; // How much damage each enemy attack does
    [Tooltip("Enemy attack hitbox")]
    public GameObject hitbox;

    [Tooltip("Recharge time for a sprinter's charge attack")]
    public float sprinterRecharge; // If enemy is a sprinter, recharge time for their charge attack
    [Tooltip("How long the sprinter will telegraph its charge attack")]
    public float sprintChargingTime; // How long the sprinter will telegraph its attack
    [Tooltip("How far the sprinter will go on its charge attack")]
    public float sprintChargeDistance; // How far the sprinter will go on their attack before they stop
    [Tooltip("Delay for enemy to read player movement")]
    public float moveDelay; // Enemy movement delay
    [Tooltip("How long enemy will stand still upon starting attack")]
    public float attackDelay;

    [Tooltip("Projectile that enemies with ranged attacks will shoot")]
    public GameObject projectilePrefab; // Project prefab for instantiating



    private string movementType; //Type of enemy movement; ie. follower, static, ranged, sprinter, flying, active/inactive patroller
    /*
     * Further explanation of each type of enemy movement can be found below
     * - Follower: an enemy that will directly follow the player as they move around the space
     * - Static: an enemy that does not move or moves very little (decide later?). Could be a obstacle like a cactus
         that disappears after defeating enemies or an enemy that attacks with ranged weapons but doesnt move
     * - Ranged: an enemy that attacks the player with ranged attacks and if the player moves close enough
         the ranged enemy will back up or move away from the player at a delay.
     * - Sprinter: an enemy that runs/charges at the player and has a short cooldown after
     * - Flying: an enemy that floats above the character possibly dropping attacks they must dodge? (Idk yet this
         one can be figured out later)
     * - Active Patroller: an enemy that patrols a certain area of a room but when the player gets close enough it 
         will break its patrol to attack the player
     * - Inactive Patroller: an enemy/obstacle which patrols a certain area of a room, no matter what
     */
    private Rigidbody2D rigidbody; // Rigidbody of enemy
    private GameObject character; // Target object of enemy, generally player
    private Vector3 direction; // Direction for enemy to head in
    private float distance; // Distance between enemy and player
    private Vector3 sprinterOldPos; // Old position of sprinter before attack; for measuring distance of charge
    private float sprinterDistance; // Distance between sprinter old and current positions

    private float timer; // How long enemy waits between actions
    private float attackTimer; // How long it takes in between attacks
    private float sprintChargeTimer; // Timer for sprint attacks to charge up
    private float sprinterRechargeTimer; // Timer for sprinter recharge time
    private float stunTimer; // Timer for stunned enemies

    private Vector3 oldTargetDirection = new Vector3(0,0,1); // Old position of target for sprinters
    //private string patrolType; // Patrol types for patroller enemies
    private string[] patrolList; // For choosing random patrol type for patrollers

    private int patrolFlag = 1; // Flag for patroller movement, 1 for starting movement, -1 for opposite direction
    private int sprinterChargeFlag = 0; // Flag for sprinter attacks; when sprinter is in range of player they stop moving to charge their attack, this flag prevents it from moving whilst charging up.
    private int sprintingFlag = 0; // Flag for whether or not sprinter is sprinting


    void Awake() // For initializing getComponents & stuff
    {
        rigidbody = GetComponent<Rigidbody2D>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        if (temp.Length != 0)
        {
            //character = targetDestination.gameObject;
            character = temp[0]; // Find player character in scene and then target them
        }
        else
        {
            Debug.LogError("Enemy cannot find player");
        }
        movementType = gameObject.tag;

        if (hitbox != null)
        { 
            attackDistance = hitbox.transform.localScale.y;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackTimer = attackSpeed; //ZACH ADDED THIS
        Debug.Log(movementType);
        attackTimer = 1;
        rigidbody.freezeRotation = true;

        direction = (character.transform.position - transform.position).normalized;
        distance = Vector3.Distance(character.transform.position, transform.position);
        patrolList = new string[] {"vertical", "horizontal", "diagonalLeft", "diagonalRight"};

        if (movementType.Equals("Follower"))
        {
            timer = moveDelay;
            Debug.Log("Follower Movement");

        }
        else if (movementType.Equals("Static"))
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log("Static Movement");
        }
        else if (movementType.Equals("Ranged"))
        {
            timer = moveDelay;
            Debug.Log("Ranged Movement");
        }
        else if (movementType.Equals("Sprinter"))
        {
            timer = moveDelay;
            Debug.Log("Sprinter Movement");
        }
        else if (movementType.Equals("Flyer"))
        {
            timer = moveDelay;
            Debug.Log("Flyer Movement");
        }
        else if (movementType.Equals("Active-Patrol"))
        {
            timer = moveDelay;
            int index = Random.Range(0, patrolList.Length);
            patrolType = patrolList[index];
            Debug.Log(patrolType);
            Debug.Log("Active Patroller Movement");

            if (patrolType.Equals("vertical")) // Patrolling in vertical line
            {
                direction = Vector3.up;
            }
            else if (patrolType.Equals("horizontal")) // Patrolling in horizontal line
            {
                direction = Vector3.right;
            }
            else if (patrolType.Equals("diagonalLeft")) // Patrolling diagonally from top left of bottom right
            {
                direction = new Vector3(-1, 1, 0);
            }
            else if (patrolType.Equals("diagonalRight")) // Patrolling diagonally from top right to bottom left
            {
                direction = new Vector3(1, 1, 0);
            }
        }
        else if (movementType.Equals("Inactive-Patrol"))
        {
            int index = Random.Range(0, patrolList.Length);
            timer = 0;
            patrolType = patrolList[index];
            Debug.Log(patrolType);
            Debug.Log("Inactive Patroller Movement");

            if (patrolType.Equals("vertical")) // Patrolling in vertical line
            {
                direction = Vector3.up;
            }
            else if (patrolType.Equals("horizontal")) // Patrolling in horizontal line
            {
                direction = Vector3.right;
            }
            else if (patrolType.Equals("diagonalLeft")) // Patrolling diagonally from top left of bottom right
            {
                direction = new Vector3(-1, 1, 0);
            }
            else if (patrolType.Equals("diagonalRight")) // Patrolling diagonally from top right to bottom left
            {
                direction = new Vector3(1, 1, 0);
            }
        }

    }

    // Update is called once per frame //ZACH UPDATED THIS
    void Update()
    {
        if (attackTimer >= 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if (sprintChargeTimer >= 0)
        {
            sprintChargeTimer -= Time.deltaTime;
        }
        if(health <= 0)
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

        if (movementType.Equals("Sprinter") && sprintingFlag == 1)
        {
            sprinterDistance = Vector3.Distance(sprinterOldPos, transform.position);
        }

        if (startDelay > 0f)
        {
            startDelay -= Time.deltaTime;
        }
        else if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else if (sprinterDistance > sprintChargeDistance && sprintingFlag == 1)
        {
            rigidbody.linearVelocity = new Vector2(0f, 0f);
            sprintingFlag = 0;
            Debug.Log("Done Sprinting");
        }
        else if (sprinterRechargeTimer > 0f)
        {
            sprinterRechargeTimer -= Time.deltaTime;
        }
        else if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
        }
        else
        {
            if (movementType.Equals("Follower"))
            {
                FollowerMovement();
                timer = moveDelay;
            }
            else if (movementType.Equals("Static"))
            {
                StaticMovement();
            }
            else if (movementType.Equals("Ranged"))
            {
                RangedMovement();
                timer = moveDelay;
            }
            else if (movementType.Equals("Sprinter"))
            {
                timer = moveDelay;
                SprinterMovement();

            }
            else if (movementType.Equals("Flyer"))
            {
                timer = moveDelay;
                FlyerMovement();
            }
            else if (movementType.Equals("Active-Patrol"))
            {
                ActivePatrolMovement();
            }
            else if (movementType.Equals("Inactive-Patrol"))
            {
                InActivePatrolMovement();
            }
            else
            {
                Debug.LogError("This is an invalid type of movement for enemies");
            }
        }
    }


    void FollowerMovement() // follows after player
    {
        direction = (character.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rigidbody.linearVelocity = direction * speed;
        if(distance <= attackDistance)
        {
            Debug.Log("Proper attack");
            Attack(character);
            rigidbody.linearVelocity = new Vector2(0f, 0f);
            timer = attackDelay;
        }
        return; 
    }


    void StaticMovement() // basically stays still
    {
        direction = (character.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (projectilePrefab == null)
        {
            if (distance <= attackDistance)
            {
                Attack(character);
            }
        }
        else
        {
            if(distance <= attackDistance)
            {
                Attack(character);
            }
            else
            {
                RangedAttack(character, direction);
            } 
        }
        
        return;
    }

    void RangedMovement() // Attacks at range, backs up if player gets too close
    {
        direction = (character.transform.position - transform.position).normalized;
        if (distance > attackDistance)
        {
            rigidbody.linearVelocity = new Vector2(0f, 0f);
            RangedAttack(character, direction);
        }
        else
        {
            Vector3 antiDirection = -(direction);
            rigidbody.linearVelocity = antiDirection * speed;
        }
        return;
    }

    void SprinterMovement() // Will charge at player when in range and chase otherwise; after charge attack they will rest
    {
        direction = (character.transform.position - transform.position).normalized;
        if (distance <= attackDistance || sprinterChargeFlag == 1)
        {
            if(oldTargetDirection == new Vector3(0, 0, 1))
            {
                oldTargetDirection = direction;
                sprintChargeTimer = sprintChargingTime;
            }
            rigidbody.linearVelocity = new Vector2(0f, 0f);
            sprinterChargeFlag = 1;
            sprinterChargeAttack();
        }
        else
        {
            rigidbody.linearVelocity = direction * speed;
        }

        return; 
    }

    void FlyerMovement()
    {
        direction = (character.transform.position - transform.position).normalized;
        rigidbody.linearVelocity = direction * speed;
        return; 
    }

    void ActivePatrolMovement() // Will patrol a part of the map until player gets close; then will act like follower enemy
    {
        if (distance < playerCloseness || patrolFlag == 0)
        {
            direction = (character.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            patrolFlag = 0;
            timer = moveDelay;
        }
        else
        {
            direction = direction * patrolFlag;
        }
        rigidbody.linearVelocity = direction * speed;

        if (distance <= attackDistance)
        {
            Attack(character);
            rigidbody.linearVelocity = new Vector2(0f, 0f);
        }
        return; 
    }

    void InActivePatrolMovement() // Will patrol a part of the map until dead
    {
        direction = direction * patrolFlag;
        rigidbody.linearVelocity = direction * speed;
        return; 
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject == character && !movementType.Equals("Sprinter"))
        //{
        //    Attack(character);
        //}
        if (collision.gameObject.CompareTag("Walls"))
        {
            patrolFlag = -patrolFlag;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == character && movementType.Equals("Sprinter"))
        {
            sprintAttack(character);
        }
        else if(collision.gameObject == character && movementType.Equals("Active-Patrol"))
        {
            if(patrolFlag != 0)
            {
                character.GetComponent<Character>().TakeDamage(damage);
            }
        }
        else if (collision.gameObject == character && movementType.Equals("Inactive-Patrol"))
        {
            character.GetComponent<Character>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Walls"))
        {
            if (movementType.Equals("Sprinter"))
            {
                Stunned();
            }
        }
    }

    public void TakeDamage(int damage) // Enemy takes damage
    {
        Debug.Log(damage);
        health -= damage;
    }

    private void Attack(GameObject cha) // for basic enemy attacks
    {
        if(attackTimer <= 0)
        {
            EnemyAttack temp = hitbox.GetComponent<EnemyAttack>();
            temp.MeleeAttack(damage);

            //Debug.Log("Attack");
            if(movementType.Equals("Sprinter"))
            {
                attackTimer = 0f;
            }
            else
            {
                attackTimer = attackSpeed;
            }
        }
    }

    private void RangedAttack(GameObject cha, Vector3 direct) // Ranged enemy attack function
    {
        if(attackTimer <= 0)
        {
            float angle = Mathf.Atan2(direct.y, direct.x) * Mathf.Rad2Deg;
            projectilePrefab.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            //Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            //rb.linearVelocity = direct * speed;
            RangedEnemyAttack temp = projectile.GetComponent<RangedEnemyAttack>();
            temp.Shoot(direct);
            attackTimer = attackSpeed;
            Debug.Log("Ranged Attack");
        }
    }

    private void sprinterChargeAttack() // Sprinter enemy attack function
    {
        if(sprintChargeTimer <= 0)
        {
            sprinterOldPos = gameObject.transform.position;
            rigidbody.linearVelocity = oldTargetDirection * attackSpeed;
            sprinterRechargeTimer = sprinterRecharge;
            sprinterChargeFlag = 0;
            sprintingFlag = 1;
            oldTargetDirection = new Vector3(0, 0, 1);
            Debug.Log("Charge Attack");
        }
        else
        {
            Debug.Log("Sprinter charging up");
        }
    }

    private void sprintAttack(GameObject cha) // Does the actual damaging part of the sprint attack
    {
        Debug.Log("Sprint attack hit");
        cha.GetComponent<Character>().TakeDamage(damage);
    }

    public void Stunned() // Stun function so that enemy is paused when stunned
    {
        if (movementType.Equals("Sprinter"))
        {
            stunTimer = sprinterRecharge * 2f;
            rigidbody.linearVelocity = new Vector2(0f, 0f);
            sprintingFlag = 0;
            Debug.Log("Stunned");
        }
    }


}