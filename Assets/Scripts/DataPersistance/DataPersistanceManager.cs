using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    private GameData gameData;
    [SerializeField]private string fileName;
    private List<IDataPersistance> dataPersistanceObjects;
    public static DataPersistanceManager instance { get; private set; }
    private FileDataHandler dataHandler;
    private string profileID ="";
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, "save");
    }
    public void SaveGame()
    {   
        foreach (IDataPersistance dataPersistanceObj in FindAllDataPersistanceObjects())
        {
            dataPersistanceObj.SaveData(ref gameData);
        }
        gameData.cas = Player.cas;
        gameData.diff = Player.diff;
        gameData.hp = Player.health;
        gameData.pos = new Vector2(MapGenerator.x_offset, MapGenerator.y_offset);
        dataHandler.Save(gameData, profileID);
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load(profileID);
        foreach (IDataPersistance dataPersistanceObj in FindAllDataPersistanceObjects()) {
            dataPersistanceObj.LoadData(gameData);
        }
        Player.diff = gameData.diff;
        Player.health = gameData.hp;
        Player.cas = gameData.cas;
        MapGenerator.x_offset = (int)gameData.pos.x;
        MapGenerator.y_offset = (int)gameData.pos.y;
    }
    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.profileID = newProfileId;
    }
}
