/*************************
 * 최종수정일 : 2017-05-30
 * 작성자 : devchanho
 * 파일명 : MissileLauncher.cs
 * http://smilejsu.tistory.com/1036
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour {

    [SerializeField]
    private GameObject missilePrefab = null;    // 미사일 프리팹
    private Transform missileTR = null;         // 미사일 트랜스폼
    private Missile missileScript = null;       // 미사일 스크립트
    private Vector3 velocity = Vector3.zero;    // 속도 벡터
    private float gravity =
        Physics.gravity.magnitude;              // 중력
    private float maxHeight = 0f;               // 최대높이
    private Vector3 startPos = Vector3.zero;    // 시작위치
    private Vector3 endPos = Vector3.zero;      // 끝위치

    [SerializeField]
    private float dat = 3f;                     // 도착점 도달 시간 

    private void MissileReady()
    {
        missileTR =
            Instantiate(
                missilePrefab,
                this.transform.position,
                Quaternion.identity).transform;

        missileScript = missileTR.GetComponent<Missile>();
    }

    public void Shoot(Vector3 _endPos)
    {
        MissileReady();
        startPos = missileTR.position;
        endPos = _endPos;
        
        maxHeight = 20f;

        float deltaHeight = endPos.y - startPos.y;
        float newMaxHeight = maxHeight - startPos.y;

        velocity.y = Mathf.Sqrt(2f * gravity * newMaxHeight);

        float a = gravity;
        float b = -2f * velocity.y;
        float c = 2f * deltaHeight;

        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        
        velocity.x = -(startPos.x - endPos.x) / dat;
        velocity.z = -(startPos.z - endPos.z) / dat;


        // 미사일 발사
        missileScript.Shoot(dat, velocity);
    }
}