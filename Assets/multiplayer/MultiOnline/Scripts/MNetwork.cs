using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using MultiPlayer;

public class MNetwork : MonoBehaviour {
	
	// Global parameters
	public string mainMenuName = "Menu";
	public int rebuildTime=60;
	public string waitRoomName;
	public bool canJoinStartedGame;
	
	// Game parameters
	public string gameName;
	public bool isOnlineGame;
	public int gameId;
	public int gamePort;
	public int gameMaxPlayer;
	public string gamePassword;
	public int gameTotalPlayer;
	public string gameRegister;
	public string gameRegisterDate;
	
	public bool isGamePrivate;
	public bool isGameUsePassword;
	public bool isGameStarted;
	public bool isGameServerRebuild;
	public bool isGameServerRebuildFailed;
	public bool isOnNetwork;
	
	// Host parameters
	public string gameHostConnectIp;
	public string gameHostPrivateIp;
	public string gameHostPublicIp;
	public string gameHostName;
	public int gameHostId;
	public bool isGameHost;
	
	// Player parameters	
	public bool isSearchGame = false;
	public bool isPlayerExitGame;
	public Vector3 playerPrefabPosition;
	public Quaternion playerPrefabRotation;	
	private GameObject playerPrefab;
	
	// Game Map parameters
	public string gameMapName = null;
	public int gameMapKey = 0;
	
	// Chat (waitroom and gamechat)
	public List<String> chatContent;	
	
	// Playerlist
	public List<MUser> playerList;
	
	
	
	/**************** SCRIPTS AND OBJECTS ****************************/
	private MMenu menuSrc;
	public MPlayerData playerDataSrc;
	private GameObject playerDataObj;
	private MCheckPort checkConnexion ;
	
	/************************** ERROR MESSAGES ***********************/
	public string errorGameCreation = "IMPOSSIBLE TO CREATE A GAME ";
	public string errorConnexion =  "IMPOSSIBLE TO CONNECT TO THIS HOST";
	public string errorMaxPlayers =  " : maximum number of players reached.";
	public string errorPassword =  " : invalid password.";
	public string errorAlreadyConnect =  " : you are already connected on this host.";
	public string errorToAnotherServer =  " : you are already connected to another host.";
	public string errorStartedGame = "Impossible to join the host : the game is already started";
	public string errorPrivateGame = "Impossible to join the host : the game is private";
	public string errorUsedPort = " : the port is already use";
	public string errorOnlineGame = "Impossible to join the host : the game is online and you are not connected";
	/********************* GAME MENU TEXTS *********************/
	
	// --- Rebuild
	public string rebuildTitle = "SEARCHING NEW HOST";
	public string rebuildSubTitle = "CONNECTION HAS BEEN LOST";
	public string rebuildInfoTxt = "The connexion with the game's host have been lost";
	public string rebuildSearchWaitTxt = "Wait ";
	public string rebuildSearchWaitSecTxt = " seconds...";
	public string rebuildFailedInfoText =  "Impossible to find a new host";
	public string rebuildFailedExitButton= "EXIT GAME";	
	public string rebuildFailedTryButton = "TRY AGAIN";
	private string rebuildText;

	private int rebuildServerTimer;
	private string rebuildServerTimerMessage;
	
	public string rebuildNewHostTxt = "You are the new host of the game";	
	public string rebuildConnexionTxt= "Successfull connexion to the new host";
	public string rebuildFailedTxt = "The connection to the new host was failed";
	public string rebuildSearchTxt = "Searching new host...";
	
	// --- Exit
	public string exitTitle = "EXIT GAME";
	public string exitMessage = "Are you really sure that you want to exit this game ?";
	public string exitButton = "YES";
	public string exitCancelButton = "NO";
	
	
	/************************** START FUNCTIONS **************************/
	public void Awake(){
		DontDestroyOnLoad(this);
	}
	
