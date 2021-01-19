﻿using System.Collections.Generic;
using UnityEngine;

namespace TAO.VertexAnimation
{
	[CreateAssetMenu(fileName = "new AnimationBook", menuName = "TAO/VertexAnimation/AnimationBook", order = 400)]
	public class VA_AnimationBook : ScriptableObject
	{
		public VA_AnimationBook(Texture2DArray a_positionMap)
		{
			positionMap = a_positionMap;
		}

		public VA_AnimationBook(Texture2DArray a_positionMap, List<VA_Animation> a_animations)
		{
			positionMap = a_positionMap;

			foreach (var a in a_animations)
			{
				TryAddAnimation(a);
			}
		}

		public int MaxFrames
		{
			get; private set;
		}

		public Texture2DArray positionMap = null;
		public List<VA_Animation> animations = new List<VA_Animation>();
		public List<Material> materials = new List<Material>();

		public bool TryAddAnimation(VA_Animation animation)
		{
			if (animations != null && animations.Count != 0)
			{
				if (!animations.Contains(animation) && animation.Data.maxFrames == MaxFrames)
				{
					animations.Add(animation);
					return true;
				}
			}
			else
			{
				// Add first animation.
				animations.Add(animation);
				// Set maxFrames for this animation book.
				MaxFrames = animations[0].Data.maxFrames;

				return true;
			}

			return false;
		}

		public bool TryAddMaterial(Material material)
		{
			if (!materials.Contains(material))
			{
				if (material.HasProperty("_PositionMap") && material.HasProperty("_MaxFrames"))
				{
					materials.Add(material);
					return true;
				}
			}

			return false;
		}

		public void RemoveAnimation(VA_Animation animation)
		{
			if (animations != null)
			{
				animations.Remove(animation);
			}
		}

		public void SetMaterials()
		{
			if (materials != null)
			{
				foreach (var mat in materials)
				{
					if (mat != null)
					{
						if (mat.HasProperty("_MaxFrames"))
						{
							mat.SetFloat("_MaxFrames", MaxFrames);
						}

						if (mat.HasProperty("_PositionMap"))
						{
							mat.SetTexture("_PositionMap", positionMap);
						}
					}
				}
			}
		}

		private void OnValidate()
		{
			if (animations != null)
			{
				foreach (var a in animations)
				{
					if (a != null)
					{
						if (a.Data.maxFrames != MaxFrames)
						{
							Debug.LogWarning(string.Format("{0} in {1} doesn't match maxFrames!", a.name, this.name));
						}
					}
				}
			}

			if (positionMap != null)
			{
				if (positionMap.depth > animations.Count)
				{
					Debug.LogWarning(string.Format("More animations ({0}) than positionMaps in {1}!", animations.Count, this.name));
				}
			}
		}
	}
}