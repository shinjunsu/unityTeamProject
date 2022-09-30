/*************************
 * 최종수정일 : 2017-05-30
 * 작성자 : devchanho
 * 파일명 : TankController.cs
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    private Transform tankTR = null;
    private Transform cannonBodyTR = null;
    private Transform cannonTR = null;
    private Transform cannonBallPointTR = null;
    private MissileLauncher missileLauncher = null;

    [SerializeField]
    private const float movingSpeed = 10f;
    [SerializeField]
    private const float rotateSpeed = 50f;
    [SerializeField]
    private const float cannonBodyRotateSpeed = 500f;

    private bool isMovingForward = true;

    [SerializeField]
    private GameObject cannonBallPrefab = null;
    private GameObject cannonBallGO = null;
    private float cannonPower = 0f;
    [SerializeField]
    private const float cannonPowerIncrement = 300f;
    [SerializeField]
    private const float cannonPowerMin = 100f;
    [SerializeField]
    private const float cannonPowerMax = 500f;
    private bool isCannonCharge = false;

    private ChargeBar powerChargeBar = null;

    private bool isAutoDriving = false;
    private Vector3 autoDrivingPoint = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        tankTR = this.transform;
        Transform[] children = GetComponentsInChildren<Transform>();
        // 인덱스 0은 자기자신
        //children[1];                      // 탱크 몸체
        cannonBodyTR = children[2];         // 포탑
        cannonTR = children[3];             // 포
        cannonBallPointTR = children[4];    // 포 발사위치
        // 미사일 런처 스트립트
        missileLauncher = children[5].GetComponent<MissileLauncher>();

        powerChargeBar = GameObject.FindGameObjectWithTag("PowerChargeBar").GetComponent<ChargeBar>();

        cannonPower = cannonPowerMin;
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
        InputMouse();
        InputPicking();

        powerChargeBar.UpdatePosition(transform.position);
    }

    private void InputKey()
    {
        bool isGetKey = false;

        if (Input.GetKey(KeyCode.W))
        {
            tankTR.position +=
                tankTR.forward * movingSpeed * Time.deltaTime;
            isMovingForward = true;

            isGetKey = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            tankTR.position +=
                -tankTR.forward * movingSpeed * Time.deltaTime;
            isMovingForward = false;

            isGetKey = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if(isMovingForward == true)
                tankTR.Rotate(tankTR.up, -rotateSpeed * Time.deltaTime);
            else
                tankTR.Rotate(tankTR.up, rotateSpeed * Time.deltaTime);

            isGetKey = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (isMovingForward == true)
                tankTR.Rotate(tankTR.up, rotateSpeed * Time.deltaTime);
            else
                tankTR.Rotate(tankTR.up, -rotateSpeed * Time.deltaTime);

            isGetKey = true;
        }

        // 자동주행 도중에 키가 입력되면 정지
        if (isGetKey == true &&
            isAutoDriving == true)
            StopAutoDriving();
    }

    private void InputMouse()
    {
        // 마우스 X축 변화량 얻어오기
        float axisValue = Input.GetAxis("Mouse X");
        float rotateValue = axisValue * cannonBodyRotateSpeed * Time.deltaTime;
        cannonBodyTR.Rotate(cannonBodyTR.up, rotateValue);


        if (Input.GetMouseButtonDown(0))
        {
            // 포탄을 눕히기 위해서 X축으로 90도 회전
            Vector3 newRot = cannonBallPointTR.rotation.eulerAngles;
            newRot.x = 90f;
             cannonBallGO =
                Instantiate(
                    cannonBallPrefab,
                    cannonBallPointTR.position,
                    Quaternion.Euler(newRot));
            cannonBallGO.transform.localScale = Vector3.one;
            cannonBallGO.transform.SetParent(cannonBodyTR);

            isCannonCharge = true;
        }

        if(isCannonCharge == true && powerChargeBar.IsFull == false)
        {
            cannonPower += Time.deltaTime * cannonPowerIncrement;
            if (!powerChargeBar.Charging(cannonPower, cannonPowerMax))
            {
                cannonPower = cannonPowerMax;
                powerChargeBar.Full();
            }
            //Debug.Log("Cannon Power : " + cannonPower);
        }

        if(Input.GetMouseButtonUp(0))
        {
            CannonBall cb = cannonBallGO.GetComponent<CannonBall>();
            cb.Shot(cannonBallPointTR.forward, cannonPower);
            cannonPower = cannonPowerMin;

            cannonBallGO.transform.parent = null;

            isCannonCharge = false;

            powerChargeBar.ResetBar();
        }
    }

    private void InputPicking()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray =
                Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    missileLauncher.Shoot(hit.point);
                }
                else
                {
                    if (hit.transform.gameObject.CompareTag("Floor"))
                    {
                        if (isAutoDriving == true)
                            StopAutoDriving();

                        //Debug.Log(hit.point);
                        autoDrivingPoint = hit.point;
                        isAutoDriving = true;
                        StartCoroutine("AutoDrivingCoroutine");
                    }
                }
            }
        }
    }

    private void StopAutoDriving()
    {
        autoDrivingPoint = tankTR.position;
        isAutoDriving = false;
        // 문자열로 함수를 호출해야 정상적으로 정지
        StopCoroutine("AutoDrivingCoroutine");
        //Debug.Log("StopAutoDriving()");
    }

    private IEnumerator AutoDrivingCoroutine()
    {
        while (true)
        {
            Vector3 targetDir = autoDrivingPoint - tankTR.position;

            tankTR.localRotation =
                Quaternion.Lerp(
                    tankTR.localRotation,
                    Quaternion.LookRotation(targetDir),
                    Time.deltaTime);

            targetDir.Normalize();
            tankTR.position +=
                targetDir * movingSpeed * Time.deltaTime;

            float distance =
                Vector3.Distance(tankTR.position, autoDrivingPoint);
            if (distance < 0.1f)
            {
                StopAutoDriving();
            }

            yield return null;
        }
    }
}
