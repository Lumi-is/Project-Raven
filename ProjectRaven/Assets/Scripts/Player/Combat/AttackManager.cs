using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    private InputAction Attack;
    private InputAction SecondaryAttack;

    [Header("Rhythm")]
    [SerializeField] LevelManager playerManager;

    [Header("Weapons")]
    [SerializeField] MeleeWeaponPreset[] weaponArray = new MeleeWeaponPreset[4];
    [SerializeField] GameObject[] weaponPrefabArray = new GameObject[4];

    [Header("Movement")]
    [Tooltip("For movement scripts to check whether or not enemy is in attack range and then to do the correct actions accordingly.")]
    public bool isInAttackRange;
    [SerializeField] MovementController movement;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Attack = InputSystem.actions.FindAction("Attack");
        SecondaryAttack = InputSystem.actions.FindAction("SecondaryAttack");
    }

    private Transform AttackableTransform;
    private MeleeWeaponPreset currentWeapon;


    // Update is called once per frame
    void Update()
    {
        
    }    

    public void PrimaryAttack()
    {
         
    }


    public void PerformSecondaryAttack()
    {

    }

    private Transform GetClosestAttackable(Transform[] attackables)
    {
        Transform tMin = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in attackables)
        {
            float distance = Vector3.Distance(t.position, currentPos);
            if (distance < minDistance)
            {
                tMin = t;
                minDistance = distance;
            }
        }
        return tMin;
    }
}
