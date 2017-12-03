using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
	public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
	public Dictionary<string, string> data = new Dictionary<string, string>();

	public void LoadAssets()
	{
		DirectoryInfo spriteDir = new DirectoryInfo(Application.streamingAssetsPath + "/Sprites");
		DirectoryInfo dataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Ships");
		FileInfo[] spriteFiles = spriteDir.GetFiles("*.png");
		FileInfo[] dataFiles = dataDir.GetFiles("*.json");

		foreach (FileInfo file in dataFiles)
		{
			LoadDataFile (file);
		}

		foreach (FileInfo file in spriteFiles)
		{
			LoadSprite (file);
		}
	}

	public void LoadDataFile(FileInfo file)
	{
		WWW www = new WWW ("file://" + file.FullName.ToString ());
		string key = file.Name.Replace(".json", "");
		data.Add(key, www.text);
	}

	public void LoadSprite(FileInfo file)
	{
		WWW www = new WWW ("file://" + file.FullName.ToString ());

		byte[] data = File.ReadAllBytes(file.FullName.ToString ());
		Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
		texture.LoadImage (data);
		Rect rect = new Rect(0, 0, www.texture.width, www.texture.height);

		Sprite s = Sprite.Create(www.texture, rect, rect.position);
		sprites.Add(file.Name, s);
	}
}
