// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using UnityEngine;
using FuryLion.UI;

public sealed class GamePage : Page
{
    [SerializeField] private BaseButton _pause;
    [SerializeField] private GameField _gameField;
    [SerializeField] private Goal _goal;

    protected override void OnCreate()
    {
        _pause.Click += () => PopupManager.Open<PausePopup>();
    }

    protected override void OnOpenStart(ViewParam viewParam)
    {
        _gameField.Init();
        _goal.Init();
    }

    protected override void OnCloseStart()
    {
        _gameField.Clear();
        _goal.Clear();
    }
}
