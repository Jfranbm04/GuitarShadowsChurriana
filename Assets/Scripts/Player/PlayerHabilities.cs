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
    public float cooldownQ = 7;
    
    
    private bool QonCooldown = false;

   
    private float currentCooldownQ;
    
    [Header("Habilidad R")] 
    public Image imageR;
    public TextMeshProUGUI textR;
    public float cooldownR = 20;
    public Image imageRBuff;
   
    private bool RonCooldown = false;

   
    private float currentCooldownR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      imageQ.fillAmount = 0;  
      textQ.text = string.Empty;
      imageR.fillAmount = 0;
      textR.text = string.Empty;
    }
    
    private void OnStun(InputValue value)
    {
        if (!QonCooldown)
        {
            StartCoroutine(QCooldown());
        } 
    }

    private void OnBuff(InputValue value)
    {
        if (!RonCooldown)
        {
            StartCoroutine(TemporalBuff());
            
        }
    }
    
    IEnumerator QCooldown()
    {
        QonCooldown = true;
        currentCooldownQ = cooldownQ;
        BulletStats.damage = 20;
        while (currentCooldownQ > 0)
        {
            currentCooldownQ -= Time.deltaTime;
            
            imageQ.fillAmount = currentCooldownQ / cooldownQ;
            textQ.text = Mathf.Ceil(currentCooldownQ).ToString();
            if (currentCooldownQ == 5)
            {
                BulletStats.damage = 10; 
            }

            yield return null;
        }

        imageQ.fillAmount = 0;
        textQ.text = string.Empty;
        QonCooldown = false;
    }
    
    
    IEnumerator RCooldown()
    {
        
        RonCooldown = true;
        currentCooldownR = cooldownR;
        while (currentCooldownR > 0)
        {
            currentCooldownR -= Time.deltaTime;

            imageR.fillAmount = currentCooldownR / cooldownR;
            textR.text = Mathf.Ceil(currentCooldownR).ToString();

            yield return null;
        }

        imageR.fillAmount = 0;
        textR.text = string.Empty;
        RonCooldown = false;
    }

    IEnumerator TemporalBuff()
    {
        imageRBuff.color = Color.yellow;
        BulletStats.damage = 20;
        yield return new WaitForSecondsRealtime(15);
        BulletStats.damage = 10;
        imageRBuff.color = Color.white;
        StartCoroutine(RCooldown());
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
