using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHabilities : MonoBehaviour
{
    [Header("Habilidad Q")] 
    public Image imageQ;
    public TextMeshProUGUI textQ;
    public float cooldownQ = 10;
    
    
    private bool QonCooldown = false;

    private float countDown = 10;
    private float currentCooldownQ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      imageQ.fillAmount = 0;  
      textQ.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void OnStun(InputValue value)
    {
        if (!QonCooldown)
        {
            StartCoroutine(Cooldown());
        } 
    }
    
    IEnumerator Cooldown()
    {
        QonCooldown = true;
        currentCooldownQ = cooldownQ;
        
        while (currentCooldownQ > 0)
        {
            currentCooldownQ--;
            yield return new WaitForSeconds(1);
        }
        imageQ.fillAmount = currentCooldownQ / cooldownQ;
        textQ.text = Mathf.Ceil(currentCooldownQ).ToString();
        yield return new WaitForSeconds(cooldownQ);
        QonCooldown = false;
    }
    /*private void AbilityCooldown(ref float currentCooldown, float maxCooldown, ref bool isCooldown, Image image ,TextMeshProUGUI text)
    {
        if (isCooldown)
            currentCooldown = Time.deltaTime;
        if (currentCooldown <= 0)
        {
            isCooldown = false;
            currentCooldown = 0;
            if (image != null)
            {
                image.fillAmount = 0;
            }

            if (text != null)
            {
                text.text = string.Empty;
            }
        }
        else
        {
            if (image != null)
            {
                image.fillAmount = currentCooldown / maxCooldown;
            }

            if (text != null)
            {
                text.text = Mathf.Ceil(currentCooldown).ToString();
            }
        }
    }*/
}
