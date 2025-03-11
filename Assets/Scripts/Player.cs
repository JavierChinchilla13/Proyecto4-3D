using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables de movimiento
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;

    public CharacterController player;
    public float playerSpeed;
    private Vector3 movePlayer;

    // Variables de gravedad
    public float gravity = 9.8f;
    public float fallVelocity = 40;

    // Variables de cámara
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    void Start()
    {
        player = GetComponent<CharacterController>();
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

    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }
        movePlayer.y = fallVelocity;
    }
}
