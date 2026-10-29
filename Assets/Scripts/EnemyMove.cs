using System.Collections;
using UnityEngine;

/*[RequireComponent(typeof(Rigidbody))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField, Min(0f)] float detectionRange = 10f;
    [SerializeField, Min(0f)] float movementSpeed = 3f;
    [SerializeField, Min(0f)] float attackRange = 1.5f;
    [SerializeField, Min(0f)] float jumpForce = 5f;
    [SerializeField, Min(0f)] float attackCooldown = 1.5f;
    [SerializeField, Min(0f)] float attackDelay = 0.25f;

    Rigidbody body;
    Transform player;
    PlayerHP playerHP;
    float baseMovementSpeed;
    bool isGrounded = true;
    bool isAttacking;
    float nextAttackTime;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        baseMovementSpeed = movementSpeed;
    }

    void OnEnable()
    {
        KillCounter.KillCountChanged += UpdateSpeedForKillCount;
    }

    void OnDisable()
    {
        KillCounter.KillCountChanged -= UpdateSpeedForKillCount;
    }

    void Start()
    {
        UpdateSpeedForKillCount(KillCounter.Kills);
    }

    void UpdateSpeedForKillCount(int killCount)
    {
        int speedTier = killCount / 15;
        movementSpeed = baseMovementSpeed * (1f + speedTier * 0.5f);
    }

    void Update()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
            {
                StopHorizontalMovement();
                return;
            }

            player = playerObject.transform;
            playerHP = playerObject.GetComponent<PlayerHP>();
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        if (distance > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (distance > detectionRange)
        {
            StopHorizontalMovement();
            return;
        }

        if (isAttacking)
        {
            StopHorizontalMovement();
            return;
        }

        if (distance <= attackRange)
        {
            StopHorizontalMovement();
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                StartCoroutine(JumpAndAttack());
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null || isAttacking)
        {
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        if (direction.magnitude <= detectionRange && direction.magnitude > attackRange)
        {
            Vector3 velocity = direction.normalized * movementSpeed;
            body.linearVelocity = new Vector3(velocity.x, body.linearVelocity.y, velocity.z);
        }
        else
        {
            StopHorizontalMovement();
        }
    }

    IEnumerator JumpAndAttack()
    {
        isAttacking = true;
        if (isGrounded)
        {
            isGrounded = false;
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(attackDelay);

        if (player != null && playerHP != null && HorizontalDistanceToPlayer() <= attackRange + 0.5f)
        {
            playerHP.TakeDamage(1);
        }

        isAttacking = false;
        StopCoroutine(JumpAndAttack());
    }

    void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                isGrounded = false;
                return;
            }
        }
    }

    float HorizontalDistanceToPlayer()
    {
        Vector3 offset = player.position - transform.position;
        offset.y = 0f;
        return offset.magnitude;
    }

    void StopHorizontalMovement()
    {
        Vector3 velocity = body.linearVelocity;
        body.linearVelocity = new Vector3(0f, velocity.y, 0f);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position + Vector3.up * 0.05f;
        DrawDistanceGizmo(center, detectionRange, Color.cyan, "Deteccion");
        DrawDistanceGizmo(center, attackRange, Color.yellow, "Ataque");
        DrawDistanceGizmo(center, attackRange + 0.5f, Color.red, "Alcance del golpe");

        if (player != null)
        {
            Vector3 playerPosition = player.position;
            playerPosition.y = center.y;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(center, playerPosition);
        }
    }

    void DrawDistanceGizmo(Vector3 center, float radius, Color color, string label)
    {
        const int segments = 48;
        float angleStep = 2f * Mathf.PI / segments;

        Gizmos.color = color;
        Vector3 previousPoint = center + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

#if UNITY_EDITOR
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.Label(center + new Vector3(radius, 0f, 0f), $"{label}: {radius:0.##}");
#endif
    }
}*/

public class EnemyIA:MonoBehaviour
{
    Transform playerTransform;
    PlayerHP playerHealth;
    public float cooldownTimer = 2.5f;
    [SerializeField] bool attackWasSucces;
    [SerializeField]float distanceToPlayer;
    public float velocity = 1.5f;
    public float force = 1.5f;
    Rigidbody enemyRigidbody;
    public enum EnemyState
    {
        Waiting, Following, Attacking
    }
    public EnemyState enemyState;
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerHealth = FindAnyObjectByType<PlayerHP>();
        enemyState = EnemyState.Waiting;
        enemyRigidbody = GetComponent<Rigidbody>();
        attackWasSucces = false;
    }
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Waiting:
                //Reproducir IDLE enemigo
            break;
        
            case EnemyState.Following:

                Vector3 followPoint = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);

                transform.position = Vector3.Lerp(transform.position, followPoint, velocity*Time.deltaTime);
                transform.LookAt(playerTransform);

            break;
        
            case EnemyState.Attacking:

                enemyRigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
                enemyRigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
                transform.LookAt(playerTransform);
                enemyState = EnemyState.Following;
            break;

        }
        if(attackWasSucces && cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if(attackWasSucces && cooldownTimer <= 0)
        {
            attackWasSucces = false;
            cooldownTimer= 2.5f;
            enemyState = EnemyState.Waiting;
        }
    }
    void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
      if(cooldownTimer != 0)
        {
            if(distanceToPlayer <= 30f && distanceToPlayer >= 10f)
            {
                enemyState = EnemyState.Following;
            }
            else if (distanceToPlayer <= 10f)
            {
                enemyState = EnemyState.Attacking;
            }
            else
            {
                enemyState = EnemyState.Waiting;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && enemyState == EnemyState.Attacking && !attackWasSucces)
        {
            playerHealth.TakeDamage(1f);
            attackWasSucces = true;
        }

    }
}
