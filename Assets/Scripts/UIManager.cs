using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField]
    TextMeshProUGUI distanceText;

    private float barWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        gameData.OnPhaseChange.AddListener(SetForPhase);
        barWidth = powerBar.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameData.CurrentPhase == 1) //power phase
        {
            MoveIndicator();
        }
        else if(gameData.CurrentPhase == 2)
        {
            distanceText.text = Mathf.Round(gameData.Distance).ToString();
        }
    }

    /// <summary>
    /// Sets the UI for the current phase when the phase switches
    /// </summary>
    private void SetForPhase()
    {
        switch(gameData.CurrentPhase)
        {
            //intro (needs changing)
            case 0:
                
                startButton.gameObject.SetActive(true);
                break;
            //power phase
            case 1:
                resetButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);
                powerUI.SetActive(true);
                break;
            //launching
            case 2:
                powerUI.SetActive(false);
                break;
            case 3:
                resetButton.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }




    private void MoveIndicator()
    {
        //move the indicator back and forth
        Vector3 pos = powerIndicator.anchoredPosition;
        float xPos = gameData.IndicatorValue / gameData.IndicatorMax * (barWidth/2);
        powerIndicator.anchoredPosition = new Vector2(xPos, pos.y); 
    }

  
}
