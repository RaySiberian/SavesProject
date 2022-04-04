using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveLevelData
{
    public int LevelId;
    public Inventory Inventory;
    public Vector3 PlayerPosition;
    public List<SerializableResource> Resources;
}

[Serializable]
public class SerializableResource
{
    public Resource Prefab;
    public Vector3 Position;
    public bool IsOnScene;
}

[Serializable]
public class GameData
{
    public List<SaveLevelData> SaveLevelDatas;
}