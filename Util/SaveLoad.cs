using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//----------------------------------------------------------
// セーブロード
//----------------------------------------------------------
public static class SaveLoad
{
	// エラーコールバック
	private static Action<Exception> m_error_callback = null;

	//----------------------------------------------------------
	// エラーコールバックセット
	//----------------------------------------------------------
	private static void SetErrorCallback( Action<Exception> error_callback )
	{
		m_error_callback	= error_callback;
	}

	//----------------------------------------------------------
	// エラーコールバック
	//----------------------------------------------------------
	private static void ErrorCallback( Exception e )
	{
		if( m_error_callback != null ){
			m_error_callback( e );
		}
	}

	//----------------------------------------------------------
	// 全削除
	//----------------------------------------------------------
	public static void DeleteAll()
	{
		try{
			PlayerPrefs.DeleteAll();
		} catch( Exception e ) {
			ErrorCallback( e );
		}
	}

	//----------------------------------------------------------
	// キー指定の削除
	//----------------------------------------------------------
	public static void DeleteKey( string key )
	{
		try{
			PlayerPrefs.DeleteKey( Encryptor.SHA1( key ) );
		} catch( Exception e ) {
			ErrorCallback( e );
		}
	}

	//----------------------------------------------------------
	// セーブ
	//----------------------------------------------------------
	//-------------------------
	// string
	//-------------------------
	public static void Save( string key, string value )
	{
		try{
			PlayerPrefs.SetString( Encryptor.SHA1( key ), Encryptor.EncryptStringToBase64( value ) );
			PlayerPrefs.Save();
		} catch( Exception e ) {
			ErrorCallback( e );
		}
	}

	//-------------------------
	// int
	//-------------------------
	public static void Save( string key, int value )
	{
		try{
			PlayerPrefs.SetString( Encryptor.SHA1( key ), Encryptor.EncryptStringToBase64( value.ToString() ) );
			PlayerPrefs.Save();
		} catch( Exception e ) {
			ErrorCallback( e );
		}
	}

	//-------------------------
	// long
	//-------------------------
	public static void Save( string key, long value )
	{
		try{
			PlayerPrefs.SetString( Encryptor.SHA1( key ), Encryptor.EncryptStringToBase64( value.ToString() ) );
			PlayerPrefs.Save();
		} catch( Exception e ) {
			ErrorCallback( e );
		}
	}

	//----------------------------------------------------------
	// ロード
	//----------------------------------------------------------
	//-------------------------
	// string
	//-------------------------
	public static string LoadString( string key )
	{
		try{
			string getString = PlayerPrefs.GetString( Encryptor.SHA1( key ) );
			if( getString.Length == 0 ){
				return "";
			}
			return Encryptor.DecryptStringToBase64( getString );
		} catch( Exception e ) {
			ErrorCallback( e );
			return "";
		}
	}

	//-------------------------
	// int
	//-------------------------
	public static int LoadInt( string key )
	{
		try{
			string getString = PlayerPrefs.GetString( Encryptor.SHA1( key ) );
			if( getString.Length == 0 ){
				return 0;
			}
			return int.Parse( Encryptor.DecryptStringToBase64( getString ) );
		} catch( Exception e ) {
			ErrorCallback( e );
			return 0;
		}
	}

	//-------------------------
	// long
	//-------------------------
	public static long LoadLong( string key )
	{
		try{
			string getString = PlayerPrefs.GetString( Encryptor.SHA1( key ) );
			if( getString.Length == 0 ){
				return 0;
			}
			return long.Parse( Encryptor.DecryptStringToBase64( getString ) );
		} catch( Exception e ) {
			ErrorCallback( e );
			return 0;
		}
	}

	//----------------------------------------------------------
}

//----------------------------------------------------------
