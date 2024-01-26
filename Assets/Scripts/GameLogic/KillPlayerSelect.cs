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
    public int slotID;

    public TextMeshProUGUI selectText;

    public bool confirmChoice = true;
    private void OnMouseOver()
    {
        if (!confirmChoice && this.enabled)
        {
            selectText.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        selectText.enabled = false;
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0) && this.enabled)
        {
            playerDecision.PlayCard(playerInventory.inventorySlots[selectedCard.cardSlotID].CardObject,
                                    playerInventory.inventorySlots[selectedCard.cardSlotID].Card.CardType,
                                    playerID, slotID);

            confirmChoice = true;
            enabled = false;
        }
    }
    

    private void OnDisable()
    {
        selectText.enabled = false;
        foreach(KillPlayerSelect obj in FindObjectsOfType<KillPlayerSelect>())
        {
            obj.enabled = false;
        }
    }
}
