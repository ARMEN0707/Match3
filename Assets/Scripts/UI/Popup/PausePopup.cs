// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using UnityEngine;
using FuryLion.UI;

public sealed class PausePopup : Popup
{
    [SerializeField] private BaseButton _setting;
    [SerializeField] private BaseButton _exit;
    [SerializeField] private BaseButton _close;
    [SerializeField] private BaseButton _restar;

    protected override void OnCreate()
    {
        _setting.Click += OpenSetting;
        _exit.Click += Exit;
        _close.Click += () => Close();
        _restar.Click += Restart;
    }

    protected override void OnOpenStart(ViewParam viewParam)
    {
        GameField.State = GameState.Pause;
    }

    protected override void OnCloseStart()
    {
        GameField.State = GameState.Playing;
    }

    private void OpenSetting()
    {
        Close();
        PopupManager.Open<SettingPopup>();
    }

    private void Exit()
    {
        Close();
        PageManager.Open<MainPage>();
    }

    private void Restart()
    {
        Close();
        BasePage.CloseLast();
        PageManager.Open<GamePage>();
    }
}
