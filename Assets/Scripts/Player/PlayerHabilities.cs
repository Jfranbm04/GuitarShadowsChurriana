using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHabilities : MonoBehaviour
{
    [Header("Habilidad Q")] 
    public Image imageQ;
    public TextMeshProUGUI textQ;
    public float cooldownQ = 7;
    public GameObject abilityQ;
    
    private bool QonCooldown = false;

   
    private float currentCooldownQ;
    
    [Header("Habilidad R")] 
    public Image imageR;
    public TextMeshProUGUI textR;
    public float cooldownR = 20;
    public Image imageRBuff;
    public GameObject stunArea;
    public GameObject abilityR;
    private bool RonCooldown = false;
    public bool QActive = false;
    public bool RActive = false;
    private float currentCooldownR;
    [SerializeField] private AudioSource sonidoQ;
    [SerializeField] private AudioSource sonidoR;
    public ParticleSystem Sparks;

    public GameObject sparksObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      imageQ.fillAmount = 0;  
      textQ.text = string.Empty;
      imageR.fillAmount = 0;
      textR.text = string.Empty;
      Sparks.Stop();
      
      abilityQ.SetActive(false);
      abilityR.SetActive(false);
    }

    public void activeQ()
    {
        QActive = true;
        abilityQ.SetActive(true);
    }

    public void activeR()
    {
        RActive = true;
        abilityR.SetActive(true);
    }
    private void OnStun(InputValue value)
    {
        if (!QonCooldown && QActive)
        {
            if (!sonidoQ.isPlaying && sonidoQ != null)
            {
                sonidoQ.Play();
            }
            stunArea.SetActive(true);
            StartCoroutine(QCooldown());
           
        } 
       
      
        
    }

    private void OnBuff(InputValue value)
    {
        if (!RonCooldown && RActive)
        {
            if (!sonidoR.isPlaying && sonidoR != null)
            {
                sonidoR.Play();
            }
            StartCoroutine(TemporalBuff());
           
        }
    }
    
    IEnumerator QCooldown()
    {
        QonCooldown = true;
        currentCooldownQ = cooldownQ;
        
        //BulletStats.damage = 20;
        while (currentCooldownQ > 0)
        {
            if (currentCooldownQ < 6)
            {
                stunArea.SetActive(false); 
            }
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
        Sparks.Pause();
        sparksObject.SetActive(false);
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
        sparksObject.SetActive(true);
        Sparks.Play();
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
