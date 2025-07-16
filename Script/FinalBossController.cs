using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class FinalBossController : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 2f;      // Tốc độ di chuyển
    public float stopDistance = 2f;      // Khoảng cách dừng trước player

    [Header("Attack")]
    public float cooldown = 2f;          // Thời gian giữa các chiêu

    // Nội bộ
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private bool isTouchingPlayer = false;
    private float timer = 0f;
    private int skillIndex = 0;
    private readonly string[] skills = {
        "IsSkiling",    // chiêu 1
        "IsSkiling2",   // chiêu 2
        "IsSkiling3",   // chiêu 3
        "IsDeading"     // chiêu cuối
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

    // Xoay mặt boss về phía player
    void FacePlayer()
    {
        sr.flipX = player.position.x > transform.position.x;
    }

    // Cập nhật thời gian và kích hoạt skill khi đủ cooldown
    void UpdateAttackTimer()
    {
        if (isTouchingPlayer)
        {
            timer += Time.deltaTime;
            if (timer >= cooldown)
            {
                PerformSkill();
                timer = 0f;
            }
        }
    }

    // Xử lý di chuyển của boss
    void HandleMovement()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        bool running = !isTouchingPlayer && dist > stopDistance;
        animator.SetBool("IsIdle", !running);

        if (running)
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
    }

    // Dừng boss khi chạm hoặc gần player
    void StopMovement()
    {
        rb.velocity = Vector2.zero;
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

    // Thực hiện skill theo thứ tự
    void PerformSkill()
    {
        string param = skills[skillIndex];
        skillIndex = (skillIndex + 1) % skills.Length;

        animator.SetBool(param, true);
        StartCoroutine(ResetBool(param, 0.5f)); // Tự tắt sau 0.5s
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