using UnityEngine;

public class SpawnMovingLaser : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float laserDeadZone;

    private GameObject currentLaser;
    MovingLaser currentMovingLaser;
    private float timer = 0f;
    void Start()
    {
        InstantiateLaser(); 
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
        currentMovingLaser = currentLaser.GetComponent<MovingLaser>();
        currentMovingLaser.deadZone = laserDeadZone;
    }
}
