using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameData gameData;
    [SerializeField][Tooltip("The critter to launch")]
    Rigidbody critterRB;
    [SerializeField]
    GameObject cam;
    Vector3 critterInitPos;
    Vector3 cameraOffset;

    private int indicatorDir = 1; //which way the indicator is moving 1=up -1=down
    // Start is called before the first frame update
    
    void Start()
    {
        critterInitPos = critterRB.transform.position;
        cameraOffset = critterInitPos - cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameData.CurrentPhase == 1) //power phase
        {
            gameData.IndicatorValue += indicatorDir * Time.deltaTime;
            Debug.Log(gameData.IndicatorValue);
            if(gameData.IndicatorValue >= gameData.indicatorMax || gameData.IndicatorValue <= -gameData.indicatorMax)
            {
                indicatorDir *= -1;
            }
        }
        else if(gameData.CurrentPhase == 2) //launching
        {
            cam.transform.position = critterRB.transform.position - cameraOffset;
        }
    }

    /// <summary>
    /// called from start button
    /// </summary>
    public void StartGame()
    {
        gameData.CurrentPhase = 1;

    }

    public void LaunchObject()
    {
        //set phase
        gameData.CurrentPhase = 2;
        //apply force to critter
        Vector3 dir = new Vector3(0.0f, 1.0f, 1.0f);
        critterRB.AddForce(dir * gameData.LaunchForce * (1 - Mathf.Abs(gameData.IndicatorValue)), ForceMode.Impulse);
        //activate gravity
        critterRB.useGravity = true;
    }

    public void ResetGame()
    {
        gameData.CurrentPhase = 0;
        gameData.IndicatorValue = 0;
        critterRB.isKinematic = true;
        critterRB.transform.position = critterInitPos;
        cam.transform.position = critterRB.transform.position - cameraOffset;
        critterRB.useGravity = false;
        critterRB.isKinematic = false;
    }
}
