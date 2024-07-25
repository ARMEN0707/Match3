using UnityEngine;

[CreateAssetMenu(menuName = "ObstacleAsset")]
public class ObstacleAsset : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private ObstacleType _type;

    public Sprite[] Sprites => _sprites;

    public ObstacleType Type => _type;
}
