using UnityEngine;
using System.Collections;

public class MPlayerMove : MonoBehaviour {
	
	
	/***** CAN BE MODIFIED *****/	
	
	public float speed = 0.05F;
	/**************************/
	
	private MNetwork networkSrc;
	
	void Start () {	
		if (networkView.isMine) { 
		// If it's my player : search the networkManager component	
			networkSrc = GameObject.Find("NetworkManager(Clone)").GetComponent<MNetwork>();
			networkSrc.PlayerSpawned(); // Informs the other players that I'm spawned
		}	
	}
	
	void Update (){
		// If we are not on host migration, and if we have not click on "exit game", a if the game is started
		if(!networkSrc.isGameServerRebuild && !networkSrc.isGameServerRebuildFailed && !networkSrc.isPlayerExitGame && networkSrc.isGameStarted) {
			MovePlayer();	// We can move the player	
		}
	}
	
	// Move player function : everything can be change on the function, except the last line (the call to SavePosition function)
	private void MovePlayer(){			
		/***** CAN BE MODIFIED *****/
		// 
		if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow)) {
			transform.Translate(new Vector3(0,0,1) * speed);
		} else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			transform.Translate(new Vector3(0,0,-1) * speed);
		}
		
		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow)) {
			transform.Rotate(new Vector3(0,-1,0) * 2);
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			transform.Rotate(new Vector3(0,1,0) * 2);
		}		
		
		/**************************/
	}

}
