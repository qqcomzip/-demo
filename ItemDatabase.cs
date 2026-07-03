using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items;

    private Dictionary<int, Item> cache;

    private void Awake()
    {
        cache = new Dictionary<int, Item>();
        foreach (var it in items) {
            cache[it.ID] = it;
        } 
    }

    public Item Get(int id)
    {
        cache.TryGetValue(id, out var it);
        return it;
    }
}
