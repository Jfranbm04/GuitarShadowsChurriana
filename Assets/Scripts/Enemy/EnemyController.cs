using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI; 

public class EnemyController : MonoBehaviour
{
    [Header("Estadísticas")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Estados")]
    public bool estaCongelado = false;

    [Header("Configuración de Jefe")]
    public bool esJefe = false;
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private float lerpSpeed = 0.05f;

    private NavMeshAgent agent; 

    void Start()
    {
        vidaActual = vidaMaxima;
        agent = GetComponent<NavMeshAgent>(); 

        if (esJefe && healthSlider != null)
        {
            healthSlider.maxValue = vidaMaxima;
            easeHealthSlider.maxValue = vidaMaxima;
            healthSlider.value = vidaMaxima;
            easeHealthSlider.value = vidaMaxima;
        }
    }

    void Update()
    {
        if (esJefe && healthSlider != null)
        {
            ActualizarVisualizacionHUD();
        }
    }

    void ActualizarVisualizacionHUD()
    {
        if (healthSlider.value != vidaActual)
        {
            healthSlider.value = vidaActual;
        }

        if (easeHealthSlider.value != healthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, vidaActual, lerpSpeed);
        }
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (vidaActual <= 0) Morir();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletStats stats = collision.gameObject.GetComponent<BulletStats>();
            if (stats != null)
            {
                RecibirDanio(stats.GetDamage());
            }
            Destroy(collision.gameObject);
        }
    }

    void Morir()
    {
        if (esJefe && healthSlider != null)
        {
            healthSlider.transform.parent.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }

    public void Congelar(float duracion)
    {
        // Solo iniciamos si no está ya congelado
        if (!estaCongelado) StartCoroutine(RutinaCongelar(duracion));
    }

    private System.Collections.IEnumerator RutinaCongelar(float tiempo)
    {
        estaCongelado = true;

        // Detenemos el movimiento del NavMeshAgent
        if (agent != null)
        {
            agent.isStopped = true;
        }

        // Cambiamos el color visual a Cian
        Renderer rend = GetComponentInChildren<Renderer>();
        Color colorOriginal = Color.white;
        if (rend != null)
        {
            colorOriginal = rend.material.color;
            rend.material.color = Color.cyan;
        }

        // Esperamos el tiempo indicado (3 segundos)
        yield return new WaitForSeconds(tiempo);

        // Restauramos el color original
        if (rend != null)
        {
            rend.material.color = colorOriginal;
        }

        // Reanudamos el movimiento del agente
        if (agent != null)
        {
            agent.isStopped = false;
        }

        estaCongelado = false;
    }
}