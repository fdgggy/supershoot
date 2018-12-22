using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDraw : MonoBehaviour {
    public void OnDrawGizmos()
    {
        BehaviorDesigner.Manager.Movement.MovementUtility.DrawLineOfSight(transform, Vector3.zero, 90, 5, false);
    }
}
