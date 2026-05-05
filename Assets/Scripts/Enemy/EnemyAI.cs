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

    [Header("Animación")]
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // Obtenemos el componente Animator

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
        ActualizarAnimaciones(); // Llamamos a la función de animación
    }

    void ActualizarAnimaciones()
    {
        if (anim != null && agent != null)
        {
            // Calculamos la velocidad actual del agente
            float velocidadActual = agent.velocity.magnitude;

            // Enviamos el valor al parámetro "Velocidad" del Blend Tree
            anim.SetFloat("Velocidad", velocidadActual);
        }
    }

    void ComprobarDistanciaJugador()
    {
        if (playerTransform == null) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= radioDeteccion)
        {
            siguiendoAlJugador = true;
            agent.SetDestination(playerTransform.position);
            agent.stoppingDistance = 1.2f;
        }
        else
        {
            if (siguiendoAlJugador)
            {
                siguiendoAlJugador = false;
                agent.stoppingDistance = 0f;
            }
        }
    }

    IEnumerator RutinaPatrulla()
    {
        while (true)
        {
            if (!siguiendoAlJugador)
            {
                Vector3 destino = GenerarPuntoEnZona(centroDeZona.position, radioDeZona);
                agent.SetDestination(destino);

                while (!siguiendoAlJugador && (agent.pathPending || agent.remainingDistance > 0.5f))
                {
                    yield return null;
                }

                if (!siguiendoAlJugador)
                    yield return new WaitForSeconds(tiempoEspera);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

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