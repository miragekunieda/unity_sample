using UnityEngine;

//---------------------------------------------------------
// カラー sin ループ
//---------------------------------------------------------
public class ColorSinLoop : MonoBehaviour {

	// 開始カラー
	[SerializeField]
	public Color m_ColorStart		= new Color( 0.0f, 0.0f, 0.0f, 1.0f );

	// 終了カラー
	[SerializeField]
	public Color m_ColorEnd			= new Color( 1.0f, 1.0f, 1.0f, 1.0f );

	// ループ秒数
	[ SerializeField]
	public float m_SetLoopSec		= 2.0f;

	// 間隔秒数
	[SerializeField]
	public float m_SetIntervalSec	= 2.0f;

	// 開始秒数
	[SerializeField]
	public float m_SetStartSec		= 0.0f;

	// カラー
	private Color m_Color			= new Color( 1.0f, 1.0f, 1.0f, 1.0f );

	// ループ秒数
	private float m_LoopSec			= 1.0f;

	// 間隔秒数
	private float m_IntervalSec		= 0.0f;

	// 開始秒数
	public float m_StartSec			= 0.0f;

	// マテリアル
	public Material	m_Material;

	//---------------------------------------------------------
	// 初期化
	//---------------------------------------------------------
	void Awake()
	{
		// マテリアル
		m_Material = GetComponent<Renderer>().material;

		// カラー
		m_Color.r = m_ColorStart.r;
		m_Color.g = m_ColorStart.g;
		m_Color.b = m_ColorStart.b;
		m_Color.a = m_ColorStart.a;

		// 秒数
		m_LoopSec = 0.0f;

		// 間隔秒数
		m_IntervalSec = m_SetIntervalSec;
	}

	//---------------------------------------------------------
	// 更新
	//---------------------------------------------------------
	void Update()
	{
		// 開始秒数
		m_StartSec += Time.smoothDeltaTime;
		if( m_StartSec < m_SetStartSec ) {
			return;
		}
		m_StartSec = m_SetStartSec;

		// 間隔秒数
		m_IntervalSec += Time.smoothDeltaTime;
		if( m_IntervalSec < m_SetIntervalSec ) {
			return;
		}
		m_IntervalSec = m_SetIntervalSec;

		// 秒数加算
		m_LoopSec += Time.smoothDeltaTime;
		if( m_LoopSec >= m_SetLoopSec ) {

			// カラー
			m_Color.r = m_ColorStart.r;
			m_Color.g = m_ColorStart.g;
			m_Color.b = m_ColorStart.b;
			m_Color.a = m_ColorStart.a;

			// 秒数
			m_LoopSec = 0.0f;

			// 間隔秒数
			m_IntervalSec = 0.0f;

			// 反映
			m_Material.color = m_Color;
		}

		float l_LoopSecPer = m_LoopSec / m_SetLoopSec;
		float l_sin = Mathf.Sin( l_LoopSecPer * Mathf.PI );

		m_Color.r = m_ColorStart.r + ( m_ColorEnd.r - m_ColorStart.r ) * l_sin;
		m_Color.g = m_ColorStart.g + ( m_ColorEnd.g - m_ColorStart.g ) * l_sin;
		m_Color.b = m_ColorStart.b + ( m_ColorEnd.b - m_ColorStart.b ) * l_sin;
		m_Color.a = m_ColorStart.a + ( m_ColorEnd.a - m_ColorStart.a ) * l_sin;

		// 反映
		m_Material.color = m_Color;
	}

	//---------------------------------------------------------
}
//---------------------------------------------------------
