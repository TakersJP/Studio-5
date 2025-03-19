using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private TextMeshProUGUI toUpdate;
    [SerializeField] private Transform scoreTextContainer;
    [SerializeField] private float duration;
    [SerializeField] private Ease animationEase = Ease.OutCubic;

    private float containerInitPosY;
    private float moveAmount;

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        current.SetText("0");
        toUpdate.SetText("0");
        containerInitPosY = scoreTextContainer.localPosition.y;
        moveAmount = current.rectTransform.rect.height;
    }

    public void UpdateScore(int score)
    {
        toUpdate.SetText($"{score}");
        scoreTextContainer.DOLocalMoveY(containerInitPosY + moveAmount, duration).SetEase(animationEase);
        StartCoroutine(ResetPosition(score));
    }

    private IEnumerator ResetPosition(int score)
    {
        yield return new WaitForSeconds(duration);
        current.SetText($"{score}");
        Vector3 pos = scoreTextContainer.localPosition;
        scoreTextContainer.localPosition = new Vector3(pos.x, containerInitPosY, pos.z);
    }
}
