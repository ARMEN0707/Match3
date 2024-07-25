using System;
using System.Collections.Generic;
using UnityEngine;
using FuryLion;

public class Bonus : Cell
{
    [SerializeField] private List<BonusAsset> _bonusAssets;
    [SerializeField] private int _sizeArea;

    private BonusType _bonusType;

    public BonusType BonusType
    {
        get => _bonusType;
        set
        {
            _bonusType = value;
            SetSprite();
        }
    }

    public void Activate(Action<int, int> removeCell, int row, int column)
    {
        switch(BonusType)
        {
            case BonusType.Vertical:
                for (var i = 0; i < row; i++)
                    removeCell?.Invoke(i, Y);
                break;
            case BonusType.Horizontal:
                for (var i = 0; i < column; i++)
                    removeCell?.Invoke(X, i);
                break;
            case BonusType.Area:
                for (var i = X - _sizeArea; i <= X + _sizeArea; i++)
                    for (var j = Y - _sizeArea; j <= Y + _sizeArea; j++)
                        removeCell?.Invoke(i, j);
                break;
        }

        if (SoundManager.GlobalSoundsVolume == 1)
            SoundManager.PlaySound(Sounds.Sound.Explosion);
    }

    private void SetSprite()
    {
        _sprite.sprite = _bonusAssets.Find(asset => asset.Type == _bonusType).Sprite;
    }
}

