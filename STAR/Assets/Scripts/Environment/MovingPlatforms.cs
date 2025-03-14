using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private float amplitude;
    [SerializeField] private float offset;
    [SerializeField] private float speed;
    float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime * speed;

        SetPlatformPositions();
        
    }

    void SetPlatformPositions()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            GameObject platform = platforms[i];
            Vector3 platformPosition = platform.transform.position;
            platform.transform.position = new Vector3(platformPosition.x, platformPosition.y, transform.position.z + amplitude * Mathf.Sin(timer + i * offset * Mathf.PI));
        }
    }

    void MovePlatform()
    {

    }
}
