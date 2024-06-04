using UnityEngine;

public interface IInteractable
{
    void Interact(); // 定义点击互动方法
    string GetName(); // 获取物品名称
    Vector3 GetTargetPosition(); // 获取物品在被选取时要移动到的位置
    Vector3 GetOriginalPosition(); // 获取物品的归位位置
    void OnInteractStart(); // 定义点击开始时的事件
    void OnInteractEnd(); // 定义点击结束后的事件
}
