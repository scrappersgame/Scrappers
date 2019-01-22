using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDrones : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player _player = collision.GetComponent<Player>();
        if (_player != null && !triggered)
        {
            StoryMaster.sm.TriggerDrones();
            triggered = true;
        }
    }
}
