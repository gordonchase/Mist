using UnityEngine;
using UnityEngine.EventSystems;

public class tintoogle : MonoBehaviour, IPointerClickHandler
{
    public PlayerController player;
    public bool flaring;
    public bool burningtin;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            player.buringtin = !player.buringtin;
        }

        if (eventData.button == PointerEventData.InputButton.Right){
        player.flaringtin = !player.flaringtin;
        }
    }
}