using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private Conversations dialogue;
    private bool interrupt = false;

    public float mesageDisplayTime = 2.5f;
    private Conversation currentConvo;
    private float nextMessageTime = Mathf.Infinity;
    private bool isPlaying = false;

    public delegate void OnShowMessage(string message);
    public static OnShowMessage onShowMessage;

    void PlayNextMessage()
    {
        int i = currentConvo.currentMessage;

        if (i == currentConvo.messages.Length)
        {
            isPlaying = false;
            currentConvo.hasBeenHeard = true;
            return;
        }

        Debug.Log(currentConvo.messages[i]);
        onShowMessage(currentConvo.messages[i]);

        currentConvo.currentMessage = i + 1;
    }

    public void PlayDialogue(int id)
    {
        currentConvo = dialogue.conversations[id];
        nextMessageTime = Time.time;
        isPlaying = true;
    }

    void Start()
    {
        dialogue = new Conversations();
    }

    void Update()
    {
        if (!GameManager.instance.IsPlaying())
            return;

        if (!isPlaying)
            return;

        if (nextMessageTime > Time.time)
            return;

        PlayNextMessage();
        nextMessageTime = Time.time + mesageDisplayTime;
    }
}