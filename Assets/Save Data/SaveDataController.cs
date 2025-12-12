using System.IO;
using UnityEngine;

public class SaveDataController : MonoBehaviour
{
    private static SaveDataController _instance;
    public static SaveDataController Instance => _instance;

    [SerializeField] private SaveDataAsset _saveDataAsset;
    [HideInInspector] public SaveData current;

    [SerializeField] private string _directory;
    [SerializeField] private string _fileName;

    private void Awake()
    {
        _instance = this;
        Load();
    }

    private void OnDestroy()
    {
        Save();
    }

    public void Save()
    {
        Serializer.Save(current, Path.Combine(Application.persistentDataPath, _directory), _fileName);
    }

    public void Load()
    {
        current = Serializer.Load(_saveDataAsset.SaveData, Path.Combine(Application.persistentDataPath, _directory), _fileName);
    }
}
