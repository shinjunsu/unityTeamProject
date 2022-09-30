#pragma warning disable 219

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace KWNET
{
	public class KWUtil
	{
		public static string ToHex(int i)
		{
			string hex = i.ToString("x"); // 대문자 X일 경우 결과 hex값이 대문자로 나온다.
			if (hex.Length % 2 != 0) {
			hex = "0" + hex;
			}
			return hex;
		}
		// 10진수로 변환 : Long
		public static long ToLong(string hex)
		{
			return Convert.ToInt64(hex, 16);
		}
		// 10진수로 변환 : Int
		public static int ToDec(string hex)
		{
			return Convert.ToInt32(hex, 16);
		}

		public static DateTime GetUTCTime()
		{
			DateTime dtNow = DateTime.Now;
			return TimeZoneInfo.ConvertTimeToUtc(dtNow);
		}

		public static string GetGUID()
		{
			string szGuid = "";
			DateTime dateTime = GetUTCTime();
			szGuid += KWUtil.ToHex(dateTime.Year%100);

			byte [] byMonth = new byte[1];
			byMonth[0] = (byte)(65+dateTime.Month);
			szGuid += Encoding.ASCII.GetString(byMonth);

			szGuid += KWUtil.ToHex(dateTime.Day);

			byte [] byHour = new byte[1];
			byHour[0] = (byte)(65+dateTime.Hour);
			szGuid += Encoding.ASCII.GetString(byHour);

			szGuid += KWUtil.ToHex(dateTime.Minute);
			szGuid += KWUtil.ToHex(dateTime.Second);
			szGuid += KWUtil.ToHex(dateTime.Millisecond);

			int nRand = UnityEngine.Random.Range(0, 25);

			byte [] byRandom = new byte[1];
			byRandom[0] = (byte)(65+nRand);
			szGuid += Encoding.ASCII.GetString(byRandom);

			return szGuid;
		}

		public static void DeleteChildGameObj(GameObject go)
		{
			List<Transform> liChildTr = new List<Transform>();
			for (int i = 0; i < go.transform.childCount; i++)
			{
				liChildTr.Add(go.transform.GetChild(i));
			}

			foreach (Transform tr in liChildTr)
			{
				GameObject.DestroyImmediate(tr.gameObject);
			}
			liChildTr.Clear();
		}

		public static void SendMessageToObj(GameObject go, string szFunc)
		{
			if (go != null && string.IsNullOrEmpty(szFunc) == false)
			{
				go.SendMessage(szFunc, SendMessageOptions.DontRequireReceiver);
			}
		}

        public static string ByteToString(byte[] strByte)
        {
            string str = Encoding.Default.GetString(strByte);
            return str;
        }

        public static byte[] StringToByte(string str)
        {
            byte[] StrByte = Encoding.UTF8.GetBytes(str);
            return StrByte;
        }

        public static string BytesToStringConverted(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
	}
	public class NetString
	{
		public static void ReadString(BinaryReader br, ref string str)
		{
			str = "";

			UInt16 nSize = (UInt16)br.ReadUInt16();
			if (nSize > 0)
				str = ExtendedTrim(Encoding.Unicode.GetString(br.ReadBytes(nSize * 2)));
		}

		public static void WriteString(BinaryWriter bw, string str)
		{
			UInt16 nSize = (UInt16)str.Length;
			bw.Write((UInt16)nSize);

			byte[] btData = new byte[nSize * 2];
			Encoding.Unicode.GetBytes(str, 0, str.Length, btData, 0);
			if (nSize > 0) bw.Write(btData);
		}

		public static string ExtendedTrim(string source)
		{
			string dest = source;

			int index = dest.IndexOf('\0');
			if (index > -1)
			{
				dest = source.Substring(0, index + 1);
			}
			return dest.TrimEnd('\0').Trim();
		}
	}


	public class NetRecvCallback : EqualityComparer<NetHead>
	{
		public GameObject m_ObjFunction;
		public string m_Func;

		public override bool Equals(NetHead s1, NetHead s2)
		{
			if (s1.m_Class == s2.m_Class && s1.m_Event == s2.m_Event)
				return true;
			else
				return false;
		}

		public override int GetHashCode(NetHead sh)
		{
			int hCode = sh.m_Class ^ sh.m_Event;
			return hCode.GetHashCode();
		}
	}
}
