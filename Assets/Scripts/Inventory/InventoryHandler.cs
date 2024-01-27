using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    //Inventory ID per Player: 0 = Player, 1 = AI 1, 2 = AI 2, 3 = AI = 3
    [SerializeField] private int _inventoryID;
    public int PlayerID { get; private set; }

    public GameObject[] cards = new GameObject[3];
    public InventorySlot[] inventorySlots = new InventorySlot[3];

    private void Awake()
    {
        PlayerID = _inventoryID;
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i] = this.AddComponent<InventorySlot>();
            inventorySlots[i].CardObject = cards[i];

            Debug.Log(inventorySlots[i].Card);
        }
    }

    public bool CheckForBean()
    {
        try
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.Card.CardType == "Bean")
                    return true;
            }

            return false;
        }
        catch
        {
            Debug.LogWarning("Checking for bean when it does not exist lol.");
            return false;
        }
    }

    //Adds Card To Designated Slot
    //0: Attack, 1: Block, 2: Swap, 3: Bean
    public void AddCard(int slotID, CardBase cardType)
    {
        inventorySlots[slotID].Card = cardType;
    }

    public void RemoveCard(int slotID)
    {
        try
        {
            inventorySlots[slotID].ResetDefaultTransform();
            inventorySlots[slotID].Card = null;

            print(inventorySlots[slotID].CardObject.transform.localRotation);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            
            inventorySlots[0].ResetDefaultTransform();
            inventorySlots[0].Card = null;

            print(inventorySlots[0].CardObject.transform.localRotation);
        }
        

       
    }

    public void RemoveAllCards()
    {
        foreach (var slot in inventorySlots)
        {
            slot.Card = null;
        }
    }

}
