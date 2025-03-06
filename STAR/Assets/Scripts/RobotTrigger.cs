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
        Debug.Log("1");
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("2");
            firstRobot.DoLazer();
            foreach(RobotEnemy robot in robots)
            {
                robot.ActivateRobots();
            }
            gameObject.SetActive(false);
        }
    }
}
