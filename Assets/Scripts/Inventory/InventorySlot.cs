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

    //[CanBeNull] public KeyValuePair<Vector3, Quaternion> defaultPosRot;
    public Tuple<Vector3, Quaternion, Quaternion> defaulPosRotLocalRot;

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
                {
                    cardGameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
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

                defaulPosRotLocalRot = new Tuple<Vector3, Quaternion, Quaternion>
                                        (cardGameObject.transform.localPosition, 
                                        cardGameObject.transform.localRotation,
                                        cardGameObject.transform.GetChild(0).rotation);

                if (card != null)
                {
                    defaulPosRotLocalRot = new Tuple<Vector3, Quaternion, Quaternion>
                                        (cardGameObject.transform.localPosition,
                                        cardGameObject.transform.localRotation,
                                        cardGameObject.transform.GetChild(0).rotation);

                    cardRenderer.material = card.CardMaterial;
                }
            }
            else
            {
                cardGameObject = null;
            }
        }
    }

    public void ResetDefaultTransform()
    {
        cardGameObject.transform.localPosition = defaulPosRotLocalRot.Item1;
        cardGameObject.transform.localRotation = defaulPosRotLocalRot.Item2;
        cardGameObject.transform.GetChild(0).rotation = defaulPosRotLocalRot.Item3;

        print("Reset Position");
    }
}
