using UnityEngine;

public class CardTypes : MonoBehaviour
{
    public CardBase CardAttack = new CardBase("Attack", "its attacks lol", "attack");
    public CardBase CardBlock = new CardBase("Block", "its blocks lol", "block");
    public CardBase CardSwap = new CardBase("Swap", "its swaps lol", "swap");
    public CardBase CardBean = new CardBase("Bean", "been", "bean");
}
