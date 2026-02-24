using System.Collections;
using TMPro;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private int timeleft = 10;
    private int vecesLanzado = 0;
    public TextMeshProUGUI countdownText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyController.ComprobarJefes())
        {
            vecesLanzado++;
            if (vecesLanzado == 1)
            {
                StartCoroutine(countdownJefe());   
            }
        }
    }
    private IEnumerator countdownJefe()
    {
        Debug.Log("CountdownEmpezado");
        while (timeleft > 0)
        {
            countdownText.text = timeleft.ToString();
            yield return new WaitForSeconds(1.0f);
            timeleft--;
        }

        if (timeleft == 0)
        {
            countdownText.gameObject.SetActive(false);
            //Jefe aparece
        }
        
        
    }
}
