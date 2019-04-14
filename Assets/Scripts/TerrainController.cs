using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Controller that manages the instantiation of terrain prefabs into scene
/// when the ToggleTerrain HUD button is clicked.
/// </summary>
public class TerrainController : MonoBehaviour {
    
    [SerializeField]
    GameObject[] terrainPrefabs;

    private GameObject aOriginGameObject;

    private GameObject[] terrainObjs;

    private void OnEnable() {
        ChangeSlide.toggleTerrainAction += toggleTerrain;
    }

    private void OnDisable() {
        ChangeSlide.toggleTerrainAction -= toggleTerrain;
    }

    private void Start() {
        // Initialize the terrain object array
        terrainObjs = new GameObject[terrainPrefabs.Length];

        aOriginGameObject = FindObjectOfType<ARSessionOrigin>().gameObject;
    }

    /// <summary>
    /// Disable all terrain objects in scene.
    /// This does not detect if multiple were enabled.
    /// </summary>
    /// <returns> returns the index of the one that was activated </returns>
    private int disableTerrains()
    {
        int idx = -1;
        for (int i = 0; i < terrainObjs.Length; i++)
        {   
            if (terrainObjs[i] != null && terrainObjs[i].activeSelf)
            {
                idx = i;
                terrainObjs[i].SetActive(false);
            }
        }
        return idx;
    }

    /// <summary>
    /// Cache terrain prefab element idx into terrainObjs
    /// </summary>
    /// <param name="idx">prefab element to instantiate</param>
    private void cacheTerrain(int idx)
    {
        terrainObjs[idx] = Instantiate<GameObject>(terrainPrefabs[idx], Vector3.zero, Quaternion.identity, aOriginGameObject.transform);
        terrainObjs[idx].AddComponent<TrackOnFloor>();

    }

    // MtGraham, Atacama, IRAM 30m, Mauna Kea, Sierra, SPT
    private int[] slideMap = {0, 1, 2, 3, 4, 3, 1, 5, 5, 5, 5};

    /// <summary>
    /// Overly advanced toggle function for the terrain floor tracking
    /// It will Instantiate the prefabs as they are toggled; it will cache the Terrains
    /// </summary>
    /// <param name="slideNumber"> The slide number that toggled the terrain </param>
    private void toggleTerrain(int slideNumber)
    {
        var idx = slideMap[slideNumber];

        int idxActive = disableTerrains();

        bool wasActive = idxActive == idx;

        // Only do anything if terrain wasn't toggled off; 
        // otherwise it was already disabled above
        if (!wasActive)
        {
            // Cache object if it hasn't been instantiated
            if (terrainObjs[idx] == null)
                cacheTerrain(idx);
            GameObject terrain = terrainObjs[idx];
            // TrackOnFloor tracking = terrain.GetComponent<TrackOnFloor>();
            TrackOnFloor tracking = terrain.GetComponent<TrackOnFloor>();
            terrain.SetActive(true);
            tracking.Reset();
        }
    }
}