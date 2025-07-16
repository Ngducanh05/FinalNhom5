using UnityEngine;

public class ZoneTrigger2D : MonoBehaviour
{
    public Boss1Controller boss;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            boss.StartChasing();
            Debug.Log("Player vào zone, boss bắt đầu đuổi!");
        }
    }
}
