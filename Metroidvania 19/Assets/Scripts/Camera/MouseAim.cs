using UnityEngine;

public class MouseAim : MonoBehaviour
{

    private void Update()
    {
        // We set this GameObject transform to the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }
}
