using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

//----------------------------------------------------------------
// エクセルマスタークリエート
// Excelからマスターデータを作成する
//----------------------------------------------------------------
public class ExcelMasterCreate
{
	// マスターxlsxファイルパス
	private const string master_xlsx_path				= "Assets/Editor/ExcelMaster/ExcelMaster.xlsx";

	// マスターVOを格納するパス
	private const string master_vo_save_path			= "Assets/Editor/ExcelMaster/VOs/";

	// スクリプタブルオブジェクトを格納するパス
	private const string scriptable_object_save_path	= "Assets/Resources/ExcelMaster/";

	// スクリプタブルオブジェクトのロードパス
	private const string scriptable_object_load_path	= "ExcelMaster/";

	// 作成したクラス
	static private List<string>	makeClass				= new List<string>();

	// 作成したクラスのデータ数
	static private List<int> makeClassDataCnt			= new List<int>();

	//----------------------------------------------------------------
	// 作成
	//----------------------------------------------------------------
	[MenuItem( "Assets/ExcelMaster/Create" )]
	static void RunCreate()
	{
		string[] delete_assets;

		// エディターが再生中は実行しない
		if( EditorApplication.isPlaying ) {
			Debug.Log( "エディタが再生中の場合は実行できません。" );
			return;
		}

		// コンパイル中
		if( EditorApplication.isCompiling ) {
			Debug.Log( "エディタがコンパイル中です。コンパイルが終了したら実行してください。" );
			return;
		}

		// スクリプタブルオブジェクトを格納するパス（削除）
		delete_assets = Directory.GetFiles( scriptable_object_save_path, "*.*" );
		foreach( string delete_asset in delete_assets ) {
			AssetDatabase.DeleteAsset( delete_asset );
		}

		// マスターVOを格納するパス（削除）
		delete_assets = Directory.GetFiles( master_vo_save_path, "*.*" );
		foreach( string delete_asset in delete_assets ) {
			AssetDatabase.DeleteAsset( delete_asset );
		}

		// エクセルファイル
		FileStream fileStream = new FileStream(
				master_xlsx_path,
				FileMode.Open,
				FileAccess.Read,
				FileShare.ReadWrite
			);

		// xlsx
		XSSFWorkbook xssfWorkbook = new XSSFWorkbook( fileStream );

		// クラス作成とスクリプタブルオブジェクトのアセット化
		if( !MakeClassAndScriptableObject( xssfWorkbook ) ) {
			return;
		}

		// エクセルファイルクローズ
		fileStream.Close();

		// 終了
		EditorUtility.DisplayDialog( "エクセルからのマスター環境構築。", "マスター環境が作成されました。コンパイル後に Import してください。", "OK" );
	}

	//----------------------------------------------------------------
	// インポート
	//----------------------------------------------------------------
	[MenuItem( "Assets/ExcelMaster/Import" )]
	static void RunImport()
	{
		// エディターが再生中は実行しない
		if( EditorApplication.isPlaying ) {
			Debug.Log( "エディタが再生中の場合は実行できません。" );
			return;
		}

		// コンパイル中
		if( EditorApplication.isCompiling ) {
			Debug.Log( "エディタがコンパイル中です。コンパイルが終了したら実行してください。" );
			return;
		}

		// エクセルファイル
		FileStream fileStream = new FileStream(
				master_xlsx_path,
				FileMode.Open,
				FileAccess.Read,
				FileShare.ReadWrite
			);

		// xlsx
		XSSFWorkbook xssfWorkbook = new XSSFWorkbook( fileStream );

		// 更新開始
		Type t = GetTypeByClassName( "ExcelMasterCtrl" );
		if( t == null ) {
			Debug.Log( "作成されていません Create を実行してください。" );
			fileStream.Close();
			return;
		}

		// スクリプタブルオブジェクトの作成
		CreateScriptableObjects( xssfWorkbook );
		
		// 更新開始を呼ぶ		
		MethodInfo methodInfo = t.GetMethod( "UpdateStart" );
		methodInfo.Invoke( null, null );

		// データの流し込み
		Read( xssfWorkbook );

		// エクセルファイルクローズ
		fileStream.Close();

		// 終了
		EditorUtility.DisplayDialog( "エクセルからのマスターインポート", "インポートが終了しました。アセットの更新をお待ちください。", "OK" );
	}

