using UnityEngine;
using UnityEngine.UI;

public class uicode : MonoBehaviour
{
    public PlayerController player;

    public Image tinImage;
    public Sprite[] tinFrames;
    public int currentframe;

    void Start()
    {
        currentframe = 0;
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

        if (isBurningtin)
        {
            if (isFlaring)
            {
                currentframe = 2;
            }
            else
            {
                currentframe = 1;
            }
        }
        else
        {
            currentframe = 0;
        }

        tinImage.sprite = tinFrames[currentframe];
    }
}
