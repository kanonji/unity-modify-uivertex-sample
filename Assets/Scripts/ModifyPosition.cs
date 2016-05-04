using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Kanonji
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MaskableGraphic))]
	public class ModifyPosition : BaseMeshEffect, IModifierOfUIVertex
	{
		[SerializeField] private Vector2[] positions = new Vector2[2]
		{
			new Vector2(-10, 0),
			new Vector2(10, 10),
		};

		protected ModifyPosition()
		{
		}

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
			if (0 == positions.Length)
				return;

			var list = new List<Vector2>();
			list.AddRange(positions);
			var circular = new CycleSequence<Vector2>(list);
			var circularEnumerator = circular.GetEnumerator();
			for (int i = 0; i < verts.Count; i++)
			{
				UIVertex v = verts[i];
				circularEnumerator.MoveNext();
				Vector3 position = circularEnumerator.Current;
				v.position += position;
				verts[i] = v;
			}
		}
	}
}
