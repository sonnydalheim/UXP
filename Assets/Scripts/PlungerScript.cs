using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlungerScript : MonoBehaviour {
	private Vector3 offset;

	public FreeParallax parallax;
	public FreeParallax parallaxParticles;
	public float maxStretch = 3.0f;
	public LineRenderer slingBand;
//	public LineRenderer frontSling;
//	public LineRenderer backSling;

	private SpringJoint2D spring;
	private Transform pipe;
	private bool clickedOn;
	[HideInInspector]
	public bool platformLaunched;
	private Ray rayToMouse;
	private Ray pipeToPlatformRay;
	private float maxStretchSqr;
	private float circleRadius;
	private Vector2 prevVelocity;

	void Awake () {
		spring = GetComponent<SpringJoint2D>();
		pipe = spring.connectedBody.transform;
	}

	// Use this for initialization
	void Start () {
		parallax = GameObject.FindWithTag("Parallax").GetComponent<FreeParallax>();
		parallaxParticles = GameObject.FindWithTag("Parallax Particles").GetComponent<FreeParallax>();
		// Set the platform position to where it's tied to on the pipe.
		transform.position = slingBand.transform.position;
		LineRendererSetup();
		rayToMouse = new Ray(pipe.position, Vector3.zero);
		pipeToPlatformRay = new Ray(slingBand.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
		circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {

//		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		Vector2 pipeToMouse = mouseWorldPoint - pipe.position;

		if (clickedOn) {
			Dragging ();
		}

		if (spring != null) {
			if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude) {
				Destroy(spring);
				GetComponent<Rigidbody2D>().velocity = prevVelocity;
				Cursor.visible = false;
				platformLaunched = true;
			}

			if (!clickedOn) {
				prevVelocity = GetComponent<Rigidbody2D>().velocity;
			}

			//LineRendererUpdate();
		}
		else {
			slingBand.enabled = false;
		}

		if (parallax != null) {
			if (platformLaunched == true)
			{
				parallax.Speed = -8.0f;
			}
			else
			{
				parallax.Speed = 0.0f;
			}
		}

		if (parallaxParticles != null) {
			if (platformLaunched == true)
			{
				parallaxParticles.Speed = -8.0f;
			}
			else
			{
				parallaxParticles.Speed = -1.0f;
			}
		}
	}

	void LineRendererSetup () {
		slingBand.SetPosition(0, slingBand.transform.position);
//		frontSling.SetPosition(0, frontSling.transform.position);
//		backSling.SetPosition(0, backSling.transform.position);
//		frontSling.sortingOrder = 3;
//		backSling.sortingOrder = 1;
	}

	void OnMouseDown () {
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 pipeToMouse = mouseWorldPoint - pipe.position;

		if(pipeToMouse.sqrMagnitude >= maxStretchSqr) {
			spring.enabled = true;
			GetComponent<Rigidbody2D>().isKinematic = false;
			clickedOn = false;	
		}
	}

	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 pipeToMouse = mouseWorldPoint - pipe.position;

		if(pipeToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = pipeToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}
			
		mouseWorldPoint.z = 0f;

		// Constrain the platform movement to the x-axis.
		mouseWorldPoint.y = 0f;

		// The if-statement is to prevent player from dragging the platform to the right.
		if(mouseWorldPoint.x <= transform.position.x) {
			transform.position = mouseWorldPoint;
		}
	}

//	void LineRendererUpdate() {
//		Vector2 pipeToPlatform = transform.position - slingBand.transform.position;
//		pipeToPlatformRay.direction = pipeToPlatform;
//		Vector3 holdPoint = pipeToPlatformRay.GetPoint(pipeToPlatform.magnitude + circleRadius);
//		slingBand.SetPosition(1, holdPoint);
//	}
}
