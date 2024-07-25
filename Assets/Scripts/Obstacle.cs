using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Cell
{
    [SerializeField] private List<ObstacleAsset> _obstacleAsset;

    private Sprite[] _activeSprites;
    private int _level = 1;
    private ObstacleType _obstacleType;

    public ObstacleType ObstacleType
    {
        get => _obstacleType;
        set
        {
            _obstacleType = value;
            SetSprite();
        }
    }

    public int Level
    {
        get => _level;      
        set
        {
            if (value > 2)
                _level = 2;
            else if (value <= 0)
                _level = 1;
            else
                _level = value;
        }
    }

    public void ReduceLevel()
    {
        if (Level > 1)
        {
            Level--;
            SetSprite();
        }
    }

    public void SetSprite()
    {
        _sprite.sprite = _obstacleAsset.Find(asset => asset.Type == _obstacleType).Sprites[Level - 1];
    }
}
