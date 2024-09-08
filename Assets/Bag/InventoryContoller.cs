using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;  // 背包面板
    public GameObject[] itemPrefabs;   // 3D物品的预制体数组
    public Transform spawnPoint;       // 物品生成的位置
    public GameObject currentItem;     // 当前持有的物品
    public Transform cameraTransform;  // 摄像机的Transform
    public float adjustSpeed = 0.1f;   // 调整物品距离的速度
    public float detectionRadius = 2.0f; // 检测范围半径
    public float rotationSpeed = 100.0f; // 旋转速度

    private bool isInventoryOpen = false; // 背包是否打开的状态

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // 检测B键是否按下
        {
            ToggleInventory();
        }

        if (currentItem != null)
        {
            HandleItemManipulation();
        }

        if (Input.GetKeyDown(KeyCode.X)) // 销毁附近物体
        {
            DestroyNearbyItem();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
    }

    public void OnItemClick(int itemIndex)
    {
        if (currentItem != null) // 如果已有物品，先销毁它
        {
            Destroy(currentItem);
        }

        // 生成选择的3D物品
        currentItem = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, spawnPoint.rotation);
        currentItem.transform.SetParent(cameraTransform); // 将物品设置为摄像机的子对象
        currentItem.transform.localPosition = new Vector3(0, 0, 2); // 调整物品相对于摄像机的本地位置
        currentItem.transform.localRotation = Quaternion.identity; // 重置旋转

        ToggleInventory(); // 关闭背包
    }

    void HandleItemManipulation()
    {
        if (Input.GetKeyDown(KeyCode.E)) // 放下物品
        {
            PlaceItem();
        }
        else if (Input.GetKey(KeyCode.F)) // 拿远
        {
            AdjustItemDistance(adjustSpeed);
        }
        else if (Input.GetKey(KeyCode.C)) // 拿进
        {
            AdjustItemDistance(-adjustSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.Z)) // 取消
        {
            CancelItem();
        }
        else if (Input.GetKey(KeyCode.R)) // 沿Y轴旋转
        {
            currentItem.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.T)) // 沿X轴旋转
        {
            currentItem.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Y)) // 沿Z轴旋转
        {
            currentItem.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        //放大缩小
        else if (Input.GetKey(KeyCode.U)) // 放大
        {
            currentItem.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        }
        else if (Input.GetKey(KeyCode.I)) // 缩小
        {
            currentItem.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    public void AdjustItemDistance(float adjustment)
    {
        // 调整物品与摄像机之间的距离
        currentItem.transform.localPosition += new Vector3(0, 0, adjustment);
    }

    void PlaceItem()
    {
        currentItem.transform.SetParent(null); // 解除父子关系，使物品留在当前场景

    // 确保物品有Collider组件，如果没有则添加一个合适的Collider
        if (currentItem.GetComponent<Collider>() == null)
        {
            currentItem.AddComponent<BoxCollider>(); // 默认添加一个BoxCollider
        }

    // 检查Shift键是否按下
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // 添加Rigidbody组件，使物品受到重力影响并参与物理碰撞
            Rigidbody rb = currentItem.AddComponent<Rigidbody>();

            // 可以根据需要调整Rigidbody的属性
            rb.mass = 1.0f; // 设置质量
            rb.drag = 0.5f; // 设置阻力
            rb.angularDrag = 0.05f; // 设置角阻力
            rb.useGravity = true; // 确保物品受重力影响
        }

        currentItem = null; // 物品已放置
    }


    void CancelItem()
    {
        Destroy(currentItem); // 取消并销毁物品
        currentItem = null;
    }

    void DestroyNearbyItem()
    {
        // 获取当前玩家的位置
        Vector3 playerPosition = cameraTransform.position;

        // 在玩家附近检测所有Collider
        Collider[] colliders = Physics.OverlapSphere(playerPosition, detectionRadius);

        // 遍历所有检测到的Collider
        foreach (Collider collider in colliders)
        {
            // 排除玩家自身的Collider，并确保检测到的是物品
            if (collider.gameObject != this.gameObject && collider.attachedRigidbody != null)
            {
                Destroy(collider.gameObject); // 销毁检测到的物品
                break; // 销毁一个物品后退出循环
            }
        }
    }
}
