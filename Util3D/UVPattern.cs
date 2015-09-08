using UnityEngine;

//---------------------------------------------------------
// UV 加算ループ
//---------------------------------------------------------
public class UVPattern : MonoBehaviour
{
	// パターン数 U
	[SerializeField]
	public int m_U_Cnt		= 4;

	// パターン数 V
	[SerializeField]
	public int m_V_Cnt		= 4;

	// 間隔
	[SerializeField]
	public float m_Interval = 0.0f;

	// 総パターン数
	private int m_Max_Cnt	= 16;

	// 現在の番号
	private int m_Now		= 0;

	// オフセット値
	[SerializeField]
	private Vector2 m_mainTextureOffset = new Vector2( 0.0f, 0.0f );

	// １ブロックあたりのオフセット値
	private Vector2 m_BlockOffset		= new Vector2( 0.0f, 0.0f );

	// マテリアル
	private Material m_Material			= null;

	// 間隔計上
	private float m_IntervalAdd = 0.0f;

	//---------------------------------------------------------
	// 初期化
	//---------------------------------------------------------
	void Awake()
	{
		// 総パターン数
		m_Max_Cnt = m_U_Cnt * m_V_Cnt;

		// マテリアル
		m_Material = GetComponent<Renderer>().material;

		// １ブロックあたりのオフセット値
		m_BlockOffset.x = 1.0f / ( float )m_U_Cnt;
		m_BlockOffset.y = 1.0f / ( float )m_V_Cnt;
	}

	//---------------------------------------------------------
	// 更新
	//---------------------------------------------------------
	void Update()
	{
		// 間隔計上
		m_IntervalAdd += Time.deltaTime;
		if( m_IntervalAdd < m_Interval ) {
			return;
		}
		m_IntervalAdd = 0.0f;

		// 現在の番号加算
		m_Now++;
		if( m_Now >= m_Max_Cnt ) {
			m_Now = 0;
        }

		// オフセット
		m_mainTextureOffset.x = ( m_Now % m_U_Cnt ) * m_BlockOffset.x;
        m_mainTextureOffset.y = ( m_Now / m_U_Cnt ) * m_BlockOffset.y;

		// 反映
		m_Material.mainTextureOffset = m_mainTextureOffset;
	}

	//---------------------------------------------------------
}
//---------------------------------------------------------
