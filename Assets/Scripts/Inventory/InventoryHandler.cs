using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    //Inventory ID per Player: 0 = Player, 1 = AI 1, 2 = AI 2, 3 = AI = 3
    [SerializeField] private int _inventoryID;

    public GameObject[] cards;
    private InventorySlot[] _inventorySlots = new InventorySlot[3];

    private void Awake()
    {
        for(int i =0; i < _inventorySlots.Length; ++i)
        {
            _inventorySlots[i].cardGameObject = cards[i];
        }
    }

    public void AddCard(int slotID, string cardName)
    {

    }

    public void RemoveCard(int slotID)
    {

    }

    public void SelectCard(int slotID)
    {

    }

    public void PlayCard(int slotID)
    {

    }
}
