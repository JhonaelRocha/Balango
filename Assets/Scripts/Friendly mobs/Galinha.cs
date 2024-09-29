using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Importa o NavMesh

public class Galinha : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    public float speed = 2f; // Velocidade padrão
    public float minTimeToWalk = 0.5f, maxTimeToWalk = 1.5f; // Intervalos de tempo para caminhar
    public float walkRadius = 5f; // Raio de movimento aleatório

    private Vector3 directionToWalk;

    void Start()
    {
        // Inicializa o NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed; // Define a velocidade do NavMeshAgent

        anim = GetComponent<Animator>();

        // Inicia a corrotina de movimento
        StartCoroutine(WalkCoroutine());
    }

    void Update()
    {
        // Atualiza a animação de andar
        anim.SetBool("isWalk", agent.velocity.magnitude > 0.1f);
    }

    IEnumerator WalkCoroutine()
    {
        while (true)
        {
            // Escolhe um ponto aleatório dentro de um raio para andar
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position; // Calcula a posição de destino
            NavMeshHit hit;
            
            // Verifica se o ponto aleatório está em uma área navegável do NavMesh
            if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position); // Define o destino no NavMesh
            }

            // Tempo aleatório de caminhada antes de escolher um novo destino
            float timeToWalk = Random.Range(minTimeToWalk, maxTimeToWalk);
            yield return new WaitForSeconds(timeToWalk);

            // Para a galinha por um tempo aleatório
            agent.ResetPath(); // Para o movimento do agente
            yield return new WaitForSeconds(timeToWalk);
        }
    }
}