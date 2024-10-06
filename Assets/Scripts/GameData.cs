using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
public class GameData : ScriptableObject
{
    public float LaunchForce = 10.0f;
    public float IndicatorMax = 0.95f; //absolute value of maximum indicator value
    [HideInInspector]
    public float Distance = 0.0f;
    [HideInInspector]
    public float DistanceTimer = 0.0f;
    [Tooltip("Amount of time to wait to check distance against last distance")]
    public float DistanceTimerMax = 2.0f;
    [HideInInspector]
    public float LastDistance = 0.0f;
    [Tooltip("Max distance since last distance to be considered stopped")]
    public float StoppedThreshold = 1.0f;

    [HideInInspector]
    public UnityEvent OnPhaseChange;
    //0=intro sequence, 1=power phase, 2=launching, 3=ending
    private int currentPhase = 0;
    public int CurrentPhase
    {
        get => currentPhase;
        set
        {
            currentPhase = value;
            OnPhaseChange?.Invoke();
        }
    }

    [HideInInspector]
    //the moving number for the power indicator. -50 to 50
    public float IndicatorValue = 0.0f;

}
