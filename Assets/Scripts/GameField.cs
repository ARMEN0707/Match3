using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FuryLion;
using FuryLion.UI;
using FuryLion.ObjectPool;

public class GameField : MonoBehaviour
{
    [SerializeField] private Vector3 _centerGameField;
    [SerializeField] private float _size;
    [SerializeField] private LayerMask _maskCell;
    [SerializeField] private LayerMask _maskCamera;
    [SerializeField] private int _numberCellForRocket;
    [SerializeField] private int _numberCellForBomb;

    public static Cell[,] _cells;
    public static GameField _instance;
    public static GameState State;

    private List<Cell> _lineCell = new List<Cell>();
    private List<Cell> _obstacles = new List<Cell>();
    private List<Cell> _bonusCells = new List<Cell>();
    private int _numberDisappearanceCell;
    private LevelInfo _level;
    private int _column;
    private int _row;

    public event Action<CellType> Matched;
    public event Action Moved;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Update()
    {
        switch(State)
        {
            case GameState.Playing when Input.GetMouseButton(0):
                SelectedCell();
                break;
            case GameState.Playing when Input.GetMouseButtonUp(0):
                RemoveMatch();
                break;
            case GameState.Falling:
                MoveCells();
                break;
        }
    }

    public void Init()
    {
        _numberDisappearanceCell = 0;
        Matched = null;
        Moved = null;
        State = GameState.Playing;
        _level = LevelManager.Level;
        _column = _level.Column;
        _row = _level.Row;
        _cells = new Cell[_row, _column];
        CreateField();
    }

    public void Clear()
    {
        for (var i = 0; i < _row; i++)
            for (var j = 0; j < _column; j++)
                if (_cells[i, j] != null)
                    CellPool.Release(_cells[i, j]);
    }

    private void SelectedCell()
    {
        var origin = tk2dCamera.CameraForLayer(_maskCamera).ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(origin, Vector2.up, 0.1f, _maskCell);
        if (hit.collider != null)
        {
            var position = hit.collider.gameObject.transform.position;
            var cell = GetItem(position - _centerGameField);
            if (Checkcell(cell))
            {
                var index = _lineCell.IndexOf(cell);
                if (index >= 0)
                {
                    for (var i = index; i < _lineCell.Count; i++)
                        _lineCell[i].RemoveOutline();

                    _lineCell.RemoveRange(index, _lineCell.Count - index);
                }

                _lineCell.Add(cell);
                cell.SetOutline();
                if (SoundManager.GlobalSoundsVolume == 1)
                    SoundManager.PlayRandomSound(
                        Sounds.Sound.SelectFirst,
                        Sounds.Sound.SelectSecond,
                        Sounds.Sound.SelectThird,
                        Sounds.Sound.SelectFourth
                        );
            }
        }
    }

    private void RemoveMatch()
    {
        if (_lineCell.Count < 3)
        {
            foreach (var cell in _lineCell)
                cell.RemoveOutline();

            if (_lineCell.Count == 1 && _lineCell[0] is Bonus bonus)
                bonus.Activate((i, j) => RemoveCell(i, j), _row, _column);
        }
        else
        {
            State = GameState.Disappearing;
            foreach (var cell in _lineCell)
            {
                if (cell.Type == CellType.Bonus)
                    _bonusCells.Add(cell);

                cell.RemoveOutline();
                FindObstacle(cell.X, cell.Y);
                RemoveCell(cell.X, cell.Y);
            }

            if (SoundManager.GlobalSoundsVolume == 1)
                SoundManager.PlaySound(Sounds.Sound.Match);

            foreach (Bonus bonus in _bonusCells)
                bonus.Activate((i, j) => RemoveCell(i, j), _row, _column);

            if (_lineCell.Count >= _numberCellForBomb)
            {
                CreateBonus(
                    _lineCell[_numberCellForBomb - 1].X,
                    _lineCell[_numberCellForBomb - 1].Y,
                    BonusType.Area
                );
            }
            else if (_lineCell.Count >= _numberCellForRocket)
            {
                CreateBonus(
                    _lineCell[_numberCellForRocket - 1].X,
                    _lineCell[_numberCellForRocket - 1].Y,
                    BonusType.Horizontal
                );
            }

            Moved?.Invoke();
        }

        _obstacles.Clear();
        _bonusCells.Clear();
        _lineCell.Clear();
    }

    private void CreateField()
    {
        for (var i = 0; i < _row; i++)
            for (var j = 0; j < _column; j++)
            {
                var index = (i * _column) + j;
                CreateCell(i, j, _level.Cells[index], true);
            }
    }

    private void CreateCell(int x, int y, CellType cellType, bool loadLevel = false)
    {
        if(cellType == CellType.None)
            cellType = (CellType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(CellType)).Length - 2);

        var cell = CellPool.Get<Cell>(cellType);
        cell.Init();
        if (loadLevel && cellType == CellType.Obstacle)
        {
            var obstacleData = _level.GetObstacle(x, y);
            var obstacle = cell as Obstacle;
            obstacle.Level = obstacleData.Level;
            obstacle.ObstacleType = obstacleData.Type;
        }

