using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KinematicCharacterController2
{
    [CustomEditor(typeof(KinematicCharacterMotor2))]
    public class KinematicCharacterMotor2Editor : Editor
    {
        protected virtual void OnSceneGUI()
        {            
            KinematicCharacterMotor2 motor = (target as KinematicCharacterMotor2);
            if (motor)
            {
                Vector3 characterBottom = motor.transform.position + (motor.Capsule.center + (-Vector3.up * (motor.Capsule.height * 0.5f)));

                Handles.color = Color.yellow;
                Handles.CircleHandleCap(
                    0, 
                    characterBottom + (motor.transform.up * motor.MaxStepHeight), 
                    Quaternion.LookRotation(motor.transform.up, motor.transform.forward), 
                    motor.Capsule.radius + 0.1f, 
                    EventType.Repaint);
            }
        }
    }
}