using UnityEngine;
using System.Collections.Generic;

public class BossDialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Boss1Controller bossController;
    public List<string> dialogueLines; // Các dòng hội thoại

    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            dialogueManager.StartDialogue(dialogueLines);
            if (bossController != null)
            {
                bossController.StartChasing();
            }
        }
    }
}