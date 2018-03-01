using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

	// Turns the flickering of the light on and off at activation or initialization
	public bool isFlickering = true;

	// The minimum intensity of the light
	public float lowRange =1.75f;

	// The maximum intensity of the light
	public float highRange = 2.25f;

	// The rate of which the light changes intensity in seconds
	public float flickerRate =.15f;

	// Optional delay in seconds for the light to flicker after activation or initialization
	public float startFlickeringIn = 0f;

	// Start is called at initialization time of the object
	void Start () {
		if (isFlickering)
			TurnOn();
	}
	// Awake is called when the object becomes active
	void Awake (){
		if (isFlickering)
			TurnOn ();
	}

	// FlickerLight changes the intensity of the light component randomly 
	// between two given floats giving the effect of a flicking light.
	void FlickerLight(){
		float random = Random.value;
		float range = highRange - lowRange;
		if (0f > lowRange || lowRange > 8f || 0f > highRange || highRange > 8f)
			Debug.Log ("lowRange and highRange cannot be negetive or higher than 8. lowRange: "+ lowRange + " highRange: " + highRange);
		else {
			float value = (range * random) + lowRange;
			this.gameObject.GetComponent<Light> ().intensity = value;
		}
	}

	// TurnOff off is an optional method that can be used in scripting to turn off the flickering
	void TurnOff(){
		CancelInvoke ("FlickerLight");
	}
	// TurnOn on is an optional method that can be used in scripting to turn on the flickering
	void TurnOn(){
		InvokeRepeating ("FlickerLight", startFlickeringIn, flickerRate);
	}

}
