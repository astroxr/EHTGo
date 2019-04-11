using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

public class TrackOnFloor : MonoBehaviour
{
    public GameObject trackingObj;
    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool detectingPlacement = true;
    private Renderer m_Renderer;


    private void OnEnable() {
        //OnClick.onClick += OnTrackedClick;
    }    
    private void OnDisable() {
        //OnClick.onClick -= OnTrackedClick;
    }

    public void OnTrackedClick(PointerEventData eventData)
    {
        Debug.Log("Click detected on Object!");
        if (detectingPlacement && placementPoseIsValid){
            Debug.Log("Placing object!");
            m_Renderer.material.SetFloat("_Mode", 0);
            var opaqueMode = StandardShaderUtils.BlendMode.Opaque;
            StandardShaderUtils.ChangeRenderMode(m_Renderer.material, opaqueMode);
            detectingPlacement = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        m_Renderer = trackingObj.GetComponent<Renderer>();
        var fadeMode = StandardShaderUtils.BlendMode.Fade;
        StandardShaderUtils.ChangeRenderMode(m_Renderer.material, fadeMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (detectingPlacement){
            updatePlacement();
        }
    }

    private void updatePlacement(){

        // Send raycast from center of screen
        Debug.Log("Updating placement!");
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        
        // Send Raycast and check if it hit any trackable
        placementPoseIsValid = arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);
        if (placementPoseIsValid){
            Debug.Log("Placement is VALID!");
            // Use its pose to update the object's pose
            placementPose = hits[0].pose;

            // Update pose to move with the camera's angle view
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

            // update placement
            arOrigin.MakeContentAppearAt(trackingObj.transform, placementPose.position);
            // arOrigin.MakeContentAppearAt(trackingObj.transform, placementPose.position, placementPose.rotation);
            trackingObj.SetActive(true);
            Debug.Log("Object is now active!");
        }
        else
        {
            trackingObj.SetActive(false);
            Debug.Log("Placement is INVALID!: Object is now inactive.");
        }
    }
}
