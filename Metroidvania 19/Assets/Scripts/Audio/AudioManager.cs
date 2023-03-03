using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField]
    private Map currentMap;

    public AK.Wwise.State approachEnemy;
    public AK.Wwise.State combat;
    public AK.Wwise.State death;
    public AK.Wwise.State defeatEnemy;
    public AK.Wwise.State lowHealth;
    public AK.Wwise.State none;
    public AK.Wwise.State normal;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        normal.SetValue();
        currentMap.mapMusic.Post(gameObject);
    }

    private void Update()
    {
        
    }
}
