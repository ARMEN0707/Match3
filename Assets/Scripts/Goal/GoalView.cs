using System.Collections.Generic;
using FuryLion.UI;
using UnityEngine;

public class GoalView : Element, IRecyclable
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();

    private int _countGoal;
    private int _currentGoal;
    private CellType _cellType;
    private bool _ready;

    public CellType CellType => _cellType;

    public bool Ready => _ready;

    public int CurrentGoal
    {
        get => _currentGoal;
        set
        {
            if (value >= _countGoal)
                _ready = true;

            if (value > _countGoal)
                _currentGoal = _countGoal;
            else
            {
                _currentGoal = value;
                _text.Value = _currentGoal.ToString() + "/" + _countGoal.ToString();
            }
        }
    }

    public void Init(CellType type, int count)
    {
        _image.Sprite = _sprites[(int)type-1];
        _cellType = type;
        _countGoal = count;
        CurrentGoal = 0;
        _ready = false;
    }
}
