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

    public bool isAnyActive = false;
    /// <summary>
    /// index of terrain GameObject that is currently toggledOn. 
    /// </summary>
    public int active_index = -1;
    public int activeIndex {
        get
        {
            return active_index;
        }
        private set 
        {
            active_index = value;
            isAnyActive = value >= 0;
        }
    }

    private void OnEnable() {
        ChangeSlide.toggleTerrainAction += toggleTerrain;
        ChangeSlide.toggleTerrainOnChange += toggleTerrainOnChange;
    }

    private void OnDisable() {
        ChangeSlide.toggleTerrainAction -= toggleTerrain;
        ChangeSlide.toggleTerrainOnChange -= toggleTerrainOnChange;
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
    private void disableTerrain()
    {   
        // if there is active terrain, disable it
        if (isAnyActive)
        {
            terrainObjs[activeIndex].SetActive(false);
            activeIndex = -1;
        }
    }

    /// <summary>
    /// Cache terrain prefab element idx into terrainObjs
    /// </summary>
    /// <param name="idx">prefab element to instantiate</param>
    private void cacheTerrain(int idx)
    {
        terrainObjs[idx] = Instantiate<GameObject>(terrainPrefabs[idx], aOriginGameObject.transform);
        terrainObjs[idx].AddComponent<TrackOnFloor>();

    }

    /// <summary>
    /// Function that will take activate the given terrain. It will deactive any other terrains.
    /// </summary>
    /// <param name="idx"></param>
    private void activateTerrain(int idx)
    {
        disableTerrain();
        // Cache object if it hasn't been instantiated
        if (terrainObjs[idx] == null)
            cacheTerrain(idx);
        GameObject terrain = terrainObjs[idx];
        TrackOnFloor tracking = terrain.GetComponent<TrackOnFloor>();
        terrain.SetActive(true);
        tracking.Reset();
        activeIndex = idx;
    }

    // ELEMENTS:        0         1        2           3       4      5       6
    // TERRAINLIST: MtGraham, Atacama, IRAM 30m, Mauna Kea, Sierra, SPT
    // SLIDELIST:   MtGraham, Atacama, IRAM 30m, Mauna Kea, Sierra, Atacama, SPT
    private int[] slideMap = { 0, 1, 2, 3, 4, 3, 1, 5 };

    /// <summary>
    /// Overly advanced toggle function for the terrain floor tracking.
    /// Toggle ON terrain only if there were no active terrains, otherwise disable current active.
    /// It will Instantiate the prefabs as they are toggled; it will cache the Terrains
    /// </summary>
    /// <param name="slideNumber"> The slide number that toggled the terrain </param>
    private void toggleTerrain(int slideNumber)
    {
        // Only do anything if terrain wasn't toggled off; 
        if (isAnyActive)
        {
            disableTerrain();
        }
        else
        {
            var idx = slideMap[slideNumber];
            activateTerrain(idx);
        }
    }

    /// <summary>
    /// While this function is similar to the one above. The big difference is that we want to toggle ON the terrain 
    /// for <paramref name="slideNumber"/> only if there was any active terrains.
    /// </summary>
    /// <param name="slideNumber"></param>
    private void toggleTerrainOnChange(int slideNumber)
    {
        if (isAnyActive)
        {
            var idx = slideMap[slideNumber];
            activateTerrain(idx);
        }
    }
}