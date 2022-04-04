using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelConfig : ScriptableObject
{
    public int LevelID;
    public int NeedWood;
    public int NeedStone;
    public int NextSceneIndex;
    [Header("LevelPreset")]
    public List<SerializableResource> Resources;
    public Player Player;
    public Vector3 PlayerPosition;
}