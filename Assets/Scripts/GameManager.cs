using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameData gameData;
    [SerializeField][Tooltip("The critter to launch")]
    Rigidbody critterRB;

    Vector3 critterInitPos;


    // Start is called before the first frame update
    void Start()
    {
        critterInitPos = critterRB.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        critterRB.AddForce(dir * gameData.LaunchForce, ForceMode.Impulse);
        //activate gravity
        critterRB.useGravity = true;
    }

    public void ResetGame()
    {
        gameData.CurrentPhase = 0;
        critterRB.isKinematic = true;
        critterRB.transform.position = critterInitPos;
        critterRB.useGravity = false;
        critterRB.isKinematic = false;
    }
}
