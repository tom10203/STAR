using UnityEngine;

public class RobotTrigger : MonoBehaviour
{
    [SerializeField] private RobotEnemy[] robots;
    [SerializeField] private FirstRobot firstRobot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (firstRobot != null)
            {
                firstRobot.DoLazer();
            }

            foreach(RobotEnemy robot in robots)
            {
                robot.ActivateRobots();
            }
            gameObject.SetActive(false);
        }
    }
}