        cell.Scale = new Vector3(_size, _size, 1);
        cell.Position = _centerGameField + GetPositionCell(x, y);
        cell.X = x;
        cell.Y = y;
        cell.Disappeared += CheckDisappearance;
        _cells[x, y] = cell;
    }

    private void CreateBonus(int x, int y, BonusType type)
    {
        CreateCell(x, y, CellType.Bonus);
        var bonus = _cells[x, y] as Bonus;
        bonus.BonusType = type;
    }

    private void RemoveCell(int i, int j)
    {
        if (i >= _row || i < 0 || j >= _column || j < 0)
            return;

        var cell = _cells[i, j];
        if (cell == null)
            return;

        if(cell is Obstacle obstacle)
        {
            if (!_obstacles.Contains(cell))
            {
                if (obstacle.Level > 1)
                {
                    _obstacles.Add(obstacle);
                    obstacle.ReduceLevel();
                    return;
                }
            }
            else
                return;
        }

        Matched?.Invoke(cell.Type);
        cell.SetRemoveAnimation();
        _cells[cell.X, cell.Y] = null;
        cell.Disappearance = true;
        _numberDisappearanceCell++;
    }

    private Vector3 GetPositionCell(int x, int y)
    {
        var offsetX = _column / 2;
        var offsetY = _row / 2;
        return new Vector3((y - offsetX) * _size, (offsetY - x) * _size, -2);
    }

    private Cell GetItem(Vector2 position)
    {
        var x = (int)Mathf.Round((_row / 2) - (position.y / _size));
        var y = (int)Mathf.Round((position.x / _size) + (_column / 2));
        return _cells[x, y];
    }

    private bool Checkcell(Cell cell)
    {
        if (_lineCell.Count == 0)
            return true;

        var lastcell = _lineCell.Last();
        if (lastcell.X == cell.X && lastcell.Y == cell.Y)
            return false;

        if (lastcell.Type != cell.Type && cell.Type != CellType.Bonus && lastcell.Type != CellType.Bonus)
            return false;

        if (!(cell.X >= lastcell.X - 1 && cell.X <= lastcell.X + 1))
            return false;

        if (!(cell.Y >= lastcell.Y - 1 && cell.Y <= lastcell.Y + 1))
            return false;

        return true;
    }

    private void FindObstacle(int x, int y)
    {
        if (x != _row - 1 && _cells[x + 1, y] != null && _cells[x + 1, y] is Obstacle)
            RemoveCell(x + 1, y);

        if (x != 0 && _cells[x - 1, y] != null && _cells[x - 1, y] is Obstacle)
            RemoveCell(x - 1, y);

        if (y != _column - 1 && _cells[x, y + 1] != null && _cells[x, y + 1] is Obstacle)
            RemoveCell(x, y + 1);

        if (y != 0 && _cells[x, y - 1] != null && _cells[x, y - 1] is Obstacle)
            RemoveCell(x, y - 1);
    }

    private void MoveCells()
    {
        for (var i = 0; i < _column; i++)
            MoveCellsVertical(i);

        for (var i = _row - 1; i >= 0; i--)
            MoveCellsHoriontal(i);

        State = GameState.Playing;
    }

    private void MoveCellsHoriontal(int i)
    {
        int? indexEmptyCell = null;
        for (var j = 0; j < _column; j++)
        {
            CheckCellMoveHorizontal(i, j, ref indexEmptyCell, () => indexEmptyCell++);
            if (j >= _column - 1 && _cells[i, j] == null)
            {
                indexEmptyCell = null;
                for (var k = j; k >= 0; k--)
                    CheckCellMoveHorizontal(i, k, ref indexEmptyCell, () => indexEmptyCell--);
            }
        }
    }

    private void MoveCellsVertical(int i)
    {
        int? indexEmptyCell = null;
        for (var j = _row - 1; j >= 0; j--)
        {
            if (_cells[j, i] == null && indexEmptyCell == null)
                indexEmptyCell = j;
            else if (_cells[j, i] != null && _cells[j, i] is Obstacle obstacle && obstacle.ObstacleType == ObstacleType.Ice)
                indexEmptyCell = null;
            else if (indexEmptyCell.HasValue && _cells[j, i] != null)
            {
                SwapCell(indexEmptyCell.Value, i, j, i);
                indexEmptyCell--;
            }

            if (j <= 0 && _cells[j, i] == null)
            {
                for (var k = indexEmptyCell.Value; k >= j; k--)
                    CreateCell(k, i, CellType.None);
            }
        }        
    }

    private void CheckCellMoveHorizontal(int i, int j, ref int? indexEmptyCell, Action editIndexEmptyCell)
    {
        if (_cells[i, j] == null && indexEmptyCell == null)
            indexEmptyCell = j;
        else if (_cells[i, j] != null && _cells[i, j] is Obstacle obstacle && obstacle.ObstacleType == ObstacleType.Ice)
            indexEmptyCell = null;
        else if (indexEmptyCell.HasValue && _cells[i, j] != null)
        {
            while (indexEmptyCell.Value != j)
            {
                SwapCell(i, indexEmptyCell.Value, i, j);
                MoveCellsVertical(j);
                editIndexEmptyCell();
            }

            indexEmptyCell = null;
        }
    }

    private void SwapCell(int x, int y, int i, int j)
    {
        _cells[x, y] = _cells[i, j];
        _cells[x, y].X = x;
        _cells[x, y].Y = y;
        _cells[x, y].TargetPosition = _centerGameField + GetPositionCell(x, y);
        _cells[i, j] = null;
    }

    private void CheckDisappearance()
    {
        _numberDisappearanceCell--;
        if (_numberDisappearanceCell <= 0)
            State = GameState.Falling;
    }
}
