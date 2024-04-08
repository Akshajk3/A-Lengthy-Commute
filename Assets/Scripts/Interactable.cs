using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;

    // This function will be called from player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // Template funciton to be overriden by other classes
    }
}
