using System.Collections.Generic;
using UnityEngine;

public class TubesManager : MonoBehaviour
{
    public float LevelTime;
    public int LevelReward;

    [SerializeField] private bool _randomGenerate;

    [SerializeField] private List<Tube> _tubes = new List<Tube>();

    [SerializeField] private Tube _selectedTube;
    [SerializeField] private Sprite[] _elementSprites;

    private int[] _countOfElements = { 0, 0, 0, 0, 0, 0 };

    private void Awake()
    {
        foreach (var tube in _tubes)
            tube.InitializeTube(this);
    }
    public void SelectTube(Tube tube)
    {
        if (_selectedTube == null)
        {
            if (!tube.IsEmpty)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.TubeSelected, 1f);

                tube.GetLastBall();
                _selectedTube = tube;
            }
        }
        else
        {
            if (_selectedTube != tube)
            {
                if (!CheckTubeFill(tube) && !CheckTubeEmpty(_selectedTube))
                {
                    if (_selectedTube != tube)
                    {
                        PutBallInTube(_selectedTube, tube);
                        AudioManager.Instance.PlaySound(AudioManager.Instance.TubeSet, 1f);
                    }
                }
            }
            else
            {
                tube.UndoLastBall();
                _selectedTube = null;
                AudioManager.Instance.PlaySound(AudioManager.Instance.TubeSet, 1f);
            }
        }
    }
    public void CheckForLevelComplete()
    {
        bool isCompleted = true;
        foreach (Tube tube in _tubes)
        {
            if (!tube.CheckForComplete())
                isCompleted = false;
        }
        if (isCompleted)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.LevelUp, 0.6f);
            SortTimer.Instance.StopTimer();
            LevelManager.Instance.OnGameSuccess();
        }
    }
    public void Generate()
    {
        if (!_randomGenerate)
            return;
        for (int i = 0; i < _countOfElements.Length; i++)
            _countOfElements[i] = 0;
        foreach (var tube in _tubes)
            tube.GenerateRandom();
    }
    public ElementType GetRandomElement()
    {
        int random = Random.Range(0, 5);
        while (_countOfElements[random] >= 4)
            random = Random.Range(0, 5);
        _countOfElements[random]++;
        return (ElementType)random;
    }
    private bool CheckTubeEmpty(Tube tube)
    {
        return tube.IsEmpty;
    }
    private bool CheckTubeFill(Tube tube)
    {
        return tube.IsFull;
    }
    private void PutBallInTube(Tube elementFrom, Tube elementIn)
    {
        var ball = elementFrom.RemoveLastBall();
        elementIn.AddBall(ball);
        _selectedTube = null;
    }
    public void OnGameComplete()
    {
        foreach (var tube in _tubes)
            tube.HideElements();

        SortUI.Instance.OnGameCompleted();
    }
    public void OnGameOver()
    {
        foreach (var item in _tubes)
            item.HideElements();
        
        SortUI.Instance.OnGameOver();
    }
    public Sprite GetElementSprite(ElementType type) => _elementSprites[(int)type];
}
