using System;
using UnityEngine;
using FuryLion;
using FuryLion.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Image _imageButton;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private BaseButton _button;
    [SerializeField] private bool _isMusic;
    
    private const string KeyMusic = "Music";
    private const string KeySound = "Sound";

    private bool _mute;
    private string _key;

    public bool Mute
    {
        get => _mute;
        set
        {
            if (_isMusic)
                SoundManager.SetMusicVolume(value ? 0 : 1);
            else
                SoundManager.SetSoundVolume(value ? 0 : 1);
            _mute = value;
            SetSprite();
        }
    }

    private void Awake()
    {
        _button.Click += ChangeState;
        if (_isMusic)
            _key = KeyMusic;
        else
            _key = KeySound;

        Mute = PlayerPrefs.GetInt(_key, 1) == 0;
    } 

    private void ChangeState()
    {
        PlayerPrefs.SetInt(_key, Convert.ToInt32(Mute));
        Mute = !Mute;
    }

    private void SetSprite() => _imageButton.Sprite = Mute ? _offSprite : _onSprite;
}
