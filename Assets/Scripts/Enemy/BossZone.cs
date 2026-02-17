using UnityEngine;

using UnityEngine;

public class BossZone : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject bossHUD; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activamos la interfaz
            bossHUD.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ocultamos la interfaz si el jugador huye de la zona
            bossHUD.SetActive(false);
        }
    }
}