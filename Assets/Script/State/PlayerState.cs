using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : IState
{
    public void OnEnter(Object data = null)
    {
        if(GameLogicSystem.instance.isPlayerStay)
        {
            GameLogicSystem.instance.ChangeState(new EnemyState());
        }

        DecalSystem.instance.SwitchMaterial(0);
    }

    public void OnExit(Object data = null)
    {
        
    }

    public void OnUpdate(Object data = null)
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.U))
        {
            CardSystem.instance.AddCard(true); //添加玩家手牌

            GameLogicSystem.instance.ChangeState(new EnemyState());
        }

        if(UnityEngine.Input.GetKeyDown(KeyCode.I))
        {
            GameLogicSystem.instance.isPlayerStay = true;
            GameLogicSystem.instance.ChangeState(new EnemyState());
        }

        Debug.Log("PlayerState OnUpdate");
    }
}

public class EnemyState : IState
{
    public async void OnEnter(Object data = null)
    {
        DecalSystem.instance.SwitchMaterial(1);
        await Task.Delay(2000);

        HitJudge();
    }

    public void OnExit(Object data = null)
    {
        
    }

    public void OnUpdate(Object data = null)
    {
        Debug.Log("EnemyState OnUpdate");
    }

    public void HitJudge(int hitRate = 80)
    {
        bool hit = false;
        bool shinyHit = Random.Range(0, 100) < hitRate;;

        int currentPoint = CardSystem.instance.GetEnemyCardCountFormAll();
        int credit = 0; 

        List<int> creditCards = new List<int>();

        for(int i = 0; i < CardSystem.instance.cards.Count; i++)
        {
            if(CardSystem.instance.cards[i].GetComponent<Card_Interactable>().cardID + currentPoint < 30)
            {
                credit++;
                creditCards.Add(i);
            }
        }

        Debug.Log("credit: " + credit);
        Debug.Log("CardSystem.instance.cards.Count: " + CardSystem.instance.cards.Count);
        Debug.Log("credit / CardSystem.instance.cards.Count: " + (float)credit / (float)CardSystem.instance.cards.Count);

        if(((float)credit / (float)CardSystem.instance.cards.Count) > 0.4f)
            hit = true;

        if(shinyHit)
        {
            if(creditCards.Count > 0)
                CardSystem.instance.AddTargetCard(creditCards[0] ,false); //添加敌人手牌
            else
                if(hit)
                    CardSystem.instance.AddCard(false); //添加敌人手牌
                else
                    GameLogicSystem.instance.isEnemyStay = true;

            Debug.Log("shinyHit");
        }
        else
        {
            if(hit)
                CardSystem.instance.AddCard(false); //添加敌人手牌
            else
                GameLogicSystem.instance.isEnemyStay = true;
        }

        GameLogicSystem.instance.ChangeState(new JudgeCardGameResultState());
    }
}


public class JudgeCardGameResultState : IState
{
    public void OnEnter(Object data = null)
    {
        DecalSystem.instance.SwitchMaterial(3);

        if(GameLogicSystem.instance.isPlayerStay && GameLogicSystem.instance.isEnemyStay)
        {
            GameLogicSystem.instance.JudgeCardGameResult();
        }
        else
        {
            GameLogicSystem.instance.ChangeState(new PlayerState());
        }

        Debug.Log("JudgeCardGameResultState OnEnter");
    }

    public void OnExit(Object data = null)
    {
        
    }

    public void OnUpdate(Object data = null)
    {

    }
}

public class StartRoundState : IState
{
    public async void OnEnter(Object data = null)
    {


        if(GameLogicSystem.instance.playerTrun > 3 || GameLogicSystem.instance.playerBulletCount == 0 || GameLogicSystem.instance.enemyBulletCount == 0)
        {
            GameLogicSystem.instance.ChangeState(new PlayerState()); //29點游戏结束
        }
        else
        {
            CardSystem.instance.AddCard(true); //添加玩家手牌
            await Task.Delay(200);
            CardSystem.instance.AddCard(true); //添加玩家手牌
            await Task.Delay(200);
            CardSystem.instance.AddCard(false); //添加敌人手牌
            await Task.Delay(200);
            CardSystem.instance.AddCard(false); //添加敌人手牌

            GameLogicSystem.instance.isPlayerStay = false;
            GameLogicSystem.instance.isEnemyStay = false;

            GameLogicSystem.instance.playerTrun++;
            GameLogicSystem.instance.ChangeState(new PlayerState());
        }
    }

    public void OnExit(Object data = null)
    {
        
    }

    public void OnUpdate(Object data = null)
    {
        Debug.Log("StartRoundState OnUpdate");
    }
}

internal class RevolverRoundState : IState
{
    public GameObject revolverPack;
    public void OnEnter(Object data = null)
    {
        revolverPack = GameObject.Instantiate(GameLogicSystem.instance.revolverPack, GameLogicSystem.instance.revolverPack.transform.position, GameLogicSystem.instance.revolverPack.transform.rotation);
    }

    public void OnExit(Object data = null)
    {
        GameObject.Destroy(revolverPack);
    }

    public void OnUpdate(Object data = null)
    {
        
    }
}