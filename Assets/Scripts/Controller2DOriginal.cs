using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Controller2DOriginal : MonoBehaviour {

	public LayerMask collisionMask;

	const float skinWidth = .015f;
	// How many rays are being fires horizontally and vertically.
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	// To calculate the spacing between each horizontal and each vertical ray.
	float horizonrtalRaySpacing;
	float verticalRaySpacing;

	[HideInInspector]
	public BoxCollider2D collider;

	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	public virtual void Awake () {
		collider = GetComponent<BoxCollider2D>();
	}

	public virtual void Start() {
		CalculateRaySpacing();
	}

	public void Move(Vector3 velocity) {
		UpdateRaycastOrigins();

		collisions.Reset();

		if (velocity.x != 0) {
			HorizontalCollisions(ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions(ref velocity);
		}

		transform.Translate(velocity);
	}

	void HorizontalCollisions(ref Vector3 velocity) {
		// Get the direction of our y-velocity
		float directionX = Mathf.Sign(velocity.x);

		// Float for the length of our ray.
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++) {
			// If we're moving left, we want our ray to start in the bottom-left corner and if we're moving right we want our ray to start in the bottom-right corner.
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

			rayOrigin += Vector2.up * (horizonrtalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
				// If the ray hit something, then we want to set our y-velocity equal to the amount that we have to move to get from our current position...
				// ...to the point of which the ray intersected with an obstacle. Essentially the ray distance.
				velocity.x =  (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				// If we've hit something and we're going left, then "collisions.left = true" and the vice versa.
				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity) {
		// Get the direction of our y-velocity
		float directionY = Mathf.Sign(velocity.y);

		// Float for the length of our ray.
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			// If we're moving down, we want our ray to start in the bottom-left corner and if we're moving up we want our ray to start in the top-left corner.
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

			rayOrigin += Vector2.right * (verticalRaySpacing * i * velocity.x);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit) {
				// If the ray hit something, then we want to set our y-velocity equal to the amount that we have to move to get from our current position...
				// ...to the point of which the ray intersected with an obstacle. Essentially the ray distance.
				velocity.y =  (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		// To shrink it in on all sides.
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		// To shrink it in on all sides.
		bounds.Expand(skinWidth * -2);

		// To make sure that "horizontalRayCount" and "verticalRayCount" are both >= 2.
		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

		// To calculate the spacing between each ray.
		horizonrtalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	// To store the positions of the ray casts.
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	// To know where collisions are occurring
	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public void Reset() {
			above = below = false;
			left = right = false;
		}
	}

}
