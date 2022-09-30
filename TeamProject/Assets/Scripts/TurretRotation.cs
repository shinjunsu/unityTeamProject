/*************************
 * 최종수정일 : 2017-05-29
 * 작성자 : devchanho
 * 파일명 : TurretRotation.cs
 * 참고 : https://gamedev.stackexchange.com/questions/110023/unitys-quaternion-lerp-slows-down-when-target-is-directly-behind-turret
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    private Transform target = null;

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 relativePosition = target.position - transform.position;
        relativePosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        // 지정된 회전방향으로 부드럽게 회전(보간)
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime);
    }
}