using UnityEngine;

public class CardBase
{
    public string CardType { get; }
    public string CardDescription { get; }
    public Texture CardTexture { get; }
    
    //Constructor
    public CardBase(string newType, string newDescription, string newTexture)
    {
        CardType = newType;
        CardDescription = newDescription;
        CardTexture = GetTexture(newTexture);
    }

    //Gets the inputted texture from the resources folder
    private Texture GetTexture(string filePath)
    {
        return Resources.Load<Texture>(filePath);
    }

}
