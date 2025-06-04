using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string text;
    public Sprite spriteCharacter1;
    public Sprite spriteCharacter2;
    public bool talkCharacter1; //true = character 1 false = character 2
}

[System.Serializable]
public class DialogueManager
{
    public List<Dialogue> dialogues = new List<Dialogue>();
    public UnityEvent onDialogueEnd;
}