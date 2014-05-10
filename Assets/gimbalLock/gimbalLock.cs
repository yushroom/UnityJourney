using UnityEngine;
using System.Collections;

public class gimbalLock : MonoBehaviour {

	//private Quaternion newDirection;
	Vector3 m_mousePosition = Vector3.zero;
	private bool m_mouseIsDown = false;

	// Use this for initialization
	void Start () {
		//newDirection = Quaternion.Euler(-90, 45, 0);

	}
	
	// Update is called once per frame
	void Update () {
		//transform.rotation = Quaternion.Lerp (transform.rotation, newDirection, Time.deltaTime);
		if (Input.GetMouseButtonDown(0)) {
			m_mouseIsDown = true;
		} else if (Input.GetMouseButtonUp(0)) {
			m_mouseIsDown = false;
		}
		
		if (m_mouseIsDown) {
			Vector3 position = m_mousePosition - Input.mousePosition;     
			this.transform.localRotation *= Quaternion.Euler(-position.y, position.x, 0); 
		}
		
		m_mousePosition = Input.mousePosition;
	}
}
