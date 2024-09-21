using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UI_TweenAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UI_TweenAnimationSO animations;

    bool hasMenu;

    Sequence tweenSequence;

    private void Start()
    {
        tweenSequence = DOTween.Sequence();
        if (GetComponentsInParent<UI_BaseMenu>().Length > 0 ) hasMenu = true;
        
        #if UNITY_EDITOR
        OnLoad();
        OnLoadingExit();
        #endif

        //SYST_SceneNavigation.OnFinishedLoading += OnLoad;
        //SYST_SceneNavigation.OnLoadingScreenExit += OnLoadingExit;
    }

    public void OnLoad()
    {
        PlaySequence(animations.onLoad);
        //SYST_SceneNavigation.OnFinishedLoading -= OnLoad;
    }

    public void OnLoadingExit()
    {
        PlaySequenceToLoop(animations.onLoadExit, animations.onIdleLoop);
        //SYST_SceneNavigation.OnLoadingScreenExit -= OnLoadingExit;
    }

    public void OnMenuEnable() => PlaySequenceToLoop(animations.onEnable, animations.onIdleLoop);

    public void OnMenuDisable() => PlaySequence(animations.onDisable);

    public void OnPointerEnter(PointerEventData eventData) => PlaySequenceToLoop(animations.onHighlight, animations.onHighlightLoop, false);

    public void OnPointerExit(PointerEventData eventData) => PlaySequenceToLoop(animations.onUnhighlight, animations.onIdleLoop);

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySequenceToLoop(animations.onClick, animations.onHighlightLoop);
        if (animations.pressSound == "" && animations.onClick.Count == 0) return;
        if (animations.onHighlightLoop.Count == 0) StartCoroutine(ChainToSequence(animations.onIdleLoop, true));
        //AUD_AudioService.PlayAudio(animations.pressSound, 0.3f);
    }

    public bool PlaySequence(List<TweenAnimation> sequenceAnimations, bool isLoop = false)
    {
        if (sequenceAnimations.Count == 0) return false;
        if (!gameObject.activeSelf) return false;
        StopAllCoroutines();
        tweenSequence.Kill();
        tweenSequence = DOTween.Sequence();
        foreach (TweenAnimation animation in sequenceAnimations) GetAnimation(animation);
        if (isLoop) tweenSequence.SetLoops(-1);
        if (tweenSequence.Duration() > 0.0f) tweenSequence.Play();
        return true;
    }

    public void PlaySequenceToLoop(List<TweenAnimation> sequenceAnimations, List<TweenAnimation> loopAnimations, bool forcePlayLoop = true)
    {
        if (PlaySequence(sequenceAnimations)) StartCoroutine(ChainToSequence(loopAnimations, true));
        else if (forcePlayLoop) PlaySequence(sequenceAnimations);
    }

    IEnumerator ChainToSequence(List<TweenAnimation> animations, bool isLoop = false)
    {
        yield return tweenSequence.WaitForCompletion();

        PlaySequence(animations, isLoop);
    }

    private void GetAnimation(TweenAnimation animation)
    {
        Tween rv;
        switch (animation.type)
        {
            case TweenType.Position:
                if (animation.isReversed) rv = transform.DOLocalMove(animation.target, animation.duration).SetEase(animation.ease).From();
                else rv = transform.DOLocalMove(animation.target, animation.duration).SetEase(animation.ease);
                break;
            case TweenType.Rotation:
                if (animation.isReversed) rv = transform.DORotate(animation.target, animation.duration).SetEase(animation.ease).From();
                else rv = transform.DORotate(animation.target, animation.duration).SetEase(animation.ease);
                break;
            case TweenType.Scale:
                if (animation.isReversed) rv = transform.DOScale(animation.target, animation.duration).SetEase(animation.ease).From();
                else rv = transform.DOScale(animation.target, animation.duration).SetEase(animation.ease);
                break;
            default:
                return;
        }

        tweenSequence.Append(rv);
    }

    private void OnDestroy()
    {
        tweenSequence.Kill();
    }
}

[Serializable]
public enum TweenType
{
    Position,
    Rotation,
    Scale
}

[Serializable]
public struct TweenAnimation
{
    public TweenType type;
    public Ease ease;
    public Vector3 target;
    public float duration;
    public bool isReversed;
}
