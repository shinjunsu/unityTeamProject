using System;
using System.Collections.Generic;
using System.IO;

namespace KWNET
{
	[System.Serializable]
	public class UserAvatar
	{
		public byte m_uKind	= 0;
		public string m_szURL = "";
		public void ReadBin(BinaryReader br)
		{
			m_uKind = br.ReadByte();
			NetString.ReadString(br, ref m_szURL);
		}

		public void WriteBin(BinaryWriter bw)
		{
			bw.Write((byte)m_uKind);
			NetString.WriteString(bw, m_szURL);
		}
	}

	[System.Serializable]
	public class UserBase
	{
		public byte m_byMainNo	= 0;	// ���μ��� ��ȣ
		public byte m_byGuest	= 0;	// �Խ�Ʈ ����
		public byte m_byPlatform = 0;	// �÷��� (PC-1, VR-2, Android-3, IOS-4)
		public Int64 m_nUserNo = 0;		// ������ȣ
		public string m_szUserID = "";   // �������̵�
		public string m_szGameID = "";   // �г���
		public UserAvatar m_clsAvatar = new UserAvatar(); // �����ƹ�Ÿ

		virtual public void ReadBin(BinaryReader br)
		{
			m_byMainNo = br.ReadByte();
			m_byGuest = br.ReadByte();
			m_byPlatform = br.ReadByte();
			m_nUserNo = br.ReadInt64();
			NetString.ReadString(br, ref m_szUserID);
			NetString.ReadString(br, ref m_szGameID);
			m_clsAvatar.ReadBin(br);
		}

		virtual public void WriteBin(BinaryWriter bw)
		{
			bw.Write((byte)m_byMainNo);
			bw.Write((byte)m_byGuest);
			bw.Write((byte)m_byPlatform);
			bw.Write((Int64)m_nUserNo);
			NetString.WriteString(bw, m_szUserID);
			NetString.WriteString(bw, m_szGameID);
			m_clsAvatar.WriteBin(bw);
		}
	}
	public class UserSession : UserBase
	{
		public byte m_ucAvatar;
		public NetVector3[] m_userTransform = new NetVector3[NetConst.SIZE_USER_TRASNFORM];
		public byte m_ucHandState1;
		public byte m_ucHandState2;
		public byte m_ucVoiceState;
		public byte m_ucVideoState;

		override public void ReadBin(BinaryReader br)
		{
			base.ReadBin(br);

			m_ucAvatar = br.ReadByte();
			for (int i = 0; i < NetConst.SIZE_USER_TRASNFORM; i++)
			{
				m_userTransform[i] = new NetVector3();
				m_userTransform[i].ReadBin(br);
			}
			m_ucHandState1 = br.ReadByte();
			m_ucHandState2 = br.ReadByte();
			m_ucVoiceState = br.ReadByte();
			m_ucVideoState = br.ReadByte();
		}

		public void ReadMoveBin(BinaryReader br)
		{
			m_nUserNo = br.ReadInt64();
			NetString.ReadString(br, ref m_szUserID);

			m_ucAvatar = br.ReadByte();
			for (int i = 0; i < NetConst.SIZE_USER_TRASNFORM; i++)
			{
				m_userTransform[i] = new NetVector3();
				m_userTransform[i].ReadBin(br);
			}
			m_ucHandState1 = br.ReadByte();
			m_ucHandState2 = br.ReadByte();
		}

		override public void WriteBin(BinaryWriter bw)
		{
			base.WriteBin(bw);

			bw.Write((byte)m_ucAvatar);
			for(int i = 0; i < NetConst.SIZE_USER_TRASNFORM; i++)
				m_userTransform[i].WriteBin(bw);

			bw.Write((byte)m_ucHandState1);
			bw.Write((byte)m_ucHandState2);
			bw.Write((byte)m_ucVoiceState);
			bw.Write((byte)m_ucVideoState);
		}

		public void SetTransform(UserSession userSession)
		{
			for (int i = 0; i < NetConst.SIZE_USER_TRASNFORM; i++)
			{
				m_userTransform[i].m_X = userSession.m_userTransform[i].m_X;
				m_userTransform[i].m_Y = userSession.m_userTransform[i].m_Y;
				m_userTransform[i].m_Z = userSession.m_userTransform[i].m_Z;
			}

			m_ucHandState1 = userSession.m_ucHandState1;
			m_ucHandState2 = userSession.m_ucHandState2;
		}
	}

	public class UserHandle : UserBase
	{
		public int m_nMoveNo = 0;	// �̵���ȣ
		public int m_nGold	= 0;	// ���
		public int m_nCash	= 0;	// ĳ��
		public byte[] m_byHandleValue = new byte[5]; //�����ڵ鰪
		public NetConHandle m_clsConHandle = new NetConHandle();// �����ڵ鰪

		override public void ReadBin(BinaryReader br)
		{
			base.ReadBin(br);

			//m_nMoveNo = br.ReadInt32();
			//m_nGold = br.ReadInt32();
			//m_nCash = br.ReadInt32();

			//m_clsConHandle.ReadBin(br);

			//for(int i = 0 ; i < 5 ; i++)
			//	m_byHandleValue[i] = br.ReadByte();
		}

		override public void WriteBin(BinaryWriter bw)
		{

		}
	}
}
