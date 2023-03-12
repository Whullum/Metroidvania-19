using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoorLock", menuName = "ScriptableObjects/DoorLock", order = 2)]
public class DoorLock : ScriptableObject
{
    public bool isLocked = true;

    public void ToggleDoor()
    {
        isLocked = !isLocked;
    }
}
