using UnityEngine;
using System.Collections;

// sunrise and sunset
public class Slerp : MonoBehaviour {

	public Transform sunrise;
	public Transform sunset;
	public float journeyTime = 1.0F;
	private float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		print (transform.position);

		// get center of the arc
		Vector3 center = (sunrise.position + sunset.position) * 0.5f;
		center -= new Vector3 (0, 1, 0);

		// Interpolate
		Vector3 riseRelCenter = sunrise.position - center;
		Vector3 setRelCenter = sunset.position - center;
		float fracComplete = (Time.time - startTime) / journeyTime;
		transform.position = Vector3.Slerp (riseRelCenter, setRelCenter, fracComplete);
		transform.position += center;
	}
}
