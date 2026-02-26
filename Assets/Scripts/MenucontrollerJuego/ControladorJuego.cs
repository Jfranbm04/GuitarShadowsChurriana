using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControladorJuego : MonoBehaviour
{
    [Header("Paneles de Interfaz")]
    [SerializeField] private GameObject pantallaPausa;
    [SerializeField] private GameObject pantallaDerrota;

    [Header("Elementos del HUD a ocultar")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject minimapHolder;
    [SerializeField] private GameObject abilityQ;
    [SerializeField] private GameObject abilityR;
    [SerializeField] private GameObject CigalaHUD;
    [SerializeField] private AudioSource sonidojuego;
    [SerializeField] private Toggle musicaActiva;

    private bool juegoPausado = false;

    void Update()
    {
        if (musicaActiva.isOn && !sonidojuego.isPlaying)
        {
            sonidojuego.Play();
        }

        if (!musicaActiva.isOn)
        {
            sonidojuego.Pause();
        }
        // Abrir/Cerrar Pausa con Escape
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (juegoPausado) ReanudarJuego();
            else PausarJuego();
        }

        // Confirmar selecci�n con Enter (solo si est� pausado o en derrota)
        if (juegoPausado && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ConfirmarSeleccion();
        }
    }

    public void PausarJuego()
    {
        juegoPausado = true;
        pantallaPausa.SetActive(true);
        Time.timeScale = 0f;
        SetEstadoHUD(false);
        sonidojuego.Pause();
    }

    public void ReanudarJuego()
    {
        sonidojuego.Play();
        juegoPausado = false;
        pantallaPausa.SetActive(false);
        Time.timeScale = 1f;
        SetEstadoHUD(true);
    }

    // --- FUNCI�N PARA MOSTRAR LA DERROTA ---
    public void ActivarDerrota()
    {
        sonidojuego.Pause();
        juegoPausado = true; // Permite usar el Enter en esta pantalla
        pantallaDerrota.SetActive(true);
        Time.timeScale = 0f; // Detiene el juego
        SetEstadoHUD(false); // Limpia el HUD

    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetEstadoHUD(bool estado)
    {
        if (healthBar != null) healthBar.SetActive(estado);
        if (minimapHolder != null) minimapHolder.SetActive(estado);
        if (abilityQ != null) abilityQ.SetActive(estado);
        if (abilityR != null) abilityR.SetActive(estado);
        if (CigalaHUD != null) CigalaHUD.SetActive(estado);
    }

    private void ConfirmarSeleccion()
    {
        GameObject seleccionado = EventSystem.current.currentSelectedGameObject;
        if (seleccionado != null)
        {
            Button btn = seleccionado.GetComponent<Button>();
            if (btn != null) btn.onClick.Invoke();
        }
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
    public void SalirDelJuego()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}