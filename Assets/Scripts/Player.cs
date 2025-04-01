using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables de movimiento
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;
    public Animator animator;

    public CharacterController player;
    public float playerSpeed;
    private Vector3 movePlayer;

    // Variables de gravedad
    public float gravity = 9.8f;
    public float fallVelocity = 40;
    public float jumpForce;

    // Variables de cámara
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;

    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Obtiene el movimiento horizontal y vertical
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");


        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        // Configura la dirección de la cámara
        camDirection();

        // Calcula el movimiento del jugador basado en la cámara
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer *= playerSpeed;

        // Rota el jugador en la dirección del movimiento
        player.transform.LookAt(player.transform.position + movePlayer);

        // Aplica la gravedad
        SetGravity();

        PlayerSkills();

        //INDICA SI TOCA EL SUELO
        animator.SetBool("IsGrounded", player.isGrounded);


        //ENVIA VELOCIDAD AL ARBOL DE ANIMACION
        animator.SetFloat("Velocity", player.velocity.magnitude);

        // Mueve al jugador
        player.Move(movePlayer * Time.deltaTime);
    }

    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            animator.SetTrigger("");
        }
    }

    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
       SlideDown();
    }
     
    void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;
        if (isOnSlope) 
        {
            //DESLIZA EN EJE X
            movePlayer.x += ((1 - hitNormal.x) * hitNormal.x) * slideVelocity;
            //DESLIZA EN EJE Z
            movePlayer.z += ((1 - hitNormal.z) * hitNormal.z) * slideVelocity;
            //DESLIZA EN EJE Y
            movePlayer.y = slopeForceDown;
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal; 
    }


}
