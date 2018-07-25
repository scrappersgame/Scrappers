using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle : MonoBehaviour {
    public float minimum = 0.2f;
    public float maximum = 1f;
    public float duration = .5f;
    public int direction = 1;
    private float startTime;
    public SpriteRenderer sprite;
    void Start()
    {
        startTime = Time.time;
        sprite = transform.gameObject.GetComponent<SpriteRenderer>();
    }
	// Update is called once per frame
	void Update () {
        Vector3 curPos = transform.position;
        Quaternion curRot = transform.rotation;
        transform.rotation = new Quaternion(curRot.x,curRot.y,curRot.z+.0001f*direction,curRot.w);
        if (duration < (Time.time - startTime)){
            startTime = Time.time;
            float _maximum = minimum;
            float _minimum = maximum;
            minimum = _minimum;
            maximum = _maximum;
        }
        float t = (Time.time - startTime) / duration;
        sprite.color = new Color(1f,1f,1f,Mathf.SmoothStep(minimum, maximum, t));      
		
	}
}
