using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAction : MonoBehaviour
{
    public Transform cardPositionReference;
    private InventoryHandler _playerInventory;
    public int playerID;

    public List<CardPlayingLogic> cardLogic = new List<CardPlayingLogic>();
    public GameObject currentPlayedCard;
    public int PlayedSlot;

    public RoundManager roundManager;

    private void Awake()
    {
        _playerInventory = GetComponent<InventoryHandler>();
        playerID = _playerInventory.PlayerID;

        try
        {
            foreach(var card in GetComponentsInChildren<CardPlayingLogic>())
            {
                cardLogic.Add(card);
            }
        }
        catch
        {}
    }

    private void OnEnable()
    {
        currentPlayedCard = null;
        StartCoroutine(FocusCards(true));
        foreach (var obj in cardLogic)
        {
            obj.enabled = true;
        }

        if (playerID != 0)
        {
            StartCoroutine(NPCCardPick());
        }
    }

    public IEnumerator NPCCardPick()
    {
        var playCardNum = Random.Range(0, _playerInventory.inventorySlots.Length);
        int target = _playerInventory.PlayerID;
            
        // If the chosen card is attack, attack a random player in the game that is not itself
        if (_playerInventory.inventorySlots[playCardNum].Card.CardType == "Attack")
        {
            // Create a list containing all active player IDs, excluding self
            List<int> playerIDs = new List<int>();
            roundManager.playerInventories.ForEach(inventory => playerIDs.Add(inventory.PlayerID));
            playerIDs.Remove(target);

            // Get a random player, if theres only 2 players left it hardcodes the choice to the non-npc player
            int randPlayer = playerIDs[Random.Range(0, playerIDs.Count)];
            target = (playerIDs.Count == 1) ? 0 : randPlayer;
        }

        yield return new WaitForSeconds(Random.Range(2f, 4.3f));
        
        currentPlayedCard = _playerInventory.inventorySlots[playCardNum].CardObject;
        PlayCard(_playerInventory.inventorySlots[playCardNum].CardObject,
            _playerInventory.inventorySlots[playCardNum].Card.CardType,
            target, playCardNum);
    }

    public void PlayCard(GameObject playedCard, string type, int target, int cardSlot)
    {
        currentPlayedCard = playedCard;
        cardLogic.ForEach(obj => obj.enabled = false);
        print(" playCard: " + playedCard.name + type + target);
        PlayedSlot = cardSlot;
        StartCoroutine(PlayCardAnimation(playedCard.transform, cardPositionReference, type, target));
    }

    private IEnumerator PlayCardAnimation(Transform cardObject, Transform cardLocation, string cardType, int targetPlayer)
    {
        print("playing animation");
        for( float i = 0; i < 1.1f; i += 0.1f )
        {
            cardObject.rotation = Quaternion.Slerp(cardObject.rotation, cardLocation.rotation, i);

            cardObject.position = Vector3.Slerp(cardObject.position, cardLocation.position, i);

            yield return new WaitForSeconds(0.02f);
        }
        print("Animation Complete for " + playerID);
        roundManager.playerDecisions[playerID] = new KeyValuePair<string, int>(cardType, targetPlayer);
        
        this.enabled = false;
    }

    public void ReturnCard(Transform cardObject)
    {
        cardObject.rotation = cardPositionReference.rotation;
        cardObject.position = cardPositionReference.position;
    }

    private void OnDisable()
    {
        StartCoroutine(FocusCards(false));
    }
    
    private IEnumerator FocusCards(bool inOut)
    {
        var startPosition = transform.position;
        var movement = (inOut) ? 0.8f : -0.8f;
        var targetPosition = transform.position + new Vector3(0, movement, 0);

        for (float i = 0; i < 1.1; i += 0.15f)
        {
            transform.position = Vector3.Slerp(startPosition, targetPosition, i);
            yield return new WaitForSeconds(0.03f);
        }
    }
}

