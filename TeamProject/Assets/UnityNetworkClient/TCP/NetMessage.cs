using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KWNET;
using KWNET.Message;
using System.IO;

public class NetMessage : StreamBinSession
{
	public static event System.Action<int> ActionOnConnectSuccess	= delegate {};
	public static event System.Action<int> ActionOnConnectFail		= delegate {};
	public static event System.Action<int> ActionOnDisconnect		= delegate {};

	public override int OnConnectSuccess()
	{
		int nHashCode = GetHashCode();
		TcpHelper.Instance.AddClientSession(nHashCode, this);

		ActionOnConnectSuccess(nHashCode);

		return 0;
	}
	public override int OnConnectFail()
	{
		ActionOnConnectFail(0);
		return 0;
	}

	public override int OnDisconnect()
	{
		TcpHelper.Instance.RemoveClientSession(GetHashCode());

		ActionOnDisconnect(0);

		return 0;
	}
}

public class NetMessageDispatcher : DefaultDispatchHelper<NetMessage>
{
	public static event System.Action< BinaryReader> ActionReceivePacket	= delegate {};

	int onRecv(NetMessage session, object message)
	{
		SBRecvBuffer  recvBuffer =	message as  SBRecvBuffer;
		BinaryReader br =	recvBuffer.Read_Start();
		ActionReceivePacket( br);
		SBReceiver.ReturnRecvBuffObj( recvBuffer );

		return 0;
	}
}