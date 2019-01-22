using UnityEngine;

public class LeaveHouse : MonoBehaviour {
    private bool leftHouse = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player _player = collision.GetComponent<Player>();
        if (_player != null && !leftHouse)
        {
            StoryMaster.sm.LeaveHouse();
            leftHouse = true;
        }
    }
}
