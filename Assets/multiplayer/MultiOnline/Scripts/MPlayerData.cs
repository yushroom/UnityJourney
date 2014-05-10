using UnityEngine;
using System.Collections;
using MultiPlayer;

public class MPlayerData : MonoBehaviour {

	public int id;
	public int gameId;
	public bool isLogin;
	public string userName;	
	public string playerName;
	public string nameInGame;
	public string mail;	
	public string privateIP;
	public string publicIP;	
	public bool isInGame;
	public string loginKey;	
	public bool isLan;
	public bool isOnline;

	public void Awake(){
		DontDestroyOnLoad(this);
	} 
			
	// OnApplicationQuit : call when player exit the game and logout the player
	void OnApplicationQuit() {
		// Logout the player on the web Server
		// And make he quit the games where he is registred
		if(isOnline){
		 	MOServer server = new MOServer();	
			StartCoroutine(server.ExitGame(id, loginKey, true));
		}
	}
       	
	// ExitGame : call when the client want to exit game
    public void ExitGame(){
		isInGame = false;
		if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.ExitGame(id, loginKey, false));	
		}
	}
	
	// RefreshGameStatus : call when the game status change
	public void RefreshGameStatus(int gameId, string gameStatus){
		if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.RefreshGameStatus(id, loginKey, gameId, gameStatus));	
		}
	}
	
	// AddPlayerInGame : call when we have a new player in gane
	public void AddPlayerInGame(int gameId){
		if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.AddPlayerInGame(id, loginKey, gameId));	
		}
	}
	
	// SaveRehostedGame : call after host migration, for save the rehosted game with his new paremeters
	public void SaveRehostedGame(int gameId, string gameName, int gamePort, int gameMaxPlayer, bool isGameUsePassword, bool isGameStarted, string gameRegister){
		if(isOnline){
			MOServer server = new MOServer();	
			StartCoroutine(server.SaveRehostedGame(id, loginKey, gameId, gameName, gamePort, gameMaxPlayer, isGameUsePassword, isGameStarted, gameRegister));	
		}
	}
	
}
