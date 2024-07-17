using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tube : MonoBehaviour
{
    public Transform _ballUpper;

    [SerializeField] private List<SortElement> _elements = new List<SortElement>();

    [SerializeField] private Transform[] _ballSpawnPoints;
    [SerializeField] private GameObject _elementPrefab;

    [SerializeField] private bool _generateElements = true;

    private TubesManager _tubesManager;

    public void InitializeTube(TubesManager tubesManager)
    { 
        _tubesManager = tubesManager;
    }
    public void ClearElemenets()
    {
        foreach (var element in _elements)
            Destroy(element.gameObject);

        _elements.Clear();
    }
    public void GenerateRandom()
    {
        ClearElemenets();

        if (!_generateElements)
            return;

        float delay = 0;
        for (int i = 0; i < _ballSpawnPoints.Length; i++)
        {
            delay += 0.2f;
            GameObject go = Instantiate(_elementPrefab, _ballSpawnPoints[i].position, Quaternion.identity, this.transform);
            _elements.Add(go.GetComponent<SortElement>());
            ElementType type = _tubesManager.GetRandomElement();
            go.GetComponent<SortElement>().Initialize(type, _tubesManager.GetElementSprite(type), SortUI.Instance.ElementsStartPosition.position, delay);
        }
    }
    public void HideElements()
    {
        foreach (var item in _elements)
            item.Hide();
    }
    public void AddBall(SortElement ball)
    {
        if (!IsFull)
        {
            _elements.Add(ball);
            ball.transform.DOMove(_ballUpper.position, 0.3f).SetLink(ball.gameObject).OnKill(() => { 
                ball.transform.DOMove(_ballSpawnPoints[_elements.Count - 1].position, 0.3f).SetLink(ball.gameObject); });

            _tubesManager.CheckForLevelComplete();
        }
    }
    public SortElement RemoveLastBall()
    {
        if (_elements.Count > 0)
        {
            int lastIndex = _elements.Count - 1;
            SortElement ballToRemove = _elements[lastIndex];
            _elements.RemoveAt(lastIndex);
            return ballToRemove;
        }
        return null;
    }
    public void GetLastBall()
    {
        if (!IsEmpty)
        {
            _elements[_elements.Count - 1].transform.DOMove(_ballUpper.position, 0.3f).SetLink(_elements[_elements.Count - 1].gameObject);
        }
    }
    public void UndoLastBall()
    {
        _elements[_elements.Count - 1].transform.DOMove(_ballSpawnPoints[_elements.Count-1].position, 0.3f).SetLink(_elements[_elements.Count - 1].gameObject);
    }
    private void OnMouseDown()
    {
        _tubesManager.SelectTube(this);
    }
    public bool CheckForComplete()
    {
        if (_elements.Count == 0)
            return true;

        if (!IsFull)
            return false;

        ElementType firstBallType = _elements[0].Type;

        foreach (SortElement ball in _elements)
        {
            if (ball.Type != firstBallType)
                return false;
        }
        return true;
    }
    public bool IsFull => _elements.Count >= 4;
    public bool IsEmpty => _elements.Count == 0;
}