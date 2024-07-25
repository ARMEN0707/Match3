using UnityEngine;

[CreateAssetMenu(menuName = "BonusAsset")]
public class BonusAsset : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private BonusType _type;

    public Sprite Sprite => _sprite;

    public BonusType Type => _type;
}
