using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDrum : MonoBehaviour
{
    [SerializeField] private List<SlotElement> _elements;
    private Vector2[] _elemetPositions;

    private bool _isSpinning;
    private float _delayStart;
    private float _delayEnd;

    private int _countOfSpins;

    private bool _isWinGame;

    private ElementType _winType;

    private void Start()
    {
        InitializeElements();
        Generate();
    }
    private void FixedUpdate()
    {
        if (_isSpinning)
        {
            foreach (var element in _elements)
                element.transform.position += Vector3.down * Time.fixedDeltaTime * 10;
        }
    }
    private void InitializeElements()
    {
        foreach (var element in _elements)
            element.Initialize(this);

        _elemetPositions = new Vector2[_elements.Count];
        for (int i = 0; i < _elements.Count; i++)
            _elemetPositions[i] = _elements[i].transform.localPosition;
    }
    private void Generate()
    {
        foreach (var element in _elements)
            SetRandomElementType(element);
    }
    private void SetRandomElementType(SlotElement element)
    {
        if (UnityEngine.Random.Range(0, 100) <= SlotMachine.Instance.GetWildChance())
            element.SetElementType(this, ElementType.Wild);
        else
            element.SetElementType(this, (ElementType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(ElementType)).Length - 1));
    }
    private IEnumerator StartSpin()
    {
        yield return new WaitForSeconds(_delayStart);
        _isSpinning = true;
    }
    private IEnumerator EndSpin()
    {
        yield return new WaitForSeconds(_delayEnd);
        for (int i = 0; i < _elements.Count; i++)
        {
            _elements[i].transform.DOLocalMove(_elemetPositions[i], 0.2f)
                .SetLink(_elements[i].gameObject)
                .SetEase(Ease.OutBack)
                .OnKill(() => { SlotMachine.Instance.ReadyForSpin(SlotMachine.Instance.GetDrumIndex(this)); });
        }
        _isSpinning = false;
    }
    public void OnDrumStartWinSpin(float delay, ElementType type)
    {
        _winType = type;
        _isWinGame = true;
        OnDrumStartSpin(delay);
    }
    public void OnDrumStartSpin(float delay)
    {
        _delayStart = delay;
        StartCoroutine("StartSpin");
    }
    public void OnDrumEndSpin(float delay)
    {
        _delayEnd = delay;
        StartCoroutine("EndSpin");
    }
    public void OnElementSpin(SlotElement element)
    {
        if(_countOfSpins % 2 == 0)
            AudioManager.Instance.PlaySound(AudioManager.Instance.Scroll, UnityEngine.Random.Range(0.95f, 1.05f));
        _countOfSpins++;
        SetRandomElementType(element);

        if (_countOfSpins == 14 && _isWinGame)
        {
            _isWinGame = false;
            _elements[0].SetElementType(this, _winType);
        }
        if (_countOfSpins == 15)
        {
            _countOfSpins = 0;
            _isSpinning = false;
            _delayEnd = 0;
            AudioManager.Instance.PlaySound(AudioManager.Instance.Scroll, 1f, 1.5f);
            StartCoroutine("EndSpin");
        }
    }
    public void RemoveElement(SlotElement element) => _elements.Remove(element);
    public void AddElement(SlotElement element) => _elements.Insert(0, element);
    public Vector2 GetLastElementPosition() => _elements[0].transform.localPosition;
    public ElementType GetMainElement() => _elements[2].GetElementType();
}
