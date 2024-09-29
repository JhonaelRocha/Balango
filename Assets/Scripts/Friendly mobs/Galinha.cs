using System.Collections;
using UnityEngine;

public class Galinha : MonoBehaviour
{
    Animator anim;
    CharacterController characterController;
    public float speed = 2f; // Velocidade padrão
    public float minTimeToWalk = 0.5f, maxTimeToWalk = 1.5f; // Intervalos de tempo para caminhar

    private Vector3 directionToWalk;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        StartCoroutine(WalkCoroutine());
    }

    void Update()
    {
        // Atualiza a animação de andar
        anim.SetBool("isWalk", directionToWalk != Vector3.zero);
    }

    void FixedUpdate()
    {
        // Move a galinha
        characterController.Move(directionToWalk * speed);

        // Gira a galinha na direção do movimento
        if (directionToWalk != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToWalk);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    IEnumerator WalkCoroutine()
    {
        while (true)
        {
            // Define uma nova direção de movimento aleatória
            directionToWalk = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            float timeToWalk = Random.Range(minTimeToWalk, maxTimeToWalk);
            yield return new WaitForSeconds(timeToWalk);

            // Para de andar
            directionToWalk = Vector3.zero;
            yield return new WaitForSeconds(timeToWalk);
        }
    }
}
