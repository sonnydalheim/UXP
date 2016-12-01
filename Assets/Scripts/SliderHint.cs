using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderHint : MonoBehaviour {


	public PlungerScript plungerScript;
	public Image SliderHintImage;
	Color sliderHintColor;

	bool transparent;

	// Use this for initialization
	void Start () {
		sliderHintColor = SliderHintImage.color;
		plungerScript = GameObject.FindWithTag("Platform").GetComponent<PlungerScript>();
	}

	IEnumerator Fade() {
		if(plungerScript.platformLaunched == false) {
			while(sliderHintColor.a > 0 && transparent == false) {
				sliderHintColor.a -= Time.deltaTime/25;
				SliderHintImage.color = sliderHintColor;
				yield return null;
			}
			transparent = true;

			while(sliderHintColor.a < 1 && transparent == true) {
				sliderHintColor.a += Time.deltaTime/25;
				SliderHintImage.color = sliderHintColor;
				yield return null;
			}
			transparent = false;
		}
		else {
			//Destroy(SliderHintImage);
			gameObject.SetActive(false);
		}
	}

	
	// Update is called once per frame
	void Update () {
		StartCoroutine("Fade");
	}
}
