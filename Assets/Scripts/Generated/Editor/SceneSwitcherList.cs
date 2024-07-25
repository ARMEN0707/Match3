using UnityEditor;

using FuryLion;

public static class SceneSwitcherList
{
	[MenuItem("Scenes/Main", false, 0)]
	private static void OpenScene_Main()
	{
		SceneSwitcher.OpenScene("Assets/Scenes/Main.unity");
	}
	[MenuItem("Scenes/Splash", false, 0)]
	private static void OpenScene_Splash()
	{
		SceneSwitcher.OpenScene("Assets/Scenes/Splash.unity");
	}
}