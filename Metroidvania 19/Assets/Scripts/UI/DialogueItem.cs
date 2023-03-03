using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueItem : MonoBehaviour
{
    private GameObject player;
    private bool nearby;
    private bool inDialogue;

    public GameObject buttonPrompt;
    public GameObject dialogueBar;
    public float detectDistance;
    public string dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        nearby = false;
        inDialogue = false;
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueText = (dialogueText == "") ? "This is the default text" : dialogueText;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("TS: " + Time.timeScale);
        // Temp variables
        Vector2 playerDirection = player.transform.position - transform.position;
        float playerDistance = (playerDirection).magnitude;

        if (playerDistance < detectDistance && !nearby) {
            nearby = true;
            LeanTween.moveLocalY(buttonPrompt, 1f, 0.5f).setEaseOutQuad();
        } else if (playerDistance >= detectDistance && nearby) {
            nearby = false;
            LeanTween.moveLocalY(buttonPrompt, 0f, 0.5f).setEaseOutQuad();
        }

        if (nearby && !inDialogue && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) {
            showDialogue();
        } else if (inDialogue && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) {
            hideDialogue();
        }
    }

    public void showDialogue() {
        Debug.Log("TIME STOP");
        inDialogue = true;
        dialogueBar.GetComponentInChildren<TextMeshProUGUI>().text = dialogueText;
        LeanTween.alphaCanvas(dialogueBar.GetComponent<CanvasGroup>(), 1f, 1f).setEaseInOutQuad();
    }

    public void hideDialogue() {
        inDialogue = false;
        LeanTween.alphaCanvas(dialogueBar.GetComponent<CanvasGroup>(), 0f, 0.5f).setEaseInOutQuad();
    }
}
