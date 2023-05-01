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
	
	public static void SetLastKillTick( this IClient self, int value )
	{
		self.SetInt( "ticks_since_last_kill", value );
	}

	/// <summary>
	/// The tick this player last got a kill.
	/// </summary>
	/// <param name="self"></param>
	public static int GetLastKillTick( this IClient self )
	{
		return self.GetInt( "ticks_since_last_kill" );
	}
}
