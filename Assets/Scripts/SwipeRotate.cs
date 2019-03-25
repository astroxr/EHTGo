using UnityEngine;
using System.Collections;

public class SwipeRotate : MonoBehaviour
{
    float rotSpeed = 20;

    public GameObject Earth;

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = -1 * Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        Earth.transform.Rotate(Vector3.up, -rotX, Space.World);
        Earth.transform.Rotate(Vector3.right, rotY, Space.World);

    }
}
