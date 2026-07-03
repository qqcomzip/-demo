using UnityEngine;
using System;

public class GameItem : MonoBehaviour, ISaveable
{
    [Header("存档设置")]
    [SerializeField] private string uniqueID;
    [SerializeField] private string prefabName;
    [SerializeField] private bool isDynamic = false; // 运行时生成的物品勾选此项

    public string GetUniqueID() => uniqueID;

    // 抓取数据
    public ItemSaveData CaptureState()
    {
        return new ItemSaveData
        {
            uniqueID = this.uniqueID,
            prefabName = this.prefabName,
            isDynamic = this.isDynamic,
            posX = transform.position.x,
            posY = transform.position.y,
            posZ = transform.position.z,
            rotX = transform.rotation.x,
            rotY = transform.rotation.y,
            rotZ = transform.rotation.z,
            rotW = transform.rotation.w,
        };
    }

    // 恢复数据
    public void RestoreState(ItemSaveData data)
    {
        transform.position = new Vector3(data.posX, data.posY, data.posZ);
        transform.rotation = new Quaternion(data.rotX, data.rotY, data.rotZ, data.rotW);
    }

    // 编辑器小工具：右键组件 -> Generate Unique ID
    [ContextMenu("Generate Unique ID")]
    private void GenerateID()
    {
        uniqueID = Guid.NewGuid().ToString();
    }

    // 如果是代码动态生成的物品，在Awake中自动生成ID
    private void Awake()
    {
        if (isDynamic && string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }
    }
}
