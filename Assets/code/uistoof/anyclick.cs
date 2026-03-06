using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq; 

public class anyclick : MonoBehaviour, IPointerClickHandler
{
    public PlayerController player;

    public void OnPointerClick(PointerEventData eventData)
    {

    if (player.spacetoogle){
        if (eventData.button == PointerEventData.InputButton.Left){
            if (player.pushmetals.Contains(player.metalsinarea[player.highlightnumthing12]))
                {
                player.pushmetals.Remove(player.metalsinarea[player.highlightnumthing12]);
                }
                else
                {
                player.pushmetals.Add(player.metalsinarea[player.highlightnumthing12]);
                }
                Debug.Log("push = " + player.pushmetals);
        }

        if (eventData.button == PointerEventData.InputButton.Right){
            if (player.pullmetals.Contains(player.metalsinarea[player.highlightnumthing12]))
                {
                player.pullmetals.Remove(player.metalsinarea[player.highlightnumthing12]);
                }
                else
                {
                player.pullmetals.Add(player.metalsinarea[player.highlightnumthing12]);
                }
                Debug.Log("pull = " + player.pullmetals);
        }

    }
    }
}
