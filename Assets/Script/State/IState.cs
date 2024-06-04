using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    void OnEnter(Object data = null);
    void OnExit(Object data = null);
    void OnUpdate(Object data = null);
}
