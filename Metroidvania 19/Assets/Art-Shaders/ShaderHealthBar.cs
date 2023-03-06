using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderHealthBar : MonoBehaviour
{
    [SerializeField] Image _image;
    [Range (0,100)] [SerializeField] int _currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _image.material.SetFloat("_Health", _currentHealth);
    }
}
