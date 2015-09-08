using UnityEngine;

//---------------------------------------------------------
// ピンポンカラー
//---------------------------------------------------------
public class ColorPingPong : MonoBehaviour {

	// 開始カラー
	[SerializeField]
	public Color m_ColorStart	= new Color( 1.0f, 1.0f, 1.0f, 1.0f );

	// 終了カラー
	[SerializeField]
	public Color m_ColorEnd		= new Color( 0.8f, 0.8f, 0.8f, 1.0f );

	// ループ秒数
	[ SerializeField]
	public float m_SetLoopSec	= 2.0f;

	// マテリアル
	public Material				m_Material;

	//---------------------------------------------------------
	// 初期化
	//---------------------------------------------------------
	void Awake()
	{
		// マテリアル
		m_Material = GetComponent<Renderer>().material;
	}

	//---------------------------------------------------------
	// 更新
	//---------------------------------------------------------
	void Update()
	{
		float lerp			= Mathf.PingPong( Time.time, m_SetLoopSec ) / m_SetLoopSec;
		m_Material.color	= Color.Lerp( m_ColorStart, m_ColorEnd, lerp );
	}

	//---------------------------------------------------------
}
//---------------------------------------------------------
