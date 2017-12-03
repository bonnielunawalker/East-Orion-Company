using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
	private AssetLoader _assetLoader;

	public GameObject starTemplate;

	public void Start()
	{
		_assetLoader = GetComponent<AssetLoader>();
		_assetLoader.LoadAssets();

		Instantiate(starTemplate);
	}
}
