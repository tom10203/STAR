using UnityEngine;

public struct CameraInput
{
    public Vector2 look;
}

public class PlayerCamera : MonoBehaviour
{
    private Vector3 _eulerAngles;
    private float camLimitX = 85;

    [SerializeField] private float sensitivity = 0.1f;
    public void Initialise(Transform target)
    {
        sensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 0.1f);
        transform.position = target.position;
        transform.eulerAngles = _eulerAngles = target.eulerAngles;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }

    public void UpdateRotation(CameraInput input)
    {
        _eulerAngles += new Vector3(-input.look.y, input.look.x) * sensitivity;
        /*var eulerAnglesX = _eulerAngles.x;
        transform.eulerAngles = _eulerAngles;*/
        var eulerAnglesX = Mathf.Clamp(_eulerAngles.x, -camLimitX, camLimitX);
        _eulerAngles = new Vector3(eulerAnglesX, _eulerAngles.y, _eulerAngles.z);
        transform.eulerAngles = _eulerAngles;
    }

    public void SetMouseSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }
}
