using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class item_VO
{
	public int item_kind_code;	// アイテム種別コード
	public int code;	// アイテムコード
	public string name;	// アイテム名
	public string description;	// 説明
}
public class item_VOs : ScriptableObject
{
	public List<item_VO> elements = new List<item_VO>();
}
