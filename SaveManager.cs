using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string savePath;
    public int ArchiveUIID;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        // 存档路径：C:\Users\用户名\AppData\LocalLow\CompanyName\ProjectName\save.json
        savePath = Path.Combine(Application.persistentDataPath, $"savegame{ArchiveUIID}.json");
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData();

        // 找到场景中所有实现了 ISaveable 接口的对象
        var saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();

        foreach (var sav in saveables)
        {
            saveData.items.Add(sav.CaptureState());
        }

        // 序列化为 JSON 并写入文件
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("游戏已保存至: " + savePath);
    }

    /// <summary>
    /// 加载游戏
    /// </summary>
    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("没有找到存档文件！");
            return;
        }

        string json = File.ReadAllText(savePath);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        // 1. 收集当前场景中的所有可保存对象，放入字典方便查找
        var currentItems = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToList();
        Dictionary<string, ISaveable> currentDict = new Dictionary<string, ISaveable>();
        foreach (var item in currentItems)
        {
            currentDict[item.GetUniqueID()] = item;//排序
        }

        HashSet<string> loadedIDs = new HashSet<string>();

        // 2. 遍历存档数据，进行匹配和生成
        foreach (var itemData in saveData.items)
        {
            loadedIDs.Add(itemData.uniqueID);

            if (currentDict.TryGetValue(itemData.uniqueID, out ISaveable existingItem))
            {
                // 【情况A】场景里已有该物品，直接覆盖数据
                existingItem.RestoreState(itemData);
            }
            else
            {
                // 【情况B】场景里没有，但存档里有（说明是动态生成后掉落的物品）
                if (itemData.isDynamic)
                {
                    // 从 Resources 文件夹加载预制体 (你也可以用 Addressables)
                    GameObject prefab = Resources.Load<GameObject>(itemData.prefabName);
                    if (prefab != null)
                    {
                        GameObject newObj = Instantiate(prefab);
                        ISaveable newItem = newObj.GetComponent<ISaveable>();
                        if (newItem != null)
                        {
                            newItem.RestoreState(itemData);
                        }
                    }
                }
            }
        }

        // 3. 清理场景中有，但存档中没有的物品（说明在游戏过程中被丢弃/销毁了）
        foreach (var kvp in currentDict)
        {
            if (!loadedIDs.Contains(kvp.Key))
            {
                // 销毁该物品
                Destroy(((MonoBehaviour)kvp.Value).gameObject);
            }
        }

        Debug.Log("游戏加载完成！");
    }
}
