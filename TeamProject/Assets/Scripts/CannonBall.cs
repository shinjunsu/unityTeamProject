/*************************
 * 최종수정일 : 2017-05-29
 * 작성자 : devchanho
 * 파일명 : CannonBall.cs
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

    private Rigidbody rb = null;
    private Vector3 direction = Vector3.zero;
    private float cannonPower = 0f;
    private bool isShot = false;

    private CapsuleCollider cc = null;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
        cc = this.GetComponent<CapsuleCollider>();
        // 스크립트 끄기
        cc.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(direction * cannonPower);
	}

    public void Shot(Vector3 _dir, float _cannonPower)
    {
        direction = _dir;
        cannonPower = _cannonPower;
        isShot = true;

        rb.useGravity = true;
        // 스크립트 켜기
        cc.enabled = true;

        // 5초후 함수 호출
        Invoke("SelfDestroy", 5f);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if(!_collision.gameObject.CompareTag("Player"))
            SelfDestroy();
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
