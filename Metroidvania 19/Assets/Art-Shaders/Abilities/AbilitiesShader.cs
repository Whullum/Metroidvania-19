using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesShader : MonoBehaviour
{

// Access the _Rotation property from RadialWipe shader and _Rotation from RadialWipeBG
// Apply _Rotation property from RadialWipe to _Rotation from RadialWipeBG, so each changes in the first slider will be changes in the second as well

    [SerializeField] Image[] _dashAbility;
    [SerializeField] Image[] _maleeAbility;
    [SerializeField] Image[] _projectileAbility;

    [Range (0,1)] [SerializeField] float _currentCooldownD;
    [Range (0,1)] [SerializeField] float _currentCooldownM;
    [Range (0,1)] [SerializeField] float _currentCooldownP;


    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
       
        _dashAbility[0].material.SetFloat("_Roation", _currentCooldownD);
        _dashAbility[1].material.SetFloat("_Roation", _currentCooldownD);

        _maleeAbility[0].material.SetFloat("_Roation", _currentCooldownM);
        _maleeAbility[1].material.SetFloat("_Roation", _currentCooldownM);

        _projectileAbility[0].material.SetFloat("_Roation", _currentCooldownP);
        _projectileAbility[1].material.SetFloat("_Roation", _currentCooldownP);

    }
}
