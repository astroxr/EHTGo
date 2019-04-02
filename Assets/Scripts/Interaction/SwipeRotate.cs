using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class SwipeRotate : MonoBehaviour, IDragHandler
{
    public float rotSpeed = 15;
    private ARSessionOrigin m_SessionOrigin;
    private Vector2 previousPointer;

    void Start()
    {
        m_SessionOrigin = FindObjectOfType<ARSessionOrigin>();

    }
    void OnMouseDrag()
    {

        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX, Space.World);
        transform.Rotate(Vector3.right, rotY, Space.World);
    }    

    public void OnDrag(PointerEventData eventData)
    {
        // This one can be used for mobile devices (that don't have mouse)?
        Debug.Log("Dragging!");
        float rotX = eventData.delta.x * rotSpeed * Mathf.Deg2Rad;
        float rotY = eventData.delta.y * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX, Space.World);
        transform.Rotate(Vector3.right, rotY, Space.World);
    }

}
