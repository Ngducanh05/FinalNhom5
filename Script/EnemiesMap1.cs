using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesMap1 : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;
    public float attackCooldown = 2f;
    public float maxHealth = 100f;
    public float attackDamage = 20f;
    private float currentHealth;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        StartCoroutine(ChaseAndAttack());
    }

    private IEnumerator ChaseAndAttack()
    {
        while (true)
        {
            if (player == null || currentHealth <= 0)
            {
                yield return null;
                continue;
            }

            Vector2 direction = (player.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, player.position);

            // Gửi hướng di chuyển vào Animator
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);

            // Lật sprite theo hướng di chuyển
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = false;
            }

            if (distance > stopDistance)
            {
                // Đuổi theo Player
                rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

                animator.SetBool("IsHeading", true);
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsAttacking", false);
            }
            else
            {
                // Trong tầm đánh
                rb.velocity = Vector2.zero;

                animator.SetBool("IsHeading", false);
                animator.SetBool("IsRunning", false);

                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
            }

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);

        // Gây sát thương cho player nếu trong tầm
        if (player != null && Vector2.Distance(transform.position, player.position) <= stopDistance)
        {
            CharacterHealth playerHealth = player.GetComponent<CharacterHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(attackCooldown);

        animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("IsDead", true);
        rb.velocity = Vector2.zero;
        enabled = false;
        Destroy(gameObject, 2f);
    }
}