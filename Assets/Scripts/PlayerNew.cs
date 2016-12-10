using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class PlayerNew : MonoBehaviour {

	private TrailRenderer trailRenderer;
	//public Transform particleSystem;

	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	bool crouch = false;
	//public FreeParallax parallax;
	public PlungerScript plungerScript;
	private CameraFollowSimple cameraFollowScript;
	public GameObject PlayerRemains;

	public BoxCollider2D boxCollider;

	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

	/*
		if (GameObject.Find("name of the gameobject holding the script with the bool").GetComponent<name of the script holding the bool>().IsLightOn);
		 -----------------
		Input.GetMouseButton (0) && Input.mousePosition.x < Screen.width / 2);
		-----------------
		if(Input.touchCount > 0){
		    Vector3 position = Input.touches[0].position;
		    float rotation = (position.x < (Screen.width / 2)) ? 45f : -45f;
		    transform.Rotate(0,0,rotation);
 		}
	*/

	Controller2D controller;

	void Start() {
		trailRenderer = GameObject.FindWithTag("TrailRenderer").GetComponent<TrailRenderer>();
		//trailRenderer.sortingLayerName = "Player";
		//parallax = GameObject.FindWithTag("Parallax").GetComponent<FreeParallax>();
		//particleSystem.GetComponent<ParticleSystem>().enableEmission = false;
		controller = GetComponent<Controller2D> ();
		plungerScript = GameObject.FindWithTag("Platform").GetComponent<PlungerScript>();
		cameraFollowScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollowSimple>();
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		print ("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);

		boxCollider = GetComponent<BoxCollider2D>();
	}

	void Update() {

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		// Touch controls.
		if(plungerScript.platformLaunched == true) {

			// New touch controls (not working properly. Player ducks everytime he jumps).

//			foreach(Touch touch in Input.touches) {
//				
//				Vector3 position = Input.touches[0].position;
//
//				if(touch.phase == TouchPhase.Began) {
//					// save began touch 2d point
//					firstPressPos = new Vector2(touch.position.x,touch.position.y);
//
//					//if ((touch.phase == TouchPhase.Stationary) || (touch.phase == TouchPhase.Moved && touch.deltaPosition.magnitude < 5)) {
//					if (touch.phase == TouchPhase.Stationary) {
//						if(crouch == false) {
//							transform.localScale += new Vector3(0f, -0.5f, 0);
//							transform.position += new Vector3(0, -0.25f, 0);
//							trailRenderer.startWidth = 0.5f;
//							crouch = true;
//						}
//					}
//				}
//				
//				if(touch.phase == TouchPhase.Ended) {
//					if(crouch == true) {
//						transform.localScale += new Vector3(0, 0.5f, 0);
//						transform.position += new Vector3(0, 0.25f,0);
//						trailRenderer.startWidth = 1.0f;
//						crouch = false;
//					}
//
//					//save ended touch 2d point
//					secondPressPos = new Vector2(touch.position.x,touch.position.y);
//
//					//create vector from the two points
//					currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
//
//					//normalize the 2d vector
//					currentSwipe.Normalize();
//
//					//swipe upwards
//				if((currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) && controller.collisions.below)
//					{
//						velocity.y = jumpVelocity;
//					}
//				}
//			}


			// Old touch controls with holding left side of screen for crouch and tapping right side for jump.

			foreach(Touch touch in Input.touches) {

				Vector3 position = Input.touches[0].position;

				if(touch.phase == TouchPhase.Began && position.x < (Screen.width / 2)) {
					if(crouch == false) {
						transform.localScale += new Vector3(0f, -0.5f, 0);
						transform.position += new Vector3(0, -0.25f, 0);
						crouch = true;
					}
				}
				else if(touch.phase == TouchPhase.Ended && position.x < (Screen.width / 2)) {
					if(crouch == true) {
						transform.localScale += new Vector3(0, 0.5f, 0);
						transform.position += new Vector3(0, 0.25f,0);
						crouch = false;
					}
				}


				if((touch.phase == TouchPhase.Began && controller.collisions.below) && position.x > (Screen.width / 2)) {
					velocity.y = jumpVelocity;
				}
			}
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

		if(Input.GetKeyDown(KeyCode.S) && crouch == false) {
			transform.localScale += new Vector3(0f, -0.5f, 0);
			transform.position += new Vector3(0, -0.25f, 0);
			trailRenderer.startWidth = 0.5f;
			crouch = true;
		}
		else if (Input.GetKeyDown(KeyCode.S) && crouch == true) {
			transform.localScale += new Vector3(0, 0.5f, 0);
			transform.position += new Vector3(0, 0.25f,0);
			trailRenderer.startWidth = 1.0f;
			crouch = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Coin")
		{
			//particleSystem.GetComponent<ParticleSystem>().enableEmission = true;
			Destroy(col.gameObject);
			//StartCoroutine(StopParticles());
		}

//		else if (col.gameObject.tag == "Obstacle")
//		{
////			Application.LoadLevel(0);
////			Cursor.visible = true;
////			PlayerRemains = (GameObject)Instantiate(PlayerRemains);
////			Instantiate(PlayerRemains, transform.position, transform.rotation);
//			PlayerRemains.SetActive(true);
//			Destroy (gameObject);
//		}
	}

//	IEnumerator StopParticles() {
//		yield return new WaitForSeconds(0.1f);
//		particleSystem.GetComponent<ParticleSystem>().enableEmission = false;
//	}

//	IEnumerator Example() {
//		yield return new WaitForSeconds(4);
//	}

	void OnCollisionEnter2D(Collision2D  collision) {
		Collider2D collider = collision.collider;

		if(collider.tag == "Obstacle") {
			PlayerRemains.transform.SetParent(null);
			Destroy (gameObject);
			PlayerRemains.SetActive(true);
			PlayerRemains.GetComponentInChildren<Rigidbody2D>().velocity = new Vector3(-50, 50, 0);
			cameraFollowScript.ShakeCamera(0.4f, 0.3f);
			plungerScript.platformLaunched = false;
//			StartCoroutine(Example());
//			Application.LoadLevel(0);
		}
	}


//	void OnCollisionEnter2D(Collision2D  collision) {
//
//		Collider2D collider = collision.collider;
//		bool collideFromLeft;
//		bool collideFromTop;
//		bool collideFromRight;
//		bool collideFromBottom;
//		float RectWidth = this.GetComponent<BoxCollider2D> ().bounds.size.x;
//		float RectHeight = this.GetComponent<BoxCollider2D> ().bounds.size.y;
//		float circleRad = collider.bounds.size.x;
//
//		if(collider.tag == "Walking Platform") {
//
//			Vector3 contactPoint = collision.contacts[0].point;
//			Vector3 center = collider.bounds.center;
//
//			if (contactPoint.x < center.x && (contactPoint.y < center.y + RectHeight / 2 && contactPoint.y > center.y - RectHeight / 2)) {
//				//collideFromLeft = true;
//				Application.LoadLevel(0);
//			}
//		} 
//	}
		
}