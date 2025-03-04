using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    [Min(0.01f)]
    [SerializeField] private float halfLife = 0.075f;
    [SerializeField] private float frequencey = 18f;
    [SerializeField] private float angularDisplacement = 2f;
    [SerializeField] private float linearDisplacement = 0.05f;
    private Vector3 _springPosition;
    private Vector3 _springVelocity;
    public void Initialise()
    {
        _springPosition = transform.position;
        _springVelocity = Vector3.zero;
    }

    public void UpdateSpring(float deltaTime, Vector3 up)
    {
        transform.localPosition = Vector3.zero;

        Spring(ref _springPosition, ref _springVelocity, transform.position, halfLife, frequencey, deltaTime);
        var localSpringPosition = _springPosition - transform.position;
        var sprintHeight = Vector3.Dot(localSpringPosition, up);

        transform.localEulerAngles = new Vector3(-sprintHeight * angularDisplacement, 0f, 0f);

        transform.localPosition = localSpringPosition * linearDisplacement;
    }

    //public void Spring(ref float x, ref float v, float xt, float zeta, float omega, float h)
    //{
    //    float f = 1.0f + 2.0f * h * zeta * omega;
    //    float oo = omega * omega;
    //    float hoo = h * oo;
    //    float hhoo = h * hoo;
    //    float detInv = 1.0f / (f + hhoo);
    //    float detX = f * x + h * v + hhoo * xt;
    //    float detV = v + hoo * (xt - x);
    //    x = detX * detInv;
    //    v = detV * detInv;
    //}

    public void Spring(ref Vector3 current, ref Vector3 velocity, Vector3 target, float halfLife, float frequency, float timeStep)
    {
        var dampingRatio = -Mathf.Log(0.5f) / (frequencey * halfLife);
        var f = 1.0f + 2.0f * timeStep * dampingRatio * frequencey;
        var oo = frequencey * frequencey;
        var hoo = timeStep * oo;
        var hhoo = timeStep * hoo;
        var detInv = 1.0f / (f + hhoo);
        var detX = f * current + timeStep * velocity + hhoo * target;
        var detV = velocity + hoo * (target - current);
        current = detX * detInv;
        velocity = detV * detInv;


        //float f = 1.0f + 2.0f * timeStep * dampingRatio * angularFrequency;
        //float oo = angularFrequency * angularFrequency;
        //float hoo = timeStep * oo;
        //float hhoo = timeStep * hoo;
        //float detInv = 1.0f / (f + hhoo);
        //Vector3 detX = f * current + timeStep * velocity + hhoo * target;
        //Vector3 detV = velocity + hoo * (target - current);
        //current = detX * detInv;
        //velocity = detV * detInv;
    }
}
