using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameData gameData;
    Vector3 cameraOffset;

    [SerializeField]
    GameObject critter;

    [SerializeField]
    [Tooltip("Where the camera moves to during the intro and will be at in angle phase")]
    Vector3 anglePos;
    Quaternion angleRot = new Quaternion(); //the rotation of the camera in angle phase
    [SerializeField]
    [Tooltip("How much the camera rotates from intro to power phase")]
    Vector3 launchRot;
    [SerializeField][Tooltip("Where the camera moves to for the power phase")]
    Vector3 launchPos;
    [SerializeField]
    float panSpeed = 1.0f;

    bool isRotated = false;

    // Start is called before the first frame update
    void Start()
    {
        gameData.OnPhaseChange.AddListener(AdjustForPhase);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameData.CurrentPhase == 0) //intro
        {
            float distance = transform.position.z - anglePos.z;
            if (distance > 0.0f)
            {
                transform.position += Vector3.back * panSpeed * Time.deltaTime;
            }
        }
        else  if(gameData.CurrentPhase == 3) //launching
        {
            transform.position = critter.transform.position - cameraOffset;
        }
    }

    void AdjustForPhase()
    {
        switch(gameData.CurrentPhase)
        {
            case 0:
                //intro

                break;
            case 1:
                //angle
                transform.position = anglePos;
                if (isRotated)
                {
                    transform.rotation = angleRot;
                    isRotated = false;
                }
                else
                {
                    angleRot = transform.rotation;
                }
                break;
            case 2:
                //power
                transform.position = launchPos;
                if(!isRotated)
                {
                    transform.Rotate(launchRot);
                    cameraOffset = critter.transform.position - transform.position;
                    isRotated = true;
                }
                break;
            case 3:
                //launching
  
                break;
            case 4:
                //end

                break;
            default:

                break;
        }
    }
}
