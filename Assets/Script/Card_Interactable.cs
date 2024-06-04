using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Card_Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    public int cardID;

    [SerializeField]
    public Transform target;

    [SerializeField]
    private string cardName;

    [SerializeField]
    private bool isTakingPosition = false;
    public bool IsEnemyOwned = false;

    [SerializeField]
    public Transform tablePosition;

    private Vector3 originalPosition;

    private Quaternion originalRotation;

    void Start()
    {
        target = GameObject.FindWithTag("CardPosition").transform;
    }

    void Update()
    {
        if(isTakingPosition)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 0.1f);
            Debug.Log("Card is taking position");
        }
    }

    public void StartMoveToTable(Vector3 tablePosition, Quaternion tableRotation)
    {
        transform.DOMove(tablePosition, 1f).SetEase(Ease.OutQuart).onComplete = () =>
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        };
        transform.DORotate(tableRotation.eulerAngles, 1f);
    }


    public string GetName()
    {
        if(IsEnemyOwned)
        {
            return cardName + "（" + "?" + "）";
        }
        else
        {
            return cardName + "（" + cardID + "）";
        }
        
    }

    public string GetFullName()
    {
        return cardName + "（" + cardID + "）";
    }

    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }

    public Vector3 GetTargetPosition()
    {
        return target.position;
    }

    public void Interact()
    {
        isTakingPosition = !isTakingPosition;
        //GetComponent<Rigidbody>().useGravity = false;

        if(!isTakingPosition)
        {
            transform.DOMove(originalPosition, 1f).SetEase(Ease.OutQuart);
            transform.DORotate(originalRotation.eulerAngles, 1f);
        }
    }

    public void OnInteractEnd()
    {
        
    }

    public void OnInteractStart()
    {
        
    }
}
