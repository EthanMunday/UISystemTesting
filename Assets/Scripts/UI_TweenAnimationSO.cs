using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTweenAnimation", menuName = "ScriptableObjects/TweenAnimationSO")]
public class UI_TweenAnimationSO : ScriptableObject
{
    public List<TweenAnimation> onLoad;
    public List<TweenAnimation> onLoadExit;
    public List<TweenAnimation> onIdleLoop;

    // These 3 could be a toggleable button mode if needed
    public List<TweenAnimation> onHighlight;
    public List<TweenAnimation> onHighlightLoop;
    public List<TweenAnimation> onUnhighlight;
    public List<TweenAnimation> onClick;

    public List<TweenAnimation> onEnable;
    public List<TweenAnimation> onDisable;

    public string pressSound;
}
