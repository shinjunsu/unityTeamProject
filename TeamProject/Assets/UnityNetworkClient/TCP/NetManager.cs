using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using LitJson;
using KWNET;
using KWNET.Message;

namespace KWNET
{
	public class NetManager : KWSingleton<NetManager>
	{
		private float	m_fSendCheck		= 0.0f;
		private DateTime m_dtConnectionTest	= new DateTime();

		override public void Awake( )
		{
			base.Awake();

            NetMessageDispatcher.ActionReceivePacket += OnNetReceivePacket;

			NetMessage.ActionOnConnectSuccess += OnNetConnectSuccess;
			NetMessage.ActionOnConnectFail += OnNetConnectFail;
			NetMessage.ActionOnDisconnect += OnNetConnectDisconnect;
		}

		public void Init(string ip, int port)
		{
            ConnectServer(ip, port, true);
        }

        public IPAddress GetIP(string serverIP)
        {
            IPAddress thisIp = null;
            IPHostEntry iphostentry = Dns.GetHostEntry(serverIP);// Find host name
            foreach (IPAddress ipAddress in iphostentry.AddressList)// Grab the first IP addresses
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    thisIp = ipAddress;
            }

            return thisIp;
        }

		private void Update ( )
		{
			m_fSendCheck += Time.deltaTime;	
		}

		private bool IsSendEnable()
		{
			if ( m_fSendCheck < 0.5f)
				return false;

			m_fSendCheck = 0.0f;

			return true;
		}

		public void InitConnectTest()
		{
			m_dtConnectionTest = DateTime.Now;
		}

		public bool IsConnected()
		{
			bool bConnect = NetInterface.instance.IsConnected();
			if (bConnect == false)
				return false;

			DateTime dtNow = DateTime.Now;
			TimeSpan ts = dtNow - m_dtConnectionTest;
			if (ts.Minutes > 2)
				return false;

			return true;
		}

		public void OnNetReceivePacket(BinaryReader br)
		{
			NetHead head = new NetHead();
			head.ReadBin(br);

			int size = br.ReadInt32();

			if (head.m_Class == 255 && head.m_Event == 254)
			{
				BinaryWriter bw = SBPacket.WriteStart();
				NetInterface.instance.SendData((byte)255 , (byte)253);

				m_dtConnectionTest = DateTime.Now;
				return;
			}

			NetRecvCallback netRecvObj;
			if (NetInterface.instance.m_dicRecvCallBack.TryGetValue(head, out netRecvObj))
			{
				if (netRecvObj.m_ObjFunction != null)
				{
					netRecvObj.m_ObjFunction.SendMessage(netRecvObj.m_Func, br, SendMessageOptions.DontRequireReceiver);
					return;
				}
			}

			OnPrcNetRecvPacket(head, br);
		}

		public void ConnectServer( string szIP, int nPort, bool isIntranet = false )
		{
			if (isIntranet)
			{
				NetInterface.instance.ConnectServer(szIP, nPort.ToString());
			}
			else
			{
				IPHostEntry host = Dns.GetHostEntry(szIP);
				foreach (IPAddress ip in host.AddressList)
				{
					NetInterface.instance.ConnectServer(ip.ToString(), nPort.ToString());
					break;
				}
			}
        }

		public void OnNetConnectSuccess(int nRet)
		{
			Debug.Log("OnNetConnectSuccess : " + nRet.ToString());
        }

		public void OnNetConnectFail(int nRet)
		{
			Debug.Log("OnNetConnectFail : " + nRet.ToString());
		}

		public void OnNetConnectDisconnect(int nRet)
		{
			Debug.LogError("OnNetConnectDisconnect : " + nRet.ToString());
		}

		public void DisConnectServer()
		{
			NetInterface.instance.Disconnect();
		}

		public void OnPrcNetRecvPacket(NetHead head, BinaryReader br)
		{// 전체유저에게 전송되는 패킷 처리
			if (head.m_Class == HeadClass.SOCK_MENU)
			{
			}
            else if (head.m_Class == HeadClass.SOCK_ROOM)
            {
				if (head.m_Event == HeadEvent.ROOM_DATA)
					NetworkSample.instance.Recv_ROOM_DATA(br);
				else if (head.m_Event == HeadEvent.ROOM_ENTER)
					NetworkSample.instance.Recv_ROOM_ENTER(br);
				else if (head.m_Event == HeadEvent.ROOM_MAN_IN)
					NetworkSample.instance.Recv_ROOM_MAN_IN(br);
				else if (head.m_Event == HeadEvent.ROOM_MAN_OUT)
					NetworkSample.instance.Recv_ROOM_MAN_OUT(br);
            }
		}
		public void Send_WAIT_LOGIN(string szUserID, byte uGroup, GameObject objCallback)
		{
			BinaryWriter bw = SBPacket.WriteStart();
			bw.Write(NetSetting.NET_PROTOCOL_VER);
			NetString.WriteString(bw, szUserID);
			bw.Write((byte)uGroup);

			NetInterface.instance.SendData(HeadClass.SOCK_WAIT, HeadEvent.WAIT_LOGIN, objCallback, "OnRecvWaitLogin");
		}

		public void Send_ROOM_DATA( string szData)
		{
			BinaryWriter bw = SBPacket.WriteStart();
			NetString.WriteString(bw, szData);
			NetInterface.instance.SendData(HeadClass.SOCK_ROOM, HeadEvent.ROOM_DATA);
		}
	}
}
