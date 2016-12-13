using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraFollowSimple : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	private float distanceMeter;
	private Text DistanceMeter;

	public float zoomSpeed = 1.0f;
	public float zoomAmount;
	public float targetOrtho;
	public float smoothSpeed = 2.0f;
	public float minOrtho = 1.0f;
	public float maxOrtho = 20.0f;

	public float shakeTimer;
	public float shakeAmount;

	public PlungerScript plungerScript;

	Scene currentScene;


	// Use this for initialization
	void Start () {
		targetOrtho = Camera.main.orthographicSize;
		plungerScript = GameObject.FindWithTag("Platform").GetComponent<PlungerScript>();
		currentScene = SceneManager.GetActiveScene();

		if(currentScene.name == "Closer Camera") {
			zoomAmount = 12.0f;
		}
		else {
			zoomAmount = 16.0f;
		}

		DistanceMeter = GameObject.FindWithTag("DistanceMeter").GetComponent<Text>();
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

			if(currentScene.name == "Screenshake") {
				Vector2 shakePos = Random.insideUnitCircle * 0.08f;
				transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);
				shakeTimer -= Time.deltaTime;
			}

			if(DistanceMeter) {
				distanceMeter += 20 * Time.deltaTime;
				DistanceMeter.text = "" + distanceMeter.ToString("f0") + "m";
			}
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
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

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Application.LoadLevel(2);
			Cursor.visible = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Application.LoadLevel(3);
			Cursor.visible = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			Application.LoadLevel(4);
			Cursor.visible = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			Application.LoadLevel(5);
			Cursor.visible = true;
		}

		if (Input.GetKeyDown(KeyCode.Alpha7)) {
			Application.LoadLevel(6);
			Cursor.visible = true;
		}

		if(shakeTimer >= 0) {
			Vector2 shakePos = Random.insideUnitCircle * shakeAmount;
			transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);
			shakeTimer -= Time.deltaTime;
		}
	}

	void ZoomOut() {
		targetOrtho = zoomAmount * zoomSpeed;
		//targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);

		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);

		//plungerScript.platformLaunched = false;
	}

	public void ShakeCamera(float shakePwr, float shakeDur) {
		shakeAmount = shakePwr;
		shakeTimer = shakeDur;
	}
}
