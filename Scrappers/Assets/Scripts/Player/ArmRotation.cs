using System.Collections;
using UnityEngine;

public class ArmRotation : MonoBehaviour {
	public int RotationOffset = 0;
    private Quaternion UpRotation;
    private Quaternion DownRotation;
    private Quaternion OriginalRotation;
    private bool Swinging = false;
    private float xDifference;
    private float SwingStartTime;
	// Update is called once per frame
	void Update () {
        if (Time.timeScale > 0)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xDifference = mouseWorldPosition.x - transform.parent.position.x;
            if (xDifference < 0)
            {
                RotationOffset = 180;
            }
            else
            {
                RotationOffset = 0;
            }
            if (Swinging){
                if (Time.time < SwingStartTime + .1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, UpRotation, .7f);
                }
                else if (Time.time < SwingStartTime + .2f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, DownRotation, .7f);
                }
                else if (Time.time < SwingStartTime + .25f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, OriginalRotation, .7f);
                }
                else
                {
                    Swinging = false;
                }
            }else{
        		// caculate difference between the mouse and the arm
        		Vector3 difference = mouseWorldPosition - transform.position;
        		difference.Normalize (); // make sure the difference points add up to 1

        		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg; // find the angle in degrees
        		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + RotationOffset); // rotate the arm
            }
        }
	}
    public void SwingArm(){
        if (!Swinging)
        {
            OriginalRotation = transform.rotation;
            UpRotation = transform.rotation * Quaternion.Euler(0, 0, 45 * Mathf.Sign(xDifference));
            DownRotation = transform.rotation * Quaternion.Euler(0, 0, -45 * Mathf.Sign(xDifference));
            SwingStartTime = Time.time;
            Swinging = true;
        }
    }
}
