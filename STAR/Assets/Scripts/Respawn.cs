using KinematicCharacterController;
using KinematicCharacterController2;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint;
    //Attach this script to wherever your invisible floor collider is. This scirpt will also work if you have multiple death points and want multiple respawn positions (just assign the respawn point you want in the inspector)
    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.transform.position = respawnPoint.transform.position;

            KinematicCharacterMotor motor = FindAnyObjectByType<KinematicCharacterMotor>();
            if (motor != null)
            {
                motor.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
            }
            KinematicCharacterMotor2 motor2 = FindAnyObjectByType<KinematicCharacterMotor2>();
            if (motor2 != null)
            {
                motor2.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
            }
        }
    }
}
