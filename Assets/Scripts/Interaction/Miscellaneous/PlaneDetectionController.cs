using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// This class demonstrates how to toggle plane detection,
/// and also hide or show the existing planes.
/// </summary>
[RequireComponent(typeof(ARPlaneManager))]
public class PlaneDetectionController : MonoBehaviour
{
    /// <summary>
    /// Toggles plane detection and the visualization of the planes.
    /// </summary>
    public void TogglePlaneDetection()
    {
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

        if (m_ARPlaneManager.enabled)
        {
            SetAllPlanesActive(true);
        }
        else
        {
            SetAllPlanesActive(false);
        }
    }

    /// <summary>
    /// Iterates over all the existing planes and activates
    /// or deactivates their <c>GameObject</c>s'.
    /// </summary>
    /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
    public void SetAllPlanesActive(bool value)
    {
        m_ARPlaneManager.GetAllPlanes(s_Planes);
        foreach (var plane in s_Planes)
            plane.gameObject.SetActive(value);
    }

    public void RemoveVisualizer()
    {
        m_ARPlaneManager.planePrefab = null;
    }

    public void RestoreVisualizer()
    {
        m_ARPlaneManager.planePrefab = planePrefab;
    }

    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        planePrefab = m_ARPlaneManager.planePrefab;
    }

    private GameObject planePrefab;
    ARPlaneManager m_ARPlaneManager;

    static List<ARPlane> s_Planes = new List<ARPlane>();
}
