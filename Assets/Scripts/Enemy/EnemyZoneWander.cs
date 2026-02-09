using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyZoneWander : MonoBehaviour
{
    [Header("Configuración de Zona")]
    public Transform zoneCenter; 
    public float wanderRadius = 15f; 

    [Header("Configuración de Tiempos")]
    public float waitTime = 0.5f; // Cuánto tiempo espera al llegar a un punto

    private NavMeshAgent agent;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Si olvidaste asignar un centro, usa su posición inicial
        if (zoneCenter == null)
        {
            GameObject centerObj = new GameObject(gameObject.name + "_Center");
            centerObj.transform.position = transform.position;
            zoneCenter = centerObj.transform;
        }

        GoToNewRandomLocation();
    }

    void Update()
    {
        if (!agent.isOnNavMesh || !agent.isActiveAndEnabled) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;


        yield return new WaitForSeconds(waitTime);

        GoToNewRandomLocation();

        isWaiting = false;
    }

    void GoToNewRandomLocation()
    {
        Vector3 newPos = GetRandomLocationInZone();
        agent.SetDestination(newPos);
    }

    Vector3 GetRandomLocationInZone()
    {
        // Calculamos el punto aleatorio SIEMPRE basándonos en zoneCenter
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += zoneCenter.position;

        NavMeshHit hit;
        // Buscamos el punto más cercano en el NavMesh dentro del radio
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return zoneCenter.position; // Backup por si falla
    }

    // Para ver el radio de la zona en el Editor (ayuda mucho a ajustar)
    void OnDrawGizmosSelected()
    {
        if (zoneCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(zoneCenter.position, wanderRadius);
        }
    }
}