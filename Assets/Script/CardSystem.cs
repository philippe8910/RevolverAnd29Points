using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    public static CardSystem instance;

    void Awake()
    {
        instance = this;
    }


    public List<GameObject> cards = new List<GameObject>(); //卡池
    public List<GameObject> cardPools = new List<GameObject>(); //卡牌池
    public List<Transform> playerCardPositions = new List<Transform>(); //玩家卡牌位置
    public List<Transform> enemyCardPositions = new List<Transform>();  //敌人卡牌位置
    public List<GameObject> playerCards = new List<GameObject>(); //玩家手牌
    public List<GameObject> enemyCards = new List<GameObject>(); //敌人手牌

    public Transform cardDeckPosition; //卡牌堆位置

    [ContextMenu("AddCard")]
    public void AddCard(bool isPlayer = true)
    {
        if(cards.Count == 0)    //如果卡池为空，返回
            return;

        Shuffle();  //洗牌

        if(isPlayer && playerCards.Count >= 5) //如果玩家手牌已满，返回
            return;
        else if(!isPlayer && enemyCards.Count >= 5) //如果敌人手牌已满，返回
            return;

        var newCardPosition = isPlayer ? playerCardPositions[0].position + new Vector3(0,0,4) : enemyCardPositions[0].position + new Vector3(0,0,4);
        var newCard = Instantiate(cards[0], newCardPosition , cardDeckPosition.rotation);
        cards.RemoveAt(0);

        if(isPlayer)
        {
            var playerCard = newCard.GetComponent<Card_Interactable>();

            playerCards.Add(newCard);

            playerCard.StartMoveToTable(playerCardPositions[playerCards.Count - 1].position, playerCardPositions[playerCards.Count - 1].rotation);
        }
        else
        {
            var enemyCard = newCard.GetComponent<Card_Interactable>();

            if(enemyCards.Count == 0)
            {
                enemyCard.IsEnemyOwned = true;
                enemyCards.Add(newCard);
            }
            else
            {
                enemyCards.Add(newCard);
            }

            enemyCard.StartMoveToTable(enemyCardPositions[enemyCards.Count - 1].position, enemyCardPositions[enemyCards.Count - 1].rotation);
        }
    }

    public void AddTargetCard(int index , bool isPlayer)
    {
        if(cards.Count == 0)
            return;

        var newCardPosition = isPlayer ? playerCardPositions[0].position + new Vector3(0,0,4) : enemyCardPositions[0].position + new Vector3(0,0,4);
        var newCard = Instantiate(cards[index], newCardPosition , cardDeckPosition.rotation);
        cards.RemoveAt(index);

        if(isPlayer)
        {
            var playerCard = newCard.GetComponent<Card_Interactable>();

            playerCards.Add(newCard);

            playerCard.StartMoveToTable(playerCardPositions[playerCards.Count - 1].position, playerCardPositions[playerCards.Count - 1].rotation);
        }
        else
        {
            var enemyCard = newCard.GetComponent<Card_Interactable>();

            if(enemyCards.Count == 0)
            {
                enemyCard.IsEnemyOwned = true;
                enemyCards.Add(newCard);
            }
            else
            {
                enemyCards.Add(newCard);
            }

            enemyCard.StartMoveToTable(enemyCardPositions[enemyCards.Count - 1].position, enemyCardPositions[enemyCards.Count - 1].rotation);
        }
    }   

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var temp = cards[i];
            var randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public int GetPlayerCardCount()
    {
        int count = 0;

        foreach (var card in CardSystem.instance.playerCards)
        {
            count += card.GetComponent<Card_Interactable>().cardID;
        }

        return count;
    }

    public string GetFirstEnemyCardName()
    {
        return CardSystem.instance.enemyCards[0].GetComponent<Card_Interactable>().GetFullName();
    }

    public int GetEnemyCardCountFormTable()
    {
        int count = 0;

        for(int i = 1; i < CardSystem.instance.enemyCards.Count; i++)
        {
            count += CardSystem.instance.enemyCards[i].GetComponent<Card_Interactable>().cardID;
        }

        return count;
    }

    public int GetEnemyCardCountFormAll()
    {
        int count = 0;

        foreach (var card in CardSystem.instance.enemyCards)
        {
            count += card.GetComponent<Card_Interactable>().cardID;
        }

        return count;
    }

    public void ResetAllGame()
    {
        foreach (var card in playerCards)
        {
            Destroy(card);
        }

        foreach (var card in enemyCards)
        {
            Destroy(card);
        }   

        cards = new List<GameObject>(cardPools);
        playerCards = new List<GameObject>();
        enemyCards = new List<GameObject>();
    }

    public void CardResult()
    {
        StartCoroutine(CardFlipAction());

        IEnumerator CardFlipAction()
        {
            foreach (var card in playerCards)
            {
                FlipCardPosition(card.transform);
                yield return new WaitForSeconds(0.1f);
            }

            foreach (var card in enemyCards)
            {
                FlipCardPosition(card.transform);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }


    public void FlipCardPosition(Transform cardTransform)
    {
        cardTransform.DOLocalMove(cardTransform.position + (Vector3.up * 0.1f), 0.3f).SetEase(Ease.InOutQuad).onComplete += () =>
        {
            cardTransform.DOLocalMove(cardTransform.position - (Vector3.up * 0.1f), 0.3f).SetEase(Ease.InOutQuad);
        };
        cardTransform.DOLocalRotate(new Vector3(cardTransform.localEulerAngles.x, cardTransform.localEulerAngles.y, cardTransform.localEulerAngles.z + 180), 0.6f);
    }
}
