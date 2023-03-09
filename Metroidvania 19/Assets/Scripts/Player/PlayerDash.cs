using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Tooltip("Length of cooldown")]
    public float timeInactive = 5f;

    private AbilitiesShader shader;

    [SerializeField] float dashSpeed = 0f;
    [SerializeField] float dashDuration = 10f;

    float cooldown = 0f;
    float dashTime = 0f;
    float previousSpeed;
    bool isDashing = false;
    PlayerMovement playerMovement;

    private void Awake()
    {
        shader = FindObjectOfType<AbilitiesShader>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(2, true)); }
    }

    private void OnDisable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(2, false)); }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time > cooldown && isDashing == false)
        {
            
            SetDashSpeed(true);
            isDashing= true;

        }
        else if (dashTime <= Time.time && cooldown == 0f && isDashing)
        {
            StartCoroutine(shader.DashCooldown((int)timeInactive));
            SetDashSpeed(false);
            isDashing = false;
        }
        else if (Time.time >= cooldown && cooldown != 0f)
        {
            Debug.Log("Reloaded");
            cooldown = 0f;
            dashTime= 0f;
        }
    }

    /// <summary>
    /// If isDashing is true: Store's player's current speed as previousSpeed and then sets their current speed as dashSpeed. dashTime is a timer for how long the dash is active.
    /// If isDashing is false: Sets player's current speed to previousSpeed. Also sets the cooldown timer for the period when the dash is inactive.
    /// </summary>
    /// <param name="isDashing"></param>
    void SetDashSpeed(bool isDashing) 
    {
        if(isDashing)
        {
            
            previousSpeed = playerMovement.GetSpeed();
            playerMovement.SetSpeed(dashSpeed);
            dashTime = Time.time + dashDuration;
            Debug.Log("Dashing at speed: " + playerMovement.GetSpeed());
        }
        else
        {
            Debug.Log("Speed reset to " + previousSpeed);
            
            playerMovement.SetSpeed(previousSpeed);
            Debug.Log(Time.time);
            cooldown = Time.time + timeInactive;
            Debug.Log(cooldown);

        }
    }
}
