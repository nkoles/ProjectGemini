using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    //Card Type
    [CanBeNull] private CardBase? card;
    //Object Linked to Slot
    [CanBeNull] public GameObject? cardGameObject;
    [CanBeNull] private Renderer cardRenderer;

    public InventorySlot(CardBase? cardType, GameObject cardObject)
    {
        Card = cardType;
        CardObject = cardObject;
    }

    public InventorySlot()
    {
        Card = null;
        CardObject = null;
    }
    

    public CardBase Card
    {
        get { return card; }

        set {

            if( value != null)
            {
                card = value;

                if(cardGameObject != null)
                {
                    cardGameObject.transform.GetChild(0).gameObject.SetActive(true);
                    cardRenderer.material = value.CardMaterial;
                }

            } 
            else
            {
                print("Card Cleared!");
                card = null;

                if( cardGameObject != null )
                    cardGameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

        }
    }

    public GameObject CardObject
    {
        get { return cardGameObject; }

        set
        {
            if(value != null)
            {
                cardGameObject = value;
                cardGameObject.transform.GetChild(0).gameObject.SetActive(false);
                cardRenderer = cardGameObject.transform.GetChild(0).GetComponentInChildren<Renderer>(true);

                if(card != null)
                {
                    cardRenderer.material = card.CardMaterial;
                }
            }
            else
            {
                cardGameObject = null;
            }
        }

    }
}
