using System.Collections.Generic;

[System.Serializable]
public class SkillNode<T>
{
    public T data;

    public bool isUnlocked = false;

    // Lista de hijos
    public List<SkillNode<T>> children = new();

    public SkillNode(T data)
    {
        this.data = data;
        this.isUnlocked = false;
        this.children = new List<SkillNode<T>>();
    }

    // Agregar un nodo 
    public void AddChild(SkillNode<T> child)
    {
        children.Add(child);
    }
}
