using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject contenedorBotones; // El objeto que agrupa los botones
    [SerializeField] private GameObject panelControles;

    // --- NUEVA PARTIDA ---
    public void NuevaPartida()
    {
        SceneManager.LoadScene("MainScene");
    }

    // --- CONTROLES ---
    public void AbrirControles()
    {
        // Desactiva solo el grupo de botones
        contenedorBotones.SetActive(false);

        // Muestra el panel de controles
        panelControles.SetActive(true);
    }

    public void VolverAlMenu()
    {
        // Oculta los controles
        panelControles.SetActive(false);

        // Vuelve a mostrar los botones
        contenedorBotones.SetActive(true);
    }

    // --- SALIR ---
    // --- SALIR ---
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        // Esta línea cierra el juego cuando ya está exportado (.exe)
        Application.Quit();

        // Esta línea detiene el modo "Play" dentro del Editor de Unity
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}