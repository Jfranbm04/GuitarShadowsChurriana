using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class FaryAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    private bool puedePerseguir = false;

    void Awake()
    {
        // Obtenemos el componente de navegación
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Localizamos al jugador una sola vez
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null) playerTransform = jugador.transform;
    }

    void OnEnable()
    {
        // Reiniciamos el estado y empezamos la espera de 3 segundos
        puedePerseguir = false;
        StartCoroutine(SecuenciaAparicion());
    }

    IEnumerator SecuenciaAparicion()
    {
        // 1. Esperamos un frame para que el NavMeshAgent se posicione bien en el mapa
        yield return null;

        if (agent != null && agent.isActiveAndEnabled)
        {
            // 2. Intentamos colocar al agente en el NavMesh más cercano por si acaso
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }

            // 3. Ahora sí, lo paramos de forma segura
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        // 4. Esperamos los 3 segundos de rigor
        yield return new WaitForSeconds(3f);

        // 5. ¡A por el jugador!
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
            puedePerseguir = true;
        }
    }

    void Update()
    {
        // Si ya pasaron los 3 segundos, seguimos al jugador sin parar
        if (puedePerseguir && playerTransform != null && agent.isOnNavMesh)
        {
            agent.SetDestination(playerTransform.position);
        }
    }
}