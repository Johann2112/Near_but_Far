using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Dialogos : MonoBehaviour
{
    [SerializeField] private List<DialogueManager> dialogueList = new List<DialogueManager>();
    [SerializeField] private int currentDialogAtMoment = 0;
    [SerializeField] private Image character1Image;
    [SerializeField] private GameObject character1Object;
    [SerializeField] private Image character2Image;
    [SerializeField] private GameObject character2Object;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;


    private int currentLineAtMoment = 0;
    private bool isDialogActive = false;

    private void Update()
    {
        if (isDialogActive && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }


    private void Start()
    {
        dialoguePanel.SetActive(false);
        character1Object.SetActive(false);
        character2Object.SetActive(false);
    }

    public void StartDialogue(int actualLine)
    {
        if (actualLine < 0 || actualLine >= dialogueList.Count)
        {
            return;
        }

        currentDialogAtMoment = actualLine;
        currentLineAtMoment = 0;
        isDialogActive = true;

        Time.timeScale = 0f;

        dialoguePanel.SetActive(true);
        character1Object.SetActive(true);
        character2Object.SetActive(true);
        ShowLine();
    }

    private void ShowLine()
    {
        var dialogue = dialogueList[currentDialogAtMoment].dialogues[currentLineAtMoment];
        dialogueText.text = dialogue.text;

        if (dialogue.talkCharacter1)
        {
            character1Image.sprite = dialogue.spriteCharacter1;
            character1Image.color = Color.white;
            character2Image.color = Color.gray;
        }
        else
        {
            character2Image.sprite = dialogue.spriteCharacter2;
            character2Image.color = Color.white;
            character1Image.color = Color.gray;
        }
    }

    private void NextLine()
    {

        if (!isDialogActive)
        {
            return;
        }
        currentLineAtMoment++;
        if (currentLineAtMoment >= dialogueList[currentDialogAtMoment].dialogues.Count)
        {
            EndDialogue();
            return;
        }
        ShowLine();

    }

    private void EndDialogue()
    {
        isDialogActive = false;
        dialoguePanel.SetActive(false);
        character1Object.SetActive(false);
        character2Object.SetActive(false);

        Time.timeScale = 1f;

        dialogueList[currentDialogAtMoment].onDialogueEnd?.Invoke();
    }

    public void TriggerDialogue(int index)
    {
        StartDialogue(index);
    }

}
