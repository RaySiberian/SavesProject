using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig _config;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private List<SerializableResource> _resources;
    private UIView _view;
    private Player _player;
    private string saveFilePath;
    
    private void Awake()
    {
        saveFilePath =  Application.dataPath + "/Gamedata.json";
        _view = FindObjectOfType<UIView>();
    }

    private void Start()
    {
        if (SceneLoader.Instance.LoadType == LoadType.New)
        {
            SpawnObjectsByConfig();
            UpdateUI();
            _camera.SetObject(_player.gameObject);
        }
        else if (SceneLoader.Instance.LoadType == LoadType.LoadSave)
        {
            LoadLevelData();
            UpdateUI();
            _camera.SetObject(_player.gameObject);
        }
    }

    private void SpawnObjectsByConfig()
    {
        _player = Instantiate(_config.Player, _config.PlayerPosition,Quaternion.identity);
        _player.Hit += PlayerOnHit;
        foreach (var res in _config.Resources)
        {
           var resource = Instantiate(res.Prefab, res.Position, Quaternion.identity);
           resource.SerializableResource = new SerializableResource
           {
               Position = res.Position,
               Prefab = res.Prefab,
               IsOnScene = res.IsOnScene
           };
           _resources.Add(resource.SerializableResource);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SaveLevelData();
        }
    }

    private void PlayerOnHit()
    {
        UpdateUI();
        CheckGameState();
    }

    private void CheckGameState()
    {
        if (_config.NeedStone == _player.Inventory.Stone && _config.NeedWood == _player.Inventory.Wood)
        {
            SceneManager.LoadScene(_config.NextSceneIndex);
        }
    }

    private void UpdateUI()
    {
        _view.HealthText.text = "Health: " + _player.Health.ToString();
        _view.StoneText.text = "Stone: " + _player.Inventory.Stone.ToString();
        _view.WoodText.text = "Wood: " + _player.Inventory.Wood.ToString();
    }
    
    public void SaveLevelData()
    {
        SaveLevelData saveLevelData = new SaveLevelData();
        saveLevelData.Inventory = _player.Inventory;
        saveLevelData.LevelId = _config.LevelID;
        saveLevelData.PlayerPosition = _player.gameObject.transform.position;
        saveLevelData.Resources = _resources;

        string json;
        string fileContents = File.ReadAllText(saveFilePath);
        GameData gameData = JsonUtility.FromJson<GameData>(fileContents);
        
        for (int i = 0; i < gameData.SaveLevelDatas.Count; i++)
        {
            if (gameData.SaveLevelDatas[i].LevelId == _config.LevelID)
            {
                gameData.SaveLevelDatas[i] = saveLevelData;
                json = JsonUtility.ToJson(gameData);
                File.WriteAllText(saveFilePath,json);
                return;
            }
        }
        
        gameData.SaveLevelDatas.Add(saveLevelData);
        json = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFilePath,json);
    }

    private void LoadLevelData()
    {
        string fileContents = File.ReadAllText(saveFilePath);
        GameData gameData = JsonUtility.FromJson<GameData>(fileContents);
        SaveLevelData saveLevelData = null;

        foreach (var save in gameData.SaveLevelDatas.Where(save => save.LevelId == _config.LevelID))
        {
            saveLevelData = save;
        }
        
        _player = Instantiate(_config.Player, saveLevelData.PlayerPosition ,Quaternion.identity);
        _player.Hit += PlayerOnHit;

        foreach (var res in saveLevelData.Resources)
        {
            var resource = Instantiate(res.Prefab, res.Position, Quaternion.identity);
            resource.SerializableResource = new SerializableResource
            {
                Position = res.Position,
                Prefab = res.Prefab,
                IsOnScene = res.IsOnScene
            };
            _resources.Add(resource.SerializableResource);
            if (!res.IsOnScene)
            {
                resource.gameObject.SetActive(false);
            }
        }
        _player.Inventory = saveLevelData.Inventory;
    }
}