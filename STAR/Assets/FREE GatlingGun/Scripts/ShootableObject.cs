using UnityEngine;

public class ShootableObject : MonoBehaviour
{
    public int health;
    public GatlingGun gattlingGun;
    public ParticleSystem smoke;


    private void Update()
    {
        if (health <= 0)
        {
            gattlingGun.enabled = false;
            gattlingGun.muzzelFlash.gameObject.SetActive(false);
            gattlingGun.audioSource.enabled = false;
            smoke.gameObject.SetActive(true);

            //Destroy(gattlingGun);
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }
    private void OnCollisionEnter(Collision collision)
    {
        ShootableObject shootableObject = collision.gameObject.GetComponent<ShootableObject>();

        if (shootableObject != null)
        {
            //shootableObject.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
