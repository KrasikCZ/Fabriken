using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BuildReqs
{
    public Item item;
    public int amount;
    void Start(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}
