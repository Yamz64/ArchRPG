using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for handling abilities
public class Ability
{
    public virtual void Use(){
        //Used the ability
    }

    public string name;
    public int type;    //int denotes type of ability can be 3 types 0 = normal, 1 = magic, 2 = eldritch
    public int position;//int denotes the place the ability can be used 0 = front and backline, 1 = frontline, 2 = backline
}
