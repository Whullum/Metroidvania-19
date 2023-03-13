using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthShader : MonoBehaviour
{
    [SerializeField] Image[] _bodySegments;
    [SerializeField] Image _head;
    [SerializeField] Image _tail;

    //Applying currenHealth to each body segment (an image in the array)
    //When the most right body segment (not the tail) is zero, start counting down the next body (slider goes from 100 to 0 for each body segment)
    //Body segments that gets to zero, deleted 
    //When the player left with 2 body segments: tail and body, start counting down from tail to head  

    // Start is called before the first frame update
    // void Start()
    // {
    //     focusedSegmentIndex = PlayerController.Body
    // }



    // Update is called once per frame
    void Update()
    {
        //_bodySegments[index].material.SetFloat("_Health", 100);
    }

    public void ToggleSegment(int index, bool turningOn) {
        _bodySegments[index].gameObject.SetActive(turningOn);
        if (turningOn) { 
            _head.material.SetFloat("_Health", 100);
            _tail.material.SetFloat("_Health", 100);
            foreach (Image bodySegment in _bodySegments) {
                bodySegment.material.SetFloat("_Health", 100);
            }
        }
    }

    public void DepleteSegment(int index, float remainingHealth) {
        _bodySegments[index-1].material.SetFloat("_Health", remainingHealth);
    }

    public void DepleteHeadAndTail(float remainingHealth) {
        if (remainingHealth <= 100) {
            _head.material.SetFloat("_Health", remainingHealth);
            _tail.material.SetFloat("_Health", 0);
        } else {
            _head.material.SetFloat("_Health", 100);
            _tail.material.SetFloat("_Health", remainingHealth - 100);
        }

        if (remainingHealth == 0) {
            StartCoroutine(FullyDepleted());
        }
    }

    public void DepleteTail(int index, float remainingHealth) {
        Debug.LogError("WHEE " + index + " " + remainingHealth);
        _tail.material.SetFloat("_Health", remainingHealth);
    }

    public IEnumerator FullyDepleted() {
        yield return new WaitForEndOfFrame();
        LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 0f, 1f);
        yield return new WaitForSeconds(1.2f);
        DepleteHeadAndTail(200);
    }
}
