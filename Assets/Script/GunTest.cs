using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GunTest : MonoBehaviour, IInteractable
{
    public Transform target;

    public bool isTakingPosition = false;

    void Start()
    {
        target = GameObject.FindWithTag("GunPosition").transform;
    }
    
    void Update()
    {
        if(isTakingPosition)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 0.1f);
        }
    }
    public void Interact()
    {
        isTakingPosition = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public string GetName()
    {
        return "Rovlover";
    }

    public Vector3 GetTargetPosition()
    {
        return target.position;
    }

    public Vector3 GetOriginalPosition()
    {
        return target.position;
    }

    public void OnInteractStart()
    {
        isTakingPosition = true;
    }

    public void OnInteractEnd()
    {
        isTakingPosition = false;
    }
}
