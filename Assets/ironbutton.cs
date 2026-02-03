using UnityEngine;
using UnityEngine.EventSystems;

public class irontoogle : MonoBehaviour, IPointerClickHandler
{
    public PlayerController player;
    public bool flaring;
    public bool burningiron;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            player.buringiron = !player.buringiron;

        }

        if (eventData.button == PointerEventData.InputButton.Right){
        player.flaring = !player.flaring;
        }
    }
}
