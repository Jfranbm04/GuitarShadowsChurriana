using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Configuración de Zona")]
    public Transform centroDeZona;
    public float radioDeZona = 20f;

    [Header("Detección del Jugador")]
    public float radioDeteccion = 5f; 
    public string tagJugador = "Player"; 
    private Transform playerTransform;
    private bool siguiendoAlJugador = false;

    [Header("Ajustes de Movimiento")]
    public float tiempoEspera = 2f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Buscamos al jugador automáticamente por su etiqueta
        GameObject jugador = GameObject.FindGameObjectWithTag(tagJugador);
        if (jugador != null) playerTransform = jugador.transform;

        // Aseguramos posición inicial en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, agent.areaMask))
        {
            agent.Warp(hit.position);
        }

        StartCoroutine(RutinaPatrulla());
    }

    void Update()
    {
        ComprobarDistanciaJugador();
    }

    void ComprobarDistanciaJugador()
    {
        if (playerTransform == null) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= radioDeteccion)
        {
            // ESTADO: PERSEGUIR
            siguiendoAlJugador = true;
            agent.SetDestination(playerTransform.position);
            agent.stoppingDistance = 1.2f; // Se detiene un poco antes de chocar para que la animación se vea bien
        }
        else
        {
            // ESTADO: VOLVER A PATRULLAR
            if (siguiendoAlJugador)
            {
                siguiendoAlJugador = false;
                agent.stoppingDistance = 0f; // Resetear distancia para que llegue bien a los puntos de patrulla
            }
        }
    }

    IEnumerator RutinaPatrulla()
    {
        while (true)
        {
            // Si NO estamos siguiendo al jugador, buscamos puntos aleatorios
            if (!siguiendoAlJugador)
            {
                Vector3 destino = GenerarPuntoEnZona(centroDeZona.position, radioDeZona);
                agent.SetDestination(destino);

                // Esperar a llegar, pero interrumpir si ve al jugador
                while (!siguiendoAlJugador && (agent.pathPending || agent.remainingDistance > 0.5f))
                {
                    yield return null;
                }

                if (!siguiendoAlJugador)
                    yield return new WaitForSeconds(tiempoEspera);
            }
            else
            {
                // Si está persiguiendo, esperamos un frame y volvemos a chequear
                yield return null;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Color de la esfera
        Gizmos.color = Color.yellow;

        // Dibuja una esfera de alambre con el radio de detección
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        // Opcional: Dibuja el radio de patrulla en otro color
        if (centroDeZona != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(centroDeZona.position, radioDeZona);
        }
    }
    Vector3 GenerarPuntoEnZona(Vector3 centro, float radio)
    {
        Vector3 direccionAleatoria = Random.insideUnitSphere * radio;
        direccionAleatoria += centro;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(direccionAleatoria, out hit, radio, agent.areaMask))
        {
            return hit.position;
        }
        return centro;
    }
}