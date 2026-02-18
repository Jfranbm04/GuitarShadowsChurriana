using UnityEngine;
using UnityEngine.InputSystem;

public class NPCSystem : MonoBehaviour
{
    // Arrastra aqu� el objeto que tiene el script "DialogueScript"
    public DialogueScript dialogueManager;

    public GameObject Interactuar;

    // Escribe aqu� los di�logos de este NPC espec�fico en el Inspector
    [TextArea(3, 10)]
    public string[] npcLines;

    bool player_detected = false;

    void Update()
    {
        // Si detectamos al jugador, pulsa E y NO hay un di�logo activo...
        if (player_detected && Keyboard.current.eKey.wasPressedThisFrame && !PlayerMovement.dialogueActive)
        {
            if (this.gameObject.tag == "Sergey")
            {
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                player.Curar();
            }
            // Descomentar cuando esten implementados todos los NPCS
            /*if (this.gameObject.tag == "Lulu")
            {
                PlayerHabilities playerA = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHabilities>();
                playerA.activeQ();
            }
            if (this.gameObject.tag == "Parsifal")
            {
                PlayerHabilities playerA = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHabilities>();
                playerA.activeR();
            }*/
            // Activamos el sistema de di�logo
            PlayerMovement.dialogueActive = true;
            dialogueManager.gameObject.SetActive(true);

            // Le pasamos nuestras l�neas al manager y lo iniciamos
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