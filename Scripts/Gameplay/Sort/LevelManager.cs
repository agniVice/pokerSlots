using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int Level {  get; private set; }

    [SerializeField] private List<GameObject> _levels;
    [SerializeField] private Transform _levelParent;

    private TubesManager _currentLevel;

    private float _levelTime;
    private int _levelReward;

    public int LevelReward => _levelReward;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        Level = PlayerPrefs.GetInt("CurrentLevel", 1);
    }
    public void InitializeLevel()
    {
        if (_currentLevel != null)
            Destroy(_currentLevel.gameObject);

        GameObject levelObject;

        if(Level > _levels.Count-1)
            levelObject = Instantiate(_levels[_levels.Count - 1], _levelParent);
        else
            levelObject = Instantiate(_levels[Level - 1], _levelParent);

        levelObject.transform.SetSiblingIndex(_levelParent.childCount - 3);

        _currentLevel = levelObject.GetComponent<TubesManager>();

        _levelTime = _currentLevel.LevelTime;
        _levelReward = _currentLevel.LevelReward;

        Generate();
        SortUI.Instance.UpdateUI();
        SortTimer.Instance.StartTimer(_levelTime);
    }
    private void Generate()
    { 
        _currentLevel.Generate();
    }
    public void OnGameFailed()
    {
        _currentLevel.OnGameOver();
        SortUI.Instance.UpdateUI();
        Save();
    }
    public void OnGameSuccess()
    {
        _currentLevel.OnGameComplete();
        PlayerBalance.Instance.AddBalance(this, LevelReward);
        SortUI.Instance.UpdateUI();
        Level++;
        Save();
    }
    private void Save()
    {
        PlayerPrefs.SetInt("CurrentLevel", Level);
    }
}
