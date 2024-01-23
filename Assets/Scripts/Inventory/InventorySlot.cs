using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    //Card Type
    [CanBeNull] public CardTypes? card;
    //Object Linked to Slot
    [CanBeNull] public GameObject? cardGameObject;

    public InventorySlot(CardTypes? cardType, GameObject cardObject)
    {
        card = cardType;
        cardGameObject = cardObject;
    }

    public InventorySlot()
    {
        card = null;
        cardGameObject = null;
    }

    private void Update()
    {
        if(card == null)
        {
            cardGameObject.SetActive(false);
        } else
        {

        }
    }
}
