using UnityEngine;

public class Load_TelescopeData : MonoBehaviour
{

    //public Transform Telescope_Prefab;
    public float distance;
    public GameObject Pin_object;
    GameObject[] CurrentTelescopes;

    public Material Not_selected;
    public Material Selected;

    Vector3[] positionArray = new[] { new Vector3(5088967.9000f, - 301681.6000f, 3825015.8000f),
        new Vector3(-1828796.200f, -5054406.800f, 3427865.200f),
    new Vector3(-5464523.400f,-2493147.080f,2150611.750f),
    new Vector3(-768713.9637f,-5988541.7982f,2063275.9472f),
    new Vector3(2225061.164f,-5440057.37f,-2481681.15f),
    new Vector3(0.01f,0.01f,-6359609.7f),
    new Vector3(2225039.53f,-5441197.63f,-2479303.36f),
    new Vector3(-5464584.68f,-2493001.17f,2150653.98f)};


    Vector3[] rotationArray = new[] { new Vector3(-50, -8, 0),
        new Vector3(-135, -65, 0),
    new Vector3(-135, -49, 0),
    new Vector3(-160, -90 ,0 ),
    new Vector3(40, 100, 0),
    new Vector3(78, -25, 0),
    new Vector3(24, 29, 0),
    new Vector3(188, 5, 0)};


    string[] Tel_Names = new[] { "PV", "SMT", "SMA", "LMT", "ALMA", "SPT", "APEX", "JCMT" };
    int [] slideMap = new [] {2 , 0, 5, 4, 6, 7, 1, 3};
    // Start is called before the first frame update
    void Start()
    {
        
        Transform TelescopeContainer = transform.Find("TelescopePinContainer");
        int NumberofPins = positionArray.Length;
        CurrentTelescopes = new GameObject[NumberofPins];
    
        for (int i = 0; i < NumberofPins; i++)
        {
            float x = positionArray[i].x;
            float y = positionArray[i].y;
            float z = positionArray[i].z;
            Vector3 Telescope_pin_location = new Vector3(x, z, y).normalized * distance;
            GameObject pinObject = Instantiate(Pin_object, new Vector3(0, 0, 0), Quaternion.identity);
            pinObject.layer = LayerMask.NameToLayer("TelescopePins"); // Telescope Pins layer is 9
            pinObject.transform.parent = TelescopeContainer;
            pinObject.transform.localPosition = Telescope_pin_location;
            PinScript pinScript  =  pinObject.AddComponent<PinScript>();
            
            pinScript.ID = i;
            pinObject.transform.tag = "TelescopePin";

            pinObject.transform.Rotate(rotationArray[i], Space.Self);

            pinObject.name = Tel_Names[i];
            CurrentTelescopes[i] = pinObject;
            
        }
        Select_Telescope(-1);
    }

    public void Select_Telescope (int ID)
    {
        foreach (GameObject i in this.CurrentTelescopes)
        {
            i.GetComponent<Renderer>().material = Not_selected;
        }
        if (ID < 0)
        {
            return;
        }
        else
        {

            CurrentTelescopes[ID].GetComponent<Renderer>().material = Selected;
            GameObject.Find("Canvas").GetComponent<ChangeSlide>().openSlides(slideMap[ID]);

        }
        Debug.Log("Selected!");
    }
    public void SelectFromSlide(int ID) //So we don't have two functions calling each other forever more
    {
        int[] pinMap = { 1, 6, 0, 7, 3, 2, 4, 5 }; //Because the pin and slide lists are in different orders

        foreach (GameObject i in this.CurrentTelescopes)
        {
            i.GetComponent<Renderer>().material = Not_selected;
        }
        if (ID < 0 || ID > 7)
        {
            return;
        }
        else
        {
            int pinID = pinMap[ID];
            CurrentTelescopes[pinID].GetComponent<Renderer>().material = Selected;
        }

    }
}
