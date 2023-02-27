using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    GameObject HurtBox;
    
    [Tooltip("Length of cooldown")]
    public float timeInactive = 2f;

    float cooldown = 0;
    void Start()
    {
        HurtBox = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            
            cooldown = Time.time + timeInactive;
            HurtBox.SetActive(true);
        }
        else if(Time.time > cooldown)
        {
            HurtBox.SetActive(false);
            
        }
    }
}
