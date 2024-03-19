using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;
    public DialogueTrigger trigger;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public bool isActive = false;
    public bool hasFinishedConvo = false;

    InputManager inputManager;
    public GameObject convoBox;
    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        Debug.Log("Started convo, loaded msg" + messages.Length);
        trigger.gameObject.SetActive(false);
        convoBox.SetActive(true);
        DisplayMessage();
    }
    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

    }
    public void NextMessage()
    {
        activeMessage++;
        if ( activeMessage<currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            Debug.Log("Conversation ended");
            isActive = false;
            hasFinishedConvo = true;
            trigger.gameObject.SetActive(true);
            convoBox.SetActive(false);
        }
    }

    
    void Start()
    {
        inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        convoBox.SetActive(false);
    }
    void Update()
    {
       
        if ( inputManager.GetConversationContinueInput() && isActive == true)
        {
            NextMessage();

        }
        //if (isActive==true)
        //{
        //    convoBox.SetActive(true);
        //    playerController.DissableAllLandActions();
        //}
        //else
        //{
        //    convoBox.SetActive(false);
        //    playerController.EnableAllLandActions();
        //}
    }
}
