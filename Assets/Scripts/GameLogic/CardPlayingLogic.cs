using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CardPlayingLogic : MonoBehaviour
{
    private InventoryHandler _playerInventory;
    private PlayerAction _playerDecision;

    public TextMeshProUGUI cardDescriptor;
    public int cardSlotID;

    public List<CardPlayingLogic> cardLogic = new List<CardPlayingLogic>();
    public List<KillPlayerSelect> players = new List<KillPlayerSelect>();

    static public bool isClicked = false;
    public bool isSwapped = false;

    private void Start()
    {
        _playerInventory = GetComponentInParent<InventoryHandler>();
        _playerDecision = GetComponentInParent<PlayerAction>();
        cardLogic = FindObjectsOfType<CardPlayingLogic>(true).ToList();
        players = FindObjectsOfType<KillPlayerSelect>(true).ToList();
    }

    private void OnMouseOver()
    {
        if (!isClicked && this.enabled && !isSwapped)
        {
            cardDescriptor.text = _playerInventory.inventorySlots[cardSlotID].Card.CardDescription;
            cardDescriptor.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (!isClicked && this.enabled)
        {
            cardDescriptor.text = "";
            cardDescriptor.gameObject.SetActive(false);
        }
    }

    //Passes Given Card To PlayerDecision Handler
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !isClicked && this.enabled && !isSwapped)
        {
            if (_playerInventory.inventorySlots[cardSlotID].Card.CardType == "Attack")
            {
                print(players.Count);
                foreach (KillPlayerSelect player in players)
                {
                    StartCoroutine(SelectAttackee(player));
                }
            } else if (_playerInventory.inventorySlots[cardSlotID].Card.CardType == "Swap")
            {
                isSwapped = true;
                int swapCardID = cardSlotID;

                if (Input.GetMouseButtonDown(0) && isSwapped && _playerInventory.inventorySlots[swapCardID].Card.CardType != "Swap")
                {
                    _playerDecision.PlayCard(_playerInventory.inventorySlots[swapCardID].CardObject,
                                         _playerInventory.inventorySlots[cardSlotID].Card.CardType,
                                         cardSlotID, swapCardID);
                }
            }
            else
                _playerDecision.PlayCard(_playerInventory.inventorySlots[cardSlotID].CardObject,
                                         _playerInventory.inventorySlots[cardSlotID].Card.CardType, 
                                         _playerInventory.PlayerID, cardSlotID);
        }
    }

    public IEnumerator SelectAttackee(KillPlayerSelect player)
    {
        player.enabled = true;
        player.confirmChoice = false;
        player.selectedCard = this;
        player.playerDecision = _playerDecision;
        player.slotID = cardSlotID;

        while (player.confirmChoice == false)
        {
            yield return null;
        }
        
        // Kill all scripts if a response comes in
        players.ForEach(playerScript => playerScript.confirmChoice = true);
        players.ForEach(playerScript => playerScript.enabled = false);
    }

    private void OnDisable()
    {
        cardDescriptor.text = "";
        cardDescriptor.gameObject.SetActive(false);
        isClicked = false;
        isSwapped = false;
    }
}
