using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "ScriptableObject/Item", order = 1)]

public class ItemSO : ScriptableObject
{
    public string title; 
    public string description;
    public Sprite icon;
    public int amount, goldToGive, amountToHeal, maxAmount;
    public bool isStackable;

    [System.Serializable]
    public enum Type
    {
        Quest, Consommable, Commun
    }

    public Type type;

}
