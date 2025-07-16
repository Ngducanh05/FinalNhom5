using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // Panel chứa khung hội thoại (tag: "DialoguePanel")
    public TextMeshProUGUI dialogueText; // Text hiển thị nội dung
    public float typingSpeed = 0.05f; // Tốc độ gõ chữ
    private List<string> dialogueLines = new List<string>();
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        // Gán thủ công nếu cần (debug)
        if (dialoguePanel == null)
        {
            dialoguePanel = GameObject.FindWithTag("DialoguePanel");
            dialogueText = dialoguePanel.transform.Find("Dialogue_Text").GetComponent<TextMeshProUGUI>();
        }
    }

    public void StartDialogue(List<string> lines)
    {
        if (isDialogueActive) return;

        dialogueLines = lines;
        currentLineIndex = 0;
        isDialogueActive = true;

        Time.timeScale = 0f; // Tạm dừng game
        dialoguePanel.SetActive(true);

        DisplayNextLine();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleInput();
        }
    }

    public void OnContinueButton() // Gọi từ Continue_Button
    {
        if (isDialogueActive)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Count)
            {
                DisplayNextLine();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void DisplayNextLine()
    {
        dialogueText.text = "";
        StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
    }
}