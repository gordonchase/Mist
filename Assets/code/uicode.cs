using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class uicode : MonoBehaviour
{
    public PlayerController player;

    public Image tinImage;
    public Sprite[] tinFrames;
    public int tinframe;

    public Image tinbarImage;
    public Sprite[] tinbarFrames;


    public Image pewterImage;
    public Sprite[] pewterFrames;
    public int pewterframe;

    public Image pewterbarImage;
    public Sprite[] pewterbarFrames;


    public Image steelImage;
    public Sprite[] steelFrames;
    public int steelframe;

    public Image steelbarImage;
    public Sprite[] steelbarFrames;


    public Image ironImage;
    public Sprite[] ironFrames;
    public int ironframe;

    public Image ironbarImage;
    public Sprite[] ironbarFrames;

        

    public Sprite[] basichelthbarFrames;
    public int basichelthframe;
    public Image basichelthbarImage;

    public Sprite[] pewterhelthbarFrames;
    public int pewterhelthframe;
    public Image pewterhelthbarImage;
    public Sprite[] flaringhelthbarFrames;
    public int flaringhelthframe;
    public Image flaringhelthbarImage;

    public TMP_Text boxings;

    void Start()
    {
        UpdateFrame();
    }

    void Update()
    {
        UpdateFrame(); // Update every frame so UI reacts to player state
    }

    void UpdateFrame()
    {
        // Get current state from player at runtime
        bool isBurningtin = player.buringtin;
        bool isBurningiron = player.buringiron;
        bool isBurningpewter = player.buringpewter;
        bool isBurningsteel = player.buringsteel;        
        bool isFlaring = player.flaring;
        int tinpercent = player.tinbarpercent;
        int pewterpercent = player.pewterbarpercent;
        int steelpercent = player.steelbarpercent;
        int ironpercent = player.ironbarpercent;
        int curenthelth = player.helth;
        
        int basichelthframe = Mathf.RoundToInt(curenthelth/3.333f);
        int pewterhelthframe = Mathf.RoundToInt((curenthelth+50)/3.125f);

        boxings.text = player.numboxings.ToString();


        if (basichelthframe>30)
        {
            basichelthframe=30;
        }
        if (basichelthframe<0)
        {
            basichelthframe=0;
        }
        if (pewterhelthframe>16)
        {
            pewterhelthframe=16;
        }
        if (pewterhelthframe<0)
        {
           pewterhelthframe=0;
        }
        if (!isFlaring)
        {
            flaringhelthframe=0;
        }
        if (isBurningpewter && isFlaring && curenthelth > -25)
        {
            flaringhelthframe=8;
        }
        if (isBurningpewter && isFlaring && curenthelth<=-25)
        {
        int flareinghelthframe = Mathf.RoundToInt((curenthelth+75)/3.125f);
        }




        int tinbarframe = Mathf.RoundToInt(tinpercent / 50f);

        if (isBurningtin)
        {
            if (isFlaring)
            {
                tinframe = 2;
            }
            else
            {
                tinframe = 1;
            }
        }
        else
        {
            tinframe = 0;
        }

        int pewterbarframe = Mathf.RoundToInt(pewterpercent / 10f);

        if (isBurningpewter)
        {
            if (isFlaring)
            {
                pewterframe = 2;
            }
            else
            {
                pewterframe = 1;
            }
        }
        else
        {
            pewterframe = 0;
        }

        int steelbarframe = Mathf.RoundToInt(steelpercent / 20f);

        if (isBurningsteel)
        {
            if (isFlaring)
            {
                steelframe = 2;
            }
            else
            {
                steelframe = 1;
            }
        }
        else
        {
            steelframe = 0;
        }


        int ironbarframe = Mathf.RoundToInt(ironpercent / 20f);

        if (isBurningiron)
        {
            if (isFlaring)
            {
                ironframe = 2;
            }
            else
            {
                ironframe = 1;
            }
        }
        else
        {
            ironframe = 0;
        }

        tinImage.sprite = tinFrames[tinframe];
        tinbarImage.sprite = tinbarFrames[tinbarframe];
        
        pewterImage.sprite = pewterFrames[pewterframe];
        pewterbarImage.sprite = pewterbarFrames[pewterbarframe];

        steelImage.sprite = steelFrames[steelframe];
        steelbarImage.sprite = steelbarFrames[steelbarframe];

        ironImage.sprite = ironFrames[ironframe];
        ironbarImage.sprite = ironbarFrames[ironbarframe];

        basichelthbarImage.sprite = basichelthbarFrames[basichelthframe];
        pewterhelthbarImage.sprite = pewterhelthbarFrames[pewterhelthframe];
        flaringhelthbarImage.sprite = flaringhelthbarFrames[flaringhelthframe];
    }
}
