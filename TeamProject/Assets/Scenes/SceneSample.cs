using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using KWNET;

[Serializable]
public class Fire_Data   //   로그인  >>  "UserID", "Tank Type"
{
    public string UserID = "";
    public int TankType ;
    }


public class SceneSample : MonoBehaviour
{
    //GameObject
    public string userName;
    public int tankType;
    public InputField inputField_ID;

    void Start()
    {
        ConnectServer();
    }

    public void loginBtn()
    {

        userName = inputField_ID.text;
        Debug.Log("로그인");
        tankType = 1;
    }

    public void startBtn()
    {
        UserLogin(userName);
        UserRoomData(userName, tankType);
    }
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {

      
    }

	private void ConnectServer()
	{
        NetworkSample.instance.ConnectServer("------------------", 3650);
	}
	private void UserLogin(string name)
	{
        NetworkSample.instance.UserLogin(name, 1);
	}
    private void UserRoomData(string name,int type)
    {
        var data = new Fire_Data
        {
            UserID = name,
            TankType =  type
        };

        string sendData = LitJson.JsonMapper.ToJson(data);


        NetworkSample.instance.RoomData(sendData);
    }
}
