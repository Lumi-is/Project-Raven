using UnityEngine;


[CreateAssetMenu(fileName = "Weapon",menuName = "New weapon/Create weapon preset")]
public class WeaponPreset : ScriptableObject
{
    [Header("UI elements")]
    public string weaponName;
    public string description;
    public Texture2D icon;

    [Header("Attributes")]
    public float damage;
    public float size;

    [Tooltip("How many half beats until weapon can be used again.")]
    public int speed;
}
