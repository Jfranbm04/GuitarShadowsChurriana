using UnityEngine;

public class Stun : MonoBehaviour
{
   void OnTriggerEnter(Collider other) {
      if (other.gameObject.tag == "Enemy")
      {
         EnemyController enemy= other.gameObject.GetComponent<EnemyController>();
         enemy.Congelar(3f);
      }
   }
}

