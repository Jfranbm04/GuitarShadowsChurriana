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
        if (value.isPressed) jumpRequested = true;
    }


    void Update()
    {
        if (characterController == null) return;
        ControlMovimiento();
        ControlAnimacion();
    }

    private void ControlMovimiento()
    {
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

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

        // Si la velocidad es mayor a 0.1, activamos la animación
        if (currentSpeed > 0.1f)
        {
            // Como tu Blend Tree tiene el Running en (1,1), enviamos 1 a ambos parámetros
            // Si quieres que sea gradual, podrías usar: currentSpeed / moveSpeed
            animator.SetFloat("X", 1f);
            animator.SetFloat("Y", 1f);
        }
        else
        {
            // Si está quieto, volvemos a Idle (0,0)
            animator.SetFloat("X", 0f);
            animator.SetFloat("Y", 0f);
        }
    }
}