using UnityEngine;
using UnityEngine.AI;

public class RobotEnemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent;
    private PlayerController player;
    private float searchTime = 1f;
    private float searchTimer = 1f;
    private int health = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (searchTimer < searchTime)
        {
            searchTimer += Time.deltaTime;
        }
        else
        {
            searchTimer = 0f;
            navAgent.SetDestination(player.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //ToDo - player take damage
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
