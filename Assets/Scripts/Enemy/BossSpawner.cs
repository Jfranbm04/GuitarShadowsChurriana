using System.Collections;
using TMPro;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Configuración del Contador")]
    public int timeleft = 10;
    public TextMeshProUGUI countdownText;

    [Header("Referencia del Jefe")]
    public GameObject faryEnEscena; // Arrastra al Fary (desactivado) desde la Jerarquía
    public float distanciaSpawn = 5f;

    private int vecesLanzado = 0;

    void Start()
    {
        // Nos aseguramos de que el texto esté oculto al empezar si quieres
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Si la función estática dice que los jefes han muerto
        if (EnemyController.ComprobarJefes())
        {
            vecesLanzado++;
            if (vecesLanzado == 1)
            {
                StartCoroutine(countdownJefe());
            }
        }
    }

    private IEnumerator countdownJefe()
    {
        Debug.Log("¡Cuenta atrás para el Fary iniciada!");

        if (countdownText != null) countdownText.gameObject.SetActive(true);

        while (timeleft > 0)
        {
            if (countdownText != null) countdownText.text = "Cuidado!! Alguien está apatrullando la ciudad... "+timeleft.ToString();
            yield return new WaitForSeconds(1.0f);
            timeleft--;
        }

        // ACCIÓN FINAL: Aparece el jefe
        AparecerJefe();
    }

    void AparecerJefe()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(false);

        // Localizamos al jugador
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null && faryEnEscena != null)
        {
            // Calculamos la posición: misma X, misma Y, pero Z + 5
            Vector3 posJugador = jugador.transform.position;
            Vector3 posicionFinal = new Vector3(posJugador.x, posJugador.y, posJugador.z + distanciaSpawn);

            // Movemos al Fary a su sitio y lo despertamos
            faryEnEscena.transform.position = posicionFinal;
            faryEnEscena.SetActive(true);

            // Al activarse, el script FaryAI ejecutará su OnEnable automáticamente
            Debug.Log("¡EL FARY HA APARECIDO!");
        }
        else
        {
            Debug.LogWarning("Ojo: No se ha encontrado al Jugador o el objeto FaryEnEscena no está asignado.");
        }
    }
}