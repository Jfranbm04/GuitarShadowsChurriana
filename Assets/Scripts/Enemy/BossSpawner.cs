using System.Collections;
using TMPro;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Configuración del Contador")]
    public int timeleft = 10;
    public TextMeshProUGUI countdownText;

    [Header("Referencia del Jefe y HUD")]
    public GameObject faryEnEscena; // El objeto "fary" de la jerarquía
    public GameObject faryHUD;      // Arrastra aquí el Canvas/Objeto del HUD
    public float distanciaSpawn = 5f;

    private bool yaHaEmpezado = false;

    void Start()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (faryHUD != null) faryHUD.SetActive(false); // Aseguramos que empiece apagado
    }

    void Update()
    {
        // Solo lanzamos la cuenta atrás si los jefes han muerto y no lo hemos hecho ya
        if (!yaHaEmpezado && EnemyController.ComprobarJefes())
        {
            yaHaEmpezado = true;
            StartCoroutine(countdownJefe());
        }
    }

    private IEnumerator countdownJefe()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(true);

        while (timeleft > 0)
        {
            if (countdownText != null)
                countdownText.text = "¡Cuidado! Alguien está apatrullando la ciudad... " + timeleft.ToString();

            yield return new WaitForSeconds(1.0f);
            timeleft--;
        }

        AparecerJefe();
    }

    void AparecerJefe()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(false);

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null && faryEnEscena != null)
        {
            // 1. Posicionamiento en la Z del jugador + 5m
            Vector3 posJugador = jugador.transform.position;
            Vector3 posicionFinal = new Vector3(posJugador.x, posJugador.y, posJugador.z + distanciaSpawn);

            faryEnEscena.transform.position = posicionFinal;

            // 2. Activamos al Jefe
            faryEnEscena.SetActive(true);

            // 3. Activamos el HUD del jefe
            if (faryHUD != null)
            {
                faryHUD.SetActive(true);
            }

            Debug.Log("¡EL FARY HA APARECIDO Y EL HUD ESTÁ ACTIVO!");
        }
        else
        {
            Debug.LogWarning("Faltan referencias: Jugador o FaryEnEscena.");
        }
    }
}