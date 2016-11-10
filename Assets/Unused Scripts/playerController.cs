using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	public Vector3 jump;
	public float jumpForce = 2.0f;
	public float distance = 2.0f;

	public GameObject playerCrouch;

	public bool isGrounded;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		playerCrouch = GameObject.FindWithTag("PlayerCrouch");
		playerCrouch.SetActive(false);
		rb = GetComponent<Rigidbody2D>();
		jump = new Vector3(0.0f, 10.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){

			rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);

			isGrounded = false;
		}

		if(Input.GetKeyDown(KeyCode.A)) {
			playerCrouch.SetActive(true);
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