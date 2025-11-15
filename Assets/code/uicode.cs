using UnityEngine;
using UnityEngine.UI;
using System;

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

    void Start()
    {
        tinframe = 0;
        pewterframe=0;
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
        bool isBurningpewter = player.buringpewter;
        bool isFlaring = player.flaring;
        int tinpercent = player.tinbarpercent;
        int pewterpercent = player.pewterbarpercent;

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

        tinImage.sprite = tinFrames[tinframe];
        tinbarImage.sprite = tinbarFrames[tinbarframe];
        
        pewterImage.sprite = pewterFrames[pewterframe];
        pewterbarImage.sprite = pewterbarFrames[pewterbarframe];
    }
}
