using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Load_TelescopeData))]
public class PinchSelect : MonoBehaviour
{
    /// <summary>
    /// The rate of change of the field of view in perspective mode.
    /// </summary>
    public float perspectiveZoomSpeed = 1f;
    /// <summary>
    /// The rate of change of the orthographic size in orthographic mode.
    /// </summary>
    public float orthoZoomSpeed = 1f;
    ARSessionOrigin m_SessionOrigin;
    private GameObject earthObj;
    private Load_TelescopeData LoadTelescope;
    private PinScript pinScript;
    

    void Awake()
    {
        m_SessionOrigin = FindObjectOfType<ARSessionOrigin>();

    }

    private void Start() {
        LoadTelescope = GetComponent<Load_TelescopeData>();
    }

    private void OnEnable() {
        EventManager.OnSelected += ProcessSelected;
        EventManager.OnSelectMouse += ProcessSelected;
        EventManager.OnPinched += ProcessPinch;
    }

    private void OnDisable() {
        EventManager.OnSelected -= ProcessSelected;
        EventManager.OnSelectMouse -= ProcessSelected;
        EventManager.OnPinched -= ProcessPinch;
    }

    void ProcessSelected(Touch touch){
        int telescopeLayers = 1 << LayerMask.NameToLayer("TelescopePins");

        // The ray to the touched object in the world
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        // Your raycast handling
        RaycastHit vHit;
        
        Debug.DrawRay(ray.origin, 10*ray.direction, Color.yellow);
        if (Physics.Raycast(ray,  out vHit, Mathf.Infinity, telescopeLayers))
        {
            if (vHit.transform.tag == "TelescopePin")
            {
                GameObject Selected_Telescope = vHit.transform.gameObject;
                pinScript = Selected_Telescope.GetComponent<PinScript>();

                LoadTelescope.Select_Telescope(pinScript.ID);
            }
        }
    }

    void ProcessSelected(Vector2 position){
        int telescopeLayers = 1 << LayerMask.NameToLayer("TelescopePins");

        // The ray to the touched object in the world
        Ray ray = Camera.main.ScreenPointToRay(position);

        // Your raycast handling
        RaycastHit vHit;
        
        Debug.DrawRay(ray.origin, 10*ray.direction, Color.yellow);
        if (Physics.Raycast(ray,  out vHit, Mathf.Infinity, telescopeLayers))
        {
            if (vHit.transform.tag == "TelescopePin")
            {
                GameObject Selected_Telescope = vHit.transform.gameObject;
                pinScript = Selected_Telescope.GetComponent<PinScript>();

                LoadTelescope.Select_Telescope(pinScript.ID);
            }
        }
    }

    void ProcessPinch(Touch touchZero, Touch touchOne){
            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            m_SessionOrigin.transform.localScale += Vector3.one * deltaMagnitudeDiff * perspectiveZoomSpeed/100f;
    }
}

