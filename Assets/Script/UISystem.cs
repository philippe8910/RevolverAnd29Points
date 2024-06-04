using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public static UISystem instance;

    void Awake()
    {
        instance = this;
    }

    [SerializeField]
    public Text subtitleText;

    [SerializeField]
    public Text interactableText;

    [SerializeField]
    public Text enemyCardCountText;

    [SerializeField]
    public Text playerCardCountText;

    void Update()
    {
        if(CardSystem.instance.playerCards.Count > 0)
        {
            SetPlayerCardCountText(CardSystem.instance.GetPlayerCardCount() + "/29");
        }

        if(CardSystem.instance.enemyCards.Count > 0)
        {
            SetEnemyCardCountText("? + " + CardSystem.instance.GetEnemyCardCountFormTable() + "/29");
        }
    }

    public void SetSubtitleText(string text)
    {
        subtitleText.text = text;
    }

    public void SetInteractableText(string text)
    {
        interactableText.text = text;
    }

    public void SetEnemyCardCountText(string text)
    {
        enemyCardCountText.text = text;
    }

    public void SetPlayerCardCountText(string text)
    {
        playerCardCountText.text = text;
    }
}
