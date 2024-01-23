using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private int roundCount;

    public List<InventoryHandler> playerInventories = new List<InventoryHandler>();

    [Header ("Dealing Animation")]
    public Transform deckRefPoint;
    public GameObject cardPrefab;
    public enum eRoundState
    {
        Deal,
        Play,
        Outcome
    }

    private void Start()
    {
        StartCoroutine(DealCards());
    }

    private IEnumerator DealCards()
    {
        Debug.Log("Started Routine");

        for(int i = 0;  i < playerInventories.Count; ++i)
        {
            for (int j = 0; playerInventories[i].inventorySlots.Length > j; ++j)
            {
                if (playerInventories[i].inventorySlots[j].Card == null)
                {

                    Debug.Log("Entered");

                    StartCoroutine(CardDealingAnimation(i, j));

                    yield return new WaitForSeconds(0.75f);
                }
            }
        }

        Debug.Log("Finished Routine");
    }

    private IEnumerator CardDealingAnimation(int inventoryID, int inventorySlotID)
    {
        GameObject dealedCard = Instantiate(cardPrefab, deckRefPoint);

        Vector3 dealedCardPosition = dealedCard.transform.position;
        Vector3 targetCardPosition = playerInventories[inventoryID].inventorySlots[inventorySlotID].CardObject.transform.position;

        Quaternion dealedCardRotation = dealedCard.transform.rotation;
        Quaternion targetCardRotation = playerInventories[inventoryID].inventorySlots[inventorySlotID].CardObject.transform.rotation;

        Debug.Log(targetCardPosition);
        Debug.Log(dealedCardPosition);

        for (float j = 0f; j < 1.1f; j += 0.1f)
        {
            dealedCard.transform.rotation = Quaternion.Slerp(dealedCardRotation, targetCardRotation, j);
            yield return new WaitForSeconds(0.02f);
        }

        for (float i = 0f; i < 1.1f; i+=0.05f)
        {
            dealedCard.transform.position = Vector3.Slerp(dealedCardPosition, targetCardPosition, i);

            yield return new WaitForSeconds(0.02f);
        }

        CardBase dealedCardType;
        int randomTypeGenerator = Random.Range(0, 101);

        switch (randomTypeGenerator)
        {
            case < 34:
                dealedCardType = CardTypes.CardAttack;
                break;
            case < 68:
                dealedCardType = CardTypes.CardBlock;
                break;
            case < 86:
                dealedCardType = CardTypes.CardSwap;
                break;
            case >= 86:
                dealedCardType = CardTypes.CardBean;
                break;
        }

        playerInventories[inventoryID].AddCard(inventorySlotID, dealedCardType);

        Debug.Log(playerInventories[inventoryID].inventorySlots[inventorySlotID].Card.CardType);

        Destroy(dealedCard);

        Debug.Log("Card Dealed");
    }
}
