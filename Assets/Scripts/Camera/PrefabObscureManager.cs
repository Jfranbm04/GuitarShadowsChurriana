using UnityEngine;
using System.Collections.Generic;

public class PrefabObscureManager : MonoBehaviour
{
    [Header("Configuración")]
    public Transform jugador;          
    public LayerMask capaEdificios;    

    // Lista para rastrear qué edificios tenemos ocultos actualmente
    private List<Renderer> renderersOcultos = new List<Renderer>();

    void LateUpdate()
    {
        if (jugador == null) return;

        // 1. Raycast para detectar qué hay entre la cámara y el jugador
        Vector3 direccion = jugador.position - transform.position;
        float distancia = direccion.magnitude;

        // Usamos RaycastAll por si hay varios edificios en fila
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direccion, distancia, capaEdificios);

        // Lista temporal para los que están obstruyendo en ESTE frame
        List<Renderer> detectadosEsteFrame = new List<Renderer>();

        foreach (RaycastHit hit in hits)
        {
            // Buscamos renderers en el objeto golpeado o sus hijos
            Renderer[] rends = hit.collider.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in rends)
            {
                if (!detectadosEsteFrame.Contains(r))
                {
                    detectadosEsteFrame.Add(r);

                    // Si no estaba ya oculto, lo ocultamos
                    if (r.enabled)
                    {
                        r.enabled = false;
                        if (!renderersOcultos.Contains(r)) renderersOcultos.Add(r);
                    }
                }
            }
        }

        // 2. Restaurar los edificios que ya no estorban
        for (int i = renderersOcultos.Count - 1; i >= 0; i--)
        {
            Renderer r = renderersOcultos[i];

            // Si el renderer ya no está en la lista de impactos de este frame, lo mostramos
            if (!detectadosEsteFrame.Contains(r))
            {
                if (r != null) r.enabled = true;
                renderersOcultos.RemoveAt(i);
            }
        }
    }
}