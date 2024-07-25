using System.Collections.Generic;
using UnityEngine;
using FuryLion.UI;

public class Goal : MonoBehaviour
{
    [SerializeField] private float height = 1f;
    [SerializeField] private GameField _gameField;
    [SerializeField] private Image _background;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private GameObject _goal;
    [SerializeField] private GameObject _score;
    [SerializeField] private Text _numberMovesText;
    [SerializeField] private Text _currentScoreText;
    [SerializeField] private Text _targetScoreText;
    [SerializeField] private int _numberScore = 100;

    public static Goal _instance;

    private List<GoalView> _goalViews = new List<GoalView>();
    private int _currentScore;
    private int _targetScore;
    private bool _goalsComplete;
    private int _numberMoves;

    private int NumberMoves
    {
        get => _numberMoves;
        set
        {
            _numberMoves = value;
            _numberMovesText.Value = _numberMoves.ToString();
            if (_numberMoves == 0 && !_goalsComplete)
            {
                EndGamePage.IsWin = false;
                PageManager.Open<EndGamePage>();
            }
        }
    }

    private void Start()
    {
        if(_instance  == null)
            _instance = this;
    }

    public void Init()
    {
        _goalsComplete = false;
        _currentScore = 0;
        NumberMoves = LevelManager.Level.NumberMoves;
        _gameField.Moved += () => NumberMoves--;
        switch (LevelManager.Level.LevelGoalType)
        {
            case LevelGoal.DestroyCell:
                InitDestroyCell();
                break;
            case LevelGoal.DestroyObstacle:
                InitDestroyObstacle();
                break;
            case LevelGoal.Score:
                InitScore();
                break;
        }
    }

    public void Clear()
    {
        foreach (var goalView in _goalViews)
            Recycler.Release(goalView);

        _goalViews.Clear();
        _score.SetActive(false);
        _goal.SetActive(true);
    }

    private void InitDestroyCell()
    {
        var i = 0;
        foreach (var goalInfo in LevelManager.Level.LevelGoalInfo)
            if (goalInfo.Count > 0)
            {
                CreateGoalView(goalInfo, i);
                i++;
            }

        _background.Size = new Vector2(_background.Size.x, i * height);
        _gameField.Matched += ChangeProgress;
    }

    private void InitDestroyObstacle()
    {
        var numberObstacle = 0;
        foreach(var type in LevelManager.Level.Cells)
            if(type == CellType.Obstacle)
                numberObstacle++;

        CreateGoalView(new GoalInfo(CellType.Obstacle, numberObstacle), 0.5f);
        _gameField.Matched += ChangeProgress;
        _background.Size = new Vector2(_background.Size.x, 2 * height);
    }

    private void InitScore()
    {
        _currentScore = 0;
        _targetScore = LevelManager.Level.Score;
        _currentScoreText.Value = _currentScore.ToString();
        _targetScoreText.Value = _targetScore.ToString();
        _progressBar.Value = 0;
        _score.SetActive(true);
        _goal.SetActive(false);
        _gameField.Matched += ChangeScore;
    }

    private void CreateGoalView(GoalInfo goalInfo, float position)
    {
        var goalView = Recycler.Get<GoalView>();
        goalView.Init(goalInfo.Type, goalInfo.Count);
        goalView.transform.position = transform.localPosition - (Vector3.up * position * height);
        _goalViews.Add(goalView);
    }

    private void ChangeProgress(CellType type)
    {
        var goalsComplete = true;
        foreach (var goalView in _goalViews)
        {
            if (goalView.CellType == type)
                goalView.CurrentGoal++;

            if (!goalView.Ready)
                goalsComplete = false;
        }

        if (goalsComplete && !_goalsComplete)
        {
            CompleteLevel();
            _goalsComplete = true;
        }
    }

    private void ChangeScore(CellType type)
    {
        if(type != CellType.Obstacle && type != CellType.Bonus)
        {
            _goalsComplete = false;
            _currentScore += _numberScore;
            _currentScoreText.Value = _currentScore.ToString();
            _progressBar.Value = (float)_currentScore / (float)_targetScore;
            if (_currentScore >= _targetScore)
            {
                CompleteLevel();
                _goalsComplete = true;
            }
        }
    }

    private void CompleteLevel()
    {        
        if (PlayerPrefs.GetInt("LevelCompleted", 0) < LevelManager.NumberLevel)
            PlayerPrefs.SetInt("LevelCompleted", LevelManager.NumberLevel);

        EndGamePage.IsWin = true;
        PageManager.Open<EndGamePage>();
    }
}
    