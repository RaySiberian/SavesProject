using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance = null;
    public LoadType LoadType;
        
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var saveFilePath =  Application.dataPath + "/Gamedata.json";
        if (!File.Exists(saveFilePath))
        {
            GameData gameData = new GameData();
            string json = JsonUtility.ToJson(gameData);
            File.WriteAllText(saveFilePath,json);
        }
    }

    public void LoadNewLevel(int id)
    {
        LoadType = LoadType.New;
        SceneManager.LoadScene(id);
    }

    public void LoadLevelSave(int id)
    {
        LoadType = LoadType.LoadSave;
        SceneManager.LoadScene(id);
    }
}

public enum LoadType
{
    New,
    LoadSave
}