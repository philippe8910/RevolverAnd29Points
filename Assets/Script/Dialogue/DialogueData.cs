using UnityEngine;
using UnityEngine.UI;

public enum CameraTargetType
{
    Demon_Forward,
    Demon_RightSide,
    Demon_LeftSide,
    Demon_Back,
    Player
}

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueEntry
    {
        [TextArea]
        public string text; // 对话文本
        public Color textColor = Color.white; // 文字颜色
        public Font textFont; // 文字字体
        public bool enableShake = false; // 是否启用震动效果
        public CameraTargetType cameraTargetType = CameraTargetType.Player; // 摄像机目标类型
    }

    public DialogueEntry[] dialogues; // 对话项数组
    public float typingSpeed = 0.05f; // 打字机效果的速度
    public float displayTime = 2.0f; // 每句话的显示时间（可选）
    public Font defaultFont; // 预设字体
}
