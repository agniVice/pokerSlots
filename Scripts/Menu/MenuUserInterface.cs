using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class MenuUserInterface : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private CanvasGroup _menuPanel;
    [SerializeField] private CanvasGroup _settingsPanel;
    [SerializeField] private CanvasGroup _gamePanel;

    [SerializeField] private List<Transform> _menuElements;
    [SerializeField] private List<Transform> _settingsElements;

    [Header("Sound")]
    [SerializeField] private Image _soundButton;
    [SerializeField] private Image _musicButton;

    [SerializeField] private Sprite _buttonEnabled;
    [SerializeField] private Sprite _buttonDisabled;

    private void Start()
    {
        OpenMenu();
    }
    public void OpenMenu()
    {
        _menuPanel.alpha = 1 ;

        _menuPanel.gameObject.SetActive(true);
        _settingsPanel.gameObject.SetActive(false);
        _gamePanel.gameObject.SetActive(false);

        foreach (Transform t in _menuElements)
        { 
            Vector2 scale = t.localScale;
            t.localScale = Vector2.zero;
            t.DOScale(scale, Random.Range(0.1f, 0.2f)).SetEase(Ease.OutBack).SetLink(t.gameObject);
        }
    }
    public void OpenSettings()
    {
        _settingsPanel.alpha = 1;

        _menuPanel.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(true);
        _gamePanel.gameObject.SetActive(false);

        foreach (Transform t in _settingsElements)
        {
            Vector2 scale = t.localScale;
            t.localScale = Vector2.zero;
            t.DOScale(scale, Random.Range(0.1f, 0.2f)).SetEase(Ease.OutBack).SetLink(t.gameObject);
        }

        UpdateSoundAndMusic();
    }
    public void OpenGame()
    {
        _gamePanel.alpha = 0;
        _gamePanel.DOFade(1, 0.3f).SetLink(_gamePanel.gameObject);

        _gamePanel.gameObject.SetActive(true);
        _menuPanel.gameObject.SetActive(false);

        SortUI.Instance.OpenSort();
        LevelManager.Instance.InitializeLevel();
    }
    public void OnSoundButtonClicked()
    {
        AudioManager.Instance.ToggleSound();
        UpdateSoundAndMusic();
    }
    public void OnMusicButtonClicked()
    {
        AudioManager.Instance.ToggleMusic();
        UpdateSoundAndMusic();
    }
    private void UpdateSoundAndMusic()
    {
        if (AudioManager.Instance.SoundEnabled)
            _soundButton.sprite = _buttonEnabled;
        else
            _soundButton.sprite = _buttonDisabled;

        if (AudioManager.Instance.MusicEnabled)
            _musicButton.sprite = _buttonEnabled;
        else
            _musicButton.sprite = _buttonDisabled;
    }
    public void OnPlayButtonClicked() => OpenGame();
    public void OnSettignsButtonClicked() => OpenSettings();
    public void OnExitButtonClicked() => Application.Quit();
}
