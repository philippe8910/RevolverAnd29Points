using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public DialogueData dialogueData; // 对话数据
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    public FirstPersonController playerController;
    public List<Transform> cameraPositions = new List<Transform>();
    public List<Text> wordTexts = new List<Text>();

    public static DialogueManager instance;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerController = FindObjectOfType<FirstPersonController>();
        StartCoroutine(TypeDialogue());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            currentDialogueIndex++;
            if (currentDialogueIndex < dialogueData.dialogues.Length)
            {
                StartCoroutine(TypeDialogue());
            }
            else
            {
                foreach (var wordText in wordTexts)
                {
                    wordText.text = "";
                }

                Camera.main.transform.DOMove(cameraPositions[(int)CameraTargetType.Player].position, 1f);
            }
        }
    }

    IEnumerator TypeDialogue()
    {
        string _text = "";
        isTyping = true;
        foreach (var wordText in wordTexts)
        {
            wordText.text = "";
        }
        DialogueData.DialogueEntry currentDialogue = dialogueData.dialogues[currentDialogueIndex];

        SetupDialogueText(currentDialogue, wordTexts[(int)currentDialogue.cameraTargetType]);

        foreach (char letter in currentDialogue.text.ToCharArray())
        {
            _text += letter;
            MoveCameraToTargetAndTexting(currentDialogue.cameraTargetType, _text);

            if (currentDialogue.enableShake)
            {
                //ShakeText(wordTexts[(int)currentDialogue.cameraTargetType]);
            }
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        yield return new WaitForSeconds(dialogueData.displayTime);
    }

    void SetupDialogueText(DialogueData.DialogueEntry dialogueEntry , Text dialogueText)
    {
        dialogueText.color = dialogueEntry.textColor;
        dialogueText.font = dialogueEntry.textFont != null ? dialogueEntry.textFont : dialogueData.defaultFont;
    }

    void ShakeText(Text dialogueText)
    {
        dialogueText.transform.localPosition = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            dialogueText.transform.localPosition.z
        );
    }

    public void MoveCameraToTargetAndTexting(CameraTargetType cameraTargetType , string dialogieText)
    {
        if(cameraTargetType == CameraTargetType.Player)
        {
            playerController.enabled = true;
            return;
        }

        Transform target = cameraPositions[(int)cameraTargetType];
        Text targetText = wordTexts[(int)cameraTargetType];
        
        playerController.enabled = false;
        Camera.main.transform.DOMove(target.position, 1f);
        Camera.main.transform.DORotate(target.rotation.eulerAngles, 1f);
        targetText.text = dialogieText;
    }

    public void SetPlayerCamera()
    {
        StartCoroutine(SetPlayerCamera());
        playerController.enabled = false;

        IEnumerator SetPlayerCamera()
        {
            Camera.main.transform.DOMove(cameraPositions[5].position, 1f);
            Camera.main.transform.DORotate(cameraPositions[5].rotation.eulerAngles, 1f).onComplete += () =>
            {
                ResultCardText.instance.StartShowText("Tower III");
            };

            yield return new WaitForSeconds(4f);

            Camera.main.transform.DOMove(cameraPositions[4].position, 1f);
            Camera.main.transform.DORotate(cameraPositions[4].rotation.eulerAngles, 1f);
            playerController.enabled = true;
        }
    }
}
