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
	public static List<int> GetListInt( Hashtable hashtabel, string key )
	{
		List<int> ret = new List<int>();

		if( !hashtabel.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtabel[ key ] as ArrayList;
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
	public static List<long> GetListLong( Hashtable hashtabel, string key )
	{
		List<long> ret = new List<long>();
		
		if( !hashtabel.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtabel[ key ] as ArrayList;
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
	public static List<float> GetListFloat( Hashtable hashtabel, string key )
	{
		List<float> ret = new List<float>();
		
		if( !hashtabel.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtabel[ key ] as ArrayList;
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
	public static List<double> GetListDouble( Hashtable hashtabel, string key )
	{
		List<double> ret = new List<double>();
		
		if( !hashtabel.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtabel[ key ] as ArrayList;
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
	public static List<string> GetListString( Hashtable hashtabel, string key )
	{
		List<string> ret = new List<string>();

		if( !hashtabel.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtabel[ key ] as ArrayList;
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
	public static List<Hashtable> GetListHashtable( Hashtable hashtabel, string key )
	{
		List<Hashtable> ret = new List<Hashtable>();
		
		if( !hashtabel.ContainsKey( key ) ){
			return ret;
		}

		ArrayList arrayList = hashtabel[ key ] as ArrayList;
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
	public static int GetInt( Hashtable hashtabel, string key )
	{
		if( hashtabel.ContainsKey( key ) ){
			return Convert.ToInt32( Convert.ToString( hashtabel[ key ] ) );
		}
		return 0;
	}

	//----------------------------------------------------------
	// long
	//----------------------------------------------------------
	public static long GetLong( Hashtable hashtabel, string key )
	{
		if( hashtabel.ContainsKey( key ) ){
			return Convert.ToInt64( Convert.ToString( hashtabel[ key ] ) );
		}
		return 0;
	}

	//----------------------------------------------------------
	// float
	//----------------------------------------------------------
	public static float GetFloat( Hashtable hashtabel, string key )
	{
		if( hashtabel.ContainsKey( key ) ){
			return Convert.ToSingle( Convert.ToString( hashtabel[ key ] ) );
		}
		return 0.0f;
	}

	//----------------------------------------------------------
	// double
	//----------------------------------------------------------
	public static double GetDouble( Hashtable hashtabel, string key )
	{
		if( hashtabel.ContainsKey( key ) ){
			return Convert.ToDouble( Convert.ToString( hashtabel[ key ] ) );
		}
		return 0.0f;
	}

	//----------------------------------------------------------
	// string
	//----------------------------------------------------------
	public static string GetString( Hashtable hashtabel, string key )
	{
		if( hashtabel.ContainsKey( key ) ){
			return Convert.ToString( hashtabel[ key ] );
		}
		return "";
	}

	//----------------------------------------------------------
	// Hashtable
	//----------------------------------------------------------
	public static Hashtable GetHashtable( Hashtable hashtabel, string key )
	{
		if( hashtabel.ContainsKey( key ) ){
			return hashtabel[ key ] as Hashtable;
		}
		return null;
	}

	//----------------------------------------------------------

}
//----------------------------------------------------------
