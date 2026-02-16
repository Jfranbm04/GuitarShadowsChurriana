using UnityEngine;

public class BulletStats : MonoBehaviour
{
   public static int damage = 10;
   
   
   public int GetDamage()
   {
      return damage;
   }

   public void SetDamage(int newDamage)
   {
    damage = newDamage;
   }
}
