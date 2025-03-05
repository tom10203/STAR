using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class FirstRobot : MonoBehaviour
{
    [SerializeField] private Transform firstRobotTarget;
    [SerializeField] private Target target;
    [SerializeField] private Transform eyeL;
    [SerializeField] private Transform eyeR;
    [SerializeField] private LineRenderer laserL;
    [SerializeField] private LineRenderer laserR;
    private bool shooting = false;
    private float shootingTimer = 0f;
    private float shootingTime = 1f;
    private Vector3 targetPos;

    public void DoLazer()
    {
        targetPos = firstRobotTarget.position;
        shooting = true;
        target.TargetHit();
    }

    void Update()
    {
        if (shooting)
        {
            laserL.SetPosition(0, eyeL.position);
            laserR.SetPosition(0, eyeR.position);
            laserL.SetPosition(1, targetPos + (targetPos - eyeL.position));
            laserR.SetPosition(1, targetPos + (targetPos - eyeR.position));

            laserL.enabled = true;
            laserR.enabled = true;

            if (shootingTimer < shootingTime)
            {
                shootingTimer += Time.deltaTime;
            }
            else
            {
                shooting = false;
                laserL.enabled = false;
                laserR.enabled = false;
                this.enabled = false;
            }
        }
    }
}
