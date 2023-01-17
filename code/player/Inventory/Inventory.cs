namespace OITC;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO: Use components instead of " is BaseCarriable or is Weapon".
partial class Inventory : IInventory
{
	public Entity Owner { get; init; }
	public List<Entity> List = new List<Entity>();
	public virtual Entity Active
	{
		get
		{
			return (Owner as BBPlayer)?.ActiveChild;
		}

		set
		{
			if ( Owner is BBPlayer player )
			{
				player.ActiveChild = value;
			}
		}
	}

	public Inventory( BBPlayer player )
	{
		Owner = player;
	}

	public bool CanAdd( Entity ent )
	{
		if ( !ent.IsValid() )
			return false;

		if ( ent is Weapon bc && bc.CanCarry( Owner ) )
			return true;

		return !IsCarryingType( ent.GetType() );
	}

	public bool Add( Entity ent, bool makeActive = false )
	{
		Game.AssertServer();

		if ( !ent.IsValid() )
			return false;

		if ( IsCarryingType( ent.GetType() ) )
			return false;

		//
		// Can't pickup if already owned
		//
		if ( ent.Owner != null )
			return false;

		//
		// Let the inventory reject the entity
		//
		if ( !CanAdd( ent ) )
			return false;

		if ( ent is not Weapon carriable )
			return false;

		//
		// Let the entity reject the inventory
		//
		if ( !carriable.CanCarry( Owner ) )
			return false;

		//
		// Passed!
		//

		ent.Parent = Owner;

		//
		// Let the item do shit
		//
		carriable.OnCarryStart( Owner );

		if ( makeActive )
		{
			SetActive( ent );
		}

		return true;
	}

	/// <summary>
	/// Make this entity the active one
	/// </summary>
	public bool SetActive( Entity ent )
	{
		if ( Active == ent ) return false;
		if ( !Contains( ent ) ) return false;

		Active = ent;
		return true;
	}

	/// <summary>
	/// Returns true if this inventory contains this entity
	/// </summary>
	public bool Contains( Entity ent )
	{
		return List.Contains( ent );
	}

	public bool IsCarryingType( Type t )
	{
		return List.Any( x => x?.GetType() == t );
	}

	public bool Drop( Entity ent )
	{
		if ( !Game.IsServer )
			return false;

		if ( !Contains( ent ) )
			return false;

		ent.Parent = null;

		if ( ent is Weapon bc )
		{
			bc.OnCarryDrop( Owner );
		}

		return ent.Parent == null;
	}
	/// <summary>
	/// Returns the number of items in the inventory
	/// </summary>
	public virtual int Count() => List.Count;

	/// <summary>
	/// Returns the index of the currently active child
	/// </summary>
	public virtual int GetActiveSlot()
	{
		var ae = Active;
		var count = Count();

		for ( int i = 0; i < count; i++ )
		{
			if ( List[i] == ae )
				return i;
		}

		return -1;
	}

	/// <summary>
	/// Try to pick this entity up
	/// </summary>
	public virtual void Pickup( Entity ent )
	{

	}

	/// <summary>
	/// A child has been added to the Owner (player). Do we want this
	/// entity in our inventory? Yeah? Add it then.
	/// </summary>
	public virtual void OnChildAdded( Entity child )
	{
		if ( !CanAdd( child ) )
			return;

		if ( List.Contains( child ) )
			throw new System.Exception( "Trying to add to inventory multiple times. This is gated by Entity:OnChildAdded and should never happen!" );

		List.Add( child );
	}

	/// <summary>
	/// A child has been removed from our Owner. This might not even
	/// be in our inventory, if it is then we'll remove it from our list
	/// </summary>
	public virtual void OnChildRemoved( Entity child )
	{
		if ( List.Remove( child ) )
		{
			// On removed etc
		}
	}

	/// <summary>
	/// Set our active entity to the entity on this slot
	/// </summary>
	public virtual bool SetActiveSlot( int i, bool evenIfEmpty = false )
	{
		var ent = GetSlot( i );
		if ( Active == ent )
			return false;

		if ( !evenIfEmpty && ent == null )
			return false;

		Active = ent;
		return ent.IsValid();
	}

	/// <summary>
	/// Switch to the slot next to the slot we have active.
	/// </summary>
	public virtual bool SwitchActiveSlot( int idelta, bool loop )
	{
		var count = Count();
		if ( count == 0 ) return false;

		var slot = GetActiveSlot();
		var nextSlot = slot + idelta;

		if ( loop )
		{
			while ( nextSlot < 0 ) nextSlot += count;
			while ( nextSlot >= count ) nextSlot -= count;
		}
		else
		{
			if ( nextSlot < 0 ) return false;
			if ( nextSlot >= count ) return false;
		}

		return SetActiveSlot( nextSlot, false );
	}

	/// <summary>
	/// Drop the active entity. If we can't drop it, will return null
	/// </summary>
	public virtual Entity DropActive()
	{
		if ( !Game.IsServer ) return null;

		var ac = Active;
		if ( ac == null ) return null;

		if ( Drop( ac ) )
		{
			Active = null;
			return ac;
		}

		return null;
	}

	/// <summary>
	/// Get the item in this slot
	/// </summary>
	public virtual Entity GetSlot( int i )
	{
		if ( List.Count <= i ) return null;
		if ( i < 0 ) return null;

		return List[i];
	}

	/// <summary>
	/// Delete every entity we're carrying. Useful to call on death.
	/// </summary>
	public virtual void DeleteContents()
	{
		Game.AssertServer();

		foreach ( var item in List.ToArray() )
		{
			item.Delete();
		}

		List.Clear();
	}
}
