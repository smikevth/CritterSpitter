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
    GameObject angleUI;
    [SerializeField]
    RectTransform powerBar;
    [SerializeField]
    RectTransform powerIndicator;
    [SerializeField]
    RectTransform angleIndicator;
    [SerializeField]
    GameObject distanceBox;
    [SerializeField]
    TextMeshProUGUI distanceText;
    [SerializeField]
    GameObject dialogueBox;
    [SerializeField]
    TextMeshProUGUI dialogueText;
    int currentSpeaker = 0;
    [SerializeField]
    GameObject[] portraits;
    [SerializeField]
    GameObject helpBox;
    [SerializeField]
    TextMeshProUGUI helpText;
    bool isHelpHidden = false;
    [SerializeField]
    [Tooltip("What the button help text says during angle phase")]
    string angleHelp = "Set the angle";
    [SerializeField][Tooltip ("What the button help text says during power phase")]
    string powerHelp = "Spit the critter";
    [SerializeField]
    [Tooltip("What the button help text says during end phase")]
    string endHelp = "Restart the game";
    [SerializeField]
    GameObject theAButton;
    private float barWidth;
    [SerializeField]
    GameObject settingsMenu;

    [SerializeField]
    AudioSource musicPlayer;
    [SerializeField]
    AudioSource sfxPlayer;
    [SerializeField]
    Button[] qualityButtons;

    [SerializeField]
    GameObject endGameScreen;
    [SerializeField]
    GameObject prevBest;
    [SerializeField]
    GameObject newBest;
    [SerializeField]
    TextMeshProUGUI currentText;
    [SerializeField]
    TextMeshProUGUI bestText;

    private void Awake()
    {
        gameData.OnPhaseChange.AddListener(SetForPhase);
        gameData.OnIntroDialogueChange.AddListener(IntroDialogue);
    }

    // Start is called before the first frame update
    void Start()
    {
        barWidth = powerBar.sizeDelta.x;
        SetQuality(QualitySettings.GetQualityLevel());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.CurrentPhase == 1) //angle phase
        {
            MoveAngleIndicator();
        }
        else if (gameData.CurrentPhase == 2) //power phase
        {
            MovePowerIndicator();
        }
        else if(gameData.CurrentPhase == 3)
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
            case 1: //angle phase
                endGameScreen.SetActive(false);
                
                dialogueBox.SetActive(false);
                angleUI.SetActive(true);
                helpText.text = angleHelp;
                break;
            case 2: //power phase
                angleUI.SetActive(false);
                powerUI.SetActive(true);
                helpText.text = powerHelp;
                break;
            case 3: //launching
                powerUI.SetActive(false);
                distanceBox.SetActive(true);
                helpBox.SetActive(false);
                theAButton.SetActive(false);
                break;
            case 4: //ended
                if(!isHelpHidden)
                {
                    helpBox.SetActive(true);
                }
                helpText.text = endHelp;
                distanceBox.SetActive(false);
                ShowEndScreen();
                theAButton.SetActive(true);
                break;
            default:
                break;
        }
    }

    void ShowEndScreen()
    {
        endGameScreen.SetActive(true);
        currentText.text = Mathf.Round(gameData.Distance) + "m";
        if (gameData.NewHighScore)
        {
            prevBest.SetActive(false);
            newBest.SetActive(true);
        }
        else
        {
            prevBest.SetActive(true);
            newBest.SetActive(false);
            bestText.text = Mathf.Round(gameData.HighScore) + "m";
        }
    }

    public void ToggleHelp()
    {
        if(gameData.CurrentPhase != 3)
        {
            helpBox.SetActive(!helpBox.activeSelf);
        }
    }


    private void MovePowerIndicator()
    {
        //move the indicator back and forth
        Vector3 pos = powerIndicator.anchoredPosition;
        float xPos = gameData.PowerValue / gameData.IndicatorMax * (barWidth/2);
        powerIndicator.anchoredPosition = new Vector2(xPos, pos.y); 
    }
    private void MoveAngleIndicator()
    {
        //move the indicator back and forth
        Vector3 pos = angleIndicator.anchoredPosition;
        float xPos = gameData.AngleValue / gameData.IndicatorMax * (barWidth / 2);
        angleIndicator.anchoredPosition = new Vector2(xPos, pos.y);
    }

    private void IntroDialogue()
    {
        if(gameData.IntroDialogueIndex >= 0)
        {
            Dialogue dialogue = gameData.IntroDialogues[gameData.IntroDialogueIndex];
            if (currentSpeaker != dialogue.Speaker)
            {
                portraits[currentSpeaker].SetActive(false);
                currentSpeaker = dialogue.Speaker;
                portraits[currentSpeaker].SetActive(true);
            }
            StartCoroutine(TypeWords(dialogue.Text));
        }
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

    public void ToggleSettingsMenu()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
        for(int i=0; i<qualityButtons.Length; i++)
        {
            qualityButtons[i].interactable = (i == quality) ? false : true;
        }
        Debug.Log(QualitySettings.GetQualityLevel());
    }
}
