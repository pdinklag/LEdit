using System;

public class SignConverter
{
	public static byte MakeUnsigned(sbyte s)
	{
		if(s>=0)return (byte)s;
		else return (byte)(0x100+s);
	}
	
	public static ushort MakeUnsigned(short s)
	{
		if(s>=0)return (ushort)s;
		else return (ushort)(0x10000+s);
	}
	
	public static uint MakeUnsigned(int s)
	{
		if(s>=0)return (uint)s;
		else return (uint)(0x100000000+s);
	}
	
	public static sbyte MakeSigned(byte u)
	{
		if((u&0x80)==0)
			return (sbyte)(u&0x7F);
		else
			return (sbyte)(-(((~u)+1)&0xFF));
	}
	
	public static short MakeSigned(ushort u)
	{
		if((u&0x8000)==0)
			return (short)(u&0x7FFF);
		else
			return (short)(-(((~u)+1)&0xFFFF));
	}
	
	public static int MakeSigned(uint u)
	{
		if((u&0x80000000)==0)
			return (int)(u&0x7FFFFFFF);
		else
			return (int)(-(((~u)+1)&0xFFFFFFFF));
	}
}
