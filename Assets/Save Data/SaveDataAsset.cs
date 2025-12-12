using UnityEngine;

[CreateAssetMenu(fileName = "SaveDataAsset", menuName = "Scriptable Objects/SaveDataAsset")]
public class SaveDataAsset : ScriptableObject
{
    [SerializeField] private SaveData _saveData;
    public SaveData SaveData => _saveData;
}
