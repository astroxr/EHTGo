using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

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

    void Awake()
    {
        m_SessionOrigin = FindObjectOfType<ARSessionOrigin>();

    }

    private void OnEnable() {
        EventManager.OnSelected += ProcessSelected;
        EventManager.OnPinched += ProcessPinch;
    }

    private void OnDisable() {
        EventManager.OnSelected -= ProcessSelected;
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
                // GameObject Telescope_Container = GameObject.FindWithTag("Earth");
                // GameObject Telescope_Container = GameObject.Find("Earth_NewModel2");
                Load_TelescopeData script = GetComponent<Load_TelescopeData>();

                GameObject Selected_Telescope = vHit.transform.gameObject;
                PinScript script_2 = Selected_Telescope.GetComponent<PinScript>();

                script.Select_Telescope(script_2.ID);
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

