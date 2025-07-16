using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool IsTouchingPlayer { get; private set; }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            IsTouchingPlayer = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            IsTouchingPlayer = false;
    }
}
