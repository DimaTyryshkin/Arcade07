using System.Linq;
using Action7;
using GamePackages.Core;
using UnityEditor;
using UnityEngine; 

namespace GamePackages.Tools.ObjectsPalette
{
	public static class Action07Tools 
	{
		[MenuItem("Action07/Sprites To Prefab")]
		public static void CreateObjectBrush()
		{
			if (Selection.objects.All(x => x is Sprite))
			{
				Sprite[] sprites = Selection.objects.Cast<Sprite>().ToArray();
				SpritesToPrefabWindow window = EditorWindow.GetWindow<SpritesToPrefabWindow>();
				window.titleContent.text = "Sprites To Prefab";
				window.Init(sprites);
			}
		}

		[MenuItem("Action07/Sprites To Prefab", isValidateFunction: true)]
		public static bool CreateObjectBrushValidate()
		{
			return Selection.objects.All(x => x is Sprite);
		}
	}
	 
	
	
	public class SpritesToPrefabWindow : EditorWindow
	{ 
		static readonly string bsdKey = "Action07.Tools.CreatePrefabsWindow.LastName";
		public Sprite[] sprites;
		
		string LastPrefabName
		{
			get => EditorPrefs.GetString(bsdKey,"new-prefab");
			set => EditorPrefs.SetString(bsdKey, value);
		}
  
		void Validate()
		{
			if (sprites == null)
			{
				sprites = new Sprite[0];
				Close();
			}
		}
 
		void OnGUI()
		{
			Validate();
 
			GUILayout.BeginVertical();
			{
				LastPrefabName = EditorGUILayout.TextField("prefab name", LastPrefabName);
				if (GUILayout.Button("Create"))
				{
					for (int i = 0; i < sprites.Length; i++)
					{
						string newName = LastPrefabName + "-" + StringExtension.GameObjectIndexToNameSuffix(i, sprites.Length);
						GameObject newGo = new GameObject(newName);
						SpriteRenderer spriteRenderer = newGo.AddComponent<SpriteRenderer>();
						spriteRenderer.sprite = sprites[i];

						TileSortingOrder sortingOrder = newGo.AddComponent<TileSortingOrder>();
						sortingOrder.SetGroupName(LastPrefabName);
						
						Undo.RegisterCreatedObjectUndo(newGo, nameof(SpritesToPrefabWindow));
					}
				}
			}
			GUILayout.EndVertical();
		}

		public void Init(Sprite[] sprites)
		{
			AssertWrapper.IsAllNotNull(sprites);
			this.sprites = sprites;
		}
	}
}