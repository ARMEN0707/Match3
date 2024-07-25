using System;
using System.Collections.Generic;

[Serializable]
public class LevelInfo
{
    public int Id;
    public int Row;
    public int Column;
    public int NumberMoves = 10;
    public int Score;
    public LevelGoal LevelGoalType;
    public List<CellType> Cells;
    public List<ObstacleData> Obstacles = new List<ObstacleData>();
    public List<GoalInfo> LevelGoalInfo = new List<GoalInfo>
    {
        new GoalInfo(CellType.Blue, 0),
        new GoalInfo(CellType.Green, 0),
        new GoalInfo(CellType.Orange, 0),
        new GoalInfo(CellType.Red, 0),
        new GoalInfo(CellType.Yellow, 0)
    };

    public LevelInfo(int id)
    {
        Id = id;
    }

    public ObstacleData GetObstacle(int x, int y) => Obstacles.Find(item => item.X == x && item.Y == y);
}
