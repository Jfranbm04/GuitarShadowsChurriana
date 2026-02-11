using UnityEngine;
using UnityEngine.InputSystem;

public class NPCSystem : MonoBehaviour
{
    // Arrastra aquí el objeto que tiene el script "DialogueScript"
    public DialogueScript dialogueManager;

    public GameObject Interactuar;

    // Escribe aquí los diálogos de este NPC específico en el Inspector
    [TextArea(3, 10)]
    public string[] npcLines;

    bool player_detected = false;

    void Update()
    {
        // Si detectamos al jugador, pulsa E y NO hay un diálogo activo...
        if (player_detected && Keyboard.current.eKey.wasPressedThisFrame && !PlayerMovement.dialogueActive)
        {
            // Activamos el sistema de diálogo
            PlayerMovement.dialogueActive = true;
            dialogueManager.gameObject.SetActive(true);

            // Le pasamos nuestras líneas al manager y lo iniciamos
            dialogueManager.StartDialogue(npcLines);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) player_detected = true;
        Interactuar.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) player_detected = false;
        Interactuar.SetActive(false);
    }
}