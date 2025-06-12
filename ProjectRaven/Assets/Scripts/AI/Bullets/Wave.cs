using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Wave : MonoBehaviour, IBullet
{
    public void OnBeatChange()
    {
        transform.localScale = transform.localScale + new Vector3(1,0,1);
    }
}
