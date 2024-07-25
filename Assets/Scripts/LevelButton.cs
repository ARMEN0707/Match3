using UnityEngine;
using FuryLion.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private BaseButton _button;
    [SerializeField] private GameObject _blackBaground;
    [SerializeField] private GameObject _lock;
    [SerializeField] private GameObject _checkMark;
    [SerializeField] private GameObject _text;
    [SerializeField] private int _numberLevel;

    private bool _isOpen;

    private void OnEnable()
    {
        var levelCompleted = PlayerPrefs.GetInt("LevelCompleted", 0);
        if (_numberLevel <= levelCompleted)
        {
            _blackBaground.SetActive(false);
            _lock.SetActive(false);
            _text.SetActive(false);
            _checkMark.SetActive(true);
            _isOpen = true;
        }
        else if (_numberLevel == levelCompleted + 1)
        {
            _blackBaground.SetActive(false);
            _lock.SetActive(false);
            _checkMark.SetActive(false);
            _isOpen = true;
        }
        else if(_numberLevel >= levelCompleted + 2)
        {
            _blackBaground.SetActive(true);
            _lock.SetActive(true);
            _text.SetActive(true);
            _checkMark.SetActive(false);
            _isOpen = false;
        }
    }

    private void Start()
    {
        _button.Click += OpenLevel;
    }

    private void OpenLevel()
    {
        if (_isOpen)
        {
            LevelManager.LoadLevel(_numberLevel);
            PageManager.Open<GamePage>();
        }
    }
}
