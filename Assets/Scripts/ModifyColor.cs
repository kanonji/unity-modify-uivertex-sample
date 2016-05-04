using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Kanonji
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MaskableGraphic))]
	public class ModifyColor : BaseMeshEffect, IModifierOfUIVertex
	{
		[SerializeField] private Color32[] colors = new Color32[]
		{
			new Color32(255,0,0,0),
			new Color32(0,255,0,255),
			new Color32(0,0,255,0),
		};

		protected ModifyColor(){}

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
			if (0 == colors.Length)
				return;

			var list = new List<Color32>();
			list.AddRange(colors);
			var circularColors = new CycleSequence<Color32>(list);
			var circularColorsEnumerator = circularColors.GetEnumerator();
			for (int i = 0; i < verts.Count; i++)
			{
				UIVertex v = verts[i];
				circularColorsEnumerator.MoveNext();
				v.color = circularColorsEnumerator.Current;
				verts[i] = v;
			}
		}
	}
}
