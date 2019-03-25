using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.


    void ProcessOneInput(Vector2 vTouchPos)
    {

        int telescopeLayers = 1 << LayerMask.NameToLayer("TelescopePins");

        // The ray to the touched object in the world
        Ray ray = Camera.main.ScreenPointToRay(vTouchPos);

        // Your raycast handling
        RaycastHit vHit;
        
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        if (Physics.Raycast(ray,  out vHit, Mathf.Infinity, telescopeLayers))
        {
            Debug.Log("Touched one finger!");
            if (vHit.transform.tag == "TelescopePin")
            {

                GameObject Telescope_Container = GameObject.Find("Earth_NewModel2");
                Load_TelescopeData script = Telescope_Container.GetComponent<Load_TelescopeData>();

                GameObject Selected_Telescope = vHit.transform.gameObject;
                PinScript script_2 = Selected_Telescope.GetComponent<PinScript>();

                script.Select_Telescope(script_2.ID);
            }
        }
    }

    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (GetComponent<Camera>().orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 5f, 140f);
            }
        }
        else if (Input.touchCount == 1)
        {
            ProcessOneInput(Input.GetTouch(0).position);
        }

        if (Input.GetMouseButton(0))
        {
            ProcessOneInput(Input.mousePosition);
        }
    }
}

