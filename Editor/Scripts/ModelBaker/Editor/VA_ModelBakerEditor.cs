using UnityEngine;
using UnityEditor;

namespace TAO.VertexAnimation.Editor
{
	[CustomEditor(typeof(VA_ModelBaker))]
	public class VA_ModelBakerEditor : UnityEditor.Editor
	{
		private VA_ModelBaker modelBaker = null;

		void OnEnable()
		{
			modelBaker = target as VA_ModelBaker;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			InputGUI();
			EditorGUILayoutUtils.HorizontalLine(color: Color.gray);
			BakeGUI();

			serializedObject.ApplyModifiedProperties();
		}

		private void InputGUI()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("model"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("animationClips"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("fps"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("textureWidth"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("applyRootMotion"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("includeInactive"));
		}

		private void BakeGUI()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("lodSettings").FindPropertyRelative("lodSettings"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("generateAnimationBook"));

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("generatePrefab"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("materialShader"), new GUIContent(""));
			}

			if (GUILayout.Button("Bake", GUILayout.Height(32)))
			{
				modelBaker.Bake();
			}

			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Delete Unused Animations", EditorStyles.miniButtonLeft))
				{
					if (EditorUtility.DisplayDialog("Delete Unused Animations", "Deleting assets will loose references within the project.", "Ok", "Cancel"))
					{
						modelBaker.DeleteUnusedAnimations();
					}
				}

				if (GUILayout.Button("Delete", EditorStyles.miniButtonRight))
				{
					if (EditorUtility.DisplayDialog("Delete Assets", "Deleting assets will loose references within the project.", "Ok", "Cancel"))
					{
						modelBaker.DeleteSavedAssets();
					}
				}
			}
		}
	}
}