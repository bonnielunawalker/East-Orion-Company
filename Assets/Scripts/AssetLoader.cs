using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        Task loadResourceData = Task.Run(() => LoadDataFiles(resourceDataDir, resourceData));
        Task loadPlanetData = Task.Run(() => LoadDataFiles(planetDataDir, planetData));
        Task loadResourceNodeData = Task.Run(() => LoadDataFiles(resourceNodeDataDir, resourceNodeData));
        Task loadFactoryData = Task.Run(() => LoadDataFiles(factoryDataDir, factoryData));
        Task loadShipData = Task.Run(() => LoadDataFiles(shipDataDir, shipData));

        Task loadShipSprites = Task.Run(() => LoadSprites(shipSpriteDir, _shipSpriteData));
        Task loadPlanetSprites = Task.Run(() => LoadSprites(planetSpriteDir, _planetSpriteData));
        Task loadStarSprites = Task.Run(() => LoadSprites(starSpriteDir, _starSpriteData));

        Debug.Log("waiting");
        Task.WaitAll(new Task[] { loadResourceData, loadPlanetData, loadResourceNodeData, loadFactoryData, loadShipData });
        Debug.Log("done waiting");

        Task createResourceTypes = Task.Run(() => CreateResourceTypes());
        Task createPlanetTypes = Task.Run(() => CreatePlanetTypes());
        Task createResourceNodeTypes = Task.Run(() => CreateResourceNodeTypes());
        Task createFactoryTypes = Task.Run(() => CreateFactoryTypes());
        Task createShipTypes = Task.Run(() => CreateShipTypes());

        Debug.Log("waiting");
        Task.WaitAll(new Task[] { createResourceTypes, createPlanetTypes, createResourceNodeTypes, createFactoryTypes, createShipTypes });
        Debug.Log("done waiting");
    }

    public void Update()
    {
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

        _doneLoading = true;
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

    private void CreateResourceTypes()
    {
        foreach (KeyValuePair<string, string> kvp in resourceData)
            Resource.AddResourceType(kvp.Key, kvp.Value);
    }

    private void CreatePlanetTypes()
    {

    }

    private void CreateResourceNodeTypes()
    {

    }

    private void CreateFactoryTypes()
    {

    }

    private void CreateShipTypes()
    {

    }

    public bool DoneLoading()
    {
        return _doneLoading;
    }
}
