using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float maxLookAngle = 85.0f;
    public float rayDistance = 5.0f; // RayCast距離
    public LayerMask layerMask; // RayCast的LayerMask

    private float verticalRotation = 0;
    private Transform playerBody; // 用於保存玩家的 Transform

    void Start()
    {
        // 鎖定滑鼠
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 獲取玩家的 Transform
        playerBody = transform;
    }

    void Update()
    {
        // 獲取滑鼠輸入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 水平旋轉玩家
        playerBody.Rotate(Vector3.up * mouseX);

        // 垂直旋轉相機
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // 獲取鍵盤輸入
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        // 移動玩家
        Vector3 move = playerBody.right * moveSide + playerBody.forward * moveForward;
        playerBody.position += move;

        // 執行RayCast
        PerformRayCast();
    }

    void PerformRayCast()
    {
        // 建立Ray從相機的中心點
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // 執行RayCast
        if (Physics.Raycast(ray, out hit, rayDistance ,layerMask))
        {
            if(hit.collider.GetComponent<IInteractable>() != null)
            {
                var interactable = hit.collider.GetComponent<IInteractable>();

                UISystem.instance.SetInteractableText(interactable.GetName());

                if(Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }

            // 可以在這裡添加更多的互動邏輯，例如：
            // if (hit.collider.CompareTag("Interactable"))
            // {
            //     // 與可互動的物體進行互動
            // }
        }
        else
        {
            Debug.Log("No Hit");
            UISystem.instance.SetInteractableText("");
        }
    }

    public void OnEnable()
    {
        Camera.main.transform.DOMove(new Vector3(2.36f, 1.73f, -0.1500002f), 1f);
    }
}
