using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player  : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private float velocityY = 0f; // Gravidade
    public float gravity = -9.81f;
    private bool isGrounded;

    public int numberOfPlayer;

    public Color[] playersColors;

    // Animação
    private Animator anim;

    void Start()
    {

        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        numberOfPlayer = players.Length;

        Debug.Log($"Jogador {numberOfPlayer} entrou.");
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < players.Length; i++)
        {
            meshes.ToList().ForEach(meshe => meshe.material.color = playersColors[i]);
        }
    }

    void FixedUpdate()
    {
        // Aplica o movimento com base na entrada
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        // Checar se o jogador está no chão
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocityY < 0)
        {
            velocityY = 0f; // Reseta a velocidade vertical quando no chão
        }

        // Aplica gravidade
        velocityY += gravity * Time.deltaTime;
        movement.y = velocityY;

        // Aplica o movimento com a velocidade do jogador
        characterController.Move(movement * speed * Time.deltaTime);

        // Define a animação de andar
        bool isWalk = movement.x != 0 || movement.z != 0;
        anim.SetBool("isWalk", isWalk);

        // Rotação suave em direção ao movimento
        if (movement.x != 0 || movement.z != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    // Método chamado pelo novo Input System para movimentação
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Armazena o input de movimento como um Vector2
    }

    // Método chamado pelo novo Input System para pulo
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            if(transform.localScale == new Vector3(1,1,1))
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
