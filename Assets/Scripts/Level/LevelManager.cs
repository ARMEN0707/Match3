using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Levels
{
    public List<LevelInfo> LevelsList = new List<LevelInfo>();
}


public static class LevelManager
{
    private static string _fileName = "Levels";
    private static string _path = "Assets/Resources/";
    private static Levels _levels;
    private static LevelInfo _level;
    private static int _numberLevel;

    public static Levels Levels => _levels;

    public static LevelInfo Level => _level;
    public static int NumberLevel => _numberLevel;

    public static int CountLevels => _levels.LevelsList.Count;


    public static LevelInfo LoadLevel(int id)
    {
        var level = _levels.LevelsList.Find(x => x.Id == id);
        if (level != null)
            _level = level;
        else
        {
            _levels.LevelsList.Add(new LevelInfo(id));
            _level = _levels.LevelsList.Last();
        }

        _numberLevel = id;
        return _level;
    }

    public static void RemoveLevel(LevelInfo level) => _levels.LevelsList.Remove(level);

    public static async Task LoadLevels()
    {
        if (_levels != null)
            return;

        var request = Resources.LoadAsync<TextAsset>("Levels");
        while(!request.isDone)
            await Task.Yield();

        _levels = JsonUtility.FromJson<Levels>(request.asset.ToString());
        if (_levels == null)
            _levels = new Levels();
    }

    public static void SaveLevels()
    {
        var levels = JsonUtility.ToJson(_levels);
        File.WriteAllText(_path + _fileName + ".json", levels);
    }
}

