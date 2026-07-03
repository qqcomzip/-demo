using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Container : MonoBehaviour
{
    public int CellCount;
    public Transform Content;

    public List<Cell> Cells = new List<Cell>();

    public GameObject UI_ItemDescribe;
    public GameObject N;

    public GameObject ShopItem;

      public Cell CellPrefab;
    public Transform CellParent;

    private void Awake() => InitializeContainer();

    public void InitializeContainer()
    {
        if (Cells == null || Cells.Count() < CellCount) Cells = new List< Cell > ();

        // 如果 Cells 为空，自动从 Content 下收集
        if (Cells.Count == 0)
        {
            for (int i = 0; i < CellCount; i++)
            {
                Cell cell = Instantiate(CellPrefab, CellParent);
                cell.CellID = i;
                cell.ItemID = 0;
                cell.StackedCount = 0;
                cell.Container = this;
                cell.UI_ItemDescribe = UI_ItemDescribe;
                cell.N = N;
                cell.item = N.GetComponent<ItemDatabase>().Get(0);
                Cells.Add(cell);
            }
        }
    }

    public Cell Get(int id) => Cells.FirstOrDefault(c => c.CellID == id);

    public Cell Add(int ItemID, int amount = 1)
    {
        // 1. 先尝试堆叠到已有同类物品且未满的槽位
        Debug.Log("1");
    var slot = Cells.FirstOrDefault(c =>
        c.ItemID == ItemID &&
        c.StackedCount < N.GetComponent<ItemDatabase>().Get(ItemID).StackedUpperLimit);
        Debug.Log("2");
    if (slot != null)
    {
            Debug.Log("3");
        int canAdd = Mathf.Min(amount, slot.N.GetComponent<ItemDatabase>().Get(ItemID).StackedUpperLimit - slot.StackedCount);
        slot.StackedCount += canAdd;
        slot.DrawCellUI();
        return slot;
    }

        Debug.Log("4");
        // 2. 再找空槽位（Item 为 null 或 ID 为 0 表示空）
        slot = Cells.FirstOrDefault(c => c.ItemID == 0);
        Debug.Log(slot.ItemID);
        
        if (slot != null)
    {
            Debug.Log("5");
            slot.ItemID = ItemID;
        slot.StackedCount = Mathf.Min(amount, slot.N.GetComponent<ItemDatabase>().Get(ItemID).StackedUpperLimit);
        slot.DrawCellUI();
        return slot;
    }
        Debug.Log("6");
    return null; // 背包已满
    }

public Cell Delete(int ItemID, int amount = 1)
{
    var slot = Cells.FirstOrDefault(c =>c.ItemID == ItemID);
    if (slot == null || slot.StackedCount < amount) return null;
    if(slot.StackedCount <= amount)
        {
            if(slot.StackedCount == amount)
            {
                slot.StackedCount = 0;
                slot.ItemID = 0;
            }
            else
            {
                Debug.Log("数量不足");
            }

        }
        else
        {
            slot.StackedCount -= amount;
        }
    slot.DrawCellUI();
    return slot;
}
    public void Button8()
    {
        Add(ShopItem.GetComponent<ItemDescribe_Cell>().Item.ID,1);
    }

    public void Button9()
    {
        Delete(ShopItem.GetComponent<ItemDescribe_Cell>().Item.ID,1);
    }
}