using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;

using KWNET;

public class NetworkSample : KWSingleton<NetworkSample>
{
	public UserHandle m_userHandle = new UserHandle();
	public List<UserSession> m_waitUserList = new List<UserSession>();
	public List<RoomBase> m_roomList = new List<RoomBase>();
	public RoomSession m_roomSession = new RoomSession();

	override public void Awake()
	{
		base.Awake();
	}

	public void ConnectServer(string szIP, int nPort)
	{
		NetManager.instance.Init(szIP, nPort);	
	}

	public void Recv_ROOM_ENTER(BinaryReader br)
	{
		m_roomSession.ReadBin(br);

		Debug.Log("Recv_ROOM_ENTER : " + m_roomSession.m_RoomNo.ToString() );
	}
	public void Recv_ROOM_MAN_IN(BinaryReader br)
	{
		UserSession userSession = new UserSession();
		userSession.ReadBin(br);

		m_roomSession.m_userList.Add(userSession);

		Debug.Log("Recv_ROOM_MAN_IN : " + userSession.m_szUserID );
	}
	public void Recv_ROOM_MAN_OUT(BinaryReader br)
	{
		UserSession userSession = new UserSession();
		userSession.ReadBin(br);

		for(int i = 0; i < m_roomSession.m_userList.Count; i++)
		{
			if (m_roomSession.m_userList[i].m_nUserNo == userSession.m_nUserNo)
			{
				m_roomSession.m_userList.RemoveAt(i);
				break;
			}
		}

		Debug.Log("Recv_ROOM_MAN_OUT : " + userSession.m_szUserID );
	}
	public void Recv_ROOM_DATA(BinaryReader br)
	{
		string szData = "";
		NetString.ReadString(br, ref szData);

		Debug.Log("Recv_ROOM_DATA" + szData);
	}

	public void UserLogin(string szID, byte byGroup)
	{
		NetManager.instance.Send_WAIT_LOGIN(szID, byGroup, this.gameObject);	
	}

	public void OnRecvWaitLogin(BinaryReader br)
	{
		ushort usResult = br.ReadUInt16();
		m_userHandle.ReadBin(br);

		m_roomList.Clear();
		int size = br.ReadInt32();
		for (int i = 0; i < size; i++)
		{
			RoomBase data = new RoomBase();
			data.ReadBin(br);
			m_roomList.Add(data);
		}

		Debug.Log("Recv_WAIT_LOGIN");
	}

	public void RoomData(string szData)
	{
		NetManager.instance.Send_ROOM_DATA( szData );	
	}
}