	public void Start(){	
		menuSrc = GameObject.Find (mainMenuName).GetComponent<MMenu>(); // Get the menu script
		playerDataObj =  GameObject.Find ("PlayerData(Clone)"); // Get the playerData component
		playerDataSrc = playerDataObj.GetComponent<MPlayerData>(); // Get the playerData script
		playerList = new List<MUser>(); // Instantiate the playerList	
		
		// If the game is online 
		if(isOnlineGame){
			// We use our userName across the game
			playerDataSrc.nameInGame = playerDataSrc.userName;
		} else {
			// Else, we use our playerName
			playerDataSrc.nameInGame = playerDataSrc.playerName;
		}
		
		if(isGameHost){ // If we want to instantiate the server
			if(!gameMaxPlayer.Equals(null) && !gamePort.Equals(null)){
				if(isGameUsePassword){
					Network.incomingPassword = gamePassword;
				}
				NetworkConnectionError error = Network.InitializeServer(gameMaxPlayer-1, gamePort, !Network.HavePublicAddress());
				if(error != NetworkConnectionError.NoError){
					string errorMsg="";
					if(error == NetworkConnectionError.CreateSocketOrThreadFailure){
						errorMsg = errorUsedPort;
					}				
					menuSrc.networkCreateMessage[0] = isOnlineGame.ToString();
					menuSrc.networkCreateMessage[1] = errorGameCreation+errorMsg;
					if(isOnlineGame){
						playerDataSrc.ExitGame();
					}
				}
			}
		} else { // Else : connection on the server
			if(gameHostConnectIp != "" && !gamePort.Equals(null)){
				NetworkConnectionError error;
				if(isGameUsePassword){
					error = Network.Connect(gameHostConnectIp, gamePort, gamePassword);
				} else {
					error = Network.Connect(gameHostConnectIp, gamePort);
				}
				
				if(error != NetworkConnectionError.NoError){
					menuSrc.networkJoinMessage[0] = isOnlineGame.ToString();
					menuSrc.networkJoinMessage[1] = errorConnexion;	
				}
			}
		}
	}
	
	public void Update(){
		// Search if the player want to exit the game (down the Escape key)
		if(Input.GetKeyDown(KeyCode.Escape)) {
			isPlayerExitGame = true;
		}						
	}
	
