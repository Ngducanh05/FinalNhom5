using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Boss2Controller : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Settings")]
    public float moveSpeed = 1.5f;
    public float stopDistance = 1.5f;
    public float cooldown = 3f;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    // State
    private bool isTouchingPlayer = false;
    private float timer = 0f;
    private int skillIndex = 0;
    private readonly string[] skills = {
        "IsSkilling1",
        "IsSkilling2",
        "IsAttacking",
        "IsReloading",
        "IsDeading",
        "IsRunning"
    };

    void Awake()
    {
        InitializeComponents();
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();
        UpdateAttackTimer();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        HandleMovement();
    }

    // Khởi tạo và cấu hình các thành phần
    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Quay mặt boss về hướng player
    void FacePlayer()
    {
        sr.flipX = player.position.x > transform.position.x;
    }

    // Cập nhật thời gian và kích hoạt tấn công khi đủ cooldown
    void UpdateAttackTimer()
    {
        if (isTouchingPlayer)
        {
            timer += Time.deltaTime;
            if (timer >= cooldown)
            {
                Attack();
                timer = 0f;
            }
        }
    }

    // Xử lý di chuyển của boss
    void HandleMovement()
    {
        if (!isTouchingPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMovement();
        }
    }

    // Di chuyển boss tới player
    void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;
        animator.SetBool("IsRunning", true);
    }

    // Dừng boss khi chạm player
    void StopMovement()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("IsRunning", false);
    }

    // Xử lý khi va chạm vào player
    void OnCollisionEnter2D(Collision2D col)
    {
        HandleCollisionEnter(col);
    }

    // Xử lý khi rời khỏi player
    void OnCollisionExit2D(Collision2D col)
    {
        HandleCollisionExit(col);
    }

    // Thực hiện tấn công với skill tương ứng
    void Attack()
    {
        string param = skills[skillIndex];
        skillIndex = (skillIndex + 1) % skills.Length;

        animator.SetBool(param, true);
        StartCoroutine(ResetBool(param, 0.1f));
    }

    // Đặt lại trạng thái bool trong Animator sau delay
    IEnumerator ResetBool(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(name, false);
    }

    // Xử lý khi va chạm vào player
    void HandleCollisionEnter(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
            isTouchingPlayer = true;
    }

    // Xử lý khi rời khỏi player
    void HandleCollisionExit(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
            isTouchingPlayer = false;
    }
}