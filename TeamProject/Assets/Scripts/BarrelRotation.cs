/*************************
 * 최종수정일 : 2017-05-29
 * 작성자 : devchanho
 * 파일명 : BarrelRotation.cs
 * 참고 : https://gamedev.stackexchange.com/questions/110023/unitys-quaternion-lerp-slows-down-when-target-is-directly-behind-turret
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotation : MonoBehaviour
{
    private Transform target = null;

    [SerializeField]
    private float speed = 1f;
    private Vector3 relPos = Vector3.zero;
    private Quaternion rot = Quaternion.identity;

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Position of target relative to my position
        relPos = (target.position - transform.position).normalized;

        rot = Quaternion.LookRotation(relPos, Vector3.up);

        // Lerping localRotatiot > rot with speed: Time.deltaTime * speed (set to 1 by default)
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, Time.deltaTime * speed);

        // Resetting y- and z-EulerAngles to 0; This way only the x-EulerAngle will be affected. 
        // The y rotation is inherited from the parent (turret_turret)
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
    }
}