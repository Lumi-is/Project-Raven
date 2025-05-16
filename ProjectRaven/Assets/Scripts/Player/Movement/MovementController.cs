using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Inputs")]

    private InputAction move;
    private InputAction dash;


    [Header("Transform")]
    public Transform playerTransform;
    Vector3 ProcessedDirection;


    [Header("Dash Attributes")]
    public float speed;
    public float dashForce;
    public float dashDuration;
    public float failedDashDelay;
    private bool isDashing;
    private bool canDash = true;


    [Header("Turning")]
    [SerializeField] AttackManager attackScript;
    public Transform orientation;
    public Transform gazeFollowedAttackable;

    [Range(0,1000)]
    [Tooltip("The speed at which player rotates towards a given point.")]
    [SerializeField] float rotationSpeed;

    void Awake()
    {
        move = InputSystem.actions.FindAction("Move");
        dash = InputSystem.actions.FindAction("Dash");
        DOTween.Init();
    }

    //Direction where player character looks if no actions are pressed 
    private Vector3 fallbackDirection;
   
    void Update()
    {
        //Rotates world Vector3 Matrix for 45 degrees in order for X and Z axis to work well with an isometric camera.
        var Matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));

        //Normalizes a direction to move to based on the before-defined matrix and device input.
        Vector3 MoveDirection = new(move.ReadValue<Vector2>().normalized.x, 0, move.ReadValue<Vector2>().normalized.y);
        ProcessedDirection = Matrix.MultiplyPoint3x4(MoveDirection);

        if(!isDashing)
        {
            playerTransform.position += new Vector3(speed * ProcessedDirection.x * Time.deltaTime, 0, speed * ProcessedDirection.z * Time.deltaTime);

            if(!attackScript.isInAttackRange)
            {  
                if(ProcessedDirection != new Vector3(0,0,0))
                {
                    orientation.LookAt(transform.position + ProcessedDirection);
                    fallbackDirection = transform.position + ProcessedDirection;
                } else {
                    orientation.LookAt(fallbackDirection);
                }
            } else {
                orientation.LookAt(gazeFollowedAttackable.position);
            }
        } else {
            orientation.LookAt(playerTransform.position + playerTransform.forward);
        }

        if(!LevelEventManager.OnBeat && dash.WasPerformedThisFrame())
        {
            //Dash delay
        }

        if(LevelEventManager.OnBeat && canDash && LevelEventManager.cooldownUntilNextAction == 0 && dash.WasPerformedThisFrame())
        {
            StartCoroutine(Dash());
        } 

        var step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation,orientation.rotation, step);
    }
    

    IEnumerator Dash()
    {
        isDashing = true;
        LevelEventManager.cooldownUntilNextAction = 1;

        var DashDir = ProcessedDirection; 
        playerTransform.DOMove(transform.position + DashDir * dashForce, dashDuration);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
}
