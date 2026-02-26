using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("UI de Vida")]
    public Slider healthSlider;
    public Slider easeHealthSlider;

    [Header("Ajustes de Vida")]
    [SerializeField] private float maxHealth = 100f;
    private float vida;
    private bool inmune = false;
    private float lerpSpeed = 0.05f;

    // Referencia al controlador de la escena
    private ControladorJuego controlador;
    [SerializeField] AudioSource sonidoDamage;
    [SerializeField] AudioSource sonidoLose;
    void Start()
    {
        vida = maxHealth;
        // Buscamos autom�ticamente el objeto que tiene el script ControladorJuego
        controlador = Object.FindFirstObjectByType<ControladorJuego>();
    }

    void Update()
    {
        // Actualizaci�n de las barras de vida
        if (healthSlider.value != vida)
        {
            healthSlider.value = vida;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, vida, lerpSpeed);
        }
    }

    private IEnumerator ActivarInmunidad()
    {
        inmune = true;
        // Usamos WaitForSecondsRealtime para que el tiempo de inmunidad 
        // no se vea afectado si el juego se pausa
        yield return new WaitForSecondsRealtime(2);
        inmune = false;
    }

    public void Curar()
    {
        vida = maxHealth;
    }

    public void quitarVida(float damage)
    {
        if (!inmune)
        {
            if (!sonidoDamage.isPlaying && sonidoDamage != null)
            {
                sonidoDamage.Play();
            }
            vida -= damage;
           

            // COMPROBACI�N DE DERROTA
            if (vida <= 0)
            {
                vida = 0; // Para que la barra no baje de cero
                Morir();
            }
            else
            {
                StartCoroutine(ActivarInmunidad());
            }
        }
    }

    private void Morir()
    {
        sonidoLose.Play();
        Debug.Log("El jugador ha muerto");
        if (controlador != null)
        {
            controlador.ActivarDerrota();
        }
    }
}