using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject respawnPoint;
    //Attach this script to wherever your invisible floor collider is. This scirpt will also work if you have multiple death points and want multiple respawn positions (just assign the respawn point youw ant in the inspector)
    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("YOO HOO");
            other.transform.position = respawnPoint.transform.position;
        }
    }
}
