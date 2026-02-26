using UnityEngine;
using UnityEngine.InputSystem;

public class BulletShot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform fireposition;
    [SerializeField] private AudioSource sonidoDisparo;
    private float tiempoVida = 1f;
    // Update is called once per frame
    void Update()
    {
    }

    void OnShoot(InputValue value)
    {
        if (value.isPressed)
        {
            if (sonidoDisparo!=null)
            {
                sonidoDisparo.Play();
            }
            GameObject bulletFired = Instantiate(projectile, fireposition.position, transform.rotation);
            bulletFired.transform.rotation = new Quaternion(90, 0, 0, 0);
            bulletFired.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 700)); 
            Destroy(bulletFired,tiempoVida);
        }
    }
    
}
