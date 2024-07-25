using System;

[Serializable]
public class GoalInfo
{
    public CellType Type;
    public int Count;

    public GoalInfo(CellType type, int count)
    {
        Type = type;
        Count = count;
    }
}
