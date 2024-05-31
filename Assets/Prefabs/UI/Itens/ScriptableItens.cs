using UnityEngine;

[CreateAssetMenu(fileName = "Itens", menuName = "ScriptableObjects/Itens da Loja", order = 1)]
public class ScriptableItens : ScriptableObject
{
    public int id;
    public bool isRuby, isRuna;
    public string nameObject;
    public string descObject;
    public int valCoin, valRuby; 
    public Sprite sprite;
    public int def, defM, ataq, ataqM, vel, velAtaq, cd;

    public int preco, qtd;
}
