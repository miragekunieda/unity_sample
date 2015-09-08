using UnityEngine;

//---------------------------------------------------------
// ローテーション加算ループ
//---------------------------------------------------------
public class RotAddLoop : MonoBehaviour {

	// 一秒あたりの回転値
	[SerializeField]
	public Vector3 m_AddEulerAngles		= new Vector3( 0.0f, 0.0f, 0.0f );

	// ローカルオイラーアングル
	private Vector3 m_localEulerAngles	= new Vector3( 0.0f, 0.0f, 0.0f );

	//---------------------------------------------------------
	// 初期化
	//---------------------------------------------------------
	void Awake()
	{
		// ローカルオイラーアングル
		m_localEulerAngles.x = transform.localEulerAngles.x;
		m_localEulerAngles.y = transform.localEulerAngles.y;
		m_localEulerAngles.z = transform.localEulerAngles.z;
	}

	//---------------------------------------------------------
	// 更新
	//---------------------------------------------------------
	void Update()
	{
		// 加算
		m_localEulerAngles.x += m_AddEulerAngles.x * Time.smoothDeltaTime;
		m_localEulerAngles.y += m_AddEulerAngles.y * Time.smoothDeltaTime;
		m_localEulerAngles.z += m_AddEulerAngles.z * Time.smoothDeltaTime;

		// ローカルオイラーアングル
		m_localEulerAngles.x = Mathf.Repeat( m_localEulerAngles.x, 360.0f );
		m_localEulerAngles.y = Mathf.Repeat( m_localEulerAngles.y, 360.0f );
		m_localEulerAngles.z = Mathf.Repeat( m_localEulerAngles.z, 360.0f );

		// 反映
		transform.localEulerAngles = m_localEulerAngles;
    }

	//---------------------------------------------------------
}
//---------------------------------------------------------
