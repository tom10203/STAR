using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] public Transform[] respawnPoints;
    [SerializeField] private Respawn fallOffCollider;

    public Transform lastRespawnPoint;
    int i = 0;

    private void Start()
    {
        if (respawnPoints.Length == 0)
        {
            return;
        }
        else
        {
            lastRespawnPoint = respawnPoints[i];
            fallOffCollider.respawnPoint = lastRespawnPoint;
        }
    }

    public void SetRespawnPoint()
    {
        if (i >= respawnPoints.Length)
        {
            return;
        }
        else
        {
            lastRespawnPoint = respawnPoints[i];
            fallOffCollider.respawnPoint = lastRespawnPoint;
            i++; 
        }
        
    }
}
