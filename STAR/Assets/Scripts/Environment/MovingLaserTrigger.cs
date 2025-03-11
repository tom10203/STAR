using KinematicCharacterController;
using UnityEngine;

public class MovingLaserTrigger : MonoBehaviour
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
            Debug.Log($"Player hit laser");
            KinematicCharacterMotor motor = other.GetComponent<KinematicCharacterMotor>();
            motor.SetPosition(respawnManager.lastRespawnPoint.position);
            motor.SetRotation(respawnManager.lastRespawnPoint.rotation);

        }
    }
}
