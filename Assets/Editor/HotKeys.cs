using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class UnityExtededShortKeys : ScriptableObject
{

	[MenuItem("HotKey/Save and Run  _F4")]
	static void SaveAndRun()
	{
		if (!Application.isPlaying)// game is not in play mode
		{
			EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false); // save scene
		}
		EditorApplication.ExecuteMenuItem("Edit/Play"); // play/ stop playing
	}

	[MenuItem("Window/LayoutShortcuts/1 _F12", false, 999)]
	static void Layout1()
	{
		EditorApplication.ExecuteMenuItem("Window/Layouts/Layout 2 + 2 + 2");
	}
	
	//[MenuItem("HotKey/Name Goes Here _F6")]
	//static void CanBeDifferent()
	//{
	//	// action goes here
	//}
}