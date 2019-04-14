using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

/// <summary>
/// This class will initialize the application.
/// </summary>
[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(PlaneDetectionController))]
public class Initialize : MonoBehaviour, IPointerClickHandler {
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject earthPrefab;
    [SerializeField]
    [Tooltip("Scale used to start up the application. This will affect the size of objects")]
    public float startingScale = 10;
    ARSessionOrigin m_SessionOrigin;
    ARPlaneManager m_ARPlaneManager;
    GameObject planeVisualizer;
    PlaneDetectionController m_planeDetection;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject EarthPrefab
    {
        get { return earthPrefab; }
        set { earthPrefab = value; }
    }
    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    static List<ARPlane> allPlanes = new List<ARPlane>();
    private TerrainController terrainController;

    private void Start() {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_planeDetection = GetComponent<PlaneDetectionController>();

        // save reference to plane visualizer
        planeVisualizer = m_ARPlaneManager.planePrefab;

        // Scale ARSessionOrigin according to starting scale
        transform.localScale = new Vector3(startingScale, startingScale, startingScale);

        // Disable TerrainController
        terrainController = GameObject.FindObjectOfType<TerrainController>();
        if (terrainController != null)
        {
            terrainController.enabled = false;
        }
    }

    private void instantiateEarth(PointerEventData eventData){
        
        if (m_SessionOrigin.Raycast(Input.GetTouch(0).position, s_Hits, TrackableType.Planes)){
            var hit = s_Hits[0];
            spawnedObject = Instantiate(earthPrefab);
            
            // Position at one unit (meter/scale applied) above the ground.
            var offset = startingScale * Vector3.up;
            // spawnedObject.transform.SetPositionAndRotation(hit.pose.position + offset, hit.pose.rotation);
            m_SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hit.pose.position + offset, hit.pose.rotation);
            m_planeDetection.SetAllPlanesActive(false);
            m_planeDetection.RemoveVisualizer();

            // Reactivate terrainController
            if (terrainController != null)
            {
                terrainController.enabled = true;
            }
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (spawnedObject == null)
            instantiateEarth(eventData);
    }
}