using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RoundManager : MonoBehaviour
{
    public int beanCount = 1;
    private int roundCount;

    public bool isStarted;

    public AudioManager audioHandler;

    public List<GameObject> aiPlayerModels = new List<GameObject>();
    public List<InventoryHandler> playerInventories = new List<InventoryHandler>( );
    public List<PlayerAction> playerDecisionHandler = new List<PlayerAction>();

    public List<KeyValuePair<string, int>> playerDecisions = new List<KeyValuePair<string, int>>();
    public List<int> playersOut = new List<int>();

    [Header ("Dealing Animation")]
    public Transform deckRefPoint;
    public GameObject cardPrefab;

    [Space]
    public GameObject cameraAnchor;

    public TMP_Text announcement;
    
    
    public enum ERoundState
    {
        Start,
        Deal,
        AIPass,
        Play,
        Outcome
    }

    public ERoundState roundState = ERoundState.Start;
    
    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            playerDecisions.Add(new KeyValuePair<string, int>(null, 0));
        }

        FindObjectsOfType<KillPlayerSelect>().ToList().ForEach(script => aiPlayerModels.Add(script.gameObject));

        StartCoroutine(GameOrder());
    }
    
    private bool p1OutCheck()
    {
        foreach(int pID in  playersOut)
        {
            if(pID == 0)
            {
                return true;
            }
        }
        return false;
    }

    // Linear Coroutines have been moved here to make following the order easier
    // using "yield return" allows for us to wait until the specified routine is complete before continuing, like
    // using "await" on an async call
    private IEnumerator GameOrder()
    {
        while (playersOut.Count > 2 || !p1OutCheck())
        {
            ResetRound();
            yield return StartCoroutine(DealPhase());
            // Initialize the card checking animation
            yield return StartCoroutine(AICheckCards(45f, 1f, 3f));
            // Begin next phase
            yield return StartCoroutine(GetPlayingCards());
            yield return StartCoroutine(CardReveal());
            yield return StartCoroutine(EvaluateCards());
            yield return StartCoroutine(RemoveDeadPlayers());
        }

        if (playersOut.Contains(0))
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            SceneManager.LoadScene("Victory");
        }
    }

    #region Card Dealing Phase
    private IEnumerator DealPhase()
    {
        Debug.Log("Started Routine");
        roundState = ERoundState.Deal;

        // Deal a card for each slot that is empty in each players inventory
        for(int i = 0;  i < playerInventories.Count; ++i)
        {
            for (int j = 0; playerInventories[i].inventorySlots.Length > j; ++j)
            {
                print("Slot " + i + j + " is " + (playerInventories[i].inventorySlots[j].Card));
                if (playerInventories[i].inventorySlots[j].Card == null && !playersOut.Contains(playerInventories[i].PlayerID))
                {
                    Debug.Log("Entered");

                    StartCoroutine(CardDealing(i, j));

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        
    }

    private CardBase CardDrawProbability(int beanCount)
    {
        int percentage = Random.Range(0, 101);

        int beanPercentage = 15 / beanCount;
        int otherCardPercentage = (int)((100 - beanPercentage*2) / 3);

        if (percentage > 100 - beanPercentage)
        {
            return CardTypes.CardBean;
        }
        
        if(percentage > 100 - beanPercentage *2)
        {
            return CardTypes.CardSwap;
        }

        if(percentage > 100 - beanPercentage*2 - otherCardPercentage)
        {
            return CardTypes.CardBlock;
        }

        print(percentage);
        return CardTypes.CardAttack;
    }

    //Swap Luck
    private CardBase CardDrawProbability()
    {
       int percentage = Random.Range(0, 256);

       if(percentage%2 == 0)
       {
            return CardTypes.CardBean;
       }

        return CardTypes.CardAttack;
    }

    // Deals the card to the player, taking in the player's ID and the inventory slot ID
    private IEnumerator CardDealing(int inventoryID, int inventorySlotID, bool isSwapped = false)
    {
        // Instantiate the "dummy card" which will be animated
        GameObject dealedCard = Instantiate(cardPrefab, deckRefPoint);  

        // Set start & goal position for the dummy card
        Vector3 dealedCardPosition = dealedCard.transform.position;
        Vector3 targetCardPosition = playerInventories[inventoryID].inventorySlots[inventorySlotID].CardObject.transform.position;

        // Set start & goal rotation for the dummy card
        Quaternion dealedCardRotation = dealedCard.transform.rotation;
        Quaternion targetCardRotation = playerInventories[inventoryID].inventorySlots[inventorySlotID].CardObject.transform.rotation;

        #region  commented code
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
        #endregion

        // Animation - rotation
        for (float j = 0f; j < 1.1f; j += 0.2f)
        {
            dealedCard.transform.rotation = Quaternion.Slerp(dealedCardRotation, targetCardRotation, j);
            yield return new WaitForSeconds(0.02f);
        }

        audioHandler.play("Deal");

        // Animation - position
        for (float i = 0f; i < 1.1f; i += 0.1f)
        {
            dealedCard.transform.position = Vector3.Slerp(dealedCardPosition, targetCardPosition, i);

            yield return new WaitForSeconds(0.02f);
        }

        // Randomly select card type
        CardBase dealedCardType = CardTypes.CardBean;
        int randomTypeGenerator = Random.Range(0, 101);

        if(!isSwapped)
        // Assign random value to a card type
            dealedCardType = CardDrawProbability(beanCount);
        else
        {
            dealedCardType = CardDrawProbability();
        }

        // Assign new card to the player's inventory
        playerInventories[inventoryID].AddCard(inventorySlotID, dealedCardType);

        Debug.Log(playerInventories[inventoryID].inventorySlots[inventorySlotID].Card.CardType);

        // Kill the dummy card
        Destroy(dealedCard);

        Debug.Log("Card Dealed");
    }

    // NPC card checking animations, takes card lift rotation, card lift amount, and dialogue allowance time
    private IEnumerator AICheckCards(float rotation, float lift, float dialogueTime)
    {
        for (int i = 1; i < playerInventories.Count; ++i)
        {
            for (int j = 0; playerInventories[i].inventorySlots.Length > j; ++j)
            {
                if (playersOut.Contains(playerInventories[i].PlayerID)) continue;
                // Set start and target rotation
                Quaternion baseRotation = Quaternion.Euler(0, 0, 0);
                Quaternion targetRotation = Quaternion.Euler(rotation, 0, 0);
                
                // Set start and target position
                Vector3 basePosition = playerInventories[i].inventorySlots[j].CardObject.transform.position;
                Vector3 targetPosition = new Vector3(basePosition.x, basePosition.y + lift, basePosition.z);

                playerInventories[i].inventorySlots[j].CardObject.transform.localRotation = baseRotation;

                // Animate movement - Picking up the card
                // NOTE: This could just be a for loop to prevent possible overshoot on the while loop but we ball
                while (playerInventories[i].inventorySlots[j].CardObject.transform.localRotation != targetRotation)
                {
                    var currentCardTransform = playerInventories[i].inventorySlots[j].CardObject.transform;
                    currentCardTransform.position = Vector3.Slerp(currentCardTransform.position, 
                                                                  targetPosition, 
                                                                  Time.deltaTime * 15);

                    currentCardTransform.localRotation = Quaternion.Slerp(currentCardTransform.localRotation, 
                                                                          targetRotation, 
                                                                          Time.deltaTime * 20);
                    
                    yield return null;
                }

                // Wait before putting the card down
                yield return new WaitForSeconds(0.1f);

                
                // Animate movement - Putting the card down
                while (playerInventories[i].inventorySlots[j].CardObject.transform.localRotation != baseRotation)
                {
                    var currentCardTransform = playerInventories[i].inventorySlots[j].CardObject.transform;
                    currentCardTransform.position = Vector3.Slerp(currentCardTransform.position, basePosition, Time.deltaTime * 15);

                    currentCardTransform.localRotation = Quaternion.Slerp(currentCardTransform.localRotation, baseRotation, Time.deltaTime * 20);

                    yield return null;
                }
            }

            // Allow for specified dialogue time before moving to the next NPC
            yield return new WaitForSeconds(dialogueTime);
        }

        Debug.Log("Finished Dealing Phase");

        
    }

    #endregion

    #region Card Playing Phase

    private IEnumerator GetPlayingCards()
    {
        for(int i = 0; i < playerInventories.Count; ++i)
        {
            if(playersOut.Contains(playerInventories[i].PlayerID)) continue;
            roundState = (i == 0) ? ERoundState.Play : ERoundState.AIPass;

            print("Player " + i + " Turn");
            playerDecisionHandler[i].enabled = true;
            print(playerDecisions[i].Key);
            while (playerDecisions[i].Key == null)
            {
                yield return null;
            }

            playerDecisionHandler[i].enabled = false;

            yield return null;
        }

        print("All Cards Played");
        playerDecisions.ForEach(decision => print(decision.Key + " | " + decision.Value));
    }

    #endregion
    
    #region Decision Phase

    private IEnumerator CardReveal()
    {
        roundState = ERoundState.Outcome;

        for (int i = 0; i < playerDecisions.Count; i++)
        {
            if (playersOut.Contains(playerInventories[i].PlayerID)) continue;
            print(playerDecisionHandler[i].currentPlayedCard.transform);
            Transform cardCameraPosition = playerDecisionHandler[i].currentPlayedCard.transform;
            foreach (Transform t in playerDecisionHandler[i].currentPlayedCard.transform)
            {
                cardCameraPosition = (t.name == "Camera Anchor")? t : cardCameraPosition;
            }
            yield return StartCoroutine(FocusCamera(cardCameraPosition));
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(FlipCard(playerDecisionHandler[i].currentPlayedCard));
            yield return new WaitForSeconds(1f);
        }
        yield return StartCoroutine(FocusCamera(cameraAnchor.transform));
    }

    private IEnumerator EvaluateCards()
    {
        var attacks = playerDecisions.Where(decision => decision.Key == "Attack").ToList();
        var swaps = playerDecisions.Where(decision => decision.Key == "Swap").ToList();
        var beans = playerDecisions.Where(decision => decision.Key == "Bean").ToList();


        for (int i = 0; i < playerDecisions.Count; ++i)
        {
            Transform cardCameraPosition = playerDecisionHandler[i].currentPlayedCard.transform;

            foreach (Transform t in playerDecisionHandler[i].currentPlayedCard.transform)
            {
                cardCameraPosition = (t.name == "Camera Anchor") ? t : cardCameraPosition;
            }

            if (beans.Contains(playerDecisions[i]))
            {
                var beanID = playerDecisions[i].Value;

                yield return StartCoroutine(FocusCamera(cardCameraPosition));

                yield return StartCoroutine(Announcement("Bean"));

                yield return new WaitForSeconds(0.2f);
            }

            yield return StartCoroutine(FocusCamera(cameraAnchor.transform));

            if (swaps.Contains(playerDecisions[i]))
            {
                var swappedCardID = playerDecisions[i].Value;

                playerInventories[i].RemoveCard(swappedCardID);
                StartCoroutine(CardDealing(i, swappedCardID));

                yield return StartCoroutine(Announcement("Player " + i + " has swapped a card "));

                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(0.3f);

        if (attacks.Count == 0)
        {
            yield return Speech("No Attacks! Lucky", 2);
            yield return Speech("Let us move onto the next round", 2);
        }
        else
        {

            yield return Speech("An Attack, Who must go", 2);
            for (int i = 0; i < playerDecisions.Count; i++)
            {
                if (!attacks.Contains(playerDecisions[i]))
                {
                    continue;
                }
                int attackedPlayer = playerDecisions[i].Value;
                Transform cardCameraPosition = playerDecisionHandler[i].currentPlayedCard.transform ;
                Transform attackedCameraPosition = playerDecisionHandler[attackedPlayer].currentPlayedCard.transform ;
                
                foreach (Transform t in playerDecisionHandler[i].currentPlayedCard.transform)
                {
                    cardCameraPosition = (t.name == "Camera Anchor")? t : cardCameraPosition;
                }
                
                foreach (Transform t in playerDecisionHandler[attackedPlayer].currentPlayedCard.transform)
                {
                    attackedCameraPosition = (t.name == "Camera Anchor")? t : cardCameraPosition;
                }

                yield return StartCoroutine(FocusCamera(cardCameraPosition));
                yield return new WaitForSeconds(1f);
                yield return StartCoroutine(Announcement("Player " + (i + 1) + " has attacked Player " +
                                                         (attackedPlayer + 1)));
                
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(FocusCamera(attackedCameraPosition));
                
                if (playerDecisions[attackedPlayer].Key == "Block" || playerDecisions[attackedPlayer].Key == "Bean" || playerInventories[attackedPlayer].CheckForBean())
                {
                    if(playerInventories[attackedPlayer].CheckForBean() && playerDecisions[attackedPlayer].Key != "Block" && playerDecisions[attackedPlayer].Key != "Bean")
                    {
                        for(int j = 0; j < playerInventories[attackedPlayer].inventorySlots.Length; ++j)
                        {
                            if (playerInventories[attackedPlayer].inventorySlots[j].Card.CardType == "Bean")
                            {
                                if(attackedPlayer != 0)
                                    StartCoroutine(FlipCard(playerInventories[attackedPlayer].inventorySlots[j].CardObject));

                                playerInventories[attackedPlayer].RemoveCard(j);
                                break;
                            }
                        }
                    }

                    yield return StartCoroutine(Announcement("They escape it! Player " + (attackedPlayer + 1) +
                                                             " gets to see another day"));
                    continue;
                }
                if (playerDecisions[attackedPlayer].Key == "Attack" && playerDecisions[attackedPlayer].Value == i)
                {
                    yield return StartCoroutine(Announcement("Attack on Attack! Player "+ (i + 1) + " and Player " +
                                                            ( attackedPlayer + 1) + " are out!"));
                    playersOut.Add(i);
                    playersOut.Add(attackedPlayer);
                    continue;
                }

                yield return StartCoroutine(Announcement("No Defenses! Player " + (attackedPlayer + 1) + " is out!"));
                playersOut.Add(attackedPlayer);
            }

            for (int i = 0; i < playerDecisionHandler.Count; i++)
            {
                if(playersOut.Contains(playerInventories[i].PlayerID)) continue;
                playerDecisionHandler[i].ReturnCard(playerDecisionHandler[i].currentPlayedCard.transform);
                playerInventories[i].RemoveCard(playerDecisionHandler[i].PlayedSlot);
            }

            yield return StartCoroutine(FocusCamera(cameraAnchor.transform));
        }
    }   

    private IEnumerator RemoveDeadPlayers()
    {
        if (playersOut.Count >= 1)
        {
            var deadPlayers = playerInventories.Where(playerInv => playersOut.Contains(playerInv.PlayerID));
            deadPlayers.ToList().ForEach(player => player.RemoveAllCards());
            var deadPlayerModels = aiPlayerModels.Where(model =>
                playersOut.Contains(model.GetComponent<KillPlayerSelect>().playerID));

            foreach (var player in deadPlayerModels)
            {
                var startPosition = player.transform.position;
                var goalPosition = startPosition - new Vector3(0, 6, 0);

                for (float i = 0; i < 1.1f; i += 0.16f)
                {
                    player.transform.position = Vector3.Lerp(startPosition, goalPosition, i);
                    yield return new WaitForSeconds(0.1f);
                }
                
                Destroy(player);
            }
        }

        if (playersOut.Contains(0))
        {
            SceneManager.LoadScene("Scenes/GameOver");
        }
    }
    #endregion

    #region Utils
    private IEnumerator FocusCamera(Transform destination)
    {
        var startPosition = Camera.main.transform;

        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition.position, destination.position, i);
            Camera.main.transform.rotation = Quaternion.Lerp(startPosition.rotation, destination.rotation , i);

            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator FlipCard(GameObject card)
    {
        var startPosition = card.transform.GetChild(0).rotation;
        var targetPosition = startPosition * Quaternion.Euler(0,0,180f);

        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            card.transform.GetChild(0).rotation = Quaternion.Slerp(startPosition, targetPosition, i);
            
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator Speech(string message, int player, float speed = 30f)
    {
        message += " . . . ";
        var findPlayer = playerInventories.Find(inv => inv.PlayerID == player).gameObject;
        var playerText = findPlayer.GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>();
        playerText.text = "";

        foreach (var letter in message)
        {
            playerText.text += letter;
            yield return new WaitForSeconds(1/speed);
        }

        yield return new WaitForSeconds(3f);
        playerText.text = "";
    }

    private IEnumerator Announcement(string message, float speed = 30f)
    {
        message += " . . . ";
        announcement.text = "";
        foreach (var letter in message)
        {
            announcement.text += letter;
            yield return new WaitForSeconds(1/speed);
        }

        yield return new WaitForSeconds(3f);
        announcement.text = "";
    }

    private void ResetRound()
    {
        for(int i = 0; i < playerDecisions.Count; ++i)
        {
            playerDecisions[i] = new KeyValuePair<string, int>(null, 0);
        }
    }
    

    #endregion
}
