using UnityEngine;
using UnityEngine.InputSystem;

public class BulletShot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform fireposition;

    private float tiempoVida = 1f;
    // Update is called once per frame
    void Update()
    {
    }

    void OnShoot(InputValue value)
    {
        if (value.isPressed)
        {
            GameObject bulletFired = Instantiate(projectile, fireposition.position, transform.rotation);
            bulletFired.transform.rotation = new Quaternion(90, 0, 0, 0);
            bulletFired.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 700)); 
            Destroy(bulletFired,tiempoVida);
        }
    }
    
}
