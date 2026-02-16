using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 15f;

    [Header("Salto / Gravedad")]
    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    [SerializeField] private CharacterController characterController;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpRequested = false;
    [SerializeField] Animator animator;
    private GameObject player => this.gameObject;
    [Header("Dash")] 
    public float dashForce = 5f;
    public float dashCooldown = 1f;
    private bool dashOnCooldown = false;

    static public bool dialogueActive = false; // Variable estática para controlar el estado del diálogo
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

  

    private void OnJump(InputValue value)
    {
        //if (value.isPressed) jumpRequested = true;
        Dash();
    }

    private void Dash()
    {
        if (dashOnCooldown == false)
        {
            
            Vector3 forceToApply = new Vector3(moveInput.x * dashForce,0,moveInput.y * dashForce) ;
            forceToApply =  Quaternion.Euler(0, 45, 0) * forceToApply;
            characterController.Move(forceToApply);
            dashOnCooldown = true;
            StartCoroutine(DashCooldown());
         
        }
       
    }

    IEnumerator DashCooldown()
    {
       
            yield return new WaitForSeconds(dashCooldown);  
            dashOnCooldown = false;
            StopCoroutine(DashCooldown());
      
    }
    void Update()
    {
        if (characterController == null) return;

        // Si hay un diálogo, no procesamos el movimiento ni las animaciones
        if (dialogueActive)
        {
            // Opcional: Forzar animaciones a Idle si quieres
            animator.SetFloat("X", 0);
            animator.SetFloat("Y", 0);
            return;
        }

        ControlMovimiento();
        ControlAnimacion();
    }

    private void ControlMovimiento()
    {
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        moveDirection = Quaternion.Euler(0, 45, 0) * moveDirection;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection, player.transform.up);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, newRotation, Time.deltaTime * 8);
        }

        if (moveDirection.sqrMagnitude > 1f) moveDirection.Normalize();

        Vector3 finalVelocity = moveDirection * moveSpeed;

        if (isGrounded && jumpRequested)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }

        verticalVelocity += gravity * Time.deltaTime;
        finalVelocity.y = verticalVelocity;

        characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void ControlAnimacion()
    {
        // Obtenemos la velocidad horizontal (ignorando el eje Y de la gravedad)
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        // Si la velocidad es mayor a 0.1, activamos la animaci�n
        if (currentSpeed > 0.1f)
        {
            animator.SetFloat("X", 1f);
            animator.SetFloat("Y", 1f);
        }
        else
        {
            // Si est� quieto, volvemos a Idle (0,0)
            animator.SetFloat("X", 0f);
            animator.SetFloat("Y", 0f);
        }
    }
}