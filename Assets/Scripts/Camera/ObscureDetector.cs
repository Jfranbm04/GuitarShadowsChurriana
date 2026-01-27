using UnityEngine;
using System.Collections.Generic;

public class ObscureDetector : MonoBehaviour
{
    [Header("Configuración")]
    public Transform jugador;
    public LayerMask capaEdificios; // Selecciona la capa "Edificios" en el Inspector

    [Range(0f, 1f)]
    public float transparenciaAlOcultar = 0.3f; // Qué tan invisible se vuelve

    private List<ObstaculoData> obstaculosActuales = new List<ObstaculoData>();

    // Clase para guardar el estado original de los materiales
    private class ObstaculoData
    {
        public Renderer renderer;
        public Color colorOriginal;
        public bool sigueObstruyendo;
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        // Marcamos todos los obstáculos previos como "no obstruyendo" para verificar ahora
        foreach (var data in obstaculosActuales) data.sigueObstruyendo = false;

        // Lanzamos un rayo desde la cámara hacia el jugador
        Vector3 direccion = jugador.position - transform.position;
        float distancia = direccion.magnitude;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direccion, distancia, capaEdificios);

        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                ObstaculoData data = obstaculosActuales.Find(x => x.renderer == rend);

                // Si es un obstáculo nuevo, lo guardamos y aplicamos transparencia
                if (data == null)
                {
                    data = new ObstaculoData
                    {
                        renderer = rend,
                        colorOriginal = rend.material.color
                    };
                    obstaculosActuales.Add(data);

                    // Aplicamos el nuevo color con transparencia
                    Color c = data.colorOriginal;
                    c.a = transparenciaAlOcultar;
                    rend.material.color = c;
                }
                data.sigueObstruyendo = true;
            }
        }

        // Limpiamos los que ya no obstruyen
        for (int i = obstaculosActuales.Count - 1; i >= 0; i--)
        {
            if (!obstaculosActuales[i].sigueObstruyendo)
            {
                // Restauramos color original y quitamos de la lista
                obstaculosActuales[i].renderer.material.color = obstaculosActuales[i].colorOriginal;
                obstaculosActuales.RemoveAt(i);
            }
        }
    }
}