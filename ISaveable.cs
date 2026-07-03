public interface ISaveable
{
    string GetUniqueID();
    ItemSaveData CaptureState();  // 抓取当前状态
    void RestoreState(ItemSaveData data); // 恢复状态
}
