using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMovement : MonoBehaviour {
    public AudioClip spaceClip;
    public float moveSpeed = 4f;                    // how fast are we going?
    private Vector3 startPoint;                     // where did we start?
    private AudioSourceCrossfade _musicSource;      // where is that music coming from?
    private bool spacePlayed = false;               // has the music changed?
	void Awake(){
        // where we started
        startPoint = transform.position;
        _musicSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSourceCrossfade>();
    }
    void Update(){
        Vector3 curPos = transform.position;
        if(curPos.y >= 23 && !spacePlayed){ 
            // Lets change the track...
            if (spaceClip != null)
            {
                _musicSource.volume = GameMaster.gm.masterVolume;
                _musicSource.Play(spaceClip);
            }
            spacePlayed = true;
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
}
