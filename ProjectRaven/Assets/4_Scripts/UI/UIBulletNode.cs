using UnityEngine;

public class UIBulletNode : MonoBehaviour
{
    public LevelEditor levelEditor;
    public int indexTag;
    public void OnClick()
    {
        //Open preview window
        levelEditor.OpenPreviewWindow();
    }
}
