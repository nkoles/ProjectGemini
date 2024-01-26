using UnityEngine;

public class CardTypes
{
    static public CardBase CardAttack = new CardBase("Attack", "Attack Card: Choose a player to attack this turn.\n", "CardMaterials/AttackCard");
    static public CardBase CardBlock = new CardBase("Block", "Block Card: Negate any attacks from other players this turn.\n", "CardMaterials/BlockCard");
    static public CardBase CardSwap = new CardBase("Swap", "Swap Card: Select a card from your remaining hand to discard and receive 2 cards at the start of your next turn.\n", "CardMaterials/SwapCard");
    static public CardBase CardBean = new CardBase("Bean", "Bean Card: You hold the power of beans. Use of this card will save you when attacked. Single use only.\n", "CardMaterials/BeanCard");
}
