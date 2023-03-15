using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AK.Wwise.Event stageMusic;
    public AK.Wwise.Event stopStageMusic;

    public AK.Wwise.State normal;
    public AK.Wwise.State approaching;
    public AK.Wwise.State combat;
    public AK.Wwise.State lowHealth;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        normal.SetValue();
        stageMusic.Post(gameObject);
    }

    public void StopMsuic()
    {
        stopStageMusic.Post(gameObject);
    }
}
