using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelsEditorWindow : EditorWindow
{
    private int _id;
    private LevelInfo _levelInfo;
    public string[] _options = new string[] { "None", "Blue", "Green", "Orange", "Red", "Yellow", "Obstacle" };

    [MenuItem("Window/LevelsEditorWindow")]
    private static async void ShowWindow()
    {
        await LevelManager.LoadLevels();
        EditorWindow.GetWindow<LevelsEditorWindow>("Levels Editor");
    }

    private void OnGUI()
    {
        _id = EditorGUILayout.IntField("Number Level", _id);
        if (_id != 0)
        {
            if(_levelInfo == null || _id != _levelInfo.Id)
                _levelInfo = LevelManager.LoadLevel(_id);

            _levelInfo.NumberMoves = EditorGUILayout.IntField("Number Moves", _levelInfo.NumberMoves);
            _levelInfo.Row = EditorGUILayout.IntField("Row", _levelInfo.Row);
            _levelInfo.Column = EditorGUILayout.IntField("Column", _levelInfo.Column);

            if (_levelInfo.Row != 0 && _levelInfo.Column != 0)
            {
                if (_levelInfo.Cells == null || 
                    _levelInfo.Row * _levelInfo.Column != _levelInfo.Cells.Count)
                {
                    _levelInfo.Cells = new List<CellType>();
                    for (var i = 0; i < _levelInfo.Row * _levelInfo.Column; i++)
                        _levelInfo.Cells.Add(CellType.None);

                    _levelInfo.Obstacles.Clear();
                }

                EditorGUILayout.LabelField("Game Field");
                for (var i = 0; i < _levelInfo.Row; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (var j = 0; j < _levelInfo.Column; j++)
                    {
                        var index = (i * _levelInfo.Column) + j;
                        _levelInfo.Cells[index] = (CellType)EditorGUILayout.Popup((int)_levelInfo.Cells[index], _options);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.LabelField("Setting Obstacle And Bonus");
                var numberObstacle = 0;
                for (var i = 0; i < _levelInfo.Row; i++)
                {
                    for (var j = 0; j < _levelInfo.Column; j++)
                    {
                        var index = (i * _levelInfo.Column) + j;
                        var type = _levelInfo.Cells[index];
                        EditorGUILayout.BeginHorizontal();
                        if (type == CellType.Obstacle || type == CellType.Bonus)
                        {
                            EditorGUILayout.LabelField(_levelInfo.Cells[index].ToString() + " " + i + " " + j);
                            if (type == CellType.Obstacle)
                            {
                                numberObstacle++;
                                if (_levelInfo.Obstacles.Count < numberObstacle)
                                    _levelInfo.Obstacles.Add(new ObstacleData());

                                _levelInfo.Obstacles[numberObstacle - 1].X = i;
                                _levelInfo.Obstacles[numberObstacle - 1].Y = j;
                                _levelInfo.Obstacles[numberObstacle - 1].Type =
                                    (ObstacleType)EditorGUILayout.EnumPopup("Type ", _levelInfo.Obstacles[numberObstacle - 1].Type);
                                _levelInfo.Obstacles[numberObstacle - 1].Level =
                                    EditorGUILayout.IntField("Level ", _levelInfo.Obstacles[numberObstacle - 1].Level);
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }

                _levelInfo.LevelGoalType = (LevelGoal)EditorGUILayout.EnumPopup(_levelInfo.LevelGoalType);
                if(_levelInfo.LevelGoalType == LevelGoal.DestroyCell)
                {
                    for(var i = 0; i < _levelInfo.LevelGoalInfo.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(_levelInfo.LevelGoalInfo[i].Type.ToString());
                        _levelInfo.LevelGoalInfo[i].Count = EditorGUILayout.IntField(_levelInfo.LevelGoalInfo[i].Count);
                        EditorGUILayout.EndHorizontal();
                    }
                }

                if(_levelInfo.LevelGoalType == LevelGoal.Score)
                    _levelInfo.Score = EditorGUILayout.IntField("Score", _levelInfo.Score);

                if (numberObstacle < _levelInfo.Obstacles.Count)
                    _levelInfo.Obstacles.RemoveRange(numberObstacle, _levelInfo.Obstacles.Count - numberObstacle);
            }

            if (GUILayout.Button("Remove Level"))
            {
                _id = 0;
                LevelManager.RemoveLevel(_levelInfo);
                _levelInfo = null;
            }
        }
    }

    private void OnLostFocus()
    {
        LevelManager.SaveLevels();
    }
}
