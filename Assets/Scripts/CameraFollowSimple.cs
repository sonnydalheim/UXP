using UnityEngine;
using System.Collections;

public class CameraFollowSimple : MonoBehaviour {

//	public GameObject player;
//	private Vector3 offset;



	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	public float zoomSpeed = 1.0f;
	public float zoomAmount = 16.0f;
	public float targetOrtho;
	public float smoothSpeed = 2.0f;
	public float minOrtho = 1.0f;
	public float maxOrtho = 20.0f;

	public PlungerScript plungerScript;



	// Use this for initialization
	void Start () {
		targetOrtho = Camera.main.orthographicSize;
		plungerScript = GameObject.FindWithTag("Platform").GetComponent<PlungerScript>();
//		offset = transform.position;
	}

	void Update() {
		if (target) {
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.3f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

		if (plungerScript.platformLaunched == true) {
			ZoomOut();
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(1);
			Cursor.visible = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Application.LoadLevel(0);
			Cursor.visible = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Application.LoadLevel(1);
			Cursor.visible = true;
		}
	}

	void ZoomOut() {
		targetOrtho = zoomAmount * zoomSpeed;
		//targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);

		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);

		//plungerScript.platformLaunched = false;
	}

	public void ClickButtonOne() {
		Application.LoadLevel(0);
	}

	public void ClickButtonTwo() {
		Application.LoadLevel(1);
	}

//	void LateUpdate () {
//		transform.position = player.transform.position + offset;
//	}
}
