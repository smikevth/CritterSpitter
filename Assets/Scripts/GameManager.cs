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
    
    void Start()
    {
        InitializeGameData();
        //critterCollider = critter.GetComponent<Collider>();
        gameData.CritterInitPos = critter.transform.position;
        ragdollInitPos = new Vector3[ragdollRBs.Length];
        ragdollInitRot = new Quaternion[ragdollRBs.Length];


        for (int i=0; i<ragdollRBs.Length; i++)
        {
            ragdollInitPos[i] = ragdollRBs[i].transform.position;
            ragdollInitRot[i] = ragdollRBs[i].transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameData.CurrentPhase == 1) //power phase
        {
            gameData.IndicatorValue += indicatorDir * Time.deltaTime;
            //Debug.Log(gameData.IndicatorValue);
            if((gameData.IndicatorValue >= gameData.IndicatorMax && indicatorDir > 0) || (gameData.IndicatorValue <= -gameData.IndicatorMax && indicatorDir < 0))
            {
                indicatorDir *= -1;
            }
        }
        else if(gameData.CurrentPhase == 2) //launching
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
                    gameData.CurrentPhase = 3;
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
            Debug.Log("pushed");
            ButtonHandler();
        }
    }

    public void ButtonHandler()
    {
        switch (gameData.CurrentPhase)
        {
            case 0:
                //intro

                //if intro is still going, advance text

                //at end, next phase
                gameData.CurrentPhase++;
                break;
            case 1:
                //power phase
                LaunchObject();
                break;
            case 2:
                //launching, don't want to advance to next phase until stopped

                break;
            case 3:
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
        gameData.CurrentPhase = 2;
        //apply force to critter
        ToggleRagdoll(true);
        Vector3 dir = new Vector3(0.0f, 1.0f, 1.0f);
        critterRB.AddForce(dir * gameData.LaunchForce * (1 - Mathf.Abs(gameData.IndicatorValue)), ForceMode.Impulse);
        //activate gravity
        //critterRB.useGravity = true;
    }

    public void ResetGame()
    {
        gameData.CurrentPhase = 1;
        InitializeGameDataPhaseless();
        //critterRB.isKinematic = true;
        ToggleRagdoll(false);
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
        gameData.IndicatorValue = 0;
        gameData.Distance = 0;
        gameData.DistanceTimer = 0.0f;
        gameData.LastDistance = 0.0f;
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
