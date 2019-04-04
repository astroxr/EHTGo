using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ChangeSlide : MonoBehaviour
{
    public string teleName;
    public string location;
    public string[] nameArray = new string[11];
    public string[] locArray = new string[11];
    public string[] descrArray = new string[11];
    public Sprite[] imageArray = new Sprite[11];
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

    public Sprite space1;
    public Sprite space2;

    public Sprite xSprite, downSprite;
    public Button NextButton, PrevButton, CloseButton, OpenButton;

    public Vector3 transformVector = new Vector3(5000, 2300, 0);

    public TextAsset namesList;
    public TextAsset locsList;
    public TextAsset descrList;

// Start is called before the first frame update
void Start()
    {
        slidesOpen = true;
        totalSlides = 11;
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
        assignText(slideNumber);
    }
    public void openSlides(int ID)
    {
        //Make sure we're on the right slide
        slideNumber = ID;
        assignText(slideNumber);

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
        nameArray = namesList.text.Split(charArray, 11);
        locArray = locsList.text.Split(charArray, 11);
        Debug.Log("---------");
        Debug.Log(descrList);
        descrArray = descrList.text.Split(charArray, 11);
    }
}
