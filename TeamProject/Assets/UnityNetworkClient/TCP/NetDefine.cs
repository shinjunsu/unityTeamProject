namespace KWNET
{
	public class NetSetting
	{
		public const int    NET_HEAD_SIZE		= 12;
		public const ushort NET_PROTOCOL_VER	= 11;
		public const float	NET_WAIT_TIME		= 0.25f;
	};

	public class HeadClass
	{
		public const byte SOCK_MENU		= 11;
		public const byte SOCK_WAIT		= 12;
		public const byte SOCK_ROOM		= 13;
	};

	public class NetConst
	{
		public const int SIZE_USER_TRASNFORM	= 4;
		public const int SIZE_OBJECT_TRASNFORM	= 3;
	}

	public class HeadEvent
	{
		public const byte WAIT_LOGIN			= 11;

		public const byte ROOM_ENTER			= 1;
		public const byte ROOM_EXIT				= 2;
		public const byte ROOM_MAN_IN			= 3;
		public const byte ROOM_MAN_OUT			= 4;
		public const byte ROOM_DATA				= 5;
	};													

	public class KW_ERROR
	{
		public const ushort ERROR_SUCCESS				= 0;
		public const ushort ERROR_DEFAULT				= 1;
		public const ushort ERROR_DB_FAIL				= 2;
		public const ushort ERROR_DB_EXEC				= 3;
	};
}
