using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameData gameData;
    [SerializeField]
    GameObject critter;
    [SerializeField][Tooltip("Part of the critter to apply force to")]
    Rigidbody critterRB;
    //Collider critterCollider;

    [SerializeField]
    Rigidbody[] ragdollRBs;
    Vector3[] ragdollInitPos;
    Quaternion[] ragdollInitRot;

    private int indicatorDir = 1; //which way the indicator is moving 1=up -1=down
    // Start is called before the first frame update

    [SerializeField]
    AudioClip spitSound;
    [SerializeField]
    AudioClip bounceSound;
    AudioSource audioPlayer;

    [SerializeField]
    GameObject hippoHead;
    Quaternion hippoRot;
    float hippoRotF = 10.0f;

    void Start()
    {
        gameData.IntroDialogueIndex = -1;
        InitializeGameData();
        //critterCollider = critter.GetComponent<Collider>();
        gameData.CritterInitPos = critter.transform.position;
        hippoRot = hippoHead.transform.rotation;
        ragdollInitPos = new Vector3[ragdollRBs.Length];
        ragdollInitRot = new Quaternion[ragdollRBs.Length];
        audioPlayer = GetComponent<AudioSource>();

        for (int i=0; i<ragdollRBs.Length; i++)
        {
            ragdollInitPos[i] = ragdollRBs[i].transform.position;
            ragdollInitRot[i] = ragdollRBs[i].transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData.CurrentPhase == 1) //angle phase
        {
            gameData.AngleValue += indicatorDir * Time.deltaTime;
            hippoHead.transform.Rotate(new Vector3(-indicatorDir * Time.deltaTime * hippoRotF, 0.0f, 0.0f));
            if ((gameData.AngleValue >= gameData.IndicatorMax && indicatorDir > 0) || (gameData.AngleValue <= -gameData.IndicatorMax && indicatorDir < 0))
            {
                indicatorDir *= -1;
            }
        }
        if (gameData.CurrentPhase == 2) //power phase
        {
            gameData.PowerValue += indicatorDir * Time.deltaTime;
            //Debug.Log(gameData.IndicatorValue);
            if((gameData.PowerValue >= gameData.IndicatorMax && indicatorDir > 0) || (gameData.PowerValue <= -gameData.IndicatorMax && indicatorDir < 0))
            {
                indicatorDir *= -1;
            }
        }
        else if(gameData.CurrentPhase == 3) //launching
        {
            gameData.Distance = ragdollRBs[0].transform.position.z - ragdollInitPos[0].z;
            gameData.DistanceTimer += Time.deltaTime;
            if(gameData.DistanceTimer >= gameData.DistanceTimerMax)
            {
                gameData.DistanceTimer -= gameData.DistanceTimerMax;
                if(gameData.Distance - gameData.LastDistance > gameData.StoppedThreshold)
                {
                    //still moving
                    gameData.LastDistance = gameData.Distance;
                }
                else
                {
                    //stopped
                    if(gameData.Distance > gameData.HighScore)
                    {
                        gameData.HighScore = gameData.Distance;
                        gameData.NewHighScore = true;
                    }
                    gameData.CurrentPhase = 4;
                }
            }
        }
    }



    /// <summary>
    /// Called from any (just Fire now) button input on keyboard and controller. Advances intro text or calls next phase.
    /// </summary>
    /// <param name="context"></param>
    public void ButtonInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ButtonHandler();
        }
    }

    public void ButtonHandler()
    {
        switch (gameData.CurrentPhase)
        {
            case 0:
                //intro

                //if text is currently printing, skip to end of it
                if(gameData.IsTextPrinting)
                {
                    gameData.SkipText = true;
                }
                //if text done printing, go to next text if there are any
                else if(gameData.IntroDialogueIndex < gameData.IntroDialogues.Length - 1)
                {
                    gameData.IntroDialogueIndex++;
                }
                //no more texts, next phase
                else
                {
                    gameData.CurrentPhase++;
                }
                break;
            case 1:
                //angle phase
                gameData.CurrentPhase++;
                break;
            case 2:
                //power phase
                LaunchObject();
                break;
            case 3:
                //launching, don't want to advance to next phase until stopped

                break;
            case 4:
                //ended
                ResetGame();
                break;
            default:
                
                break;
        }
    }

    public void LaunchObject()
    {
        //set phase
        gameData.CurrentPhase = 3;
        //apply force to critter
        ToggleRagdoll(true);
        audioPlayer.PlayOneShot(spitSound);
        float angle = 45 - (gameData.AngleValue * 35);
        Vector3 dir = new Vector3(0.0f, Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)).normalized;
        critterRB.AddForce(dir * gameData.LaunchForce * (1 - Mathf.Abs(gameData.PowerValue)), ForceMode.Impulse);
        //activate gravity
        //critterRB.useGravity = true;
    }

    public void ResetGame()
    {
        gameData.CurrentPhase = 1;
        InitializeGameDataPhaseless();
        //critterRB.isKinematic = true;
        ToggleRagdoll(false);
        hippoHead.transform.rotation = hippoRot;
        //critter.transform.position = critterInitPos;
        for (int i = 0; i < ragdollRBs.Length; i++)
        {
            ragdollRBs[i].transform.position = ragdollInitPos[i];
            ragdollRBs[i].transform.rotation = ragdollInitRot[i];
        }
        //critterRB.useGravity = false;
        //critterRB.isKinematic = false;
    }

    private void ToggleRagdoll(bool isRagdoll)
    {
        //critterCollider.enabled = !isRagdoll;
        foreach(Rigidbody boneRB in ragdollRBs)
        {
            boneRB.isKinematic = !isRagdoll;
        }
    }

    /// <summary>
    /// Resets game data other than phase
    /// </summary>
    private void InitializeGameDataPhaseless()
    {
        gameData.PowerValue = 0;
        gameData.AngleValue = 0;
        gameData.Distance = 0;
        gameData.DistanceTimer = 0.0f;
        gameData.LastDistance = 0.0f;
        gameData.SkipText = false;
        gameData.IsTextPrinting = false;
        gameData.IntroDialogueIndex = 0;
        gameData.NewHighScore = false;
    }

    /// <summary>
    /// resets game data and phase (to replay intro)
    /// </summary>
    private void InitializeGameData()
    {
        gameData.CurrentPhase = 0;
        InitializeGameDataPhaseless();
    }
}
