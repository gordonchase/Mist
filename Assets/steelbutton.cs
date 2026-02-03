using UnityEngine;
using UnityEngine.EventSystems;

public class steeltoogle : MonoBehaviour, IPointerClickHandler
{
    public PlayerController player;
    public bool flaring;
    public bool burningsteel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            player.buringsteel = !player.buringsteel;
        }

        if (eventData.button == PointerEventData.InputButton.Right){
        player.flaring = !player.flaring;

        }
    }
}