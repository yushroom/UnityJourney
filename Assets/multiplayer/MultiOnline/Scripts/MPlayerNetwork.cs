using UnityEngine;
using System.Collections;

public class MPlayerNetwork : MonoBehaviour {
	
	public GameObject cameraPlayer;
	private MNetwork networkSrc;

	void Awake(){		
		DontDestroyOnLoad(this);		
		this.name = "Player";
	}
	
	void Start () {	
		if (!networkView.isMine) { // If this is not my player
			Destroy(cameraPlayer); // Destroy camera
			this.enabled = false; // Disabled this script	
			this.GetComponent<MPlayerMove>().enabled = false; // Disabled PlayerMove script			
		} else  {// If it's my player : search the networkManager component	
			networkSrc = GameObject.Find("NetworkManager(Clone)").GetComponent<MNetwork>();
			networkSrc.PlayerSpawned(); // Informs the other players that I'm spawned			
		}	
	}
	
	void Update (){
		SavePosition();	
	}
	
	private void SavePosition(){
		networkSrc.playerPrefabPosition = transform.position;
		networkSrc.playerPrefabRotation = transform.rotation;
	}
}
