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
    [SerializeField] private GameObject PaquirrinHUD;
    [SerializeField] private GameObject QuestMenu;
    //[SerializeField] private GameObject FaryHUD;

    [Header("Referencias Externas")]
    [SerializeField] private PlayerHabilities playerHabilities;

    [Header("Ajustes de Sonido")]
    [SerializeField] private AudioSource musicaFondo; // Arrastra aquí el objeto que tiene la música
    [SerializeField] private Toggle toggleMusica;    // Arrastra aquí el Toggle del menú de pausa

    private bool juegoPausado = false;

    void Start()
    {
        if (musicaFondo != null && toggleMusica != null)
        {
            toggleMusica.isOn = musicaFondo.mute == false; // Si no está muteado, el check está marcado
            toggleMusica.onValueChanged.AddListener(ControlarMusica);
        }
    }

    public void ControlarMusica(bool estado)
    {
        if (musicaFondo != null)
        {
            // Si 'estado' es true (marcado), mute es false (suena)
            musicaFondo.mute = !estado;
        }
    }

    void Update()
    {
        // Abrir/Cerrar Pausa con Escape
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (juegoPausado) ReanudarJuego();
            else PausarJuego();
        }

        // Confirmar selección con Enter (solo si está pausado o en derrota)
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
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        pantallaPausa.SetActive(false);
        Time.timeScale = 1f;

        // Al reanudar, llamamos a la función pero con una lógica especial
        ActualizarHUDAlReanudar();
    }

    // --- FUNCIÓN PARA MOSTRAR LA DERROTA ---
    public void ActivarDerrota()
    {
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



    // Nueva función específica para reanudar
    private void ActualizarHUDAlReanudar()
    {
        // Elementos básicos que siempre vuelven
        if (healthBar != null) healthBar.SetActive(true);
        if (minimapHolder != null) minimapHolder.SetActive(true);
        if (QuestMenu != null) QuestMenu.SetActive(true);

        // Lógica condicional para habilidades
        if (playerHabilities != null)
        {
            // Solo se activan si están desbloqueadas/activas en el script del jugador
            if (abilityQ != null) abilityQ.SetActive(playerHabilities.QActive);
            if (abilityR != null) abilityR.SetActive(playerHabilities.RActive);
        }
        else
        {
            // Opcional: Si no encuentras el script, por defecto las dejamos apagadas
            if (abilityQ != null) abilityQ.SetActive(false);
            if (abilityR != null) abilityR.SetActive(false);
        }

        // Otros elementos que mencionaste que quedan apagados
        if (CigalaHUD != null) CigalaHUD.SetActive(false);
        if (PaquirrinHUD != null) PaquirrinHUD.SetActive(false);
    }
    // Modificamos esta para que solo la use Pausar y Derrota
    private void SetEstadoHUD(bool estado)
    {
        if (healthBar != null) healthBar.SetActive(estado);
        if (minimapHolder != null) minimapHolder.SetActive(estado);
        if (abilityQ != null) abilityQ.SetActive(estado);
        if (abilityR != null) abilityR.SetActive(estado);
        if (CigalaHUD != null) CigalaHUD.SetActive(estado);
        if (PaquirrinHUD != null) PaquirrinHUD.SetActive(estado);
        if (QuestMenu != null) QuestMenu.SetActive(estado);
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