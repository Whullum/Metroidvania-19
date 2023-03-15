using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueItem : MonoBehaviour
{
    private GameObject player;
    private bool nearby;
    private bool inDialogue;
    private bool transitioning;

    public GameObject buttonPrompt;
    public GameObject dialogueBar;
    public List<GameObject> dialogueContent;
    public float detectDistance;

    // Start is called before the first frame update
    void Start()
    {
        nearby = false;
        inDialogue = false;
        transitioning = false;
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueBar = GameObject.Find("DialogueBar");
        wipeDialogue();
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
            LeanTween.moveLocalY(buttonPrompt, 1.2f, 0.5f).setEaseOutQuad();
        } else if (playerDistance >= detectDistance && nearby) {
            nearby = false;
            LeanTween.moveLocalY(buttonPrompt, 0f, 0.5f).setEaseOutQuad();
        }

        if (!transitioning) {
            if (nearby && !inDialogue && (Input.GetMouseButtonDown(0))) {
                StartCoroutine(showDialogue());
            } else if (inDialogue && ((Input.GetMouseButtonDown(0)) || !nearby)) {
                StartCoroutine(hideDialogue());
            }
        }
    }

    public IEnumerator showDialogue() {
        yield return new WaitForEndOfFrame();
        transitioning = true;
        wipeDialogue();
        addDialogue();
        LeanTween.alphaCanvas(dialogueBar.GetComponent<CanvasGroup>(), 1f, 0.5f).setEaseInOutQuad();
        yield return new WaitForSeconds(0.5f);
        inDialogue = true;
        transitioning = false;
    }

    public IEnumerator hideDialogue() {
        yield return new WaitForEndOfFrame();
        transitioning = true;
        LeanTween.alphaCanvas(dialogueBar.GetComponent<CanvasGroup>(), 0f, 0.5f).setEaseInOutQuad();
        yield return new WaitForSeconds(0.5f);
        inDialogue = false;
        transitioning = false;
    }

    public void wipeDialogue() {
        foreach (Transform child in dialogueBar.GetComponentInChildren<HorizontalLayoutGroup>().gameObject.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void addDialogue() {
        foreach (GameObject newChild in dialogueContent) {
            GameObject g = Instantiate (newChild);
            
            g.transform.parent = dialogueBar.GetComponentInChildren<HorizontalLayoutGroup>().gameObject.transform;
            g.transform.localScale = new Vector3(1,1,1);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueBar.GetComponentInChildren<HorizontalLayoutGroup>().GetComponent<RectTransform>());
    }
}
