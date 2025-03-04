using UnityEngine;
using UnityEngine.AI;

public class RobotEnemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent;
    private PlayerController2 player;
    private float searchTime = 1f;
    private float searchTimer = 1f;
    private int health = 3;
    private int damageAmount = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerController2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
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
        else
        {
            //Die
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.TakeDamage(damageAmount);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
