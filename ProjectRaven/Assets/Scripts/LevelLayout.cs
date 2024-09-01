using UnityEngine;

public class LevelLayout : MonoBehaviour
{
    public int indexTag;
    public void OnClick()
    {
        LevelEditor levelEditor = FindAnyObjectByType<LevelEditor>();
        levelEditor.OpenPreviewWindow(indexTag);
    }
}
