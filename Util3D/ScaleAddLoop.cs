using UnityEngine;

//---------------------------------------------------------
// スケール加算ループ
//---------------------------------------------------------
public class ScaleAddLoop : MonoBehaviour {

	// 一秒あたりのスケール加算数
	[SerializeField]
	public Vector3 m_AddScale		= new Vector3( 0.0f, 0.0f, 0.0f );

	// 初期スケール値
	public Vector3 m_StartScale		= new Vector3( 1.0f, 1.0f, 1.0f );

	// ループ秒数
	[SerializeField]
	public float m_SetLoopSec			= 2.0f;

	// 間隔秒数
	[SerializeField]
	public float m_SetIntervalSec		= 2.0f;

	// 開始秒数
	[SerializeField]
	public float m_SetStartSec			= 0.0f;

	// ローカルスケール
	private Vector3 m_localScale		= new Vector3();

	// ループ秒数
	private float m_LoopSec				= 0.0f;

	// 間隔秒数
	private float m_IntervalSec			= 0.0f;

	// 開始秒数
	private float m_StartSec			= 0.0f;

	//---------------------------------------------------------
	// 初期化
	//---------------------------------------------------------
	void Awake()
	{
		// ローカルスケール
		m_localScale.x = m_StartScale.x;
		m_localScale.y = m_StartScale.y;
		m_localScale.z = m_StartScale.z;

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

			// ローカルスケール
			m_localScale.x	= m_StartScale.x;
			m_localScale.y	= m_StartScale.y;
			m_localScale.z	= m_StartScale.z;

			// 秒数
			m_LoopSec = 0.0f;

			// 間隔秒数
			m_IntervalSec = 0.0f;

			// 反映
			transform.localScale = m_localScale;

			return;
        }

		// 加算
		m_localScale.x += m_AddScale.x * Time.smoothDeltaTime;
		m_localScale.y += m_AddScale.y * Time.smoothDeltaTime;
		m_localScale.z += m_AddScale.z * Time.smoothDeltaTime;

		// 反映
		transform.localScale = m_localScale;
	}

	//---------------------------------------------------------
}
//---------------------------------------------------------
