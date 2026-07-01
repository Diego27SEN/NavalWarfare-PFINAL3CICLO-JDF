using UnityEngine;

public class SkillTree : MonoBehaviour 
{
    // Arbol de Cartas
    private SkillNode<CardsDatabase> rootNode;

    public void BuildTree(CardsDatabase startCard, CardsDatabase nextCard)
    {
        // Nodo raíz
        rootNode = new SkillNode<CardsDatabase>(startCard);

        SkillNode<CardsDatabase> nextNode = new SkillNode<CardsDatabase>(nextCard);
        rootNode.AddChild(nextNode);

        Debug.Log($"Nodo raíz creado con: {rootNode.data.NameCart}");
    }
}
