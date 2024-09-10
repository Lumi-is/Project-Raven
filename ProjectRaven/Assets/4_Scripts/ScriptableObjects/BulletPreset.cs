using UnityEngine;


[CreateAssetMenu(fileName = "Bullet",menuName = "New bullet/Create bullet preset")]
public class BulletPreset : ScriptableObject
{
    [Header("Stats: ")]
    public float damage;

    public float speed;

    public float lifeTime;

    [Header("Shape: ")]

    public Vector3 EndPosition; 

    public float size;

    public Collider collider;

    [Header("Visual: ")]

    public Color color;

    public GameObject model;

    [Header("AI")]
    public bool shootAtPlayer;
}
