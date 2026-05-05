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

    [Header("Efectos de Sonido")]
    [SerializeField] private AudioSource sonidoDano;   // Arrastra el AudioSource de daño
    [SerializeField] private AudioSource sonidoMuerte; // Arrastra el AudioSource de muerte

    private ControladorJuego controlador;
    [SerializeField] AudioSource sonidoDamage;
    [SerializeField] AudioSource sonidoLose;
    void Start()
    {
        vida = maxHealth;
        controlador = Object.FindFirstObjectByType<ControladorJuego>();
    }

    void Update()
    {
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

            if (vida <= 0)
            {
                vida = 0;
                Morir();
            }
            else
            {
                // REPRODUCIR SONIDO DE DAÑO
                if (sonidoDano != null)
                {
                    sonidoDano.Play();
                }

                StartCoroutine(ActivarInmunidad());
            }
        }
    }

    private void Morir()
    {
        sonidoLose.Play();
        Debug.Log("El jugador ha muerto");

        // REPRODUCIR SONIDO DE MUERTE
        if (sonidoMuerte != null)
        {
            sonidoMuerte.Play();
        }

        if (controlador != null)
        {
            controlador.ActivarDerrota();
        }
    }
}