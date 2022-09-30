using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KWNET;
using KWNET.Message;

using System.IO;

public class NetInterface : KWSingleton<NetInterface>
{
	public bool IsInit { get; private set; }

	private NetMessage m_netMessage = null;
	public	NetRecvCallback	m_NetRecvCallback	= new NetRecvCallback();
	public	Dictionary<NetHead,NetRecvCallback> 	m_dicRecvCallBack;

	override public void Awake()
	{
		base.Awake();

		IsInit = false;
		KWNET.Message.SBReceiver.Alloc();

		m_dicRecvCallBack = new Dictionary<NetHead, NetRecvCallback>(m_NetRecvCallback);
	}

	void Start()
    {

	}

	void OnApplicationQuit()
	{
        if (IsInit == true)
		{
			Disconnect();
 		}

		TcpHelper.Instance.Stop();
	}

	void Update()
	{
        if (IsInit == false)
            return;

		TcpHelper.Instance.DipatchNetworkInterMessage();
	}


	public bool ConnectServer(string szIP, string szPort)
	{
        if (IsInit == true)
		{
			m_netMessage.Disconnect();
			m_netMessage.IsConnected = false;
			IsInit = false;
 		}

		if (TcpHelper.Instance.IsRunning == false)
		{
			if (TcpHelper.Instance.Start(isRunThread:false) == false)
			{
				return false;
			}
		}

		m_netMessage = TcpHelper.Instance.AsyncConnect<NetMessage, NetMessageDispatcher>(szIP, szPort);
		
		if (m_netMessage == null)
		{
			return false;
		}

		IsInit = true;

		return true;
	}

	public	void	Disconnect()
	{
		if (m_netMessage != null)
		{
			m_netMessage.Disconnect();
			m_netMessage.IsConnected = false;
		}
		IsInit = false;
	}

    public bool IsConnected()
    {
        if (IsInit)
		{
			if (m_netMessage == null)
				return false;

            return m_netMessage.IsConnected;
		}
        return false;
    }

	public	void	SendData(byte byClass, byte byEvent, GameObject goFunction = null, string szRecvFunc = "")
	{
		NetHead head = new NetHead();
		head.MakeHead(byClass, byEvent);

		if( goFunction != null )
		{
			NetRecvCallback netRecvObj;
			if (NetInterface.instance.m_dicRecvCallBack.TryGetValue(head, out netRecvObj) == false)
			{
				NetRecvCallback 	netCallBack = new NetRecvCallback();
				netCallBack.m_ObjFunction	= goFunction;
				netCallBack.m_Func = szRecvFunc;

				m_dicRecvCallBack[head] = netCallBack;
			}
			else
			{
				netRecvObj.m_ObjFunction = goFunction;
				netRecvObj.m_Func = szRecvFunc;
			}
		}

		SBPacket.SetHeader( head );

        m_netMessage.AsyncSend();
	}

	public void SetRecvCallBack(byte uClass, byte uEvent, GameObject goFunction, string szRecvFunc)
	{
		NetHead head = new NetHead();
		head.MakeHead(uClass, uEvent);

		if( goFunction != null )
		{
			NetRecvCallback netRecvObj;
			if (NetInterface.instance.m_dicRecvCallBack.TryGetValue(head, out netRecvObj) == false)
			{
				NetRecvCallback 	netCallBack = new NetRecvCallback();
				netCallBack.m_ObjFunction	= goFunction;
				netCallBack.m_Func = szRecvFunc;

				m_dicRecvCallBack[head] = netCallBack;
			}
			else
			{
				netRecvObj.m_ObjFunction = goFunction;
				netRecvObj.m_Func = szRecvFunc;
			}
		}
	}
}
