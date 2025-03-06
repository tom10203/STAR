using UnityEngine;

public class CameraLean : MonoBehaviour
{
    [SerializeField] private float attackDamping = 0.5f;
    [SerializeField] private float decayDamping = 0.3f;
    [SerializeField] private float walkStrength = 1.5f;
    [SerializeField] private float slideStrength = 0.5f;
    [SerializeField] private float strengthResponse = 5f;

    private Vector3 _dampedAcceleration;
    private Vector3 _dampedAccelerationVel;
    private float smoothStrength;

    public void Initialise()
    {
        smoothStrength = walkStrength;
    }

    public void UpdateLean(float deltaTime, bool sliding, Vector3 acceleration, Vector3 up)
    {
        var planarAcceleration = Vector3.ProjectOnPlane(acceleration, up);
        var damping = planarAcceleration.magnitude > _dampedAcceleration.magnitude ? attackDamping: decayDamping;

        _dampedAcceleration = Vector3.SmoothDamp
        (
            current: _dampedAcceleration,
            target: planarAcceleration,
            currentVelocity: ref _dampedAccelerationVel,
            smoothTime: damping,
            maxSpeed: float.PositiveInfinity,
            deltaTime: deltaTime
        );

        var leanAxis = Vector3.Cross(_dampedAcceleration.normalized, up).normalized;

        transform.localRotation = Quaternion.identity;

        var targetStrength = sliding ? slideStrength: walkStrength;

        smoothStrength = Mathf.Lerp(smoothStrength, targetStrength, 1f - Mathf.Exp(-strengthResponse * deltaTime));

        transform.rotation = Quaternion.AngleAxis(_dampedAcceleration.magnitude * smoothStrength, leanAxis) * transform.rotation;
    }
}   
