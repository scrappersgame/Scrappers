using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

    //stay on target
    public Transform target;

    //times per second path updates
    public float updateRate = 2f;

    //caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //Calculated path
    public Path path;

    //The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //max distance from point before AI find next
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    private bool facingRight = false;

    private bool searchingForPlayer = false;

    void Start () {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null){
            if (!searchingForPlayer){
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        // Start a new path to taget, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());

    }

    IEnumerator SearchForPlayer()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult == null){
            GameObject emptyObj = new GameObject("Cool GameObject made from Code");
            emptyObj.transform.position = new Vector3(transform.position.x + Random.Range(-5f, 5f), transform.position.y + Random.Range(-5f, 5f), transform.position.z);
            target = emptyObj.transform;
            yield return new WaitForSeconds(5f);
            Destroy(emptyObj);
        }else{
            searchingForPlayer = false;
            target = searchResult.transform;
            StartCoroutine(UpdatePath());
            yield break;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                StartCoroutine(SearchForPlayer());
            }
            yield break;
        }
        Vector3 _targetPosition = new Vector3(target.position.x, target.position.y + 1, target.position.z);
        seeker.StartPath(transform.position, _targetPosition, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p){
        if(p.error){
            Debug.Log("path error: " + p.error);
        }else{
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        float targetInd = target.position.x - transform.position.x;
        if (targetInd > 0 && !facingRight)
        {
            // ... flip the enemy.
            Flip();
        }
        // Otherwise if the input is moving the enemy left and the enemy is facing right...
        else if (targetInd < 0 && facingRight)
        {
            // ... flip the enemy.
            Flip();
        }
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count){
            if (pathIsEnded)
                return;

            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to the next waypoint

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //move the ai
        if (!GameMaster.gm.paused && !GameMaster.gm.speaking)
        {
            rb.AddForce(dir, fMode);
        }

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance){
            currentWaypoint++;
            return;
        }
    }

    private void Flip()
    {
        // Switch the way the enemy is labelled as facing.
        facingRight = !facingRight;

        // Multiply the enemy's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
