using UnityEngine;

public class GatlingBullet : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
       if (other.CompareTag("GatlingBullet"))
        {
            //deal damage
            Debug.Log("PlayerHit");
            dealDamage();
        }

    }

    void dealDamage()
    {
        FindAnyObjectByType<ScreenShake>().start = true;
        FindAnyObjectByType<PlayerCharacter>().TakeDamage(1);
    }
}
