using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;
    public bool isLoading = false;
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }
    public void ActivateMenu(bool isLoading)
    {
        this.isLoading = isLoading;
        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.instance.GetAllProfilesGameData();
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfilId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoading)
            {
                saveSlot.enabled = false;
            }
            else
            {
                saveSlot.enabled = true;
            }
        }
    }
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistanceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfilId());
        if (isLoading)
        {
            DataPersistanceManager.instance.LoadGame();
        } else
        {
            DataPersistanceManager.instance.SaveGame();
        }
    }
}
