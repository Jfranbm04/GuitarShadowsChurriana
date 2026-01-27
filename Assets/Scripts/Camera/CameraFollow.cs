using UnityEngine;

public partial class CameraFollow : MonoBehaviour
{
    [Header("Configuración")]
    public Transform Player; 
    public Vector3 desplazamiento; // La distancia entre la cámara y el personaje

    [Range(0, 1)]
    public float suavizado = 0.125f; 

    void Start()
    {
       
        if (desplazamiento == Vector3.zero)
        {
            desplazamiento = transform.position - Player.position;
        }
    }

    // Usamos LateUpdate para que la cámara se mueva DESPUÉS de que el personaje haya terminado de moverse
    void LateUpdate()
    {
        if (Player == null) return;

        // Calculamos la posición deseada
        Vector3 posicionDeseada = Player.position + desplazamiento;

        // Movemos la cámara suavemente a esa posición
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        transform.position = posicionSuavizada;
    }
}