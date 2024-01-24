using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private int roundCount;

    public List<GameObject> aiPlayerModels = new List<GameObject>();
    public List<InventoryHandler> playerInventories = new List<InventoryHandler>();
    public List<PlayerAction> playerDecisionHandler = new List<PlayerAction>();

    public List<KeyValuePair<string, int>> playerDecisions = new List<KeyValuePair<string, int>>();

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
        for(int i = 0; i < 4; i++)
        {
            playerDecisions.Add(new KeyValuePair<string, int>(null, 0));
        }

        StartCoroutine(DealPhase());
    }

    #region Card Dealing Phase
    private IEnumerator DealPhase()
    {
        Debug.Log("Started Routine");

        for(int i = 0;  i < playerInventories.Count; ++i)
        {
            for (int j = 0; playerInventories[i].inventorySlots.Length > j; ++j)
            {
                if (playerInventories[i].inventorySlots[j].Card == null)
                {

                    Debug.Log("Entered");

                    StartCoroutine(CardDealing(i, j));

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(AICheckCards(45f, 1f, 3f));
    }

    private IEnumerator CardDealing(int inventoryID, int inventorySlotID)
    {
        GameObject dealedCard = Instantiate(cardPrefab, deckRefPoint);

        Vector3 dealedCardPosition = dealedCard.transform.position;
        Vector3 targetCardPosition = playerInventories[inventoryID].inventorySlots[inventorySlotID].CardObject.transform.position;

        Quaternion dealedCardRotation = dealedCard.transform.rotation;
        Quaternion targetCardRotation = playerInventories[inventoryID].inventorySlots[inventorySlotID].CardObject.transform.rotation;

        //while(dealedCard.transform.rotation != targetCardRotation)
        //{
        //    dealedCard.transform.rotation = Quaternion.Slerp(dealedCard.transform.rotation, targetCardRotation, Time.deltaTime * 20);

        //    yield return null;
        //}

        //while (dealedCard.transform.position != targetCardPosition)
        //{
        //    dealedCard.transform.position = Vector3.Slerp(dealedCard.transform.position, targetCardPosition, Time.deltaTime * 10);
        //    yield return null;
        //}

        for (float j = 0f; j < 1.1f; j += 0.2f)
        {
            dealedCard.transform.rotation = Quaternion.Slerp(dealedCardRotation, targetCardRotation, j);
            yield return new WaitForSeconds(0.02f);
        }

        for (float i = 0f; i < 1.1f; i += 0.1f)
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

    private IEnumerator AICheckCards(float rotation, float lift, float dialogueTime)
    {
        for (int i = 1; i < playerInventories.Count; ++i)
        {
            for (int j = 0; playerInventories[i].inventorySlots.Length > j; ++j)
            {
                Quaternion baseRotation = Quaternion.Euler(0, 0, 0);
                Quaternion targetRotation = Quaternion.Euler(rotation, 0, 0);

                Vector3 basePosition = playerInventories[i].inventorySlots[j].CardObject.transform.position;
                Vector3 targetPosition = new Vector3(playerInventories[i].inventorySlots[j].CardObject.transform.position.x, playerInventories[i].inventorySlots[j].CardObject.transform.position.y + lift, playerInventories[i].inventorySlots[j].CardObject.transform.position.z);

                playerInventories[i].inventorySlots[j].CardObject.transform.localRotation = baseRotation;

                while (playerInventories[i].inventorySlots[j].CardObject.transform.localRotation != targetRotation)
                {
                    playerInventories[i].inventorySlots[j].CardObject.transform.position = Vector3.Slerp(playerInventories[i].inventorySlots[j].CardObject.transform.position, targetPosition, Time.deltaTime * 15);

                    playerInventories[i].inventorySlots[j].CardObject.transform.localRotation = Quaternion.Slerp(playerInventories[i].inventorySlots[j].CardObject.transform.localRotation, targetRotation, Time.deltaTime * 20);
                    
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);

                while (playerInventories[i].inventorySlots[j].CardObject.transform.localRotation != baseRotation)
                {
                    playerInventories[i].inventorySlots[j].CardObject.transform.position = Vector3.Slerp(playerInventories[i].inventorySlots[j].CardObject.transform.position, basePosition, Time.deltaTime * 15);

                    playerInventories[i].inventorySlots[j].CardObject.transform.localRotation = Quaternion.Slerp(playerInventories[i].inventorySlots[j].CardObject.transform.localRotation, baseRotation, Time.deltaTime * 20);

                    yield return null;
                }
            }

            yield return new WaitForSeconds(dialogueTime);
        }

        Debug.Log("Finished Dealing Phase");

        StartCoroutine(GetPlayingCards());
    }

    #endregion

    #region Card Playing Phase

    private IEnumerator GetPlayingCards()
    {
        for(int i = 0; i < playerInventories.Count; ++i)
        {
            playerDecisionHandler[i].enabled = true;

            while (playerDecisions[i].Key == null)
            {
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(2, 4));
        }

        print("bruh");
    }

    #endregion
}
