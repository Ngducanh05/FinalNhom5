using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Boss1Controller : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Settings")]
    public float moveSpeed = 1.5f;
    public float cooldown = 3f;
    public float attackDamage = 30f;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    // State
    private bool isChasing = false;
    private bool isTouchingPlayer = false;
    private bool isDead = false;
    private float attackTimer = 0f;
    private int skillIndex = 0;

    private readonly string[] skills = {
        "IsSkilling1",
        "IsSkilling2",
        "IsPunch to the lefting",
        "IsPunch to the righting",
        "IsLeft_hand attacking",
        "IsRight_hand attacking"
    };

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        Debug.Log("Boss initialized.");
    }

    void Update()
    {
        if (!isChasing || isDead || player == null) return;

        if (isTouchingPlayer)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= cooldown)
            {
                PerformAttack();
                attackTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isChasing || isDead || player == null) return;

        if (!isTouchingPlayer)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            animator.SetBool("IsRunning", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsRunning", false);
        }

        sr.flipX = player.position.x > transform.position.x;
    }

    void PerformAttack()
    {
        string skill = skills[skillIndex];
        skillIndex = (skillIndex + 1) % skills.Length;

        Debug.Log(">> TUNG CHIÊU: " + skill);
        animator.SetBool(skill, true);
        StartCoroutine(ResetAttack(skill));
    }

    IEnumerator ResetAttack(string skill)
    {
        // Gây sát thương
        if (player.TryGetComponent(out CharacterHealth health))
        {
            health.TakeDamage(attackDamage);
            Debug.Log("Damage dealt: " + attackDamage);
        }

        yield return new WaitForSeconds(0.2f); // Delay animation
        animator.SetBool(skill, false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            Debug.Log("Touching player");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
            Debug.Log("Left player");
        }
    }

    public void StartChasing()
    {
        if (!isChasing)
        {
            isChasing = true;
            Debug.Log("Boss started chasing");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log("Boss took damage: " + damage);
        if (damage >= 500f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.velocity = Vector2.zero;
        animator.SetBool("IsDead", true);
        Debug.Log("Boss died");
        StartCoroutine(DieAfterDelay(2f));
    }

    IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
