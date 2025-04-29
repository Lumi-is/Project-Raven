using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputSystem_Actions playerInputs;

    private InputAction Attack;
    private InputAction Attack2;

    [Header("Rhythm")]
    [SerializeField] BeatManager beatManager;

    [Header("Weapons")]
    [SerializeField] MeleeWeaponPreset[] weaponArray = new MeleeWeaponPreset[4];
    [SerializeField] GameObject[] weaponPrefabArray = new GameObject[4];

    [Header("Movement")]
    [Tooltip("For movement scripts to check whether or not enemy is in attack range and then to do the correct actions accordingly.")]
    public bool isInAttackRange;
    [SerializeField] MovementController movement;

    [Header("Read-only")]
    public int currentWeaponIndex;

    void OnEnable()
    {
        Attack = playerInputs.Player.Attack;
        Attack2 = playerInputs.Player.SecondaryAttack;
        Attack.Enable();
        Attack2.Enable();
    }

    void OnDisable()
    {
        Attack.Disable();
        Attack2.Disable();
    }

    void Awake()
    {
        playerInputs = new InputSystem_Actions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private Transform AttackableTransform;
    private MeleeWeaponPreset currentWeapon;

    // Update is called once per frame
    void Update()
    {
        // Make list of enemies that are in viscinity and then sort them, then make player always turn towards the closest one and hit it

        Attackable[] attackables = FindObjectsByType<Attackable>(FindObjectsSortMode.None);
        Transform[] attackableTList = new Transform[attackables.Length];
        currentWeapon = weaponArray[currentWeaponIndex];

        for(int i = 0; i < attackables.Length; i++)
        {
            attackableTList[i] = attackables[i].transform;
        }   

        if(attackableTList == null) return;

        Transform closest = GetClosestAttackable(attackableTList);

        if(closest == null) return;

        if(Vector3.Distance(transform.position,closest.position) < currentWeapon.size)
        {
            isInAttackRange = true;
            AttackableTransform = closest;
            movement.gazeFollowedAttackable = AttackableTransform;
        } else {
            isInAttackRange = false;
        }
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        if(context.performed && beatManager.OnBeat && beatManager.cooldownUntilNextAction == 0)
        {
            beatManager.cooldownUntilNextAction = weaponArray[currentWeaponIndex].speed;
            PrimaryAttack();
        }
    }

    public void PrimaryAttack()
    {
        MeleeWeaponPreset currentWeapon = weaponArray[currentWeaponIndex];
        GameObject currentWeaponPrefab = weaponPrefabArray[currentWeaponIndex];
        Animator anim = currentWeaponPrefab.GetComponent<Animator>();        

        if(isInAttackRange)
        {               
            anim.SetTrigger("doPrimaryAttack");
            
            Attackable attackable = AttackableTransform.GetComponent<Attackable>();
            attackable.takeDamage(currentWeapon.damage);
        }       
    }

    public void RightClick(InputAction.CallbackContext context)
    {
        if(context.started && beatManager.OnBeat && beatManager.cooldownUntilNextAction == 0)
        {
            beatManager.cooldownUntilNextAction = weaponArray[currentWeaponIndex].speed;
            SecondaryAttack();
        }
    }

    public void SecondaryAttack()
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