	//----------------------------------------------------------------
	// クラス作成とスクリプタブルオブジェクトのアセット化
	//----------------------------------------------------------------
	static public bool MakeClassAndScriptableObject( IWorkbook book )
	{
		int i;
		int x;
		int y;
		int j;

		// ファイル名
		string filename;

		// プロパティ情報
		List<string>	description		= new List<string>();
		List<string>	name			= new List<string>();
		List<string>	type			= new List<string>();
		string			now				= "";

		// 列のコメント情報
		List<int>		comment_column	= new List<int>();

		ISheet	sheet;
		IRow	row;
		ICell	cell;

		// 終了列
		int end_column;

		// データ数
		int data_count;

		// 値
		string value;

		// BOM有り UTF8
		Encoding encoding = new System.Text.UTF8Encoding( true );

		//------------------------------------------------
		// マスターコントローラー
		//------------------------------------------------

		// マスターコントローラーファイル名
		filename = master_vo_save_path + "ExcelMasterCtrl.cs";

		// ストリーム作成(マスターコントローラー)
		StreamWriter msStreamWriter = new StreamWriter( filename, false, encoding );

		// 共通部分
		msStreamWriter.WriteLine( "using UnityEngine;" );
		msStreamWriter.WriteLine( "using System;" );
		msStreamWriter.WriteLine( "using System.Collections;" );
		msStreamWriter.WriteLine( "using System.Collections.Generic;" );
		msStreamWriter.WriteLine( "" );
		msStreamWriter.WriteLine( "//------------------------------------------------" );
		msStreamWriter.WriteLine( "// エクセルマスターコントローラー" );
		msStreamWriter.WriteLine( "//------------------------------------------------" );
		msStreamWriter.WriteLine( "public class ExcelMasterCtrl" );
		msStreamWriter.WriteLine( "{" );

		// 作成したクラス初期化
		makeClass.Clear();

		// 作成したクラスのデータ数初期化
		makeClassDataCnt.Clear();

		//---------------------------------------------
		// シート
		//---------------------------------------------
		for( i = 0 ; i < book.NumberOfSheets ; ++i ) {

			// シート取得
			sheet = book.GetSheetAt( i );
			if( sheet == null ) {
				continue;
			}

			// プロパティ情報クリア
			description.Clear();
			name.Clear();
			type.Clear();
			now	= "";

			// 列のコメント情報クリア
			comment_column.Clear();

			// 終了列
			end_column = -1;

			// データ数
			data_count = 0;

			//---------------------------------------------
			// 行
			//---------------------------------------------
			for( y = sheet.FirstRowNum ; y < sheet.LastRowNum ; y++ ) {

				// 行取得
				row = sheet.GetRow( y );
				if( row == null ) {
					continue;
				}

				//---------------------------------------------
				// 列
				//---------------------------------------------
				for( x = row.FirstCellNum ; x < row.LastCellNum ; x++ ) {

					// 列取得
					cell = row.GetCell( x );
					if( cell == null ) {
						continue;
					}

					// 値
					value = Convert.ToString( cell );
					value = value.Replace( "\r", "" ).Replace( "\n", "" );

					// コメント列の時は無視
					if( comment_column.Contains( x ) ) {
						continue;
					}

					//----------------------------------------------------------------
					// ここからヘッダ解析

					// 0 列目にヘッダタイプ
					if( x == 0 ) {
						now = "";
						if( value  == "S" ) {	// 開始行
							now = value;
						}
						if( value  == "D" ) {	// 説明行
							now = value;
						}
						if( value  == "N" ) {	// プロパティ名行
							now = value;
						}
						if( value  == "T" ) {	// 型行
							now = value;
						}
						if( value  == "E" ) {	// 最終行
							break;
						}
						if( value  == "#" ) {	// 無効行
							break;
						}

						// ヘッダ解析時、上記以外はデータ
						if( now == "" ) {
							data_count++;
							break;
						}

						// 上記に該当ありなので列を進める
						continue;
					}

					// コメント列 or 終了列情報
					if( now == "S" ) {
						// コメント列
						if( value == "#" ) {
							comment_column.Add( x );
							continue;
						}
						// 終了列
						if( value == "E" ) {
							end_column = x;
							break;
						}
						continue;
					}

					// 終了列
					if( end_column == x ) {
						break;
					}

					// 空白
					if( cell.CellType == CellType.Blank ) {
						Debug.LogError( "列の終端Eがないか " + now + " 行に空白があります。" );
						continue;
					}

					// プロパティ情報を登録
					if( now == "D" ) {				// 説明
						description.Add( value );
						continue;
					}
					if( now == "N" ) {				// プロパティ名
						name.Add( value );
						continue;
					}
					if( now == "T" ) {				// 型
						type.Add( value );
						continue;
					}

					// ここまでヘッダ解析
					//----------------------------------------------------------------

				}

			}

			// エラーチェック
			int count = description.Count;
			if( count != name.Count || count != type.Count ) {
				Debug.LogError( sheet.SheetName + ":プロパティ情報の数が一致しません" + ":description=" +  description.Count + ":name=" +  name.Count + ":type=" +  type.Count );
				continue;
			}

			// ファイル名
			filename = master_vo_save_path + sheet.SheetName + "_VOs.cs";

			// ストリーム作成
			StreamWriter voStreamWriter = new StreamWriter( filename, false, encoding );

			// using
			voStreamWriter.WriteLine( "using UnityEngine;" );
			voStreamWriter.WriteLine( "using System.Collections;" );
			voStreamWriter.WriteLine( "using System.Collections.Generic;" );
			voStreamWriter.WriteLine( "" );

			// Serializable
			voStreamWriter.WriteLine( "[System.Serializable]" );

			//--------------------------------------------------
			// 単数 class
			//--------------------------------------------------
			voStreamWriter.WriteLine( "public class " + sheet.SheetName + "_VO" );
			voStreamWriter.WriteLine( "{" );

			// プロパティ作成
			for( j = 0 ; j < description.Count ; j++ ) {
				string p = "\tpublic " + type[ j ] + " " + name[ j ] + ";\t// " + description[ j ];
				voStreamWriter.WriteLine( p );
			}

			voStreamWriter.WriteLine( "}" );

			//--------------------------------------------------
			// 複数 class
			//--------------------------------------------------
			voStreamWriter.WriteLine( "public class " + sheet.SheetName + "_VOs : ScriptableObject" );
			voStreamWriter.WriteLine( "{" );

			// プロパティ作成
			voStreamWriter.WriteLine( "\tpublic List<" + sheet.SheetName + "_VO> elements = new List<" + sheet.SheetName + "_VO>();" );
			voStreamWriter.WriteLine( "}" );

			// ストリームクローズ
			voStreamWriter.Close();

			// 作成したクラス
			makeClass.Add( sheet.SheetName );

			// 作成したクラスのデータ数
			makeClassDataCnt.Add( data_count );

			//------------------------------------------------
			// マスターコントローラー
			//------------------------------------------------

			msStreamWriter.WriteLine( "\t//------------------------------------------------" );
			msStreamWriter.WriteLine( "\t// " + sheet.SheetName );
			msStreamWriter.WriteLine( "\t//------------------------------------------------" );

			// プロパティ宣言
			msStreamWriter.WriteLine( "\tstatic public " + sheet.SheetName + "_VOs m_" + sheet.SheetName + "_VOs;" );

			// AddData VOs メソッド
			msStreamWriter.WriteLine( "\tstatic public void UpdateData_" + sheet.SheetName + "_VOs( int i, string propertyName, string value )" );
			msStreamWriter.WriteLine( "\t{" );

			// add
			msStreamWriter.WriteLine( "\t\t" + sheet.SheetName + "_VO add_" + sheet.SheetName + "_VO = m_" + sheet.SheetName + "_VOs.elements[ i ];" );
			msStreamWriter.WriteLine( "" );

			for( j = 0 ; j < name.Count ; j++ ) {

				msStreamWriter.WriteLine( "\t\tif( propertyName == \"" + name[ j ] +"\" ){" );

				// 型別
				if( type[ j ] == "string" ) {
					msStreamWriter.WriteLine( "\t\t\tadd_" + sheet.SheetName + "_VO." + name[ j ] + " = value;" );
				}
				if( type[ j ] == "int" ) {
					msStreamWriter.WriteLine( "\t\t\tadd_" + sheet.SheetName + "_VO." + name[ j ] + " = ( " + type[ j ] + " )Convert.ToInt32( value );" );
				}
				if( type[ j ] == "float" ) {
					msStreamWriter.WriteLine( "\t\t\tadd_" + sheet.SheetName + "_VO." + name[ j ] + " = ( " + type[ j ] + " )Convert.ToSingle( value );" );
				}
				if( type[ j ] == "double" ) {
					msStreamWriter.WriteLine( "\t\t\tadd_" + sheet.SheetName + "_VO." + name[ j ] + " = ( " + type[ j ] + " )Convert.ToDouble( value );" );
				}
				if( type[ j ] == "long" ) {
					msStreamWriter.WriteLine( "\t\t\tadd_" + sheet.SheetName + "_VO." + name[ j ] + " = ( " + type[ j ] + " )Convert.ToInt64( value );" );
				}

				msStreamWriter.WriteLine( "\t\t}" );

				msStreamWriter.WriteLine( "" );
			}

			msStreamWriter.WriteLine( "\t}" );
			msStreamWriter.WriteLine( "" );
		}

		//------------------------------------------------
		// マスターコントローラー
		//------------------------------------------------

		//---------------------------
		// UpdateStart メソッド
		//---------------------------

		msStreamWriter.WriteLine( "\t//------------------------------------------------" );
		msStreamWriter.WriteLine( "\t// 更新開始" );
		msStreamWriter.WriteLine( "\t//------------------------------------------------" );

		msStreamWriter.WriteLine( "\tstatic public void UpdateStart()" );
		msStreamWriter.WriteLine( "\t{" );
		for( i = 0 ; i < makeClass.Count ; i++ ) {

			// Resources.Load
			msStreamWriter.WriteLine( "\t\tm_" + makeClass[ i ] + "_VOs = " + "Resources.Load<" + makeClass[ i ] + "_VOs>( \"" + scriptable_object_load_path + makeClass[ i ] + "_VOs\" );" );

			// Clear
			msStreamWriter.WriteLine( "\t\tm_" + makeClass[ i ] + "_VOs.elements.Clear();" );

			// for
			msStreamWriter.WriteLine( "\t\tfor( int i = 0 ; i < " + makeClassDataCnt[ i ] + " ; i++ ) {" );

			// Add
			msStreamWriter.WriteLine( "\t\t\tm_" +makeClass[ i ] + "_VOs.elements.Add( new " + makeClass[ i ] + "_VO() );" );

			msStreamWriter.WriteLine( "\t\t}" );
			
			// 改行
			msStreamWriter.WriteLine( "" );
		}
		msStreamWriter.WriteLine( "\t}" );
		msStreamWriter.WriteLine( "" );

		//---------------------------
		// UpdateData メソッド
		//---------------------------

		msStreamWriter.WriteLine( "\t//------------------------------------------------" );
		msStreamWriter.WriteLine( "\t// 更新" );
		msStreamWriter.WriteLine( "\t//------------------------------------------------" );

		msStreamWriter.WriteLine( "\tstatic public void UpdateData( string className, int i, string propertyName, string value )" );
		msStreamWriter.WriteLine( "\t{" );
		for( i = 0 ; i < makeClass.Count ; i++ ) {
			msStreamWriter.WriteLine( "\t\tif( className == \"" + makeClass[ i ] + "_VOs\" ){" );
			msStreamWriter.WriteLine( "\t\t\tUpdateData_" + makeClass[ i ] + "_VOs( i, propertyName, value );" );
			msStreamWriter.WriteLine( "\t\t}" );
			msStreamWriter.WriteLine( "" );
		}

		msStreamWriter.WriteLine( "\t}" );
		msStreamWriter.WriteLine( "" );

		// 終端
		msStreamWriter.WriteLine( "\t//------------------------------------------------" );
		msStreamWriter.WriteLine( "}" );

		// ストリームクローズ
		msStreamWriter.Close();

		//------------------------------------------------
		// アセット
		//------------------------------------------------

		// アセットデータベースをリフレッシュ
		AssetDatabase.ImportAsset( master_vo_save_path, ImportAssetOptions.ForceUpdate );
		AssetDatabase.ImportAsset( master_vo_save_path, ImportAssetOptions.ForceUpdate  );
		AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );

