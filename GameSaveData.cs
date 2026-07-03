using System;
using System.Collections.Generic;

// 主存档类
[Serializable]
public class GameSaveData
{
    public List<ItemSaveData> items = new List<ItemSaveData>();
    // 可以在这里继续添加玩家数据、全局变量等
    // public PlayerSaveData playerData;
}

// 物品数据类
[Serializable]
public class ItemSaveData
{
    public string uniqueID;       // 唯一标识符，用于匹配场景中的物品
    public string prefabName;     // 预制体名称，用于动态生成的物品
    public bool isDynamic;        // 标记是否为游戏运行时动态生成的物品

    // 位置与旋转 (手动拆解以兼容JsonUtility)
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ, rotW;
}
