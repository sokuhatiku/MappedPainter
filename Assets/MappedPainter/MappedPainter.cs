using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappedPainter : MonoBehaviour
{
	[SerializeField]
	Texture sourceTexture;

	[Space]
	[SerializeField]
	Renderer targetRenderer;
	[SerializeField]
	Mesh targetMesh;
	[SerializeField]
	string targetPropertyName = "_MainTex";

	[Space]
	[SerializeField]
	Material positionMapper;
	[SerializeField]
	Material extender;
	[SerializeField]
	Material painter;

	[Space]
	[SerializeField]
	Color paintColor = Color.white;
	[SerializeField]
	float paintRadius = 0.1f;

	[Space]
	[SerializeField]
	Renderer debugMap;
	[SerializeField]
	Renderer debugResult;


	RenderTexture positionMap;
	RenderTexture destTexture;

	Vector3 targetPoint = Vector3.zero;


	private void Reset()
	{
		targetRenderer = GetComponent<Renderer>();
	}

	private void Start()
	{
		var width = sourceTexture.width;
		var height = sourceTexture.height;

		positionMap = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf)
		{
			anisoLevel = 0,
			autoGenerateMips = false,
			filterMode = FilterMode.Point,
		};

		destTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
		{
			anisoLevel = 0,
			autoGenerateMips = false,
		};

		Graphics.Blit(sourceTexture, destTexture);


		var block = new MaterialPropertyBlock();
		block.SetTexture(targetPropertyName, destTexture);
		targetRenderer.SetPropertyBlock(block);

		UpdatePositionMap();


		if (debugMap != null)
			debugMap.material.mainTexture = positionMap;
		if (debugResult != null)
			debugResult.material.mainTexture = destTexture;
	}

	private void Update()
	{
		var mainCam = Camera.main;
		if (mainCam == null)
			return;

		var mouse = Input.mousePosition;
		var ray = mainCam.ScreenPointToRay(mouse);
		RaycastHit hitinfo;
		var hit = Physics.Raycast(ray, out hitinfo);

		targetPoint = hit ? hitinfo.point : Vector3.zero;

		if (Input.GetMouseButton(0))
			Draw();
	}


	void UpdatePositionMap()
	{
		positionMapper.SetPass(0);
		Graphics.SetRenderTarget(positionMap);
		Graphics.DrawMeshNow(targetMesh, Matrix4x4.identity);

		UpdateRenderTexture(positionMap, extender);
	}
	
	void Draw()
	{
		painter.SetTexture("_PositionMap", positionMap);
		painter.SetMatrix("_ObjectToWorld", transform.localToWorldMatrix);
		painter.SetVector("_Pos_Rad", new Vector4(targetPoint.x, targetPoint.y, targetPoint.z, paintRadius));
		painter.SetColor("_Color", paintColor);

		UpdateRenderTexture(destTexture, painter);
	}

	void UpdateRenderTexture(RenderTexture texture, Material useMat)
	{
		var temp = RenderTexture.GetTemporary(texture.descriptor);
		Graphics.Blit(texture, temp);
		Graphics.Blit(temp, texture, useMat);
		RenderTexture.ReleaseTemporary(temp);
	}


	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(targetPoint, paintRadius);
	}
}
