/*************************
 * 최종수정일 : 2017-05-29
 * 작성자 : devchanho
 * 파일명 : ChargeBar.cs
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour {

    private RectTransform tr = null;
    private Image image = null;
    [SerializeField]
    private Vector3 offset = new Vector3(10f, 20f, 0f);
    private float maxWidth = 0f;

    private bool isFull = false;

    public bool IsFull { get { return isFull; } }

    // Use this for initialization
    void Start () {
        tr = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        maxWidth = tr.sizeDelta.x;

        ResetBar();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void UpdatePosition(Vector3 _pos)
    {
        // 게임 오브젝트의 월드위치를 카메라기준 스크린위치로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_pos);
        tr.position = screenPos + offset;
    }

    public void ResetBar()
    {
        // UI 크기 변경
        tr.sizeDelta = new Vector2(0f, tr.sizeDelta.y);
        image.color = Color.yellow;
        isFull = false;
    }

    public bool Charging(float _value, float _max)
    {
        float percent = _value / _max;
        tr.sizeDelta = new Vector2(maxWidth * percent, tr.sizeDelta.y);

        if (_value < _max) return true;
        else return false;
    }

    public void Full()
    {
        image.color = Color.red;
        tr.sizeDelta = new Vector2(maxWidth, tr.sizeDelta.y);
        isFull = true;
    }
}
