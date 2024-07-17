using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SortElement : MonoBehaviour
{
    public ElementType Type;
    private Image _image;

    private Vector2 _startPosition;
    private Vector3 _startScale;

    private void Awake()
    {
        _image = GetComponent<Image>();    
    }
    public void Initialize(ElementType type, Sprite sprite, Vector2 position, float delay)
    {
        Type = type;
        _image.sprite = sprite;

        _startPosition = transform.localPosition;
        _startScale = transform.localScale;
        
        transform.position = position;
        //transform.localScale = Vector3.zero;
        _image.color = new Color32(255, 255, 255, 0);

        transform.DOLocalMove(_startPosition, 1f).SetLink(gameObject).SetEase(Ease.OutCubic).SetDelay(delay);
        _image.DOFade(1, 1f).SetLink(gameObject).SetDelay(delay);
    }
    public void Hide()
    {
        transform.DOScale(0, Random.Range(0.2f, 0.7f)).SetLink(gameObject).SetEase(Ease.InBack);
    }
}
