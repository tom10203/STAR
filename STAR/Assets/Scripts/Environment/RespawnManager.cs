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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            KinematicCharacterMotor motor = FindAnyObjectByType<KinematicCharacterMotor>();
            if (motor != null)
            {
                motor.SetPositionAndRotation(testRespawnPoint.position, testRespawnPoint.rotation);
            }
            KinematicCharacterMotor2 motor2 = FindAnyObjectByType<KinematicCharacterMotor2>();
            if (motor2 != null)
            {
                motor2.SetPositionAndRotation(testRespawnPoint.position, testRespawnPoint.rotation);
            }
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
            lastRespawnPoint.gameObject.SetActive(false);
            fallOffCollider.respawnPoint = lastRespawnPoint;
            i++;
            Debug.Log($"lastRespawnPoint {lastRespawnPoint.name}");
        }
        
    }
}
