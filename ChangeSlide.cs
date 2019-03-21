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
    public Sprite[] imageArray = new Sprite[11];
    public string namePath;
    public string locPath;
    public int slideNumber; //Starts at 0
    public int totalSlides;
    public bool slidesOpen; 
    public Text telescopeName;
    public Text telescopeLocation;
    public Image telescopeImage;
    public Image holoSlide;

    public Sprite space1;
    public Sprite space2;

    public Sprite xSprite, downSprite;
    public Button NextButton, PrevButton, OpenCloseButton;
    
    // Start is called before the first frame update
    void Start()
    {
        slidesOpen = true;
        totalSlides = 11;
        slideNumber = 0;

        initArrays();

        assignText(slideNumber);

        NextButton.onClick.AddListener(Next);
        PrevButton.onClick.AddListener(Prev);
        OpenCloseButton.onClick.AddListener(toggleSlides);
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
    void openSlides(int ID)
    {
        //Make sure we're on the right slide
        slideNumber = ID;
        assignText(slideNumber);

        //One vector for almost all transformations
        Vector3 openVector = new Vector3(-2000, -900, 0);
        OpenCloseButton.image.sprite = xSprite; //Change button image

        //Move everything
        holoSlide.transform.Translate(openVector);
        telescopeName.transform.Translate(openVector);
        telescopeLocation.transform.Translate(openVector);
        telescopeImage.transform.Translate(openVector);
        NextButton.transform.Translate(openVector);
        PrevButton.transform.Translate(openVector);
        OpenCloseButton.transform.Translate(new Vector3(-100,-100,0)); //Button moves relative to slide
    }
    void closeSlides()
    {
        //One vector for most transformations
        Vector3 closeVector = new Vector3(2000, 900, 0);
        OpenCloseButton.image.sprite = downSprite; //Change button image

        //Move everything
        holoSlide.transform.Translate(closeVector);
        telescopeName.transform.Translate(closeVector);
        telescopeLocation.transform.Translate(closeVector);
        telescopeImage.transform.Translate(closeVector);
        NextButton.transform.Translate(closeVector);
        PrevButton.transform.Translate(closeVector);
        OpenCloseButton.transform.Translate(new Vector3(100,100, 0)); //Button moves relative to slide
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
        telescopeImage.sprite = imageArray[ID]; //This works
    }
    void initArrays()
    {
        namePath = "Names";
        locPath = "Locations";
        //space1 = Resources.Load<Sprite>("Space1") as Sprite; //This does not
        TextAsset namesList = Resources.Load<TextAsset>(namePath) as TextAsset;
        TextAsset locsList = Resources.Load<TextAsset>(locPath) as TextAsset;
        char[] charArray = { '\n' };
        nameArray = namesList.text.Split(charArray, 11);
        locArray = locsList.text.Split(charArray, 11);
    }
}
