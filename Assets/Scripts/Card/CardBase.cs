using UnityEngine;

public class CardBase
{
    public string CardType { get; }
    public string CardDescription { get; }
    public Material CardMaterial { get; }
    
    //Constructor
    public CardBase(string newType, string newDescription, string newMaterial)
    {
        CardType = newType;
        CardDescription = newDescription;
        CardMaterial = GetMaterial(newMaterial);
    }

    //Gets the inputted texture from the resources folder
    private Material GetMaterial(string filePath)
    {
        return Resources.Load<Material>(filePath);
    }

}
