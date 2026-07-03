using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite Image;
    public int StackedUpperLimit = 99;
    public string Describe;
}
