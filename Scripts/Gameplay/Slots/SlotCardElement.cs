using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlotCardElement : MonoBehaviour
{
    public void Initialize(Transform endPos, Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
        transform.DOMove(endPos.position, 0.3f).SetLink(gameObject).SetEase(Ease.InBack).SetDelay(0.3f);
        transform.DOScale(0, 0.2f).SetLink(gameObject).SetDelay(0.7f).OnKill(() => { Destroy(gameObject, 0.2f); });
    }
}
