using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Load_TelescopeData : MonoBehaviour
{

    //public Transform Telescope_Prefab;
    public float distance;
    public float scale_;

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

    // Start is called before the first frame update
    void Start()
    {
        
        //StreamReader inp_stm = new StreamReader(".\\Assets\\Earth\\Materials\\Materials\\Telescope_Coordinates\\Data.txt");
        Transform TelescopeContainer = transform.Find("TelescopePinContainer");
        //while (!inp_stm.EndOfStream)
        int array_index = 0;
        int NumberofPins = 8;
        CurrentTelescopes = new GameObject[NumberofPins];
        while (array_index <= NumberofPins - 1)
        {
            //string inp_ln = inp_stm.ReadLine();
            //string[] splited = inp_ln.Split(' ');


            //float x = float.Parse(splited[1]);
            //float y = float.Parse(splited[2]);
            // float z = float.Parse(splited[3]);
            float x = positionArray[array_index].x;
            float y = positionArray[array_index].y;
            float z = positionArray[array_index].z;
            Vector3 Telescope_pin_location = new Vector3(x, z, y);
            Telescope_pin_location = Telescope_pin_location.normalized*distance;

            //Instantiate(Telescope_Prefab, Telescope_pin_location, new Quaternion(0, 0, 0, 0),);
            GameObject sphere = Instantiate(Pin_object, new Vector3(0, 0, 0), Quaternion.identity);
            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.layer = LayerMask.NameToLayer("TelescopePins"); // Telescope Pins layer is 9
            sphere.transform.parent = TelescopeContainer;
            sphere.transform.localPosition = Telescope_pin_location;
            PinScript p  =  sphere.AddComponent<PinScript>();
            BoxCollider sc = sphere.AddComponent<BoxCollider>();
            
            p.ID = array_index;
            sphere.transform.tag = "TelescopePin";


            //Vector3 normalVector = sphere.transform.localPosition;
            //float size = normalVector.magnitude;

            sphere.transform.Rotate(rotationArray[array_index], Space.Self);



            //sphere.transform.localScale = new Vector3(1, 1, 1) * scale_;
            //sphere.name = splited[0];
            sphere.name = Tel_Names[array_index];
            CurrentTelescopes[array_index] = sphere;
            array_index++;
        }

        this.Select_Telescope(-1);
        //inp_stm.Close();

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
