using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    //Inventory ID per Player: 0 = Player, 1 = AI 1, 2 = AI 2, 3 = AI = 3
    [SerializeField] private int _inventoryID;

    public int PlayerID
    {
        get { return _inventoryID; }
    }


    public GameObject[] cards = new GameObject[3];
    public InventorySlot[] inventorySlots = new InventorySlot[3];

    private void Awake()
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            inventorySlots[i] = this.AddComponent<InventorySlot>();
            inventorySlots[i].CardObject = cards[i];

            Debug.Log(inventorySlots[i].Card);
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
        inventorySlots[slotID].Card = null;
    }
}
