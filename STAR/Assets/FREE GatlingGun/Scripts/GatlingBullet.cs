using UnityEngine;

public class GatlingBullet : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
       if (other.CompareTag("GatlingBullet"))
        {
            //deal damage
            Debug.Log("PlayerHit");
        }

    }
}
