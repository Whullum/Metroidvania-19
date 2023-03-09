using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    private GameObject HurtBox;
    private AbilitiesShader shader;
    
    [Tooltip("Length of cooldown")]
    public float timeInactive = 2f;

    float cooldown = 0;
    void Awake()
    {
        HurtBox = transform.GetChild(0).gameObject;
        shader = FindObjectOfType<AbilitiesShader>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void OnEnable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(0, true)); }
    }

    private void OnDisable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(0, false)); }
    }

    private void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(shader.MeleeCooldown((int)timeInactive));
            cooldown = Time.time + timeInactive;
            HurtBox.SetActive(true);
        }
        else if(Time.time > cooldown)
        {
            HurtBox.SetActive(false);
            
        }
    }
}
