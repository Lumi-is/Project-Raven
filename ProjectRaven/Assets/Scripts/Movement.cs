using System.Collections;
using DG.Tweening;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputSystem_Actions playerInputs;

    private InputAction move;
    private InputAction dash;


    [Header("Physics")]
    [SerializeField] Rigidbody rb;

    Vector3 ProcessedDirection;


    [Header("Attributes")]
    public float speed;

    public float dashForce;
    public float dashDuration;
    public float dashCooldownAmount;

    private bool isDashing;
    private bool isDashOnCoolDown;

    [Header("Rhythm")]
    [SerializeField] BeatManager beatManager;

    [Header("Turning")]
    [SerializeField] Offensive attackScript;
    public Transform orientation;
    public Transform gazeFollowedAttackable;

    [Range(0,1000)]
    [Tooltip("The speed at which player rotates towards a given point.")]
    [SerializeField] float rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        move = playerInputs.Player.Move;
        dash = playerInputs.Player.Dash;
        move.Enable();
        dash.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        dash.Disable();
    }

    void Awake()
    {
        DOTween.Init();
        playerInputs = new InputSystem_Actions();
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
            rb.linearVelocity = new Vector3(speed * Time.deltaTime * ProcessedDirection.x, rb.linearVelocity.y, speed * Time.deltaTime * ProcessedDirection.z);

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
        }

        var step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation,orientation.rotation, step);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(beatManager.OnBeat && !isDashOnCoolDown && beatManager.cooldownUntilNextAction == 0)
            {
                beatManager.cooldownUntilNextAction = 1;
                isDashOnCoolDown = true;
                StartCoroutine(Dashing());
            } else {
                //Incorrect sound
            }
            
        }
    }

    IEnumerator Dashing()
    {
        isDashing = true;
        var DashDir = ProcessedDirection; 


        rb.DOMove(transform.position + DashDir * dashForce, dashDuration);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldownAmount-dashDuration);
        isDashOnCoolDown = false;
    }
}
