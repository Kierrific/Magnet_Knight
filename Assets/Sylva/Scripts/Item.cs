using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Chance")]
    [SerializeField] private int chance;

    [Header("Remove after grab?")]
    [SerializeField] private bool removecheck;

    private bool removed;

    public int getChance()
    {
        return chance;
    }

    public bool getRemoveCheck()
    {
        return removecheck;
    }

    public void setRemoved()
    {
        removed = true;
    }
    
    public bool isRemoved()
    {
        return removed;
    }
}
