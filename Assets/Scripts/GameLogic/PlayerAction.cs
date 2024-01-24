using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Transform cardPositionReference;
    private InventoryHandler _playerInventory;
    public int playerID;

    public List<CardPlayingLogic> cardLogic = new List<CardPlayingLogic>();

    public RoundManager roundManager;

    [CanBeNull] public string? cardPlayedType;
    [CanBeNull] public int? cardTarget;

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
        foreach (var obj in cardLogic)
        {
            obj.enabled = true;
        }

        if (playerID != 0)
        {
            PlayCard(_playerInventory.inventorySlots[0].CardObject, _playerInventory.inventorySlots[0].Card.CardType, _playerInventory.PlayerID);
        }
    }

    private void OnDisable()
    {
        cardPlayedType = null;
        cardTarget = null;
    }

    public void PlayCard(GameObject playedCard, string type, int target)
    {
        StartCoroutine(PlayCardAnimation(playedCard.transform, cardPositionReference, type, target));
    }

    private IEnumerator PlayCardAnimation(Transform cardObject, Transform cardLocation, string cardType, int targetPlayer)
    {
        while(cardObject.position != cardLocation.position || cardObject.rotation != cardLocation.rotation)
        {
            cardObject.rotation = Quaternion.Slerp(cardObject.rotation, cardLocation.rotation, Time.deltaTime * 20);

            cardObject.position = Vector3.Slerp(cardObject.position, cardLocation.position, Time.deltaTime * 10);

            yield return null;
        }

        roundManager.playerDecisions[playerID] = new KeyValuePair<string, int>(cardType, targetPlayer);

        this.enabled = false;
    }
}
