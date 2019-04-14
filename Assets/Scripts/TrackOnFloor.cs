using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class TrackOnFloor : MonoBehaviour, IPointerClickHandler
{
    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool detectingPlacement = true;
    private Renderer objRenderer;
    private Collider objCollider;
    private static StandardShaderUtils.BlendMode OpaqueMode = StandardShaderUtils.BlendMode.Opaque;
    private static StandardShaderUtils.BlendMode TransMode = StandardShaderUtils.BlendMode.Transparent;
    public void Reset()
    {
        StandardShaderUtils.ChangeRenderMode(objRenderer.material, TransMode);   
        placementPoseIsValid = false;
        detectingPlacement = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (detectingPlacement && placementPoseIsValid){
            StandardShaderUtils.ChangeRenderMode(objRenderer.material, OpaqueMode);
            detectingPlacement = false;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        objRenderer = GetComponent<Renderer>();
        objCollider = GetComponent<Collider>();
        StandardShaderUtils.ChangeRenderMode(objRenderer.material, TransMode);
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
        _SetVisibility(placementPoseIsValid);
        if (placementPoseIsValid){
            Debug.Log("Placing " + gameObject.name);
            // Use its pose to update the object's pose
            placementPose = hits[0].pose;
            // update placement
            
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

            gameObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            Debug.Log("Placement is INVALID!: Object is now inactive.");
        }
    }

    /// <summary>
    /// Make object invisible
    /// </summary>
    /// <param name="visible"> whether you want it visible</param>
    private void _SetVisibility(bool visible)
    {
        objRenderer.enabled = visible;
        objCollider.enabled = visible;
    }
}
