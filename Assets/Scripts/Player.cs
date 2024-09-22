using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public int numberOfPlayer;
    CharacterController characterController;
    public Color playerColor;
    public float speed = 5f;
    float moveX, moveZ;
    public Material material;
    private MeshRenderer[] meshes;

    // Gravidade
    public float gravity = -9.81f;
    private float velocityY = 0f; // Velocidade vertical

    // Checar se está no chão
    private bool isGrounded;

    //Animation
    Animator anim;
    bool isWalk;

    // Rotation
    public float rotationSpeed = 5f;

    void Start()
    {
        meshes = GetComponentsInChildren<MeshRenderer>();
        meshes.ToList().ForEach(meshe => meshe.material.color = playerColor);
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveX = Input.GetAxisRaw($"Horizontal {numberOfPlayer}");
        moveZ = Input.GetAxisRaw($"Vertical {numberOfPlayer}");

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized;

        // Checar se o jogador está no chão (usando o CharacterController)
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = 0f; // Reseta a velocidade vertical quando no chão
        }

        // Aplicar movimento
        Vector3 move = movement * speed;

        // Aplicar gravidade
        velocityY += gravity * Time.deltaTime;
        move.y = velocityY;

        characterController.Move(move * Time.deltaTime);

        // Definir se o personagem está andando
        isWalk = moveX != 0 || moveZ != 0;
        anim.SetBool("isWalk", isWalk);

        // Rotação suave
        if (movement != Vector3.zero) // Se houver movimento
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement); // Rotação para a direção do movimento
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Rotação suave
        }
    }

    void Update()
    {
        TestarComandosSeparados();
    }

    void TestarComandosSeparados()
    {
        if (Input.GetButton($"Jump {numberOfPlayer}"))
        {
            if (transform.localScale == new Vector3(1, 1, 1))
            {
                StartCoroutine(Crescer());
            }
        }
    }

    IEnumerator Crescer()
    {
        transform.localScale = new Vector3(2, 2, 2);
        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector3(1, 1, 1);
    }
}
