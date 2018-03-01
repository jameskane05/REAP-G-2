using UnityEngine;
using System.Collections;

public class LampControl : MonoBehaviour {

	//Turns the fire light and particles on
	public bool fireOn = true;

	//Turns the cone light on
	public bool coneOn = true;

	//Turns the cone particles on
	public bool conePartOn = true;

	//Fire gameobject
	public GameObject fire;

	//Cone gameobject
	public GameObject cone;

	//Cone particles
	public GameObject coneParticles;

	// Start is called at initialization time of the object
	void Start(){
		if (fire == null)
			fire = this.gameObject.transform.Find ("Fire").gameObject;
		if (cone == null)
			cone = this.gameObject.transform.Find ("Cone Light").gameObject;
		if (coneParticles == null)
			coneParticles = this.gameObject.transform.Find ("Cone Particles").gameObject;
	}
	// Awake is called when the object becomes active
	void Awake(){
		if (fire == null)
			fire = this.gameObject.transform.Find ("Fire").gameObject;
		if (cone == null)
			cone = this.gameObject.transform.Find ("Cone Light").gameObject;
		if (coneParticles == null)
			coneParticles = this.gameObject.transform.Find ("Cone Particles").gameObject;
	}

	//Update is being used for demostation only.
	//Recommended to use the methods below instead of an update method per French_Street_Lamp for performance reasons
	void Update(){
		if (fireOn)
			TurnOn (fire);
		else
			TurnOff (fire);

		if (coneOn)
			TurnOn (cone);
		else
			TurnOff (cone);

		if (conePartOn)
			TurnOn (coneParticles);
		else
			TurnOff (coneParticles);
	}

	//Activates given gameobject in the scene
	void TurnOn(GameObject go){
		go.SetActive (true);
	}

	//Deactivates given gameobject in the scene
	void TurnOff(GameObject go){
		go.SetActive (false);
	}


}
