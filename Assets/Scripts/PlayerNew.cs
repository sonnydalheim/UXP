using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class PlayerNew : MonoBehaviour {

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
	public PlungerScript plungerScript;
	public GameObject PlayerRemains;

	public BoxCollider2D boxCollider;

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
		controller = GetComponent<Controller2D> ();
		plungerScript = GameObject.FindWithTag("Platform").GetComponent<PlungerScript>();
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
					transform.localScale += new Vector3(0, 0.5f, 0);
					transform.position += new Vector3(0, 0.25f,0);
					crouch = false;
				}

				if((touch.phase == TouchPhase.Began && controller.collisions.below) && position.x > (Screen.width / 2)) {
					velocity.y = jumpVelocity;
				}
			}
		}
//		else {
//			boxCollider.size = new Vector3(2.0f, 2.0f, 2.0f);
//		}

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
			crouch = true;
		}
		else if (Input.GetKeyDown(KeyCode.S) && crouch == true) {
			transform.localScale += new Vector3(0, 0.5f, 0);
			transform.position += new Vector3(0, 0.25f,0);
			crouch = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Coin")
		{
			Destroy(col.gameObject);
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