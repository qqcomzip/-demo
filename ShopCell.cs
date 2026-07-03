using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour , IPointerClickHandler
{
    public int CellID;
    public int ItemID;
    public int StackedCount;

    [Header("References")]
    public GameObject UI_ItemDescribe;

    private Image iconImage;
    private TextMeshProUGUI countText;

    public GameObject N;
    public Item item;

    private void Start()
    {
        item = N.GetComponent<ItemDatabase>().Get(ItemID);
        iconImage = transform.Find("Image")?.GetComponent<Image>();
        countText = transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
        DrawCellUI();
    }

    public void DrawCellUI()
    {
        item = N.GetComponent<ItemDatabase>().Get(ItemID);

        if (iconImage != null)
            iconImage.sprite = item?.Image;

        if (countText != null)
            countText.text = StackedCount > 0 ? StackedCount.ToString() : 0.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || UI_ItemDescribe == null) return;
        
        var image = UI_ItemDescribe.GetComponent<Image>();
        var describe = UI_ItemDescribe.GetComponent<ItemDescribe_Cell>();

        if (image != null) image.sprite = item.Image;
        if (describe != null)
        {
            describe.Item = item;
            describe.StackedCount = StackedCount;
        }

        var t = UI_ItemDescribe.transform;
        var descText = t.Find("Image/Text (TMP)")?.GetComponent<TextMeshProUGUI>();
        if (descText != null) descText.text = item.Describe;
    }
}
