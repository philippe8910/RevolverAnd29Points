using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Action : IInteractable
{
    public string GetName()
    {
        return "Demon";
    }

    public Vector3 GetOriginalPosition()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetTargetPosition()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        GameLogicSystem.instance.ChangeState(new StartRoundState());
    }

    public void OnInteractEnd()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteractStart()
    {
        throw new System.NotImplementedException();
    }
}
