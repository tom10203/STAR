using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    RespawnManager respawnManager;
    private void Start()
    {
        respawnManager = FindFirstObjectByType<RespawnManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            respawnManager.SetRespawnPoint();
        }
    }
}
