using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitiesShader : MonoBehaviour
{

    // Access the _Rotation property from RadialWipe shader and _Rotation from RadialWipeBG
    // Apply _Rotation property from RadialWipe to _Rotation from RadialWipeBG, so each changes in the first slider will be changes in the second as well

    [SerializeField] public GameObject _meleeIcon;
    [SerializeField] public GameObject _projectileIcon;
    [SerializeField] public GameObject _dashIcon;

    // First element should be BG, second is Top, third is Frame, fourth is Key Icon
    [SerializeField] public Image[] _meleeAbility;
    [SerializeField] public Image[] _projectileAbility;
    [SerializeField] public Image[] _dashAbility;

    [Range (0,1)] [SerializeField] float _currentCooldownM;
    [Range (0,1)] [SerializeField] float _currentCooldownP;
    [Range (0,1)] [SerializeField] float _currentCooldownD;

    // Update is called once per frame
    void Update()
    {
        _meleeAbility[0].material.SetFloat("_Roation", _currentCooldownM);
        _meleeAbility[1].material.SetFloat("_Roation", _currentCooldownM);

        _projectileAbility[0].material.SetFloat("_Roation", _currentCooldownP);
        _projectileAbility[1].material.SetFloat("_Roation", _currentCooldownP);

        _dashAbility[0].material.SetFloat("_Roation", _currentCooldownD);
        _dashAbility[1].material.SetFloat("_Roation", _currentCooldownD);

        // if (Input.GetKeyDown(KeyCode.Z) && _currentCooldownM == 1) {
        //     StartCoroutine(MeleeCooldown(6));
        // }

        // if (Input.GetKeyDown(KeyCode.X) && _currentCooldownP == 1) {
        //     StartCoroutine(ProjectileCooldown(2));
        // }

        // if (Input.GetKeyDown(KeyCode.C) && _currentCooldownD == 1) {
        //     StartCoroutine(DashCooldown(10));
        // }
    }

    public IEnumerator MeleeCooldown(int cooldownTime) {
        if (_currentCooldownM == 1) {
            yield return new WaitForEndOfFrame();
            StartCoroutine(CooldownAnim(_meleeAbility, "Q", cooldownTime));
            yield return new WaitForSeconds(0.1f);

            // Sets cooldown slider to 0, and then back to 1 over the course of 3 seconds
            _currentCooldownM = 0;
            LeanTween.value(gameObject, _currentCooldownM, 1f, cooldownTime).setOnUpdate((float flt) => {
                _currentCooldownM = flt;
            });
        }
    }

    public IEnumerator ProjectileCooldown(int cooldownTime) {
        if (_currentCooldownP == 1) {
            yield return new WaitForEndOfFrame();
            StartCoroutine(CooldownAnim(_projectileAbility, "E", cooldownTime));
            yield return new WaitForSeconds(0.1f);

            // Sets cooldown slider to 0, and then back to 1 over the course of 3 seconds
            _currentCooldownP = 0;
            LeanTween.value(gameObject, _currentCooldownP, 1f, cooldownTime).setOnUpdate((float flt) => {
                _currentCooldownP = flt;
            });
        }
    }

    public IEnumerator DashCooldown(int cooldownTime) {
        if (_currentCooldownD == 1) {
            yield return new WaitForEndOfFrame();
            StartCoroutine(CooldownAnim(_dashAbility, "F", cooldownTime));
            yield return new WaitForSeconds(0.1f);

            // Sets cooldown slider to 0, and then back to 1 over the course of 3 seconds
            _currentCooldownD = 0;
            LeanTween.value(gameObject, _currentCooldownD, 1f, cooldownTime).setOnUpdate((float flt) => {
                _currentCooldownD = flt;
            });
        }
    }

    public IEnumerator CooldownAnim(Image[] keyImage, string keyString, int cooldownTime) {
        // Frame pops out and icon image scrunches
        LeanTween.scale(keyImage[2].gameObject, new Vector3(1f, 1f, 1f), 0.1f);
        LeanTween.scaleX(keyImage[0].gameObject, 0.55f, 0.1f);
        LeanTween.scaleY(keyImage[0].gameObject, 0.55f, 0.1f);

        // Frame shrinks and icon image expands
        yield return new WaitForSeconds(0.2f);
        LeanTween.scale(keyImage[2].gameObject, new Vector3(0.8f, 0.8f, 0.8f), cooldownTime/2);
        LeanTween.scaleX(keyImage[0].gameObject, 0.6f, 0.2f);
        LeanTween.scaleY(keyImage[0].gameObject, 0.6f, 0.2f);

        // Key icon turns gray with white text, starts countdown with icon text
        keyImage[3].color = Color.gray;
        keyImage[3].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        for (int i = cooldownTime; i > 0; i--) {
            keyImage[3].GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        keyImage[3].GetComponentInChildren<TextMeshProUGUI>().text = keyString;
        keyImage[3].color = Color.white;
        keyImage[3].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }

    public IEnumerator toggleAnim(int ability, bool turningOn) {
        yield return new WaitForEndOfFrame();
        float newY = (turningOn == true) ? 50f : -250f;
        switch (ability) {
            case 0: // Melee
                LeanTween.moveLocalY(_meleeIcon, newY, 0.5f).setEaseOutQuad();
                break;
            case 1: // Projectile
                LeanTween.moveLocalY(_projectileIcon, newY, 0.5f).setEaseOutExpo();
                break;
            case 2: // Dash
                LeanTween.moveLocalY(_dashIcon, newY, 0.5f).setEaseOutExpo();
                break;
            default:
                break;
        }       
    }

    private void OnApplicationQuit() {
        this.enabled = false;
    }
}
