using UnityEngine;

public class Spin : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float rotateSpeed = Random.Range(25, 35);

        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
