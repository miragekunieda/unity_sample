using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class item_kind_VO
{
	public int code;	// アイテム種別コード
	public string name;	// アイテム種別名
	public string description;	// 説明
	public int icon_no;	// アイコン番号
}
public class item_kind_VOs : ScriptableObject
{
	public List<item_kind_VO> elements = new List<item_kind_VO>();
}
