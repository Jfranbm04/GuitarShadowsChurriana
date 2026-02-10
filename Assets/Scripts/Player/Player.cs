using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int vida = 100;
    private bool inmune = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            if (!inmune)
            {
                vida -= 10;
                Debug.Log("Vida: " + vida);
                StartCoroutine(ActivarInmunidad());
            }
            
        }
    }
    private IEnumerator ActivarInmunidad(){
   
        inmune = true;
        yield return new WaitForSecondsRealtime(2);
        inmune = false;
        
    }
}
