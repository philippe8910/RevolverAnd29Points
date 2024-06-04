using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TableButton : MonoBehaviour, IInteractable
{
    public string buttonContent;

    public UnityEvent onButtonPressed;

    public void Interact()
    {
        onButtonPressed.Invoke();
        Debug.Log(transform.name + " -Interact");
    }

    public string GetName()
    {
        return buttonContent;
    }

    public Vector3 GetTargetPosition()
    {
        return new Vector3(0, 0, 0);
    }

    public Vector3 GetOriginalPosition()
    {
        return new Vector3(0, 0, 0);
    }

    public void OnInteractStart()
    {
        Debug.Log("TableButton OnInteractStart");
    }

    public void OnInteractEnd()
    {
        Debug.Log("TableButton OnInteractEnd");
    }
}

