using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        vidaActual = vidaMaxima;

        // Inicializar sliders si es jefe
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
        // Solo ejecutamos la lógica visual si es un jefe y tiene HUD asignado
        if (esJefe && healthSlider != null)
        {
            ActualizarVisualizacionHUD();
        }
    }

    // Esta función maneja el movimiento suave (Lerp), igual que en tu Player
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
            // Desactiva el objeto padre que contiene todo el HUD (HealthBar)
            healthSlider.transform.parent.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }

    // --- CONGELAMIENTO ---
    public void Congelar(float duracion)
    {
        if (!estaCongelado) StartCoroutine(RutinaCongelar(duracion));
    }

    private System.Collections.IEnumerator RutinaCongelar(float tiempo)
    {
        estaCongelado = true;
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Color colorOriginal = rend.material.color;
            rend.material.color = Color.cyan;
            yield return new WaitForSeconds(tiempo);
            rend.material.color = colorOriginal;
        }
        estaCongelado = false;
    }
}

//using UnityEngine;

//using UnityEngine.UI;

//public class EnemyController : MonoBehaviour
//{
//    [Header("Estadísticas")]
//    public float vidaMaxima = 100f;
//    private float vidaActual;
//    //public Slider barraVida;

//    [Header("Estados")]
//    public bool estaCongelado = false;

//    [Header("Configuración de Jefe")]
//    public bool esJefe = false;
//    public Slider healthSlider;
//    public Slider easeHealthSlider;
//    private float lerpSpeed = 0.02f;

//    void Start()
//    {
//        vidaActual = vidaMaxima;
//        //ActualizarUI();
//    }

//    // Recibe daño de la bala y quita vida al enemigo
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Bullet"))
//        {
//            BulletStats stats = collision.gameObject.GetComponent<BulletStats>();
//            if (stats != null)
//            {
//                RecibirDanio(stats.GetDamage());
//                Debug.Log("Enemigo impactado. Vida restante: " + vidaActual);
//            }

//            // Destruimos la bala para que no atraviese al enemigo
//            Destroy(collision.gameObject);
//        }
//    }


//    public void RecibirDanio(float cantidad)
//    {
//        vidaActual -= cantidad;
//        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
//        //ActualizarUI();
//        if (vidaActual <= 0) Morir();
//    }

//    void ActualizarUI() {
//        if (esJefe && healthSlider != null)
//        {
//            if (healthSlider.value != vida)
//            {
//                healthSlider.value = vida;
//            }

//            if (healthSlider.value != easeHealthSlider.value)
//            {
//                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, vida, lerpSpeed);
//            }
//        }
//    }

//    void Morir() {
//        if (esJefe)
//        {
//            // Oculta el HUD al morir
//            healthSlider.transform.parent.gameObject.SetActive(false); 
//            easeHealthSlider.transform.parent.gameObject.SetActive(false); 
//        }
//        Destroy(gameObject); 
//    }

//    // --- FUNCIÓN ÚNICA DE CONGELAMIENTO ---
//    public void Congelar(float duracion)
//    {
//        if (!estaCongelado)
//        {
//            StartCoroutine(RutinaCongelar(duracion));
//        }
//    }

//    private System.Collections.IEnumerator RutinaCongelar(float tiempo)
//    {
//        estaCongelado = true;
//        Renderer rend = GetComponentInChildren<Renderer>();
//        Color colorOriginal = rend.material.color;
//        rend.material.color = Color.cyan;

//        yield return new WaitForSeconds(tiempo);

//        rend.material.color = colorOriginal;
//        estaCongelado = false;
//    }
//}
