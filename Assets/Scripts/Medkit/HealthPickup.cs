using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Ajustes de Curación")]
    public float puntosDeCuracion = 5f;

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que entra en el trigger es el jugador
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.CurarCantidad(puntosDeCuracion);

                // Opcional: Aquí podrías instanciar un sistema de partículas o reproducir un sonido

                // Destruimos el botiquín tras recogerlo
                Destroy(gameObject);
            }
        }
    }
}