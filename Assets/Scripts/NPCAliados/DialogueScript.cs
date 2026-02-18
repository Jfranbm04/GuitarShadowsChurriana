using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class DialogueScript : MonoBehaviour
{
    
    public TextMeshProUGUI dialogueText; // Referencia al componente de texto para mostrar el di�logo
    public string[] dialogueLines; // Array de l�neas de di�logo para mostrar

    public float textSpeed = 0.1f; // Velocidad a la que se muestra el texto

    int index; // �ndice para rastrear la l�nea de di�logo actual

    public GameObject minimapCanvas;
    void Start()
    {

        dialogueText.text = ""; // Limpiar el texto al iniciar el di�logo
        StartDialogue(dialogueLines); // Iniciar el di�logo con las l�neas definidas en el Inspector
    }

    // Update is called once per frame
    void Update()
    {
        // "rightButton" es el click derecho
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (dialogueText.text == dialogueLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[index];
            }
        }
    }

    IEnumerator WriteLine() {

        foreach (char letter in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed); // Esperar un poco antes de mostrar la siguiente letra
        }
    }

    public void StartDialogue(string[] lines)
    {
        // 1. Apagamos el minimapa al empezar
        if (minimapCanvas != null) minimapCanvas.SetActive(false);

        
        dialogueLines = lines; // Recibimos las frases del NPC
        index = 0;
        dialogueText.text = "";
        StopAllCoroutines(); // Por seguridad, paramos cualquier escritura previa
        StartCoroutine(WriteLine());
    }

    public void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(WriteLine());
        }
        else
        {
            dialogueText.text = "";
            PlayerMovement.dialogueActive = false; // El jugador ya puede moverse

            // 2. Encendemos el minimapa al terminar
            if (minimapCanvas != null) minimapCanvas.SetActive(true);
            // �ESTO ES LO QUE TE FALTABA!
            
            gameObject.SetActive(false);
        }
    }
}
