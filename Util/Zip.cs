using UnityEngine;
using System.IO; 
using ICSharpCode.SharpZipLib.GZip;

//----------------------------------------------------------
// Zip
//----------------------------------------------------------
public static class Zip
{
	//----------------------------------------------------------
	// 圧縮
	//----------------------------------------------------------
	public static byte[] Compress( byte[] data )
	{
		MemoryStream memoryStreamOut		= new MemoryStream();
		
		GZipOutputStream gZipOutputStream	= new GZipOutputStream( memoryStreamOut );
		gZipOutputStream.Write( data, 0, data.Length );
		gZipOutputStream.Close();
		
		memoryStreamOut.Close();
		
		byte[] zipBytes = memoryStreamOut.ToArray();
		
		return zipBytes;
	}
	
	//----------------------------------------------------------
	// 解凍
	//----------------------------------------------------------
	public static byte[] Uncompress( byte[] data )
	{
		MemoryStream memoryStreamIn			= new MemoryStream( data );
		
		GZipInputStream gZipInputStream		= new GZipInputStream( memoryStreamIn );
		
		MemoryStream memoryStreamOut		= new MemoryStream(); 
		
		byte[] buf							= new byte[ 1024 ];
		
		int read_size;
		
		while( ( read_size = gZipInputStream.Read( buf, 0, buf.Length ) ) > 0 ){
			memoryStreamOut.Write( buf, 0, read_size );
		}
		
		memoryStreamIn.Close();
		gZipInputStream.Close();
		memoryStreamOut.Close();
		
		byte[] unzipBytes = memoryStreamOut.ToArray();
		
		return unzipBytes;
	}

	//----------------------------------------------------------
}