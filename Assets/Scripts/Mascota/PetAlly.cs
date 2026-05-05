using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PetAlly : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;         // Arrastra a tu jugador en el Inspector
    public LayerMask enemyLayer;     // Selecciona la capa "Enemy"

    [Header("Estadísticas")]
    public float speed = 6f;            // Velocidad de la mascota
    public float followDistance = 3f;   // Distancia a la que se queda del jugador
    public float aggroRadius = 10f;     // Radio para detectar enemigos
    public float attackRange = 2f;      // Distancia a la que puede pegar
    public float attackCooldown = 1.5f; // Tiempo entre ataques
    public float damage = 20f;          // Daño que hace

    private Rigidbody rb;
    private Transform currentEnemy;
    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Congelamos la rotación para que no ruede de forma descontrolada
        rb.freezeRotation = true;
    }

    void Update()
    {
        FindNearestEnemy();
    }

    void FixedUpdate()
    {
        MovePet();
    }

    void FindNearestEnemy()
    {
        // Detecta los enemigos dentro del radio
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroRadius, enemyLayer);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hitCollider.transform;
            }
        }

        currentEnemy = closestEnemy;
    }

    void MovePet()
    {
        Vector3 targetPosition = player.position;

        // 1. LÓGICA DE COMBATE
        if (currentEnemy != null)
        {
            targetPosition = currentEnemy.position;
            float distanceToEnemy = Vector3.Distance(transform.position, currentEnemy.position);

            if (distanceToEnemy <= attackRange)
            {
                // Estamos a rango de ataque: Nos detenemos y atacamos
                rb.linearVelocity = Vector3.zero;
                Attack();
                return;
            }
        }
        // 2. LÓGICA DE SEGUIR AL JUGADOR
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= followDistance)
            {
                // Ya estamos cerca del jugador: Nos detenemos
                rb.linearVelocity = Vector3.zero;
                return;
            }
        }

        // 3. MOVER EL RIGIDBODY HACIA EL OBJETIVO (Jugador o Enemigo)
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 newVelocity = direction * speed;

        // Conservamos la velocidad de caída/físicas en el eje Y para que no flote
        newVelocity.y = rb.linearVelocity.y;

        // Aplicamos la velocidad al Rigidbody
        rb.linearVelocity = newVelocity;
    }

    void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("¡La mascota atacó al enemigo!");

            if (currentEnemy != null)
            {
                EnemyController enemyScript = currentEnemy.GetComponent<EnemyController>();

                if (enemyScript != null)
                {
                    enemyScript.RecibirDanio(damage);
                }
            }

            lastAttackTime = Time.time;
        }
    }

    // Dibuja las esferas de rango en la vista Scene
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}