using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    public Dictionary<string, string> resourceData = new Dictionary<string, string>();
    public Dictionary<string, string> planetData = new Dictionary<string, string>();
    public Dictionary<string, string> resourceNodeData = new Dictionary<string, string>();
    public Dictionary<string, string> factoryData = new Dictionary<string, string>();
    public Dictionary<string, string> shipData = new Dictionary<string, string>();

    public Dictionary<string, Sprite> shipSprites = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> planetSprites = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> starSprites = new Dictionary<string, Sprite>();

    public void LoadAssets()
	{
        // Data directories
        DirectoryInfo resourceDataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Resources");
        DirectoryInfo planetDataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Planets");
        DirectoryInfo resourceNodeDataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Producers");
        DirectoryInfo factoryDataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Factories");
        DirectoryInfo shipDataDir = new DirectoryInfo(Application.streamingAssetsPath + "/Ships");

        // Sprite directories
        DirectoryInfo shipSpriteDir = new DirectoryInfo(Application.streamingAssetsPath + "/Ships/Sprites");
        DirectoryInfo planetSpriteDir = new DirectoryInfo(Application.streamingAssetsPath + "/Planets/Sprites");
        DirectoryInfo starSpriteDir = new DirectoryInfo(Application.streamingAssetsPath + "/Stars/Sprites");

        // Load data files
        Debug.Log("Loading resource data");
        LoadDataFiles(resourceDataDir, resourceData);
        Debug.LogFormat("{0} files loaded", resourceData.Count);

        Debug.Log("Loading planet data");
        LoadDataFiles(planetDataDir, planetData);
        Debug.LogFormat("{0} files loaded", planetData.Count);

        Debug.Log("Loading resource node data");
        LoadDataFiles(resourceNodeDataDir, resourceNodeData);
        Debug.LogFormat("{0} files loaded", resourceNodeData.Count);

        Debug.Log("Loading factory data");
        LoadDataFiles(factoryDataDir, factoryData);
        Debug.LogFormat("{0} files loaded", factoryData.Count);

        Debug.Log("Loading ship data");
        LoadDataFiles(shipDataDir, shipData);
        Debug.LogFormat("{0} files loaded", shipData.Count);

        // Load sprites
        Debug.Log("Loading ship sprites");
        LoadSprites(shipSpriteDir, shipSprites);
        Debug.LogFormat("{0} files loaded", shipSprites.Count);

        Debug.Log("Loading planet sprites");
        LoadSprites(planetSpriteDir, planetSprites);
        Debug.LogFormat("{0} files loaded", planetSprites.Count);

        Debug.Log("Loading star sprites");
        LoadSprites(starSpriteDir, starSprites);
        Debug.LogFormat("{0} files loaded", starSprites.Count);
    }

	public void LoadDataFiles(DirectoryInfo dir, Dictionary<string, string> dataDict)
	{
        foreach (FileInfo file in dir.GetFiles("*.json"))
        {
            WWW www = new WWW("file://" + file.FullName.ToString ());
		    string key = file.Name.Replace(".json", "");
            dataDict.Add(key, www.text);
        }       
	}

	public void LoadSprites(DirectoryInfo dir, Dictionary<string, Sprite> spriteDict)
	{
        foreach (FileInfo file in dir.GetFiles("*.png"))
        {
            WWW www = new WWW("file://" + file.FullName.ToString());
            string key = file.Name.Replace(".png", "");
            float pixelsPerUnit = 200f;

            Texture2D texture = new Texture2D(www.texture.width, www.texture.height);
            www.LoadImageIntoTexture(texture);

            Rect rect = new Rect(0, 0, texture.width, texture.height);

            Sprite s = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), pixelsPerUnit);

            spriteDict.Add(key, s);
        }

	}
}
