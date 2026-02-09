using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float radioMovimiento = 20f; // Qué tan lejos puede ir
    public float tiempoEspera = 2f;    // Cuánto espera al llegar
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Iniciamos el ciclo de movimiento
        StartCoroutine(RutinaMovimiento());
    }

    IEnumerator RutinaMovimiento()
    {
        while (true)
        {
            // 1. Buscar un punto aleatorio
            Vector3 destinoAleatorio = GenerarPuntoAleatorio(transform.position, radioMovimiento);

            // 2. Ordenar al agente que vaya allí
            agent.SetDestination(destinoAleatorio);

            // 3. Esperar hasta que el agente llegue al destino
            // Comprobamos si la distancia restante es pequeña
            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                yield return null;
            }

            // 4. Una vez llega, espera un tiempo antes de buscar otro punto
            yield return new WaitForSeconds(tiempoEspera);
        }
    }

    Vector3 GenerarPuntoAleatorio(Vector3 origen, float distancia)
    {
        // Genera una dirección aleatoria dentro de una esfera
        Vector3 direccionAleatoria = Random.insideUnitSphere * distancia;
        direccionAleatoria += origen;

        NavMeshHit hit;
        Vector3 puntoFinal = origen;

        // "SamplePosition" busca el punto más cercano en el NavMesh legal
        // para que el enemigo no intente irse fuera del mapa
        if (NavMesh.SamplePosition(direccionAleatoria, out hit, distancia, 1))
        {
            puntoFinal = hit.position;
        }

        return puntoFinal;
    }
}