		return true;
	}

	//----------------------------------------------------------------
	// スクリプタブルオブジェクトの作成
	//----------------------------------------------------------------
	static public void CreateScriptableObjects( IWorkbook book )
	{
		int i;
		string filename;

		ISheet	sheet;

		// 作成したクラス初期化
		makeClass.Clear();

		//---------------------------------------------
		// シート
		//---------------------------------------------
		for( i = 0 ; i < book.NumberOfSheets ; ++i ) {

			// シート取得
			sheet = book.GetSheetAt( i );
			if( sheet == null ) {
				continue;
			}

			// 作成したクラス
			makeClass.Add( sheet.SheetName );
		}

		//------------------------------------------------
		// アセット
		//------------------------------------------------

		// スクリプタブルオブジェクトアセット化
		for( i = 0 ; i < makeClass.Count ; ++i ) {

			// インスタンス作成
			ScriptableObject scriptableObject = ScriptableObject.CreateInstance( makeClass[ i ] + "_VOs" );

			// アセット化
			filename = scriptable_object_save_path + makeClass[ i ] + "_VOs.asset";
			AssetDatabase.CreateAsset( scriptableObject, filename );

		}

		// アセットデータベースをリフレッシュ
		AssetDatabase.ImportAsset( scriptable_object_save_path, ImportAssetOptions.ForceUpdate );
		AssetDatabase.Refresh( ImportAssetOptions.ForceUpdate );
	}

	//----------------------------------------------------------------
	// 読込
	//----------------------------------------------------------------
	static public void Read( IWorkbook book )
	{
		int i;
		int x;
		int y;

		// プロパティ情報
		List<string>	description		= new List<string>();
		List<string>	name			= new List<string>();
		List<string>	type			= new List<string>();
		string			now				= "";

		// 列のコメント情報
		List<int>		comment_column	= new List<int>();

		ISheet	sheet;
		IRow	row;
		ICell	cell;

		// 終了列
		int end_column;

		// データ数
		int data_count;

		// プロパティインデックス
		int property_index;

		// 値
		string value;

		//---------------------------------------------
		// シート
		//---------------------------------------------
		for( i = 0 ; i < book.NumberOfSheets ; ++i ) {

			// シート取得
			sheet = book.GetSheetAt( i );
			if( sheet == null ) {
				continue;
			}

			// プロパティ情報クリア
			description.Clear();
			name.Clear();
			type.Clear();
			now	= "";

			// 列のコメント情報クリア
			comment_column.Clear();

			// 終了列
			end_column = -1;

			// データ数
			data_count = 0;

			//---------------------------------------------
			// 行
			//---------------------------------------------
			for( y = sheet.FirstRowNum ; y < sheet.LastRowNum ; y++ ) {

				// 行取得
				row = sheet.GetRow( y );
				if( row == null ) {
					continue;
				}

				// プロパティインデックス
				property_index	= 0;

				//---------------------------------------------
				// 列
				//---------------------------------------------
				for( x = row.FirstCellNum ; x < row.LastCellNum ; x++ ) {

					// 列取得
					cell = row.GetCell( x );
					if( cell == null ) {
						continue;
					}

					// 値
					value = Convert.ToString( cell );
					value = value.Replace( "\r", "" ).Replace( "\n", "" );

					// コメント列の時は無視
					if( comment_column.Contains( x ) ) {
						continue;
					}

					//----------------------------------------------------------------
					// ここからヘッダ解析

					// 0 列目にヘッダタイプ
					if( x == 0 ) {
						now = "";
						if( value  == "S" ) {	// 開始行
							now = value;
						}
						if( value  == "D" ) {	// 説明行
							now = value;
						}
						if( value  == "N" ) {	// プロパティ名行
							now = value;
						}
						if( value  == "T" ) {	// 型行
							now = value;
						}
						if( value  == "E" ) {	// 最終行
							break;
						}
						if( value  == "#" ) {	// 無効行
							break;
						}

						// ヘッダ解析時、上記以外はデータ
						if( now == "" ) {
							data_count++;
							continue;
						}

						// 上記に該当ありなので列を進める
						continue;
					}

					// コメント列 or 終了列情報
					if( now == "S" ) {
						// コメント列
						if( value == "#" ) {
							comment_column.Add( x );
							continue;
						}
						// 終了列
						if( value == "E" ) {
							end_column = x;
							break;
						}
						continue;
					}

					// 終了列
					if( end_column == x ) {
						break;
					}

					// プロパティ情報を登録
					if( now == "D" ) {				// 説明
						description.Add( value );
						continue;
					}
					if( now == "N" ) {				// プロパティ名
						name.Add( value );
						continue;
					}
					if( now == "T" ) {				// 型
						type.Add( value );
						continue;
					}

					// ここまでヘッダ解析
					//----------------------------------------------------------------

					// 空白
					if( cell.CellType == CellType.Blank ) {
						Debug.LogError( sheet.SheetName + ":" + ( y + 1).ToString() + "行目" + ( x + 1 ).ToString() + "列目に空白があります。" );
						break;
					}

					// 更新
					string className	= sheet.SheetName + "_VOs";
					int data_index		= data_count - 1;
					string propertyName = name[ property_index ];
					//ExcelMasterCtrl.UpdateData( className, data_index, propertyName, value );


					// 更新開始
					Type t = GetTypeByClassName( "ExcelMasterCtrl" );
					MethodInfo methodInfo = t.GetMethod( "UpdateData", new Type[]	{
																						typeof( string ),
																						typeof(int),
																						typeof( string ),
																						typeof( string )
																					} );
					methodInfo.Invoke( null, new object[] { className, data_index, propertyName, value } );


					// プロパティインデックス
					property_index++;

				}

			}

		}

	}

	//----------------------------------------------------------------
	// クラス名からタイプを取得
	//----------------------------------------------------------------
	public static Type GetTypeByClassName( string className )
	{
		foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() ) {
			foreach( Type type in assembly.GetTypes() ) {
				if( type.Name == className ) {
					return type;
				}
			}
		}
		return null;
	}

	//----------------------------------------------------------------
}
//----------------------------------------------------------------
