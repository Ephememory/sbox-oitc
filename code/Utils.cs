using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public static class Utils
{


	public static bool InRange( this float self, float min, float max )
	{
		return self >= min && self <= max;
	}

	public static bool InRange( this int self, int min, int max )
	{
		return self >= min && self <= max;
	}

	public static Rotation Random( this Rotation self )
	{
		return Rotation.From( Angles.Random );
	}


	public static void UtilLog(string text)
	{
		var side = Host.IsServer ? "[SERVER]" : "[CLIENT]";
		Log.Info( $"{side} : {text}" );
	}

	public static void UtilLog(object objectIn)
	{
		var side = Host.IsServer ? "[SERVER]" : "[CLIENT]";
		Log.Info( $"{side} : {objectIn}" );
	}
}

