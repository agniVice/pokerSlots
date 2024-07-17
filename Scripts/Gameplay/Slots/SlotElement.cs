using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class SlotElement : MonoBehaviour
{
    [SerializeField] private Sprite[] _elementSprites;
    private Image _elementImage;

    private SlotDrum _drum;
    private ElementType _elementType;

    private void Awake()
    {
        _elementImage = GetComponent<Image>();
    }
    public void Initialize(SlotDrum drum) => _drum = drum;
    public void SetElementType(object sender, ElementType type)
    {
        if (sender.GetType() == typeof(SlotDrum))
            _elementType = type;

        _elementImage.sprite = _elementSprites[(int)_elementType];
    }
    public ElementType GetElementType()
    { 
        return _elementType;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BottomTrigger"))
        {
            _drum.RemoveElement(this);
            _drum.OnElementSpin(this);
            transform.localPosition = (_drum.GetLastElementPosition() + new Vector2(0, 192));
            _drum.AddElement(this);
        }
    }
}
