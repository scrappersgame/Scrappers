using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;
using Pathfinding.Serialization;
using Pathfinding.Util;

public class PlaceTiles : MonoBehaviour {

	public RuleTile TileToPlace;
    public Tilemap theTileMap;
    private GridGraph gg;
	public float placeDistance;
	public float placeDepth;
	public bool isGrounded;

	private Vector3Int previous;
	private Vector3Int currentCell;
	private Vector3Int placeCell;
	private int groundLevel;
	private Camera cam;

    private float camHorizontalExtend;
    private float camVerticalExtend;

 	private void Awake(){
        
		cam = Camera.main;
        camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
        camVerticalExtend = cam.orthographicSize * Screen.height / Screen.width;
        theTileMap = GameObject.FindGameObjectWithTag("TileMap").GetComponent<Tilemap>();
        gg = AstarData.active.data.gridGraph;
        currentCell = theTileMap.WorldToCell(transform.position);
		groundLevel = currentCell.y - 1;
		placeCell = new Vector3Int ( currentCell.x, groundLevel, currentCell.z);
		theTileMap.SetTile (placeCell, TileToPlace);
	}


	// do late so that the player has a chance to move in update if necessary
	private void LateUpdate()
	{
        placeDistance = camHorizontalExtend + 5;
        placeDepth = camVerticalExtend + 5;
		if (theTileMap == null) {
			theTileMap = GameObject.FindGameObjectWithTag ("TileMap").GetComponent<Tilemap>();
		}

		// get current grid location
		currentCell = theTileMap.WorldToCell(transform.position);
		// add one in a direction (you'll have to change this to match your directional control)
		currentCell.x += 1;

		// if the position has changed
		if (currentCell != previous) {
			// spawn the floor
			SpawnFloor ();
			// save the new position for next frame
			previous = currentCell;
		}
	}
	public void SpawnFloor() {
		//place tiles to the right
		for (int i = 0; i < placeDistance; i++){
			for (int ii = 0; ii < placeDepth; ii++) {
				placeCell = new Vector3Int (currentCell.x + i, groundLevel - ii, currentCell.z);

				// set the new tile
				Sprite currentTile = theTileMap.GetSprite(placeCell);
				if (currentTile == null) {
					theTileMap.SetTile (placeCell, TileToPlace);
				}
			}
		}
		//place tiles to the left
		for (int i = 0; i > 0-placeDistance; i--){
			for (int ii = 0; ii < placeDepth; ii++) {
				placeCell = new Vector3Int (currentCell.x + i, groundLevel - ii, currentCell.z);

				// set the new tile
				Sprite currentTile = theTileMap.GetSprite (placeCell);
				if (currentTile == null) {
					theTileMap.SetTile (placeCell, TileToPlace);
				}
			}
		}
        int newY = 10;
        if (transform.position.y > 10){
            newY = Mathf.RoundToInt(transform.position.y);
        }
        gg.center = new Vector3(Mathf.RoundToInt(transform.position.x), newY, Mathf.RoundToInt(transform.position.z));
        AstarPath.active.Scan();
    }
}
