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
    GameObject powerUI;
    [SerializeField]
    RectTransform powerBar;
    [SerializeField]
    RectTransform powerTarget;
    [SerializeField]
    RectTransform powerIndicator;
    [SerializeField]
    GameObject distanceBox;
    [SerializeField]
    TextMeshProUGUI distanceText;
    [SerializeField]
    GameObject dialogueBox;
    [SerializeField]
    TextMeshProUGUI dialogueText;
    [SerializeField]
    GameObject helpBox;
    [SerializeField]
    TextMeshProUGUI helpText;
    bool isHelpHidden = false;
    [SerializeField][Tooltip ("What the button help text says during power phase")]
    string powerHelp = "Spit the critter";
    [SerializeField]
    [Tooltip("What the button help text says during end phase")]
    string endHelp = "Restart the game";
    [SerializeField]
    GameObject theAButton;



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
            case 0: //intro (needs changing)

                break;
            case 1: //power phase
                dialogueBox.SetActive(false);
                distanceBox.SetActive(false);
                powerUI.SetActive(true);
                helpText.text = powerHelp;
                break;
            case 2: //launching
                powerUI.SetActive(false);
                distanceBox.SetActive(true);
                helpBox.SetActive(false);
                theAButton.SetActive(false);
                break;
            case 3: //ended
                if(!isHelpHidden)
                {
                    helpBox.SetActive(true);
                }
                helpText.text = endHelp;
                theAButton.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ToggleHelp()
    {
        if(gameData.CurrentPhase != 2)
        {
            helpBox.SetActive(!helpBox.activeSelf);
        }
    }


    private void MoveIndicator()
    {
        //move the indicator back and forth
        Vector3 pos = powerIndicator.anchoredPosition;
        float xPos = gameData.IndicatorValue / gameData.IndicatorMax * (barWidth/2);
        powerIndicator.anchoredPosition = new Vector2(xPos, pos.y); 
    }

  
    private IEnumerator TypeWords(string words)
    {
        dialogueText.text = "";
        gameData.IsTextPrinting = true;
        for (int i=0; i<words.Length; i++)
        {
            dialogueText.text += words[i];
            if(!gameData.SkipText)
            {
                /*if(gameData.TalkTimer >= gameData.TalkTimerMax)
                {
                    int rando = Random.Range(0, talkClips.Count);
                    sfxPlayer.PlayOneShot(talkClips[rando]);
                    gameData.TalkTimer - gameData.TalkTimerMax;
                }*/
                yield return new WaitForSeconds(gameData.TextSpeed);
            }
            else
            {
                dialogueText.text = words;
            }
        }
        gameData.IsTextPrinting = false;
        gameData.SkipText = false;
    }
}
