using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed; //Speed of enemy movement
    public int playerCloseness; //How close is the player from enemy. For ranged
    public Transform targetDestination; //Where the enemy will target/follow if it is meant to, generally the player
    public int alive; // Is the enemy object alive, 1 = yes, 0 = no
    public float startDelay; // How long enemies wait before moving at start of round

    public string patrolType; // Patrol types for patroller enemies
    public float sprinterRecharge; // If enemy is a sprinter, recharge time for their charge attack
    public float sprinterChargeTime; // How long the sprinter will telegraph its attack
    public float moveDelay; // Enemy movement delay
    public string movementType; //Type of enemy movement; ie. follower, static, ranged, sprinter, flying, active/inactive patroller
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
    private GameObject targetGameobject; // Target object of enemy, generally player
    private Vector3 direction; // Direction for enemy to head in
    private float distance; // Distance between enemy and player
    private float timer; // How long enemy waits between actions
    private float sprinterRechargeTimer; // Timer for sprinter recharge time
    private Transform oldTargetPosition; // Old position of target for sprinters
    private string[] patrolList; // For choosing random patrol type for patrollers
    private int patrolFlag = 1; // Flag for patroller movement, 1 for starting movement, -1 for opposite direction

    void Awake() // For initializing getComponents & stuff
    {
        rigidbody = GetComponent<Rigidbody2D>();
        targetGameobject = targetDestination.gameObject;
        movementType = movementType.ToLower(); // Convert to lower case for ease of comparison
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = (targetDestination.position - transform.position).normalized;
        distance = Vector3.Distance(targetDestination.position, transform.position);
        patrolList = new string[] {"vertical", "horizontal", "diagonalLeft", "diagonalRight"};

        if (movementType.Equals("follower"))
        {
            timer = moveDelay;
            Debug.Log("Follower Movement");

        }
        else if (movementType.Equals("static"))
        {
            Debug.Log("Static Movement");
        }
        else if (movementType.Equals("ranged"))
        {
            timer = moveDelay;
            Debug.Log("Ranged Movement");
        }
        else if (movementType.Equals("sprinter"))
        {
            timer = moveDelay;
            Debug.Log("Sprinter Movement");
        }
        else if (movementType.Equals("flyer"))
        {
            timer = moveDelay;
            Debug.Log("Flyer Movement");
        }
        else if (movementType.Equals("active-patrol"))
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
        else if (movementType.Equals("inactive-patrol"))
        {
            int index = Random.Range(0, patrolList.Length);
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

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //direction = (targetDestination.position - transform.position).normalized;
        distance = Vector3.Distance(targetDestination.position, transform.position);

        if (startDelay > 0f)
        {
            startDelay -= Time.deltaTime;
        }
        else if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else if (sprinterRechargeTimer > 0f)
        {
            sprinterRechargeTimer -= Time.deltaTime;
        }
        else
        {
            if (movementType.Equals("follower"))
            {
                FollowerMovement();
                timer = moveDelay;
            }
            else if (movementType.Equals("static"))
            {
                StaticMovement();
            }
            else if (movementType.Equals("ranged"))
            {
                RangedMovement();
                timer = moveDelay;
            }
            else if (movementType.Equals("sprinter"))
            {
                timer = moveDelay;
                SprinterMovement();
                
            }
            else if (movementType.Equals("flyer"))
            {
                timer = moveDelay;
                FlyerMovement();
            }
            else if (movementType.Equals("active-patrol"))
            {
                ActivePatrolMovement();
            }
            else if (movementType.Equals("inactive-patrol"))
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
        direction = (targetDestination.position - transform.position).normalized;
        rigidbody.linearVelocity = direction * speed;
        return; 
    }


    void StaticMovement() // basically stays still
    {
        direction = (targetDestination.position - transform.position).normalized;
        return;
    }

    void RangedMovement() // Attacks at range, backs up if player gets too close
    {
        direction = (targetDestination.position - transform.position).normalized;
        if (distance > playerCloseness)
        {
            rigidbody.linearVelocity = new Vector2(0f, 0f);
            RangedAttack();
        }
        else
        {
            Vector3 antiDirection = -(direction);
            rigidbody.linearVelocity = antiDirection * speed;
        }
        return;
    }

    void SprinterMovement()
    {
        direction = (targetDestination.position - transform.position).normalized;
        if (distance <= playerCloseness)
        {
            rigidbody.linearVelocity = new Vector2(0f, 0f);
            sprinterChargeAttack();
        }
        else
        {
            rigidbody.linearVelocity = direction * speed;
        }

        return; // Temporary
    }

    void FlyerMovement()
    {
        direction = (targetDestination.position - transform.position).normalized;
        rigidbody.linearVelocity = direction * speed;
        return; // Temporary
    }

    void ActivePatrolMovement()
    {
        if (distance < playerCloseness || patrolFlag == 0)
        {
            direction = (targetDestination.position - transform.position).normalized;
            patrolFlag = 0;
        }
        else
        {
            direction = direction * patrolFlag;
        }
        rigidbody.linearVelocity = direction * speed;
        return; // Temporary
    }

    void InActivePatrolMovement()
    {
        direction = direction * patrolFlag;
        rigidbody.linearVelocity = direction * speed;
        return; // Temporary
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == targetGameobject)
        {
            Attack();
        }
        else if (collision.gameObject.CompareTag("Walls"))
        {
            patrolFlag = -patrolFlag;
        }

    }

    private void Attack() // Make coroutine later probly
    {
        Debug.Log("Attack");
    }

    private void RangedAttack() // Make coroutine later probly
    {
        Debug.Log("Ranged Attack");
    }

    private void sprinterChargeAttack()
    {
        Debug.Log("Charge attack");
        sprinterRechargeTimer = sprinterRecharge;
        timer = 0f; // I am not too sure about this tbh, i just feel like having both timers going at the same time is redundant
        // The above line can be deleted if we deem it better without it; was mainly for testing.
    }
}