using UnityEngine;
using System.Collections;

public class CameraFollowSimple : MonoBehaviour {

//	public GameObject player;
//	private Vector3 offset;



	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;





	// Use this for initialization
//	void Start () {
//		offset = transform.position;
//	}

	void Update() {
		if (target)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.3f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}

//	void LateUpdate () {
//		transform.position = player.transform.position + offset;
//	}
}
