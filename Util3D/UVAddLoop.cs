using UnityEngine;

//---------------------------------------------------------
// UV 加算ループ
//---------------------------------------------------------
public class UVAddLoop : MonoBehaviour
{
	// 一秒あたりのオフセット値
	[SerializeField]
	private Vector2		m_Offset			= new Vector2( 0.0f, 0.0f );

	// 初期オフセット値
	[SerializeField]
	private Vector2		m_mainTextureOffset	= new Vector2( 0.0f, 0.0f );

	// マテリアル
	private Material	m_Material			= null;

	//---------------------------------------------------------
	// 初期化
	//---------------------------------------------------------
	void Awake()
	{
		m_Material = GetComponent<Renderer>().material;
	}

	//---------------------------------------------------------
	// 更新
	//---------------------------------------------------------
	void Update()
	{
		// 加算
		m_mainTextureOffset.x += m_Offset.x * Time.smoothDeltaTime;
		m_mainTextureOffset.y += m_Offset.y * Time.smoothDeltaTime;

		// 数値範囲
		m_mainTextureOffset.x -= Mathf.Floor( m_mainTextureOffset.x );
		m_mainTextureOffset.y -= Mathf.Floor( m_mainTextureOffset.y );

		// 反映
		m_Material.mainTextureOffset = m_mainTextureOffset;
    }

	//---------------------------------------------------------
}
//---------------------------------------------------------
