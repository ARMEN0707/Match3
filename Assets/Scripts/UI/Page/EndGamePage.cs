// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using UnityEngine;
using FuryLion.UI;

public sealed class EndGamePage : Page
{
    [SerializeField] private BaseButton _nextLevel;
    [SerializeField] private BaseButton _exit;
    [SerializeField] private BaseButton _restart;
    [SerializeField] private Text _result;

    public static bool IsWin;

    protected override void OnCreate()
    {
        _nextLevel.Click += OpenNextLevel;
        _exit.Click += () => PageManager.Open<MainPage>();
        _restart.Click += Restart;
    }

    protected override void OnOpenStart(ViewParam viewParam)
    {
        if (IsWin)
            _result.Value = "Win";
        else
            _result.Value = "Lose";

        if (LevelManager.NumberLevel + 1 > LevelManager.CountLevels)
            _nextLevel.gameObject.SetActive(false);
        else
            _nextLevel.gameObject.SetActive(IsWin);
    }

    private void OpenNextLevel()
    {
        LevelManager.LoadLevel(LevelManager.NumberLevel + 1);
        PageManager.Open<GamePage>();
    }

    private void Restart()
    {
        CloseLast();
        PageManager.Open<GamePage>();
    }
}
