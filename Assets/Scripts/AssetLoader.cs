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

    private Dictionary<string, byte[]> _shipSpriteData = new Dictionary<string, byte[]>();
    private Dictionary<string, byte[]> _planetSpriteData = new Dictionary<string, byte[]>();
    private Dictionary<string, byte[]> _starSpriteData = new Dictionary<string, byte[]>();

    // Load checks
    private int _spriteFileDictsFilled = 0;
    private int _numSpriteFileDicts = 3;

    private bool _generatingSprites = false;
    private bool _doneLoading = false;

    public void Start()
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

        // Set up threads
        Thread resourceDataThread = new Thread(() => LoadDataFiles(resourceDataDir, resourceData));
        Thread planetDataThread = new Thread(() => LoadDataFiles(planetDataDir, planetData));
        Thread resourceNodeDataThread = new Thread(() => LoadDataFiles(resourceNodeDataDir, resourceNodeData));
        Thread factoryDataThread = new Thread(() => LoadDataFiles(factoryDataDir, factoryData));
        Thread shipDataThread = new Thread(() => LoadDataFiles(shipDataDir, shipData));

        Thread shipSpriteThread = new Thread(() => LoadSprites(shipSpriteDir, _shipSpriteData));
        Thread planetSpriteThread = new Thread(() => LoadSprites(planetSpriteDir, _planetSpriteData));
        Thread starSpriteThread = new Thread(() => LoadSprites(starSpriteDir, _starSpriteData));

        // Load JSON files
        resourceDataThread.Start();
        planetDataThread.Start();
        resourceNodeDataThread.Start();
        factoryDataThread.Start();
        shipDataThread.Start();

        // Load sprites
        shipSpriteThread.Start();
        planetSpriteThread.Start();
        starSpriteThread.Start();
    }

    public void Update()
    {
        // TODO: Split this off into coroutines to save a little time
        if (SpriteFilesLoaded() && !_generatingSprites)
        {
            _generatingSprites = true;

            // Load all ship sprites from byte arrays
            foreach (KeyValuePair<string, byte[]> kvp in _shipSpriteData)
            {
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(kvp.Value);

                Rect rect = new Rect(0, 0, texture.width, texture.height);

                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 200f);

                shipSprites.Add(kvp.Key, sprite);
            }

            // Load all planet sprites from byte arrays
            foreach (KeyValuePair<string, byte[]> kvp in _planetSpriteData)
            {
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(kvp.Value);

                Rect rect = new Rect(0, 0, texture.width, texture.height);

                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 200f);

                planetSprites.Add(kvp.Key, sprite);
            }

            // Load all star sprites from byte arrays
            foreach (KeyValuePair<string, byte[]> kvp in _starSpriteData)
            {
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(kvp.Value);

                Rect rect = new Rect(0, 0, texture.width, texture.height);

                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 200f);

                starSprites.Add(kvp.Key, sprite);
            }

            Debug.Log("Done loading!");
            _doneLoading = true;
        }
    }

    // Populates a dictionary that holds a string name for the JSON file and another string with the contents of the file.
    public void LoadDataFiles(DirectoryInfo dir, Dictionary<string, string> dataDict)
	{
        foreach (FileInfo file in dir.GetFiles("*.json"))
        {
            string text = file.OpenText().ReadToEnd();
            string key = file.Name.Replace(".json", "");
            dataDict.Add(key, text);
        }
    }

    // Populates a dictionary that holds a string name for the sprite and an array of bytes that holds the texture data.
	public void LoadSprites(DirectoryInfo dir, Dictionary<string, byte[]> spriteDataDict)
	{
        foreach (FileInfo file in dir.GetFiles("*.png"))
        {
            byte[] pixels = File.ReadAllBytes(file.FullName);
            string key = file.Name.Replace(".png", "");
            spriteDataDict.Add(key, pixels);
        }

        _spriteFileDictsFilled++;
    }

    private bool SpriteFilesLoaded()
    {
        return _spriteFileDictsFilled == _numSpriteFileDicts;
    }

    public bool DoneLoading()
    {
        return _doneLoading;
    }
}
