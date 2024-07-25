// Copyright (c) 2012-2022 FuryLion Group. All Rights Reserved.

using UnityEngine;
using FuryLion;
using FuryLion.UI;

public sealed class MainPage : Page, IMainPage
{
    [SerializeField] private BaseButton _setting;

    protected override void OnCreate()
    {
        SoundManager.SetMusicVolume(PlayerPrefs.GetInt("Music", 1));
        SoundManager.SetSoundVolume(PlayerPrefs.GetInt("Sound", 1));
        SoundManager.PlayMusic(Sounds.Music.Nightwing);
        _setting.Click += () => PopupManager.Open<SettingPopup>();
    }
}
