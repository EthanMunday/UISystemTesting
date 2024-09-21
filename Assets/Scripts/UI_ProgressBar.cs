using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProgressBar : MonoBehaviour
{
    const float PROGRESS_BAR_UPDATE_SECONDS = 1.0f;
    Image barImage;

    private void Start() => barImage = GetComponent<Image>();
    public void SetProgress(float progress) => barImage.DOFillAmount(progress, PROGRESS_BAR_UPDATE_SECONDS).SetEase(Ease.OutSine);
    public void ResetProgress() => barImage.fillAmount = 0;
}
