using UnityEngine;

public class UIArrayNode : MonoBehaviour
{
    public int indexTag;
    public void OnClick()
    {
        LevelEditor levelEditor = FindAnyObjectByType<LevelEditor>();
        levelEditor.OpenPreviewWindow(indexTag);
    }
}
