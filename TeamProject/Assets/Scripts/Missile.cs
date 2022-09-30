/*************************
 * 최종수정일 : 2017-05-30
 * 작성자 : devchanho
 * 파일명 : Missile.cs
 * http://smilejsu.tistory.com/1036
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private float gravity =
        Physics.gravity.magnitude;              // 중력
    private float elapsedTime = 0f;             // 흐른시간, 누적시간
    private Vector3 startPos = Vector3.zero;    // 시작위치
    
    public void Shoot(float _dat, Vector3 _velocity)
    {
        startPos = transform.position;
        StartCoroutine(ShootImpl(_dat, _velocity));
    }

    private IEnumerator ShootImpl(float _dat, Vector3 _velocity)
    {
        Vector3 curPos = Vector3.zero;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= _dat)
                break;

            curPos.x = startPos.x + _velocity.x * elapsedTime;
            curPos.y = startPos.y + _velocity.y * elapsedTime
                // 1/2
                - (0.5f * gravity * elapsedTime * elapsedTime);
            curPos.z = startPos.z + _velocity.z * elapsedTime;

            transform.LookAt(curPos);
            transform.position = curPos;

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (!_collision.gameObject.CompareTag("Player") &&
            !_collision.gameObject.CompareTag("Missile"))
            Destroy(this.gameObject);
    }
}