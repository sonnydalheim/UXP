using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

//	public GameObject player;
//	private Vector3 offset;
//
//	// Use this for initialization
//	void Start () {
//		offset = transform.position - player.transform.position;
//	}
//	
//	// Update is called once per frame
//	void LateUpdate () {
//		transform.position = player.transform.position + offset;
//	}


	public Controller2D target;
	public float verticalOffset;
	public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;

	// Size of the focus area around the player
	public Vector2 focusAreaSize;

	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	void Start() {
		focusArea = new FocusArea(target.GetComponent<BoxCollider2D>().bounds, focusAreaSize);
	}

	void LateUpdate() {
		focusArea.Update(target.GetComponent<BoxCollider2D>().bounds);

		Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

		if (focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
		}

		targetLookAheadX = lookAheadDirX * lookAheadDstX;
		currentLookAheadX =  Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

		focusPosition += Vector2.right * currentLookAheadX;

		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, .5f);
		Gizmos.DrawCube(focusArea.centre, focusAreaSize);
	}
		
	struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;

		float left, right;
		float top, bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2((left + right) / 2, (top + bottom) / 2);
		}

		// Updating the focus area position when the player moves against one of the edges.
		public void Update(Bounds targetBounds) {
			// Check if the player is moving against the left or right edge.
			float shiftX = 0;
			if (targetBounds.min.x  < left) {
				shiftX = targetBounds.min.x - left;
			}
			else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			// Check if the player is moving against the top or bottom edge.
			float shiftY = 0;
			if (targetBounds.min.y  < bottom) {
				shiftY = targetBounds.min.y - bottom;
			}
			else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;

			centre = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2(shiftX, shiftY);
		}
	}
}
