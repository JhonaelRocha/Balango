using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController characterController;
    public float speed = 5f;
    float moveX, moveZ;

    //Animation
    Animator anim;
    bool isWalk;

    // Rotation
    public float rotationSpeed = 5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized;

        // Movimento do player
        characterController.Move(movement * speed * 0.1f);

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
}
