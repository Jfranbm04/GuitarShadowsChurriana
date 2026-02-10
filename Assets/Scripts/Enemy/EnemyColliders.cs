using UnityEngine;

public class EnemyColliders : MonoBehaviour
{
   public  Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.quitarVida(10);
        }
            
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
