using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Movimiento")]
    public float moveSpeed = 5.0f;

    [Header("Salto / Gravedad")]
    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    [SerializeField] private CharacterController characterController;
    //[SerializeField] private HacerseHijoMano hacerseHijoMano;

    [SerializeField] private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpRequested = false;

    //[SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        //hacerseHijoMano = GetComponent<HacerseHijoMano>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController == null) return;
        ControlMovimiento();

        ControlAnimacion();

    }

    //private void OnSoltar(InputValue value)
    //{
    //    hacerseHijoMano.SoltarPalo();

    //}
    private void ControlAnimacion()
    {
        Vector3 velocidad = characterController.velocity;
        Vector3 movimientoLocal = characterController.transform.InverseTransformDirection(velocidad);

        //animator.SetFloat("X", movimientoLocal.x);
        //animator.SetFloat("Y", movimientoLocal.z);
        //animator.SetBool("EnSuelo", characterController.isGrounded);
        //animator.SetFloat("Z", verticalVelocity);

    }

    private void OnJump(InputValue value) {
        if (value.isPressed) jumpRequested = true;
    }

    


    private void ControlMovimiento()
    {
        bool isGrounded = characterController.isGrounded;
        // Reset vertical
        if (isGrounded && verticalVelocity < 0f) verticalVelocity = - 2f;

        // Movimiento local XZ
        Vector3 localMove = new Vector3(moveInput.x, 0, moveInput.y);

        // Convertir de local a mundo
        Vector3 worldMove = transform.TransformDirection(localMove);

        if (worldMove.sqrMagnitude > 1f) worldMove.Normalize();
        
        Vector3 horizontalvelocity = worldMove * moveSpeed;

        // Salto
        if(isGrounded && jumpRequested)
        {
            //animator.SetTrigger("Saltar");
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;


        }



        // Movimiento
        verticalVelocity += gravity * Time.deltaTime;
        //Vector3 velocity = horizontalvelocity;
        //velocity.y = verticalVelocity;
        horizontalvelocity.y = verticalVelocity;
        characterController.Move(horizontalvelocity * Time.deltaTime);

    }
}
