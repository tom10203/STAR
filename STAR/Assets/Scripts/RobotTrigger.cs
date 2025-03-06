using UnityEngine;

public class RobotTrigger : MonoBehaviour
{
    private RobotEnemy[] robots;
    [SerializeField] private FirstRobot firstRobot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        robots = FindObjectsByType<RobotEnemy>(FindObjectsSortMode.None);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            firstRobot.DoLazer();
            foreach(RobotEnemy robot in robots)
            {
                robot.ActivateRobots();
            }
            gameObject.SetActive(false);
        }
    }
}
