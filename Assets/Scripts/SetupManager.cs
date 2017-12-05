using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    private AssetLoader _assets;
    private bool _worldLoaded = false;
	public GameObject starTemplate;

    public void Start()
    {
        _assets = GetComponent<AssetLoader>();
    }

    public void Update()
    {
        if (_assets.DoneLoading() && !_worldLoaded)
        {
            _worldLoaded = true;
            GenerateWorld();
        }
    }

    public void GenerateWorld()
    {
        Instantiate(starTemplate);
    }
}
