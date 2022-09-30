#pragma warning disable 219

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace KWNET
{
	public class NetHead
	{
		public ushort m_Head1;
		public ushort m_Head2;
		public ushort m_Check;
		public byte m_Class;
		public byte m_Event;

		public void Init()
		{
			m_Head1 = 255;
			m_Head2 = 250;
			m_Check = 0;
			m_Class = 0;
			m_Event = 0;
		}

		public void MakeHead(byte byClass, byte byEvent)
		{
			Init();
			m_Check = m_Head1;
			m_Check += (ushort)m_Head2;
			m_Check += (ushort)byClass;
			m_Check += (ushort)byEvent;

			m_Class = byClass;
			m_Event = byEvent;
		}

		public void ReadBin(BinaryReader br)
		{
			m_Head1 = br.ReadUInt16();
			m_Head2 = br.ReadUInt16();
			m_Check = br.ReadUInt16();
			m_Class = br.ReadByte();
			m_Event = br.ReadByte();
		}

		public void WriteBin(BinaryWriter bw)
		{
			bw.Write((UInt16)m_Head1);
			bw.Write((UInt16)m_Head2);
			bw.Write((UInt16)m_Check);
			bw.Write((byte)m_Class);
			bw.Write((byte)m_Event);
		}

		public bool IsNull()
		{
			return ((m_Class == 0) && (m_Event == 0));
		}

		public NetHead Copy()
		{
			NetHead copy = new NetHead();
			copy.m_Head1 = this.m_Head1;
			copy.m_Head2 = this.m_Head2;
			copy.m_Check = this.m_Check;
			copy.m_Class = this.m_Class;
			copy.m_Event = this.m_Event;

			return copy;
		}
	}
}
