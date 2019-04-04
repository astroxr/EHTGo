using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using System;


[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARPlaneManager))]
public class Initialize : MonoBehaviour, IPointerClickHandler {
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    [SerializeField]
    [Tooltip("Scale used to start up the application. This will affect the size of objects")]
    public float startingScale;
    ARSessionOrigin m_SessionOrigin;
    ARPlaneManager m_ARPlaneManager;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }
    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    static List<ARPlane> allPlanes = new List<ARPlane>();

    void Awake()
    {   
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }
    private void Start() {
        startingScale = 10f;
        transform.localScale = new Vector3(startingScale, startingScale, startingScale);
    }

    private void instantiatePrefab(PointerEventData eventData){
        
        if (m_SessionOrigin.Raycast(Input.GetTouch(0).position, s_Hits, TrackableType.Planes)){
            var hit = s_Hits[0];
            spawnedObject = Instantiate(m_PlacedPrefab);
            // Position at one meter above the ground.
            var offset = startingScale * Vector3.up;
            m_SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hit.pose.position + offset, hit.pose.rotation);
            SetAllPlanesActive(false);
        }
    }
    
    /// <summary>
    /// Toggles plane detection and the visualization of the planes.
    /// </summary>
    public void TogglePlaneDetection()
    {
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

        if (m_ARPlaneManager.enabled)
            SetAllPlanesActive(true);
        else
            SetAllPlanesActive(false);
    }

    /// <summary>
    /// Iterates over all the existing planes and activates
    /// or deactivates their <c>GameObject</c>s'.
    /// </summary>
    /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
    void SetAllPlanesActive(bool value)
    {
        m_ARPlaneManager.GetAllPlanes(allPlanes);
        foreach (var plane in allPlanes)
            plane.gameObject.SetActive(value);

        m_ARPlaneManager.planePrefab = null;
        Debug.Log("Removed AR Plane prefab!");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (spawnedObject == null)
            instantiatePrefab(eventData);
    }
}