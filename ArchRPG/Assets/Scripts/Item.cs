using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public void Add() {
        if(amount >= limit)
        {
            Debug.Log("You cannot carry anymore" + name + "s in your inventory!");
            return;
        }
        amount++;
        Debug.Log("Added " + name + " to inventory!");
    }
    public void Remove() { amount--; }
    public virtual void Use()
    {
        Debug.Log("The item has been used");
    }
    public string name;
    public string description;
    public int amount;
    public int limit;
}

public class HotDog : Item
{
    public HotDog()
    {
        name = "Hot Dog";
        description = "A tube of unidentified, but delicious meat!  Heals 50 HP when eaten!";
        amount = 1;
        limit = 30;
    }

    public override void Use()
    {
        Debug.Log("The Hot Dog has been consumed");
        Remove();
    }
}
