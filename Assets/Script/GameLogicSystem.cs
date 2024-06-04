using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameLogicSystem : MonoBehaviour
{
    #region Singleton
    public static GameLogicSystem instance;

    public GameObject revolverPack;

    public int playerBulletCount = 0;
    public int enemyBulletCount = 0;
    public int playerBetCount = 0;
    public int playerTrun = 1;

    public bool isPlayerStay = false;
    public bool isEnemyStay = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public IState currentState = new StartRoundState();

    void Start()
    {
        currentState.OnEnter();
    }
    void Update()
    {
        currentState?.OnUpdate();

        if (Input.GetKeyDown(KeyCode.O))
        {
            JudgeCardGameResult();
        }
    }

    [ContextMenu("JudgeCardGameResult")]
    public void JudgeCardGameResult()
    {
        StartCoroutine(DelayChangeState());

        IEnumerator DelayChangeState()
        {
            CardSystem.instance.CardResult();
            yield return new WaitForSeconds(2.5f);
            DialogueManager.instance.SetPlayerCamera();
            CardGameResult(CardSystem.instance.GetPlayerCardCount(), CardSystem.instance.GetEnemyCardCountFormAll());

            yield return new WaitForSeconds(5.5f);
            CardSystem.instance.ResetAllGame();
            ChangeState(new StartRoundState());
        }
    }

    public void CardGameResult(int playerCount, int enemyCount)
    {
        const int maxCount = 29;

        bool playerBust = playerCount > maxCount;
        bool enemyBust = enemyCount > maxCount;

        if (playerBust && enemyBust)
        {
            DrawAction();
            Debug.Log("Draw");
        }
        else if (playerBust)
        {
            PlayerLoseAction();
            Debug.Log("Lose");
        }
        else if (enemyBust)
        {
            PlayerWinAction();
            Debug.Log("Win");
        }
        else
        {
            int playerDiff = maxCount - playerCount;
            int enemyDiff = maxCount - enemyCount;

            if (playerDiff < enemyDiff)
            {
                PlayerWinAction();
                Debug.Log("Win");
            }
            else if (playerDiff > enemyDiff)
            {
                PlayerLoseAction();
                Debug.Log("Lose");
            }
            else
            {
                DrawAction();
                Debug.Log("Draw");
            }
        }
    }

    public void RussianRouletteResult()
    {
        var isPlayerWin = Random.Range(1, 6) < playerBulletCount;
        var isEnemyWin = Random.Range(1, 6) < enemyBulletCount;

        if (isPlayerWin && isEnemyWin)
        {
            //ChangeState(new DrawState());
            Debug.Log("Draw");
        }
        else if (isPlayerWin)
        {
            //ChangeState(new WinState());
            Debug.Log("Win");
        }
        else if (isEnemyWin)
        {
            //ChangeState(new LoseState());
            Debug.Log("Lose");
        }
        else
        {
            //ChangeState(new DrawState());
            Debug.Log("Draw");
        }
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public void PlayerWinAction()
    {
        if(playerBetCount <= enemyBulletCount)
        {
            playerBulletCount += playerBetCount;
            enemyBulletCount -= playerBetCount;
        }
        else
        {
            playerBulletCount += enemyBulletCount;
            enemyBulletCount = 0;
        }
    }

    public void PlayerLoseAction()
    {
        if(playerBetCount <= playerBulletCount)
        {
            playerBulletCount -= playerBetCount;
            enemyBulletCount += playerBetCount;
        }
        else
        {
            enemyBulletCount += playerBulletCount;
            playerBulletCount = 0;
        }
    }

    public void DrawAction()
    {
        
    }

    public void AddPlayerBetCount()
    {
        playerBetCount++;
        playerBetCount = Mathf.Clamp(playerBetCount, 0, playerBulletCount);
    }

    public void MinusPlayerBetCount()
    {
        playerBetCount--;
        playerBetCount = Mathf.Clamp(playerBetCount, 0, playerBulletCount);
    }

    public void ResetBullet()
    {
        playerBulletCount = 3;
        enemyBulletCount = 3;
    }



}
