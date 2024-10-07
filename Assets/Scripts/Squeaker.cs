using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Squeaker : MonoBehaviour
{
    //it squeaks when you bang it

    [SerializeField]
    AudioSource sfxPlayer;
    [SerializeField]
    AudioClip squeakClip;
    //[SerializeField]
    //GameData gameData;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            sfxPlayer.PlayOneShot(squeakClip);
        }
    }
}
