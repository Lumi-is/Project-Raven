using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Offensive : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputSystem_Actions playerInputs;

    private InputAction Attack;
    private InputAction Attack2;

    [Header("Rhythm")]
    [SerializeField] BeatManager beatManager;

    [Header("Weapons")]
    [SerializeField] WeaponPreset[] weaponArray = new WeaponPreset[4];
    [SerializeField] GameObject[] weaponPrefabArray = new GameObject[4];

    [Header("Movement")]
    [Tooltip("For movement scripts to check whether or not enemy is in attack range and then to do the correct actions accordingly.")]
    public bool isInAttackRange;
    [SerializeField] Movement movement;

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
        //Do not destroy on load
    }

    // Update is called once per frame
    void Update()
    {
        WeaponPreset currentWeapon = weaponArray[currentWeaponIndex];

        for(int i = 0; i < FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length; i++)
        {
            Transform EnemyTransform = FindObjectsByType<Enemy>(FindObjectsSortMode.None)[i].transform;            

            if(Vector3.Distance(transform.position, EnemyTransform.position) < currentWeapon.size && EnemyTransform != null)
            {
                Debug.Log("Check 1: "+isInAttackRange);
                isInAttackRange = true;
                Debug.Log("Check 2: "+isInAttackRange);
                movement.orientation.LookAt(EnemyTransform.position);
                Debug.Log("Check 3: "+isInAttackRange);

                //PROBLEM!
            }
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
        WeaponPreset currentWeapon = weaponArray[currentWeaponIndex];
        GameObject currentWeaponPrefab = weaponPrefabArray[currentWeaponIndex];
        Animator anim = currentWeaponPrefab.GetComponent<Animator>();        

        for(int i = 0; i < FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length; i++)
        {
            Transform EnemyTransform = FindObjectsByType<Enemy>(FindObjectsSortMode.None)[i].transform;

            if(Vector3.Distance(transform.position, EnemyTransform.position) < currentWeapon.size)
            {
                transform.LookAt(EnemyTransform.position);
                anim.SetTrigger("doPrimaryAttack");
                Enemy enemy = EnemyTransform.GetComponent<Enemy>();
                enemy.takeDamage(currentWeapon.damage);
            }
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
}
