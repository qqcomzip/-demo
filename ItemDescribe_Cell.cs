using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemDescribe_Cell : MonoBehaviour
{
    public Item Item;
    public int StackedCount;

    public void Equip()
    {
        var go = GameObject.Find("unitychan");
        if (go == null) return;

        // Unity 支持用路径字符串直接查找，无需链式调用
        const string handPath = "Character1_Reference/Character1_Hips/Character1_Spine/Character1_Spine1/Character1_Spine2/Character1_RightShoulder/Character1_RightArm/Character1_RightForeArm/Character1_RightHand/GameObject";
        var hand = go.transform.Find(handPath);
        if (hand == null) return;

        Addressables.InstantiateAsync("Axe").Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                var axe = op.Result;
                axe.transform.SetParent(hand, false);
                axe.transform.localPosition = Vector3.zero;
                axe.transform.localRotation = Quaternion.identity;
            }
        };
    }
}