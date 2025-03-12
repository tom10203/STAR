using UnityEngine;

public class SpawnMovingLaser : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float laserMoveDistance;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private float laserMoveSpeed;

    private GameObject currentLaser;
    MovingLaser currentMovingLaser;
    private float timer = 0f;

    void Start()
    {
        
        InstantiateLaser(); 
        timer = Time.time + spawnRate * delay;
    }

    void Update()
    {
        if (Time.time > timer)
        {
            timer = Time.time + spawnRate;

            currentMovingLaser.move = true;

            InstantiateLaser();
        }
    }

    void InstantiateLaser()
    {
        currentLaser = Instantiate(laser, transform.position, transform.rotation, transform);
        currentLaser.transform.localScale = new Vector3(currentLaser.transform.localScale.x, 0.5f, currentLaser.transform.localScale.z);
        currentMovingLaser = currentLaser.GetComponent<MovingLaser>();
        currentMovingLaser.moveDistance = laserMoveDistance;
        currentMovingLaser.moveSpeed = laserMoveSpeed;
    }
}
