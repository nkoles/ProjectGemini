using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillPlayerSelect : MonoBehaviour
{
    public CardPlayingLogic selectedCard;
    public PlayerAction playerDecision;
    public InventoryHandler playerInventory;

    public int playerID;

    public TextMeshProUGUI selectText;

    private void OnMouseOver()
    {
        selectText.enabled = true;
    }

    private void OnMouseExit()
    {
        selectText.enabled = false;
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            playerDecision.PlayCard(playerInventory.inventorySlots[selectedCard.cardSlotID].CardObject, playerInventory.inventorySlots[selectedCard.cardSlotID].Card.CardType, playerID);

        }
    }

    private void OnDisable()
    {
        foreach(KillPlayerSelect obj in FindObjectsOfType<KillPlayerSelect>())
        {
            obj.enabled = false;
        }
    }
}
