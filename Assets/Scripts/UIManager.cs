using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameData gameData;

    [SerializeField]
    Button startButton;
    [SerializeField]
    Button resetButton;
    [SerializeField]
    GameObject powerUI;
    [SerializeField]
    RectTransform powerBar;
    [SerializeField]
    RectTransform powerTarget;
    [SerializeField]
    RectTransform powerIndicator;

    
    // Start is called before the first frame update
    void Start()
    {
        gameData.OnPhaseChange.AddListener(SetForPhase);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameData.CurrentPhase == 1) //power phase
        {
            MoveIndicator();
        }
    }

    /// <summary>
    /// Sets the UI for the current phase when the phase switches
    /// </summary>
    private void SetForPhase()
    {
        switch(gameData.CurrentPhase)
        {
            //pre game
            case 0:
                resetButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(true);
                break;
            //power phase
            case 1:
                startButton.gameObject.SetActive(false);
                powerUI.SetActive(true);
                break;
            //launching
            case 2:
                powerUI.SetActive(false);
                resetButton.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }




    private void MoveIndicator()
    {
        //move the indicator back and forth
    }

  
}
