using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using KWNET;

[Serializable]
public class Fire_Data
{
    public string UserID = "";
    public string DataID = "";
    public string Power = "";
    public string Position = "";
}


public class SceneSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ConnectServer();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UserLogin();
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UserRoomData();
        } 
    }

	private void ConnectServer()
	{
        NetworkSample.instance.ConnectServer("3.34.116.91", 3650);
	}
	private void UserLogin()
	{
        NetworkSample.instance.UserLogin("UserID111", 1);
	}
    private void UserRoomData()
    {
        var data = new Fire_Data
        { 
            UserID = "User10", 
            DataID = "1",
            Power = "10",
            Position = "1,1,1"
        };

        string sendData = LitJson.JsonMapper.ToJson(data);


        NetworkSample.instance.RoomData(sendData);
    }
}
