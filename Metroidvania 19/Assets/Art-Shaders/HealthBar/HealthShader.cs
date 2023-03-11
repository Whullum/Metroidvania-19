using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthShader : MonoBehaviour
{
    [SerializeField] Image[] _bodys;
    [SerializeField] Image _head;
    [SerializeField] Image _tail;

    [Range (0,100)] [SerializeField] int _currentHealth;


    //Applying currenHealth to each body segment (an image in the array)
    //When the most right body segment (not the tail) is zero, start counting down the next body (slider goes from 100 to 0 for each body segment)
    //Body segments that gets to zero, deleted 
    //When the player left with 2 body segments: tail and body, start counting down from tail to head  

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
       
        _bodys[_bodys.Length-1].material.SetFloat("_Health", _currentHealth);
    }
}
