// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using UnityEngine;
using FuryLion.UI;

public sealed class SettingPopup : Popup
{
    [SerializeField] private BaseButton _close;

    protected override void OnCreate()
    {
        _close.Click += Close;        
    }

    protected override void OnOpenStart(ViewParam viewParam)
    {
        GameField.State = GameState.Pause;
    }

    protected override void OnCloseStart()
    {
        GameField.State = GameState.Playing;
    }
}
