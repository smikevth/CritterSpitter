using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    GameObject cam;
    Vector3 critterInitPos;
    Vector3 cameraOffset;

    [SerializeField]
    Rigidbody[] ragdollRBs;
    Vector3[] ragdollInitPos;
    Quaternion[] ragdollInitRot;

    private int indicatorDir = 1; //which way the indicator is moving 1=up -1=down
    // Start is called before the first frame update
    
    void Start()
    {
        //critterCollider = critter.GetComponent<Collider>();
        critterInitPos = critter.transform.position;
        cameraOffset = critterInitPos - cam.transform.position;
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
            if(gameData.IndicatorValue >= gameData.IndicatorMax || gameData.IndicatorValue <= -gameData.IndicatorMax)
            {
                indicatorDir *= -1;
            }
        }
        else if(gameData.CurrentPhase == 2) //launching
        {
            cam.transform.position = critterRB.position - cameraOffset;
            gameData.Distance = ragdollRBs[0].transform.position.z - ragdollInitPos[0].z;
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
        ToggleRagdoll(true);
        Vector3 dir = new Vector3(0.0f, 1.0f, 1.0f);
        critterRB.AddForce(dir * gameData.LaunchForce * (1 - Mathf.Abs(gameData.IndicatorValue)), ForceMode.Impulse);
        //activate gravity
        critterRB.useGravity = true;
    }

    public void ResetGame()
    {
        gameData.CurrentPhase = 0;
        gameData.IndicatorValue = 0;
        gameData.Distance = 0;
        //critterRB.isKinematic = true;
        ToggleRagdoll(false);
        //critter.transform.position = critterInitPos;
        Debug.Log(ragdollInitPos);
        for (int i = 0; i < ragdollRBs.Length; i++)
        {
            ragdollRBs[i].transform.position = ragdollInitPos[i];
            ragdollRBs[i].transform.rotation = ragdollInitRot[i];
        }
        cam.transform.position = critter.transform.position - cameraOffset;
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
}
