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
        float pixelsPerUnit = 200f;

        Texture2D texture = new Texture2D(www.texture.width, www.texture.height);
        www.LoadImageIntoTexture(texture);

        Rect rect = new Rect(0, 0, texture.width, texture.height);

        Sprite s = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), pixelsPerUnit);

        sprites.Add(file.Name, s);
	}
}
