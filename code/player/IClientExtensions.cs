using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OITC;

public static class IClientExtensions
{
	public static int GetKills( this IClient self )
	{
		return self.GetInt( "kills" );
	}

	public static int GetDeaths( this IClient self )
	{
		return self.GetInt( "deaths" );
	}
}
