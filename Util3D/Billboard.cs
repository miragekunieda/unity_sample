using UnityEngine;

//---------------------------------------------------------
// ビルボード
//---------------------------------------------------------
public class Billboard : MonoBehaviour
{
	// Y 軸のみ回転
	[SerializeField]
	public bool m_OnlyY = false;

	// カメラ
	public Camera m_Camera;

	// ターゲット座標
	public Vector3 m_Target	= new Vector3();

	//---------------------------------------------------------
	// 初期化
	//---------------------------------------------------------
	void Awake()
	{
		m_Camera = Camera.main;
	}

	//---------------------------------------------------------
	// 更新
	//---------------------------------------------------------
	void Update()
	{
		// ターゲット座標
		m_Target.x = m_Camera.transform.position.x;
		m_Target.z = m_Camera.transform.position.z;

		if( m_OnlyY ) {

			// Y 軸のみ回転
			m_Target.y = transform.position.y;

		} else {

			// 全軸回転
			m_Target.y = m_Camera.transform.position.y;

		}

		// 向ける
		transform.LookAt( m_Target );
	}

	//---------------------------------------------------------
}
//---------------------------------------------------------