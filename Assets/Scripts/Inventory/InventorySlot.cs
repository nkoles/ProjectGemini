using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    //Card Type
    [CanBeNull] private CardBase? card;
    //Object Linked to Slot
    [CanBeNull] private GameObject? cardGameObject;
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
                    cardRenderer.enabled = true;
                    cardRenderer.material = value.CardMaterial;
                }

            } 
            else
            {
                card = null;

                if( cardGameObject != null )
                    cardRenderer.enabled = false;
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
                cardRenderer = cardGameObject.GetComponent<Renderer>();

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
