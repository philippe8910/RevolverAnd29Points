using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResultCardText : MonoBehaviour
{
    public static ResultCardText instance;

    public void Awake()
    {
        instance = this;
    }

    public Transform textPosition;
    public TextMesh resultText;
    public Vector3 defaultPosition;
    void Start()
    {
        defaultPosition = transform.position;
    }

    public async void StartShowText(string text)
    {
        resultText.text = "" + CardSystem.instance.GetFirstEnemyCardName();
        resultText.transform.position = defaultPosition;

        resultText.transform.DOMove(textPosition.position, 0.2f).onComplete = () =>
        {
            Camera.main.transform.DOShakePosition(0.5f, 0.2f, 40, 90, false, true);
        };

        await Task.Delay(3000);

        resultText.text = "";
    }
}
