using UnityEngine;

//----------------------------------------------------------
// シェーダーのアサイン
//----------------------------------------------------------
public class AssignShader : MonoBehaviour {

	//----------------------------------------------------------
	// 初期化
	//----------------------------------------------------------
	void Awake()
	{
		// メッシュ系
		Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>( true );
		foreach( Renderer rendrer in renderers ) {
			foreach( Material material in rendrer.sharedMaterials ) {
				material.shader = Shader.Find( material.shader.name );
			}
		}

		// プロジェクター系
		Projector[] projectors = gameObject.GetComponentsInChildren<Projector>( true );
		foreach( Projector projector in projectors ) {
			projector.material.shader = Shader.Find( projector.material.shader.name );
		}
	}
}
//----------------------------------------------------------
