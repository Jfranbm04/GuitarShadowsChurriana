using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private float maxHealth = 100f;
    private float vida;
    private bool inmune = false;

    private float lerpSpeed = 0.02f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida =  maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != vida)
        {
            healthSlider.value = vida; 
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value= Mathf.Lerp(easeHealthSlider.value,vida,lerpSpeed);
        }
        
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Enemy"))
        {
            if (!inmune)
            {
                vida -= 10;
                Debug.Log("Vida: " + vida);
                StartCoroutine(ActivarInmunidad());
            }
        }
    }*/
    
    private IEnumerator ActivarInmunidad(){
   
        inmune = true;
        yield return new WaitForSecondsRealtime(2);
        inmune = false;
        
    }

    public void quitarVida(float damage)
    {
        if (!inmune)
        {
            vida -= damage;
            StartCoroutine(ActivarInmunidad());
        }
    }
}
