using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingHealthBar : MonoBehaviour
{
    [SerializeField] Image[] bodys;
    [SerializeField] GameObject[] bodysObjects;
    [SerializeField] Image _head;
    [SerializeField] Image _tail;

    [Range (0,100)] [SerializeField] int _currentHealth;



    void Start()
    {
        _currentHealth = 100;
        for (int i= bodys.Length-1; i>=0; i--)
        {
            bodys[i].material.SetFloat("_Health", _currentHealth);
        }

        _head.material.SetFloat("_Health", _currentHealth);
        _tail.material.SetFloat("_Health", _currentHealth);

        StartCoroutine(StartingCoroutine());
    }

    IEnumerator CountDownAndWait(int i)
    {
        while (_currentHealth>0)
        {
            yield return new WaitForSeconds(0.02f);
            bodys[i].material.SetFloat("_Health", _currentHealth);
            _currentHealth--;
        }
        bodysObjects[i].SetActive(false);
    }

    IEnumerator StartingCoroutine()
    {
       for (int index= bodys.Length-1; index>=0;)
       {
         yield return StartCoroutine(CountDownAndWait(index));
        index--;
        _currentHealth = 100;
       }

       yield return StartCoroutine(CountDownAndWaitEnding(_tail));
       _currentHealth = 100;
       yield return StartCoroutine(CountDownAndWaitEnding(_head));


    }

    IEnumerator CountDownAndWaitEnding(Image segment)
    {
        while (_currentHealth>0)
        {
            yield return new WaitForSeconds(0.02f);
            segment.material.SetFloat("_Health", _currentHealth);
            _currentHealth--;
        }
    }

    void Update()
    {
       


    }
}
