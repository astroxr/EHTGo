using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ChangeSlide : MonoBehaviour
{
    public string[] nameArray = new string[8];
    public string[] locArray = new string[8];
    public string[] descrArray = new string[8];
    public Sprite[] imageArray = new Sprite[8];
    public string namePath;
    public string locPath;
    public string descrPath;
    public int slideNumber; //Starts at 0
    public int totalSlides;
    public bool slidesOpen; 
    public Text telescopeName;
    public Text telescopeLocation;
    public Text telescopeDescription;
    public Image telescopeImage;
    public Image holoSlide;

    public Sprite xSprite, downSprite;
    public Button NextButton, PrevButton, CloseButton, OpenButton, ResetEarth, ToggleTerrain;

    public Vector3 transformVector = new Vector3(4 * Screen.width, 4 * Screen.height, 0);

    public TextAsset namesList;
    public TextAsset locsList;
    public TextAsset descrList;
    GameObject Earth_object;
    public static event Action<int> toggleTerrainAction = delegate {};


// Start is called before the first frame update
void Start()
    {
        slidesOpen = true;
        totalSlides = 8;
        slideNumber = 0;

        namePath = "Names";
        locPath = "Locations";
        descrPath = "Descriptions";
        namesList = Resources.Load<TextAsset>(namePath) as TextAsset;
        locsList = Resources.Load<TextAsset>(locPath) as TextAsset;
        descrList = Resources.Load<TextAsset>(descrPath) as TextAsset;
        Debug.Log(locsList);
        Debug.Log(descrList);
        initArrays();

        assignText(slideNumber);

        OpenButton.transform.Translate(transformVector); //Start open button off-screen

        NextButton.onClick.AddListener(Next);
        PrevButton.onClick.AddListener(Prev);
        CloseButton.onClick.AddListener(toggleSlides);
        OpenButton.onClick.AddListener(toggleSlides);
        ResetEarth.onClick.AddListener(Reset_Earth);
        ToggleTerrain.onClick.AddListener(() => 
        { 
            toggleTerrainAction(slideNumber); // Use lambda function to use slideNumber as parameter.
        });
        
        toggleSlides();
    }

    //Next slash right button
    void Next()
    {
        if(slideNumber < totalSlides - 1)
        {
            slideNumber++;
        } else
        {
            slideNumber = 0;
        }
        selectPin(slideNumber); //Select new pin
        assignText(slideNumber);
    }
    //Previous slash left button
    void Prev()
    {
        if(slideNumber == 0)
        {
            slideNumber = totalSlides - 1;
        } else
        {
            slideNumber--;
        }
        selectPin(slideNumber); //Select new pin
        assignText(slideNumber);
    }
    public void openSlides(int ID)
    {
        //Make sure we're on the right slide
        slideNumber = ID;
        assignText(slideNumber);
        selectPin(slideNumber); //Select new pin

        if (!slidesOpen) //Just in case you select a new pin while everything is open, so it doesn't drift off the screen
        {
            //One vector for almost all transformations
            Vector3 openVector = -transformVector;

            //Move everything
            holoSlide.transform.Translate(openVector);
            telescopeName.transform.Translate(openVector);
            telescopeLocation.transform.Translate(openVector);
            telescopeDescription.transform.Translate(openVector);
            telescopeImage.transform.Translate(openVector);
            NextButton.transform.Translate(openVector);
            PrevButton.transform.Translate(openVector);
            CloseButton.transform.Translate(openVector);
            ResetEarth.transform.Translate(openVector);
            ToggleTerrain.transform.Translate(openVector);
            OpenButton.transform.Translate(-openVector); //Move other button off screen
            slidesOpen = true;
        }
    }
    void closeSlides()
    {
        //One vector for most transformations
        Vector3 closeVector = transformVector;

        //Move everything
        holoSlide.transform.Translate(closeVector);
        telescopeName.transform.Translate(closeVector);
        telescopeLocation.transform.Translate(closeVector);
        telescopeDescription.transform.Translate(closeVector);
        telescopeImage.transform.Translate(closeVector);
        NextButton.transform.Translate(closeVector);
        PrevButton.transform.Translate(closeVector);
        CloseButton.transform.Translate(closeVector);
        ResetEarth.transform.Translate(closeVector);
        ToggleTerrain.transform.Translate(closeVector);
        OpenButton.transform.Translate(-closeVector); //Move other button onto screen
    } 
    void toggleSlides()
    {
        if(slidesOpen)
        {
            closeSlides();
            slidesOpen = false;
        } else
        {
            openSlides(slideNumber);
            slidesOpen = true;
        }
    }
    void assignText(int ID)
    {
        telescopeName.text =  nameArray[ID];
        telescopeLocation.text = locArray[ID];
        telescopeDescription.text = descrArray[ID];
        telescopeImage.sprite = imageArray[ID]; //This works
    }
    void initArrays()
    {
        char[] charArray = { '\n' };
        nameArray = namesList.text.Split(charArray, 8);
        locArray = locsList.text.Split(charArray, 8);
        descrArray = descrList.text.Split(charArray, 8);
    }
    void selectPin(int ID)
    {
        var earth = GameObject.Find("Earth_small Variant");
        if (earth != null)
        {
            earth.GetComponent<Load_TelescopeData>().SelectFromSlide(ID);
        }
    }

    private void Reset_Earth(){
        Debug.Log("Resetting the earth rotation!");
        Earth_object = GameObject.Find("Earth_small Variant");
        if (Earth_object == null )
        {
            Debug.Log("404: Earth Not found");
            return;
        }
        Earth_object.transform.eulerAngles = new Vector3(0,0,0);
    }
}
