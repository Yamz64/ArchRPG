using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBaseClass : MonoBehaviour
{
    public virtual void Interact()
    {
        Debug.Log("Interacting");
    }
}
