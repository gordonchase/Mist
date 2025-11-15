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

    void Start()
    {
        tinframe = 0;
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
        bool isFlaring = player.flaring;
        int tinpercent = player.tinbarpercent;

        int tinbarframe = (int)Math.Round((double)tinpercent / 50);

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

        tinImage.sprite = tinFrames[tinframe];
        tinbarImage.sprite = tinbarFrames[tinbarframe];
    }
}
