using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="New Camera Shake Force", menuName = "Camera/Shake")]
public class CameraShake : ScriptableObject
{
    public float AmplitudeForce;
    public float FrequencyGain;
    public float Duration;
}
