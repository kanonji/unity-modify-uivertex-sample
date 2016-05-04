using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Kanonji
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MaskableGraphic))]
	public class ModifyUv1 : BaseMeshEffect, IModifierOfUIVertex
	{
		[SerializeField] private Vector2[] uvPositions = new Vector2[]
		{
			Vector2.one,
			Vector2.right,
			Vector2.zero,
		};

		protected ModifyUv1(){}

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
			if (0 == uvPositions.Length)
				return;

			var list = ListPool<Vector2>.Get();
			list.AddRange(uvPositions);
			var circular = new CycleSequence<Vector2>(list);
			var circularEnumerator = circular.GetEnumerator();

			for (int i = 0; i < verts.Count; i++)
			{
				UIVertex v = verts[i];
				circularEnumerator.MoveNext();
				if(list.Count > i) v.uv1 = circularEnumerator.Current;
				verts[i] = v;
			}
		}
	}
}

