using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBoxMenuInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    private Button Reset_btn;
    void Start()
    {
        GameObject.Find("Rest_btn").GetComponent<Button>();
        Reset_btn.onClick.AddListener(Reset_Earth);
    }

    private void Reset_Earth(){
        GameObject Earth_object;
        Earth_object = GameObject.Find("Earth_small Variant");
        if (Earth_object == null )
        {
            Debug.Log("Earth Not found");
            return;}
        Earth_object.transform.eulerAngles = new Vector3(0,0,0
);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
