using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class Initialize : MonoBehaviour {
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    ARSessionOrigin m_SessionOrigin;
    ARPlaneManager m_ARPlaneManager;
    ARReferencePointManager m_RefPointManager;
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
        // m_RefPointManager = GetComponent<ARReferencePointManager>();
    }
    private void Start() {
        
    }
    private void Update() {
        // if ((spawnedObject != null) || Input.touchCount == 0)
        //     return;

        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began)
            return;

        Debug.Log("Touched Screen!");

        var hitsomething = m_SessionOrigin.Raycast(Input.GetTouch(0).position, s_Hits, TrackableType.Planes);

        Debug.Log("Sent RayCast!");

        if (hitsomething)
            Debug.Log("RayCast hit a plane! INFO:");
        else
        {
            Debug.Log("RayCast hit nothing!");
            return;
        }

        // if (!m_SessionOrigin.Raycast(Input.GetTouch(0).position, s_Hits, TrackableType.Planes))
        //     return;

        var hit = s_Hits[0];

        Debug.Log("Position: " + hit.pose);

        // var plane = m_ARPlaneManager.TryGetPlane(hit.trackableId);
        // if (plane == null)
        //     return;

        if (spawnedObject == null)
        {
            // spawnedObject = Instantiate(m_PlacedPrefab, hit.pose.position, hit.pose.rotation);

            spawnedObject = Instantiate(m_PlacedPrefab);
            // Position at one meter above the ground.
            m_SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hit.pose.position + Vector3.up, hit.pose.rotation);
            SetAllPlanesActive(false);
        }

        // var planeNormal = plane.transform.up;
        // var refPoint = m_RefPointManager.TryAttachReferencePoint(plane, hit.pose.position + planeNormal + 1*Vector3.up, hit.pose.rotation);
        // spawnedObject = Instantiate(m_PlacedPrefab, hit.pose.position, hit.pose.rotation);
        // spawnedObject.transform.SetParent(refPoint.transform);
        // m_ARPlaneManager.planePrefab = null ;

        // TogglePlaneDetection();
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

}