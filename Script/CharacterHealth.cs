using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // Thêm dòng này để dùng IEnumerator

public class CharacterHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Slider healthSlider;
    public Image fillImage;

    private bool isDead = false;
    private Animator animator;  // Thêm Animator

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        animator = GetComponent<Animator>();  // Lấy Animator
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Test mất máu
        {
            TakeDamage(20f);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            float ratio = currentHealth / maxHealth;
            healthSlider.value = ratio;

            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.red, Color.green, ratio);
            }
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Bật animation "IsDeading"
        if (animator != null)
            animator.SetBool("IsDeading", true);

        // Đợi 1 giây cho animation chạy rồi tắt object
        StartCoroutine(DieAfterDelay(1f));
    }

    private IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
