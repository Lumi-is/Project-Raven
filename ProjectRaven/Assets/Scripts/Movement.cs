using System.Collections;
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

    [SerializeField] AudioSource DashSFX;

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
        playerInputs = new InputSystem_Actions();
    }

    // Update is called once per frame
    void Update()
    {
        var Matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));

        Vector3 MoveDirection = new(move.ReadValue<Vector2>().normalized.x, 0, move.ReadValue<Vector2>().normalized.y);
        ProcessedDirection = Matrix.MultiplyPoint3x4(MoveDirection);

        if(!isDashing)
        {
            rb.linearVelocity = new Vector3(speed * Time.deltaTime * ProcessedDirection.x, rb.linearVelocity.y, speed * Time.deltaTime * ProcessedDirection.z);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(beatManager.OnBeat && !isDashOnCoolDown && !beatManager.hasDoneActionOnCurrentBeat)
            {
                beatManager.hasDoneActionOnCurrentBeat = true;
                isDashOnCoolDown = true;
                StartCoroutine(Dashing());
                DashSFX.Play();
            } else {
                //Incorrect sound
            }
            
        }
    }

    IEnumerator Dashing()
    {
        isDashing = true;
        var DashDir = ProcessedDirection; 

        rb.AddForce(DashDir * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldownAmount-dashDuration);
        isDashOnCoolDown = false;
    }
}
