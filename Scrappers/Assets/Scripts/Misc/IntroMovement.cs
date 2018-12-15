using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMovement : MonoBehaviour {
    public AudioClip shipClip;
    public AudioClip happyClip;
    public AudioClip sadClip;
    public AudioClip spaceClip;
    public float moveSpeed = 4f;                    // how fast are we going?
    private Vector3 startPoint;                     // where did we start?
    private AudioSourceCrossfade _musicSource;      // where is that music coming from?
    private bool shipPlayed = false;                // has the music changed?
    private bool happyPlayed = false;               // has the music changed to be happy?
    private bool sadPlayed = false;                 // has the music changed to be sad?
    private bool spacePlayed = false;               // has the music changed to be spacey?
    void Awake(){
        // where we started
        startPoint = transform.position;
        _musicSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSourceCrossfade>();
    }
    void Update(){
        Vector3 curPos = transform.position;
        if (curPos.y >= 23 && !shipPlayed && curPos.y < 50)
        {
            // Lets change the track to something shippy...
            ChangeTrack(shipClip);
            shipPlayed = true;
        }
        if (curPos.y >= 79.5 && !happyPlayed)
        {
            // Lets change the track to something happy...
            ChangeTrack(happyClip);
            happyPlayed = true;
        }
        if (curPos.y >= 119 && !spacePlayed)
        {
            // Lets change the track to something spacey...
            ChangeTrack(spaceClip);
            spacePlayed = true;
        }
        if (curPos.y >= 165 && !sadPlayed)
        {
            // Lets change the track to something sad-y...
            ChangeTrack(sadClip);
            sadPlayed = true;
            shipPlayed = false;
        }
        if (curPos.y >= 232 && !shipPlayed)
        {
            // Lets change the track back to shippy...
            ChangeTrack(shipClip);
            shipPlayed = true;
        }
        // Going up?
        if (curPos.x < startPoint.x + 10) // Take 10 steps to the right
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        } else if (curPos.x < startPoint.x + 20) // then 10 to the up and 10 more to the right
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        } else {
            if (moveSpeed > .1f) // Keep on keeping on
            {
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
                if (curPos.y > 507){
                    moveSpeed = Mathf.Lerp(moveSpeed, 0, .01f); // slow it down baybee
                }
            }
        }

	}
    void ChangeTrack(AudioClip track)
    {
        if (track != null)
        {
            _musicSource.volume = GameMaster.gm.masterVolume;
            _musicSource.Play(track);
        }
    }
}


