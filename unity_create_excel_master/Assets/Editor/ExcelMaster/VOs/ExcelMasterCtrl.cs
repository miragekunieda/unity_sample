using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------------
// エクセルマスターコントローラー
//------------------------------------------------
public class ExcelMasterCtrl
{
	//------------------------------------------------
	// item_kind
	//------------------------------------------------
	static public item_kind_VOs m_item_kind_VOs;
	static public void UpdateData_item_kind_VOs( int i, string propertyName, string value )
	{
		item_kind_VO add_item_kind_VO = m_item_kind_VOs.elements[ i ];

		if( propertyName == "code" ){
			add_item_kind_VO.code = ( int )Convert.ToInt32( value );
		}

		if( propertyName == "name" ){
			add_item_kind_VO.name = value;
		}

		if( propertyName == "description" ){
			add_item_kind_VO.description = value;
		}

		if( propertyName == "icon_no" ){
			add_item_kind_VO.icon_no = ( int )Convert.ToInt32( value );
		}

	}

	//------------------------------------------------
	// item
	//------------------------------------------------
	static public item_VOs m_item_VOs;
	static public void UpdateData_item_VOs( int i, string propertyName, string value )
	{
		item_VO add_item_VO = m_item_VOs.elements[ i ];

		if( propertyName == "item_kind_code" ){
			add_item_VO.item_kind_code = ( int )Convert.ToInt32( value );
		}

		if( propertyName == "code" ){
			add_item_VO.code = ( int )Convert.ToInt32( value );
		}

		if( propertyName == "name" ){
			add_item_VO.name = value;
		}

		if( propertyName == "description" ){
			add_item_VO.description = value;
		}

	}

	//------------------------------------------------
	// 更新開始
	//------------------------------------------------
	static public void UpdateStart()
	{
		m_item_kind_VOs = Resources.Load<item_kind_VOs>( "ExcelMaster/item_kind_VOs" );
		m_item_kind_VOs.elements.Clear();
		for( int i = 0 ; i < 2 ; i++ ) {
			m_item_kind_VOs.elements.Add( new item_kind_VO() );
		}

		m_item_VOs = Resources.Load<item_VOs>( "ExcelMaster/item_VOs" );
		m_item_VOs.elements.Clear();
		for( int i = 0 ; i < 15 ; i++ ) {
			m_item_VOs.elements.Add( new item_VO() );
		}

	}

	//------------------------------------------------
	// 更新
	//------------------------------------------------
	static public void UpdateData( string className, int i, string propertyName, string value )
	{
		if( className == "item_kind_VOs" ){
			UpdateData_item_kind_VOs( i, propertyName, value );
		}

		if( className == "item_VOs" ){
			UpdateData_item_VOs( i, propertyName, value );
		}

	}

	//------------------------------------------------
}
