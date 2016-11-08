using UnityEngine;
using System.Collections;

public class playerCrouchController : MonoBehaviour {

	public Vector3 jump;
	public float jumpForce = 2.0f;
	public float distance = 2.0f;

	public GameObject playerNormal;

	public bool isGrounded;
	Rigidbody rb;

	void Awake () {
		playerNormal = GameObject.FindWithTag("PlayerNormal");
	}

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		jump = new Vector3(0.0f, 10.0f, 0.0f);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){

			rb.AddForce(jump * jumpForce, ForceMode.Impulse);

			isGrounded = false;
		}

		if(Input.GetKeyDown(KeyCode.D)) {
			playerNormal.SetActive(true);
			gameObject.SetActive(false);
		}

		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(Vector3.up * distance);
		}
		else if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(Vector3.down * distance);
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "Coin")
		{
			Destroy(col.gameObject);
		}
	}

}