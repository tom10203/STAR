using KinematicCharacterController;
using KinematicCharacterController2;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] public Transform[] respawnPoints;
    [SerializeField] private Respawn fallOffCollider;

    [SerializeField] public Transform testRespawnPoint;

    public Transform lastRespawnPoint;
    int i = 0;



    public void SetRespawnPoint()
    {
        if (i >= respawnPoints.Length)
        {
            return;
        }
        else
        {
            lastRespawnPoint = respawnPoints[i];
            lastRespawnPoint.gameObject.SetActive(false);
            fallOffCollider.respawnPoint = lastRespawnPoint;
            i++;
        }
        
    }
}
