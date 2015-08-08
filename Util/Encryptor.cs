using UnityEngine;
using System; 
using System.IO; 
using System.Text;
using System.Security.Cryptography; 

//----------------------------------------------------------
// 暗号化 & 複合化
//----------------------------------------------------------
public static class Encryptor
{
	private const string m_apiKey	= "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
	private const string m_apiIv	= "BBBBBBBBBBBBBBBB";

	private static byte[] m_Key		= Encoding.UTF8.GetBytes( m_apiKey );
	private static byte[] m_Iv		= Encoding.UTF8.GetBytes( m_apiIv );

	//-------------------------------------------------------------------------------------------------------
	// 暗号化
	//-------------------------------------------------------------------------------------------------------
	public static byte[] Encrypt( byte[] data )
	{
		MemoryStream memoryStream	= new MemoryStream();

		Rijndael rijndael			= Rijndael.Create();
		rijndael.Mode				= CipherMode.CBC;
		rijndael.Key				= m_Key;
		rijndael.IV					= m_Iv;

		CryptoStream cryptoStream	= new CryptoStream( memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write );
		cryptoStream.Write( data, 0, data.Length );
		cryptoStream.Close();

		memoryStream.Close();

		byte[] encryptBytes			= memoryStream.ToArray();

		return encryptBytes;
	}

	//----------------------------------------------------------
	// string ==> 暗号化 ==> Base64
	//----------------------------------------------------------
	public static string EncryptStringToBase64( string str )
	{ 
		if( str.Length == 0 ){
			return "";
		}

		byte[] strBytes		= Encoding.UTF8.GetBytes( str );

		byte[] encryptBytes	= Encrypt( strBytes );

		return Convert.ToBase64String( encryptBytes );
	}

	//----------------------------------------------------------
	// string ==> Zip ==> 暗号化 ==> ByteArray
	//----------------------------------------------------------
	public static byte[] EncryptStringZip( string str )
	{
		if( str.Length == 0 ){
			return null;
		}

		byte[] strBytes		= Encoding.UTF8.GetBytes( str );

		byte[] zipBytes		= Zip.Compress( strBytes );

		byte[] encryptBytes	= Encrypt( zipBytes );

		return encryptBytes;
	}
	
	//----------------------------------------------------------
	// string ==> Zip ==> 暗号化 ==> ByteArray ==> Base64
	//----------------------------------------------------------
	public static string EncryptStringToZipBase64( string str )
	{ 
		if( str.Length == 0 ){
			return "";
		}

		byte[] encryptStringZipBytes = EncryptStringZip( str );

		return Convert.ToBase64String( encryptStringZipBytes );
	}

	//-------------------------------------------------------------------------------------------------------
	// 複合化
	//-------------------------------------------------------------------------------------------------------
	public static byte[] Decrypt( byte[] data )
	{ 
		MemoryStream memoryStream	= new MemoryStream();

		Rijndael rijndael			= Rijndael.Create();
		rijndael.Mode				= CipherMode.CBC;
		rijndael.Key				= m_Key;
		rijndael.IV					= m_Iv;

		CryptoStream cryptoStream	= new CryptoStream( memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write );
		cryptoStream.Write( data, 0, data.Length );
		cryptoStream.Close();

		memoryStream.Close();

		byte[] decryptBytes			= memoryStream.ToArray();

		return decryptBytes;
	}

	//----------------------------------------------------------
	// ( string ==> 暗号化 ==> Base64 )
	// 
	//
	// Base64 ==> 複合化 ==> string
	//----------------------------------------------------------
	public static string DecryptStringToBase64( string base64 )
	{
		if( base64.Length == 0 ){
			return "";
		}

		byte[] base64Bytes		= Convert.FromBase64String( base64 );

		byte[] decryptBytes	= Decrypt( base64Bytes );

		return Encoding.UTF8.GetString( decryptBytes );
	}

	//----------------------------------------------------------
	// ( string ==> Zip ==> 暗号化 ==> ByteArray )
	//
	//
	// ByteArray ==> 複合化 ==> Unzip ==> string
	//----------------------------------------------------------
	public static string DecryptStringZip( byte[] data )
	{
		byte[] decryptBytes	= Decrypt( data );

		byte[] unzipBytes	= Zip.Uncompress( decryptBytes );

		return Encoding.UTF8.GetString( unzipBytes );
	}

	//----------------------------------------------------------
	// ( string ==> Zip ==> 暗号化 ==> ByteArray ==> Base64 )
	//
	//
	// Base64 ==> ByteArray ==> 複合化 ==> Unzip ==> string
	//----------------------------------------------------------
	public static string DecryptStringToZipBase64( string base64 )
	{
		if( base64.Length == 0 ){
			return "";
		}

		byte[] base64Bytes			= Convert.FromBase64String( base64 );

		string decryptStringZipStr	= DecryptStringZip( base64Bytes );

		return decryptStringZipStr;
	}

	//-------------------------------------------------------------------------------------------------------
	// MD5
	//-------------------------------------------------------------------------------------------------------
	public static string MD5( string str )
	{
		if( str.Length == 0 ){
			return "";
		}

		string result	= "";
		byte[] strBytes	= Encoding.UTF8.GetBytes( str );

		MD5CryptoServiceProvider rds	= new MD5CryptoServiceProvider();
		byte[] rdsBytes					= rds.ComputeHash( strBytes );

		rds.Clear();

		result = BitConverter.ToString( rdsBytes ).ToLower().Replace( "-", "" );

		return result;
	}

	//-------------------------------------------------------------------------------------------------------
	// SHA1
	//-------------------------------------------------------------------------------------------------------
	public static string SHA1( string str )
	{
		if( str.Length == 0 ){
			return "";
		}

		string result	= "";
		byte[] strBytes	= Encoding.UTF8.GetBytes( str );

		SHA1CryptoServiceProvider sha1	= new SHA1CryptoServiceProvider();
		byte[] sha1Bytes				= sha1.ComputeHash( strBytes );

		sha1.Clear();

		result = BitConverter.ToString( sha1Bytes ).ToLower().Replace( "-", "" );

		return result;
	}

	//----------------------------------------------------------

}
//----------------------------------------------------------
