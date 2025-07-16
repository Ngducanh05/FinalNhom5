using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    [Header("Skill Settings")]
    public float skillDamage = 50f;
    public float skillRange = 2f;
    public float skillCooldown = 5f;

    private float lastSkillTime;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer sr;
    private Animator animator;
    private CharacterHealth health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = GetComponent<CharacterHealth>();
        lastSkillTime = -skillCooldown;
    }

    void Update()
    {
        ReadInput();
        HandleFacing();
        UpdateMovementAnimation();
        HandleAttackInput();
        HandleSkillInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    //———— Input ————
    private void ReadInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    //———— Facing ————
    private void HandleFacing()
    {
        if (Input.GetKeyDown(KeyCode.D)) sr.flipX = true;
        else if (Input.GetKeyDown(KeyCode.A)) sr.flipX = false;
    }

    //———— Movement Animation ————
    private void UpdateMovementAnimation()
    {
        bool isMoving = movement.sqrMagnitude > 0f;
        animator.SetBool("IsRunning", isMoving);
    }

    //———— Move Character ————
    private void Move()
    {
        float baseSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && movement.sqrMagnitude > 0f)
        {
            baseSpeed *= sprintMultiplier;
        }

        Vector2 newPos = rb.position + movement.normalized * baseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    //———— Attack Input ————
    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J)) animator.SetBool("IsAttacking", true);
        else if (Input.GetKeyUp(KeyCode.J)) animator.SetBool("IsAttacking", false);
    }

    //———— Skill Input ————
    private void HandleSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.time >= lastSkillTime + skillCooldown)
        {
            animator.SetBool("IsSkilling", true);
            UseSkill();
            lastSkillTime = Time.time;
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            animator.SetBool("IsSkilling", false);
        }
    }

    //———— Skill Logic ————
    private void UseSkill()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, skillRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemiesMap1 enemy = hit.GetComponent<EnemiesMap1>();
                if (enemy != null)
                {
                    enemy.TakeDamage(skillDamage);
                }
            }
            else if (hit.CompareTag("Boss"))
            {
                Boss1Controller boss = hit.GetComponent<Boss1Controller>();
                if (boss != null)
                {
                    boss.TakeDamage(skillDamage);
                }
            }
        }
    }

    //———— Debug Skill Range ————
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BackGround"))
        {
            if (health != null) health.TakeDamage(health.maxHealth);
        }
    }
}