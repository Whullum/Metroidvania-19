using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected AK.Wwise.Event activationSound;

    /// <summary>
    /// Triggers this object active state.
    /// </summary>
    public virtual void OnActivation() { }

    /// <summary>
    /// Reverts this object to a previous unactivated state.
    /// </summary>
    public virtual void OnDeactivation() { }
}
