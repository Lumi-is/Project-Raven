using UnityEngine;

public class UIArrayNode : MonoBehaviour
{
    public LevelEditor levelEditor;
    public int indexTag;
    public void OnClick()
    {
        levelEditor.OpenPreviewWindow(indexTag);
    }
}
