using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Utils
{


	public static bool InRange( this float self, float min, float max )
	{
		return self >= min && self <= max;
	}

	public static Rotation Random( this Rotation self )
	{
		return Rotation.From( Angles.Random );
	}
}

