using UnityEngine;
using UnityEngine.EventSystems;

public class pewtertoogle : MonoBehaviour, IPointerClickHandler
{
    public PlayerController player;
    public bool flaring;
    public bool burningpewter;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            player.buringpewter = !player.buringpewter;
        }

        if (eventData.button == PointerEventData.InputButton.Right){
        player.flaringpewter = !player.flaringpewter;
        }
    }
}
