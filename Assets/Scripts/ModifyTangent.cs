using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Kanonji
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MaskableGraphic))]
	public class ModifyTangent : BaseMeshEffect, IModifierOfUIVertex
	{
		[SerializeField] private Vector4[] tangents = new Vector4[1]{new Vector4(1,1,1,1)};

		protected ModifyTangent(){}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
				return;

			var output = ListPool<UIVertex>.Get();
			vh.GetUIVertexStream(output);
			ApplyVerts(output);
			vh.Clear();
			vh.AddUIVertexTriangleStream(output);
			ListPool<UIVertex>.Release(output);
			GetComponent<PositionAsUV1>();
		}

		protected void ApplyVerts(List<UIVertex> verts)
		{
			var list = new List<Vector4>();
			list.AddRange(tangents);
			var circular = new CycleSequence<Vector4>(list);
			var circularEnumerator = circular.GetEnumerator();
			for (int i = 0; i < verts.Count; i++)
			{
				UIVertex v = verts[i];
				circularEnumerator.MoveNext();
				Vector4 tangent = circularEnumerator.Current;
				v.tangent = tangent;
				verts[i] = v;
			}
		}
	}
}