		/***************************** GUI *****************************/
	public void OnGUI(){		
		
		// Positions settings
		int menuSizeX = 450;
		int menuSizeY = 160 ;
		int menuPosX =  Screen.width/2-(menuSizeX/2);
		int menuPosY = Screen.height/2-(menuSizeY/2);
		Rect menu = new Rect(menuPosX, menuPosY, menuSizeX, menuSizeY);
		int buttonSizeX = 150;
		int buttonSizeY = 30;
		int sizeY = 30;
		
		GUI.BeginGroup(menu);
		
		// IF WE ARE REBUILDING THE SERVER _:
		if(isGameServerRebuild){
			GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), rebuildTitle);
			sizeY+= 10;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), rebuildSubTitle);
			sizeY+= 30;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), rebuildInfoTxt);
			sizeY+= 20;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), rebuildText);
			sizeY+= 30;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), rebuildServerTimerMessage);
		
		// ELSE, IF THE SERVER REBUILD FAILED :
		} else if(isGameServerRebuildFailed){
			GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), rebuildTitle);
			sizeY+= 10;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), rebuildFailedInfoText );				
			sizeY+= 50;
			// If we want to try the rebuild one more time :
			if(GUI.Button(new Rect(20,sizeY, buttonSizeX, buttonSizeY), rebuildFailedTryButton)) {
				// Put the rebuild parameters as before the rebuild 
				// and try to rebuild one more time
				isGameServerRebuildFailed = false;
				isGameServerRebuild = true;
				rebuildServerTimerMessage = "";
				StartCoroutine(RebuildServer());// Start the rebuild
				StartCoroutine(RebuildServerTimer()); // Start rebuild timer			
			}
			if(GUI.Button(new Rect(buttonSizeX+50,sizeY, buttonSizeX, buttonSizeY), rebuildFailedExitButton)) {
				Destroy(this);
				Application.LoadLevel(mainMenuName); // Load main menu
			}
		
		// ELSE IF THE PLAYER WANT TO EXIT THE GAME
		// and if we are not on the main menu scene (because this scene have her own exit system)
		} else if(isPlayerExitGame && Application.loadedLevelName != mainMenuName){
			GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), exitTitle);
			sizeY+= 10;
			GUI.Label(new Rect(20,sizeY, menuSizeX,menuSizeY), exitMessage);
			sizeY+= 50;
			// If we click on "OK" :
			if(GUI.Button(new Rect(20, sizeY, buttonSizeX, buttonSizeY), exitButton)){
				// If the game is online 
				if(isOnlineGame){
					// Exit the game on the dataBase
					playerDataSrc.ExitGame();
				}
				
				Network.Disconnect(); // Disconnect of the network
				CleanGame(); // Clean the game (remove all players objects)
				Destroy(this); // Destoy the NetworkManager
				Application.LoadLevel(mainMenuName); // Load the main menu				
				
			}
			// If we click on "cancel" :
			if(GUI.Button(new Rect(buttonSizeX+40, sizeY, buttonSizeX, buttonSizeY), exitCancelButton)){
				isPlayerExitGame = false;
			}
		} 	
		GUI.EndGroup();	
	}
	
	/************************** PUBLIC FUNCTIONS **************************/
	
	public void StartGame(bool canLoadGame){
		if(isGameServerRebuild){ // If server has been rebuilt	
			isGameServerRebuild = false;
			if(isGameStarted) {
				LoadPlayer(); // Load the player on map	
			}
		} else if(canLoadGame) { // else if the game is ready to be load				
			if(Application.loadedLevelName != gameMapName){ // If the map is not already load
				Application.LoadLevel(gameMapName);					
			}					
		} else if(!playerDataSrc.isInGame){ // Else, load the waitroom
			if(Application.loadedLevelName != waitRoomName && waitRoomName != "" && waitRoomName != null){ // If the map is not already load
				Application.LoadLevel(waitRoomName);	
			}
		}
	}
	
	// Destroy all player GameObject (because they will be reloaded)
	public void CleanGame(){		
		GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		for (int i = 0; i < gameArray.Length; i++) {
			if(gameArray[i].name == "Player") {
				GameObject.Destroy(gameArray[i]);
			}
		}
	}
	
	// When my player spwaned 
	public void PlayerSpawned(){	
		// Send DisabledPlayers to all other players, so that players in waitroom can disabled my player
		networkView.RPC ("DisabledPlayers", RPCMode.Others);	
	}
	
	// Chat function : call from the waitRoom or from the gameChat form sycronise chat
	public void SendChatMessage(string message){
		networkView.RPC ("ChatMessage", RPCMode.All, message);	
	}
	
	/******************** PRIVATE FUNCTIONS **********************/
	
	private void LoadPlayer(){
		// Load the player in his previous position and rotation	
		try{
			Network.Instantiate(playerPrefab, playerPrefabPosition, playerPrefabRotation, 0);
		} catch(NullReferenceException){
			return;
		}
	}
	
	//-------- REBUILD SERVER FUNCTIONS --------------
	private IEnumerator RebuildServer() {
		DateTime startTime = DateTime.Now;
		DateTime maxRebuildTime = startTime.AddSeconds(rebuildTime);			
		// Wait 2 seconds for disconnection be effective
		yield return new WaitForSeconds(2);			
		do {	
			DateTime currentTime = DateTime.Now;
			int compare = DateTime.Compare(currentTime, maxRebuildTime);
			if(compare >=0) {
				FailedRebuildServer();
				break;
			}					
			for(int i = 0; i < playerList.Count; i++) {// Loop the playerList		
				yield return new WaitForSeconds(0);	
				
				if (Network.peerType == NetworkPeerType.Disconnected){ // If I am not yet connect					
					// If the player can host and if the game is not started or if he is on the game
					if(playerList[i].canHost && (!isGameStarted || playerList[i].isPlayerInGame == 1)){
						if(playerList[i].id == playerDataSrc.gameId){ // If the current id is mine	
							// I create the server	
							// (with password if the game had a password)
							if(isGameUsePassword){
								Network.incomingPassword = gamePassword;
							}
							Network.InitializeServer(gameMaxPlayer, gamePort, !Network.HavePublicAddress()); 									
						} else { // else
							string connectIp = "";
							if(playerList[i].publicIp != playerDataSrc.publicIP) { // If the server hasn't the same public ip as me :
								connectIp = playerList[i].publicIp; // Use his public IP
							} else { // else
								connectIp = playerList[i].privateIp; // Use his private IP
							}
							
							// I connect myself to the server by his IP
							// (with password if the game had a password)
							if(isGameUsePassword){
								Network.Connect(connectIp, gamePort, gamePassword);
							} else {
								Network.Connect(connectIp, gamePort); 
							}			
							// Wait 2 seconds for connection be effective
							yield return new WaitForSeconds(2);							
						}						
						
					} 
				}
			}	
		} while(Network.peerType == NetworkPeerType.Disconnected);
	}	
		
	private void FailedRebuildServer(){
		isGameServerRebuild = false;
		isGameServerRebuildFailed = true;
	}	

	private IEnumerator RebuildServerTimer(){
		rebuildServerTimer=rebuildTime;
		int i = 1;
		while(rebuildServerTimer > i){
			yield return new WaitForSeconds(1);	
			rebuildServerTimer--;
			rebuildServerTimerMessage = rebuildSearchWaitTxt+rebuildServerTimer.ToString()+rebuildSearchWaitSecTxt;
		}
	}
	
	// ------- DISABLED / ENABLED player --------------
	private void DisabledPlayer(){
		if(!networkView.isMine) {			
			GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
			for (int i = 0; i < gameArray.Length; i++) {
				if(gameArray[i].name == "Player") {	
					// Make player invisible : disabled renderer on him and his childrens
					gameArray[i].renderer.enabled  = false; 
					Transform[] chidrens = gameArray[i].GetComponentsInChildren<Transform>();
					foreach (Transform child in chidrens) {
						if(child.renderer != null) {
							child.renderer.enabled = false;
						}
					}									
				}
			}
		}
	}
	
	private void EnabledPlayer(){	
		if(!networkView.isMine) {
			GameObject[] gameArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
			for (int i = 0; i < gameArray.Length; i++) {
				if(gameArray[i].name == "Player") {
					// Make player visible : enabled renderer on him and his childrens
					gameArray[i].renderer.enabled  = true; 				
					Transform[] chidrens = gameArray[i].GetComponentsInChildren<Transform>();
					foreach (Transform child in chidrens) {
						if(child.renderer != null) {
							child.renderer.enabled = true;
						}
					}
					// Ask player position to the server
				networkView.RPC ("AskPosition", RPCMode.Server, gameArray[i].networkView.viewID, Network.player);
				}
			}
		}		
	}	
	
	/***************************** EVENTS *****************************/
	
	// OnServerInitialized : call on the server
	void OnServerInitialized(){	
		if(isGameServerRebuild){ // If we have rebuild the server	
			rebuildText = rebuildNewHostTxt;	
			if(isOnlineGame){ // If the game is online : save the new host in dataBase
				playerDataSrc.SaveRehostedGame(gameId, gameName, gamePort, gameMaxPlayer, isGameUsePassword, isGameStarted, gameRegisterDate);
			}
		}
		
		// Save host parameters
		isGameHost = true;	
		
		playerList.Clear(); // Clear the playerList 
		
		// Server save his id around the game
		playerDataSrc.gameId = int.Parse(Network.player.ToString()); 
		gameTotalPlayer = 1;
		// Add server to playerList
		int inGame = 0;
		if(playerDataSrc.isInGame){
			inGame = 1;
		}
		
		playerList.Add(new MUser(playerDataSrc.gameId, 
			playerDataSrc.privateIP,playerDataSrc.publicIP,
			playerDataSrc.nameInGame,inGame, 1, true)); 		
		StartGame(false); // Try to start the game
	}	
	
	// OnConnectedToServer : call on the client
	void OnConnectedToServer(){		
		// If the client want join the game
		if(!isSearchGame) {			
			// Client save his id around the game
			playerDataSrc.gameId = int.Parse(Network.player.ToString()); 
			networkView.RPC ("AddPlayer", RPCMode.Server, 
					playerDataSrc.gameId, 
					playerDataSrc.nameInGame, 
					playerDataSrc.privateIP, 
					playerDataSrc.publicIP,
					isOnNetwork,
					playerDataSrc.isInGame,
					playerDataSrc.isOnline, Network.player);
				
			// If server is rebuild
			if(isGameServerRebuild) {
				rebuildText = rebuildConnexionTxt;
				StartGame(false); // Call StartGame to directly restart the game 
			}			
		} else {
			// Else, if the client is just searching a game : call SearchGame on the server
			networkView.RPC ("SearchGame", RPCMode.Server, Network.player);
		}
	}
	
	// OnPlayerConnected : call on the server
	void OnPlayerConnected(NetworkPlayer player){
		networkView.RPC ("DisabledPlayers", player);	
	}	
	
	// OnFailedToConnect : call on the client
	void OnFailedToConnect(NetworkConnectionError error){		
		if(!isSearchGame) {			
			if(isGameServerRebuild){
				return;					
			} else {
				string errorMsg="";
				if(error == NetworkConnectionError.TooManyConnectedPlayers){
					 errorMsg = errorMaxPlayers;
				} else if(error == NetworkConnectionError.InvalidPassword){
					 errorMsg = errorPassword;
				} else if(error == NetworkConnectionError.AlreadyConnectedToServer){
					 errorMsg = errorAlreadyConnect;
				} else if(error == NetworkConnectionError.AlreadyConnectedToAnotherServer){
					 errorMsg = errorToAnotherServer;
				} 					
				menuSrc.networkJoinMessage[0] = isOnlineGame.ToString();
				menuSrc.networkJoinMessage[1] = errorConnexion+errorMsg;	
				Destroy(gameObject);
			}
		} else if(isSearchGame && menuSrc.useLan){ // If we are searching games 
			// This IP has no game, so :			
			menuSrc.LanGetNetworkGames(false); // Go to next IP			
		}
	}
	
	// OnPlayerDisconnected : call on the server
	void OnPlayerDisconnected(NetworkPlayer player) {		
		if(MUser.inList(playerList, int.Parse(player.ToString()))) {
			gameTotalPlayer--;
			networkView.RPC("RefreshPlayerCount", RPCMode.All, gameTotalPlayer);	
			
			playerList = MUser.RemoveFromId(playerList, int.Parse(player.ToString()));
			networkView.RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));		
		}
		Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
	
	// OnDisconnectedFromServer : call on the client
	void OnDisconnectedFromServer(){
		// If we are not just searching a game :
		if(!isSearchGame) {
			// All the game will be rebuilt with a new host (if we find one)			
			
			gameTotalPlayer = 0;// Put the total player on 0	
			CleanGame(); // Clean the game (remove all players prefab) (they will be re-loaded on the new host)	
			
			playerList = MUser.RemoveServer(playerList);// Remove server from playerList (because he have leave the game)
			
			isGameServerRebuild = true;	// Put the rebuild bool on true
			StartCoroutine(RebuildServer()); // Try to rebuild a new server
			StartCoroutine(RebuildServerTimer()); // Start the rebuild timer
			rebuildText = rebuildSearchTxt ;			
			rebuildServerTimerMessage = ""; // Clear the rebuildServerTimerMessage
		}
	}
	 
	// OnLevelWasLoaded 
	void OnLevelWasLoaded (){
		// save the playerPrefab in local variable
		if(Application.loadedLevelName != waitRoomName) {
			if(!isGameStarted){
				chatContent.Clear(); // Clear the chat (because it contains the waitroom Thread and timer)
			}
			EnabledPlayer(); // Enabled the render of the other players
			MSpawn spawnSrc = GameObject.Find("Spawns").GetComponent<MSpawn>();
			playerPrefab = spawnSrc.playerPrefab;
			isGameStarted = true;
			playerDataSrc.isInGame = true;	
			rebuildText="";								
			
			// Save the server new statut (in game)
			if(isGameHost) {
				playerList = MUser.SavePlayerStatus(playerDataSrc.gameId, 1, playerList);
				// Send my new status to the clients
				networkView.RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));						
							
				if(isOnlineGame){
					playerDataSrc.RefreshGameStatus(gameId, "1");	
				}
			} else {
				// Send to the server that I'm now in game
				networkView.RPC ("RefreshPlayerStatus", RPCMode.Server, playerDataSrc.gameId, 1);
			}	
		}
	}		
	
	/***************************** RPC FUNCTIONS *****************************/	
		
	
	// --------------- USER LIST FUNCTIONS ---------------------------
	
	// Call by the player, to the server, to be add on the playerList	
	[RPC]  
	void AddPlayer(int id, string playerName, string privateIp, string publicIp, bool isOnNetwork, bool isPlayerInGame, bool isPlayerOnline, NetworkPlayer player){	
		// If the game is private and that we are not on the network of the host
		// (check by comparing public ip)
		if(isGamePrivate && publicIp != playerDataSrc.publicIP && !isOnNetwork) {			
			networkView.RPC ("CantJoinGame", player, 2); // We cannont join the game
			return; // Exit the function here
		}
		
		// If the game is Online but the player is not connected
		if(isOnlineGame && !isPlayerOnline){			
			networkView.RPC ("CantJoinGame", player, 3); // We cannont join the game
			return; // Exit the function here
		}
		// If the game is not yet started or if we don't use the wait room
		//or if the player is already on game = if the server has been rebuild
		if(!isGameStarted || isPlayerInGame || canJoinStartedGame) {
			gameTotalPlayer++;	// Increments player count	
			
			int inGame = 0;
			if(isPlayerInGame){
				inGame = 1;
			}			
						
			// Add the new player on the user list
			playerList.Add (new MUser(id, privateIp, publicIp, playerName, inGame, 0, false));
			
			// Send the game informations to the new player
			networkView.RPC("GetGameInfos", player, gameTotalPlayer, gameMaxPlayer, gameMapKey, gameName, playerDataSrc.nameInGame,playerDataSrc.privateIP, playerDataSrc.publicIP, isGamePrivate, isGameStarted, true);
			
			// Refresh the lists of the clients : send on everybody except the server
			networkView.RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));		
			
			// Send the game state and try to start game
			networkView.RPC("RefreshPlayerCount", RPCMode.Others, gameTotalPlayer);	
			
			// Say to the player to open a connexion in order to test his hosting capacity
			networkView.RPC("OpenConnexion", player);
			
			if(isOnlineGame){
				// Say to the player to save himself on the online list of game's players
				networkView.RPC("AddPlayerInGame", player);
			}
		} else {
			// Else, we cannot join the game : 
			networkView.RPC ("CantJoinGame", player, 1);
		}
	}
	
	// Call by the server to all clients when the list has changed
	[RPC]
	void RefreshUserList(string list){
		playerList = MUser.ListToObject(list);// Save the new list			
	}
	
	// Call by a client to ther server, when a load the game
	[RPC]
	void RefreshPlayerStatus(int playerId, int isPlayerInGame){
		// Save the new status of the player
		playerList = MUser.SavePlayerStatus(playerId, isPlayerInGame, playerList);
		// Call RefreshUserList on the other players
		networkView.RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));		
	}
	
	// Call by the server to a client when he join the game :
	// so that the new client save himself on the game's player list on DataBase
	[RPC]
	void AddPlayerInGame(){
		if(isOnlineGame){
			playerDataSrc.AddPlayerInGame(gameId);
		}
	}
	
	// --------------- GAME STATUS AND INFO FUNCTIONS ---------------------
	// Call by the server on the client, each time a new player is connected to server
	// Call by the server on the client, each time a new player is connected to server
	
		// Call by ther server to all clients when a player quit the game
	[RPC]
	void RefreshPlayerCount(int totalPlayer){
		this.gameTotalPlayer = totalPlayer;
	}
	
	// Call by the client to the server to received game informations
	[RPC]
	void SearchGame(NetworkPlayer player){
		// If it's not an online game
		if(!isOnlineGame){
			// Go search the games informations
			networkView.RPC("GetGameInfos", player, gameTotalPlayer, gameMaxPlayer, gameMapKey, gameName, playerDataSrc.nameInGame,playerDataSrc.privateIP, playerDataSrc.publicIP, isGamePrivate, isGameStarted, false);
		} else { // Else
			
			// Call GoNextGame to the client
			networkView.RPC("GoNextGame", player);
		}
	}
	
	// Call by the server on the client who asked it
	[RPC]
	void GetGameInfos(int gameTotalPlayer, int maxPlayer, int gameMapKey, string gameName, string gameHostName, string hostPrivateIp, string hostPublicIp, bool isGamePrivate, bool isGameStarted, bool startGame){				
	
		this.gameTotalPlayer = gameTotalPlayer;
		this.gameMaxPlayer = maxPlayer;
		this.gameMapKey = gameMapKey;
		this.gameName = gameName;		
		this.gameHostName = gameHostName;
		this.gameHostPrivateIp = hostPrivateIp;
		this.gameHostPublicIp = hostPublicIp;
		this.isGameStarted = isGameStarted;			
		
		// If we are allowed to start the game
		if(startGame){
			// If the game is not yet started 	
			if(!isGameStarted || canJoinStartedGame) {
				StartGame(false); // Try to start game
			}	
		}
		
		if(isSearchGame && menuSrc.useLan){			
			// If we are searching parties
			// We have now every informations we need, so :
			// This IP has no game, so :
			// This IP has no game, so :			
			menuSrc.LanGetNetworkGames(false); // Go to next IP			
		}			
	}
	
	// Send to the client, when he cannot join the game
	[RPC]
	void CantJoinGame(int index){
		CleanGame(); // Delete all players on the client scene
	
		// Define a message
		menuSrc.networkJoinMessage[0] = isOnlineGame.ToString();
		if(index == 1) {			
			menuSrc.networkJoinMessage[1] = errorStartedGame;	
		} else if(index == 2) {
			menuSrc.networkJoinMessage[1] = errorPrivateGame;
		} else if(index == 3) {
			menuSrc.networkJoinMessage[1] = errorOnlineGame;
		}
		Destroy(gameObject); // Destroy the client network GameObject
	}
			
	//By by the server on the client when a fall on an Online game when he searchs network games
	[RPC]
	void GoNextGame(){
		if(menuSrc.useLan){
			menuSrc.LanGetNetworkGames(false); // Go to next IP		
		}
	}
		
	// --------------- GAME PLAYERS AND ENVIRONMENT FUNCTIONS ---------------------
		
	// Call by the server to the client, when the connexion is started
	[RPC] 
	void DisabledPlayers(){			
		// If I'm not in game : disabled the rendrer of the other players
		// (Else I can see them before have load the game level)
		if(!playerDataSrc.isInGame){			
			DisabledPlayer();		
		}
	}
	
	// Call by the client (when he load the map) to the server to receive the players positions
	[RPC]
	void AskPosition(NetworkViewID viewId, NetworkPlayer askPlayer){
		NetworkView view = NetworkView.Find(viewId); // Search the view form M
		GameObject gamePlayer = view.observed.gameObject; // Get the gameObject
		// Send the viewid and the position to the askPlayer
		networkView.RPC ("SendPosition", askPlayer, viewId, gamePlayer.transform.position);
	}
	
	// Call by the server to the clien, send the players positions to the player who asked it
	[RPC]
	void SendPosition(NetworkViewID viewId, Vector3 position){
		NetworkView view = NetworkView.Find(viewId); // Search the view form M
		GameObject gamePlayer = view.observed.gameObject; // Get the gameObject
		// Move the gameObject on his good position
		gamePlayer.transform.position = position;
	}
	
	// --------------- TESTING HOSTING CAPACITIES FUNCTION ----------------------
	// Call on the client to make him open a connexion
	[RPC]
	void OpenConnexion(){
		/***** Check the client hosting capacity ******/			
		checkConnexion = new MCheckPort(); 
		bool canConnect = false; // Connexion parameter
			if(checkConnexion.OpenConnexion(playerDataSrc.privateIP, gamePort)){
				// If the client can create a server on the game port, turn canConnect on true
				canConnect = true;
			} 			
		// Call TryConnexion on the server with the connexion parameter
		networkView.RPC ("TryConnexion", RPCMode.Server, playerDataSrc.gameId, Network.player, canConnect, isOnNetwork);
	}
	
	// Call on the server in order to make it test the player hosting capacity
	[RPC]
	void TryConnexion(int playerId, NetworkPlayer askPlayer, bool canConnect, bool isOnNetwork){
		bool canBeHost = false; // Player's hosting capacity
		// If the player opened a server with success
		if(canConnect) {
			// Search the askPlayer ip
			MUser user = MUser.SearchPlayer(playerId, playerList);
			if(user != null){
				string connectIp = null;
				if(user.publicIp == playerDataSrc.publicIP || isOnNetwork){
					 connectIp = user.privateIp;
				} else {
					connectIp = user.publicIp;
				}	
				
				// If the connexion on the player works
				if(MCheckPort.TryConnexion(connectIp, gamePort)){						
					canBeHost = true; // Turn canConnect on true
				} 	
				// Call CloseConnexion on the ask player : that notice him that he can close his port
				networkView.RPC ("CloseConnexion", askPlayer);
			}
			// Save the player hosting capacity
			playerList = MUser.SavePlayerHostingCapacity(playerId, canBeHost, playerList);
			// Call RefreshUserList on all clients
			networkView.RPC("RefreshUserList", RPCMode.Others, MUser.ListToString(playerList));		
		}
	}		

	// Call on the client to make him close his connexion
	[RPC]
	void CloseConnexion() {	
		// Close the connexion opened for test hosting capacity
		checkConnexion.CloseConnexion();		
	}
	
	
	// --------------------- OTHER FUNCTIONS ------------------------
	// Call on ererybody : send message from chat on waitRoom or gameChat
	[RPC]
	void ChatMessage(string message){
		// Add the message on the messages list
		chatContent.Add (message);			
				
		// Make the chat scroll down
		try {
			// Search if the waiting room is open
			MWaitRoom waitRoomScr = GameObject.Find ("WaitRoom").GetComponent<MWaitRoom>();
			if(waitRoomScr != null) {
				// Update the waiting room chat scroll
				waitRoomScr.chatScroll = new Vector2(0, (20*(chatContent.Count+1)) - waitRoomScr.chatContentSizeY);
			}
		} catch (NullReferenceException) {
			try {
				// Search if the game chat is open
				MGameChat gameChatSrc = GameObject.Find ("GameChat").GetComponent<MGameChat>();
				if(gameChatSrc != null){
					// Update the game chat scroll
					gameChatSrc.chatScroll = new Vector2(0, (20*(chatContent.Count+1)) - gameChatSrc.chatContentSizeY);
				}
			} catch (NullReferenceException) {
				return;
			}
		}
	}
	


}
