using UnityEngine;

using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Estadísticas")]
    public float vidaMaxima = 100f;
    private float vidaActual;
    //public Slider barraVida;

    [Header("Estados")]
    public bool estaCongelado = false;

    void Start()
    {
        vidaActual = vidaMaxima;
        //ActualizarUI();
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        //ActualizarUI();
        if (vidaActual <= 0) Morir();
    }

    //void ActualizarUI() { if (barraVida != null) barraVida.value = vidaActual / vidaMaxima; }

    void Morir() { Destroy(gameObject); }

    // --- FUNCIÓN ÚNICA DE CONGELAMIENTO ---
    public void Congelar(float duracion)
    {
        if (!estaCongelado)
        {
            StartCoroutine(RutinaCongelar(duracion));
        }
    }

    private System.Collections.IEnumerator RutinaCongelar(float tiempo)
    {
        estaCongelado = true;
        Renderer rend = GetComponentInChildren<Renderer>();
        Color colorOriginal = rend.material.color;
        rend.material.color = Color.cyan;

        yield return new WaitForSeconds(tiempo);

        rend.material.color = colorOriginal;
        estaCongelado = false;
    }
}
