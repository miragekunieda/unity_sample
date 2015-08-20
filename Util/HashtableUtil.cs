using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//----------------------------------------------------------
// ハッシュテーブルユーティリティ
//----------------------------------------------------------
public static class HashtableUtil
{
	//-------------------------------------------------------------------------------------------------------
	// List
	//-------------------------------------------------------------------------------------------------------

	//----------------------------------------------------------
	// List<int>
	//----------------------------------------------------------
	public static List<int> GetListInt( Hashtable hashtable, string key )
	{
		List<int> ret = new List<int>();

		if( !hashtable.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtable[ key ] as ArrayList;
		if( arrayList == null ){
			return ret;
		}

		foreach( int val in arrayList ){
			ret.Add( val );
		}
		return ret;
	}

	//----------------------------------------------------------
	// List<long>
	//----------------------------------------------------------
	public static List<long> GetListLong( Hashtable hashtable, string key )
	{
		List<long> ret = new List<long>();
		
		if( !hashtable.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtable[ key ] as ArrayList;
		if( arrayList == null ){
			return ret;
		}

		foreach( long val in arrayList ){
			ret.Add( val );
		}
		return ret;
	}

	//----------------------------------------------------------
	// List<float>
	//----------------------------------------------------------
	public static List<float> GetListFloat( Hashtable hashtable, string key )
	{
		List<float> ret = new List<float>();
		
		if( !hashtable.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtable[ key ] as ArrayList;
		if( arrayList == null ){
			return ret;
		}

		foreach( float val in arrayList ){
			ret.Add( val );
		}
		return ret;
	}

	//----------------------------------------------------------
	// List<double>
	//----------------------------------------------------------
	public static List<double> GetListDouble( Hashtable hashtable, string key )
	{
		List<double> ret = new List<double>();
		
		if( !hashtable.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtable[ key ] as ArrayList;
		if( arrayList == null ){
			return ret;
		} 
		
		foreach( double val in arrayList ){
			ret.Add( val );
		}
		return ret;
	}

	//----------------------------------------------------------
	// List<string>
	//----------------------------------------------------------
	public static List<string> GetListString( Hashtable hashtable, string key )
	{
		List<string> ret = new List<string>();

		if( !hashtable.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtable[ key ] as ArrayList;
		if( arrayList == null ){
			return ret;
		} 

		foreach( string val in arrayList ){
			ret.Add( val );
		}
		return ret;
	}

	//----------------------------------------------------------
	// List<Hashtable>
	//----------------------------------------------------------
	public static List<Hashtable> GetListHashtable( Hashtable hashtable, string key )
	{
		List<Hashtable> ret = new List<Hashtable>();
		
		if( !hashtable.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtable[ key ] as ArrayList;
		if( arrayList == null ){
			return ret;
		} 

		foreach( Hashtable val in arrayList ){
			ret.Add( val );
		}
		return ret;
	}

	//-------------------------------------------------------------------------------------------------------
	// One
	//-------------------------------------------------------------------------------------------------------

	//----------------------------------------------------------
	// int
	//----------------------------------------------------------
	public static int GetInt( Hashtable hashtable, string key )
	{
		if( hashtable.ContainsKey( key ) ){
			return Convert.ToInt32( Convert.ToString( hashtable[ key ] ) );
		}
		return 0;
	}

	//----------------------------------------------------------
	// long
	//----------------------------------------------------------
	public static long GetLong( Hashtable hashtable, string key )
	{
		if( hashtable.ContainsKey( key ) ){
			return Convert.ToInt64( Convert.ToString( hashtable[ key ] ) );
		}
		return 0;
	}

	//----------------------------------------------------------
	// float
	//----------------------------------------------------------
	public static float GetFloat( Hashtable hashtable, string key )
	{
		if( hashtable.ContainsKey( key ) ){
			return Convert.ToSingle( Convert.ToString( hashtable[ key ] ) );
		}
		return 0.0f;
	}

	//----------------------------------------------------------
	// double
	//----------------------------------------------------------
	public static double GetDouble( Hashtable hashtable, string key )
	{
		if( hashtable.ContainsKey( key ) ){
			return Convert.ToDouble( Convert.ToString( hashtable[ key ] ) );
		}
		return 0.0f;
	}

	//----------------------------------------------------------
	// string
	//----------------------------------------------------------
	public static string GetString( Hashtable hashtable, string key )
	{
		if( hashtable.ContainsKey( key ) ){
			return Convert.ToString( hashtable[ key ] );
		}
		return "";
	}

	//----------------------------------------------------------
	// Hashtable
	//----------------------------------------------------------
	public static Hashtable GetHashtable( Hashtable hashtable, string key )
	{
		if( hashtable.ContainsKey( key ) ){
			return hashtable[ key ] as Hashtable;
		}
		return null;
	}

	//----------------------------------------------------------

}
//----------------------------------------------------------
