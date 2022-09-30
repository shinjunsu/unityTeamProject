
using System;
using System.Collections.Generic;
using System.IO;

namespace KWNET
{
	[System.Serializable]
	public class RoomObject
	{

		public int m_nIdx;
		public NetVector3[] m_transform = new NetVector3[NetConst.SIZE_OBJECT_TRASNFORM];
		public void ReadBin(BinaryReader br)
		{
			m_nIdx = br.ReadInt32();
			for (int i = 0; i < NetConst.SIZE_OBJECT_TRASNFORM; i++)
			{
				m_transform[i] = new NetVector3();
				m_transform[i].ReadBin(br);
			}
		}

		public void WriteBin(BinaryWriter bw)
		{
			bw.Write((int)m_nIdx);
			for(int i = 0; i < NetConst.SIZE_OBJECT_TRASNFORM; i++)
				m_transform[i].WriteBin(bw);
		}
		public void SetTransform(RoomObject obj)
		{
			for (int i = 0; i < NetConst.SIZE_OBJECT_TRASNFORM; i++)
			{
				m_transform[i].m_X = obj.m_transform[i].m_X;
				m_transform[i].m_Y = obj.m_transform[i].m_Y;
				m_transform[i].m_Z = obj.m_transform[i].m_Z;
		
			}
		}
	}

	[System.Serializable]
	public class RoomBase
	{
		public int m_RoomNo = 0;
		public Int64 m_nMakeUserNo = 0;
		public Int64 m_nManagerUserNo = 0;
		public string m_szRoomCode  = "";  
		public string m_szRoomName  = "";  
		public string m_szRoomPass = ""; 
		
		public byte m_byRoomMode	= 0;
		public ushort m_usMapNo	= 0;
		public ushort m_usMaxUser = 0;
		public ushort m_usNowUser = 0;
		public NetDateTime m_clsBeginTime = new NetDateTime();
		public int m_inMinutes = 0;

		virtual public void ReadBin(BinaryReader br)
		{
			m_RoomNo = br.ReadInt32();
			m_nMakeUserNo = br.ReadInt64();
			m_nManagerUserNo = br.ReadInt64();
			NetString.ReadString(br, ref m_szRoomCode);
			NetString.ReadString(br, ref m_szRoomName);
			NetString.ReadString(br, ref m_szRoomPass);

			m_byRoomMode = br.ReadByte();
			m_usMapNo = br.ReadUInt16();
			m_usMaxUser = br.ReadUInt16();
			m_usNowUser = br.ReadUInt16();
			m_clsBeginTime.ReadBin(br);
			m_inMinutes = br.ReadInt32();
		}

		virtual public void WriteBin(BinaryWriter bw)
		{

		}
	}
	public class RoomSession : RoomBase
	{
		public List<UserSession> m_userList = new List<UserSession>();
		public string m_szFileURL = "";
		public int m_nFilePos1 = 0;
		public int m_nFilePos2 = 0;
		public byte m_uVoiceState	= 0;
		public byte m_uVideoState	= 0;
		public byte m_uChatState	= 0;
		public List<RoomObject> m_objectList = new List<RoomObject>();

		override public void ReadBin(BinaryReader br)
		{
			base.ReadBin(br);

			m_userList.Clear();
			int size = br.ReadInt32();
			for (int i = 0; i < size; i++)
			{
				UserSession data = new UserSession();
				data.ReadBin(br);
				m_userList.Add(data);
			}

			NetString.ReadString(br, ref m_szFileURL);
			m_nFilePos1 = br.ReadInt32();
			m_nFilePos2 = br.ReadInt32();
			m_uVoiceState = br.ReadByte();
			m_uVideoState = br.ReadByte();
			m_uChatState = br.ReadByte();

			m_objectList.Clear();
			size = br.ReadInt32();
			for (int i = 0; i < size; i++)
			{
				RoomObject data = new RoomObject();
				data.ReadBin(br);
				m_objectList.Add(data);
			}
		}

		override public void WriteBin(BinaryWriter bw)
		{

		}
	}
}
