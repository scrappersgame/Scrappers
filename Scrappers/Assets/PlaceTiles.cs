using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceTiles : MonoBehaviour {

	public RuleTile TileToPlace;
	public Tilemap theTileMap;
	public int placeDistance;
	public int placeDepth;
	public bool isGrounded;

	private Animator m_Anim;            // Reference to the player's animator component.
	private Vector3Int previous;
	private Vector3Int currentCell;
	private Vector3Int placeCell;
	private int groundLevel;

	private void Awake(){
		m_Anim = GetComponent<Animator>();
		currentCell = theTileMap.WorldToCell(transform.position);
		groundLevel = currentCell.y - 1;
		placeCell = new Vector3Int ( currentCell.x, groundLevel, currentCell.z);
		theTileMap.SetTile (placeCell, TileToPlace);
	}
	// do late so that the player has a chance to move in update if necessary
	private void LateUpdate()
	{
		
		isGrounded = m_Anim.GetBool ("Ground");
		// get current grid location
		currentCell = theTileMap.WorldToCell(transform.position);
		// add one in a direction (you'll have to change this to match your directional control)
		currentCell.x += 1;

		// if the position has changed
		if (currentCell != previous) {
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

			// save the new position for next frame
			previous = currentCell;
		}
	}

}
