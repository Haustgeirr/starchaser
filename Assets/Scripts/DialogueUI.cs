using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject messagePrefab;
    public float displayDuration = 5f;

    public void ShowMessage(string message)
    {
        var obj = GameManager.Instantiate(messagePrefab, dialoguePanel.transform);
        obj.GetComponent<Message>().Init(displayDuration);

        var text = obj.GetComponentInChildren<TextMeshProUGUI>();
        text.SetText(message);
    }

    private void OnEnable()
    {
        Dialogue.onShowMessage += ShowMessage;
    }

    private void OnDisable()
    {
        Dialogue.onShowMessage -= ShowMessage;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
