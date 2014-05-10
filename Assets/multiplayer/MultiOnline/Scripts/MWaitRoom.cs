using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MWaitRoom : MonoBehaviour {
	
	/**************** SCRIPTS AND OBJECTS ****************************/
	private MNetwork networkSrc;
	private GameObject networkObj;
	private MPlayerData playerDataSrc;
	private GameObject playerDataObj;
	
	/********* BUTTONS, MESSAGES AND OTHER GUI PARAMETRES ********/
	// --- Parameters
	private bool exitGame;
	private bool isLoading = false;		
	
	// -- Text and message	
	public string textChat;
	
	// --- Buttons
	private string loadGameButton;
	private string loadGameCount;
	
	// --- Size
	public int chatContentSizeY;
	
	//--- Maps
	public string[] maps;
	public Texture[] mapsScreen;
	public bool displayMapScreen = true;
	public int mapScreenX = 150;
	public int mapScreenY = 150;
	
	
	/********************* SCROLLS ****************************/
	public Vector2 mapScroll = Vector2.zero;
	public Vector2 playerScroll = Vector2.zero;	
	public Vector2 chatScroll = Vector2.zero;	
	
	/************** WAITROOM TEXTS **************/
	// Player list
	public string playerListTitle= "PLAYER LIST";
	public string playerListHostTxt = " [host] ";
	public string playerListClientTxt= "";
	public string playerInGameTxt = "in game";
	public string playerInWaitRoomTxt = "in waitroom";
	
	// Game panel
	public string gamePanelTitle=  "GAME : ";
	public string gamePanelPlayersRequireTxt =  "Players require to start game:  ";
	public string gamePanelPlayersMaxTxt =  "Players max: ";
	public string gamePanelPlayersNbrTxt = "Players : ";
	public string gamePanelMapTxt =  "Map : ";		
	public string gamePanelMapTitle = "SELECT THE MAP";
	public string gamePanelLoadMessage = "THE GAME IS GOING TO START";
	public string gamePanelLoadButton = "START GAME";
	public string gamePanelJoinButton = "JOIN GAME";
	public string gamePanelWaitButton = "WAIT...";
	
	// Chat
	public string chatTitle="CHAT";
	public string chatButtonTxt="Send";
	
	// Exit
	public string exitButtonTxt= "Exit game";	
	public string exitTitle = "EXIT GAME";
	public string exitTxt = "Are you really sure that you want to exit this game ?";
	public string exitConfirmButtonTxt =  "YES";
	public string exitCancelButtonTxt = "NO";
	
	/******************* TIMER PARAMETRES *************************/
	public bool useLoadTimer = true;
	public bool useLoadChatTimer = true;
	public int loadTimerTime = 5;
	
	void Start(){	
		// Find networkSrc
		networkObj = GameObject.Find ("NetworkManager(Clone)");
		networkSrc = networkObj.GetComponent<MNetwork>();
		
		// Find playerDataSrc
		playerDataObj =  GameObject.Find ("PlayerData(Clone)");
		playerDataSrc = playerDataObj.GetComponent<MPlayerData>();
	}
	
	void OnGUI(){		
		// Positions settings
		int sizeLabelY = 30;		
		int loadButtonSizeY = 40;	
		int loadPannelSizeY = loadButtonSizeY+20;
				
		int playerSizeX = 250;
		int playerSizeY = Screen.height-20;
		int playerPosX = 10;
		int playerPosY = 10;
		Rect playerMenu = new Rect(playerPosX, playerPosY, playerSizeX, playerSizeY);
			
		int mapButtonX = 120;
		int mapButtonY = 30;
		int mapSizeX = mapButtonX + 60;
		int mapSizeY = (Screen.height-loadPannelSizeY-20)/2;		
		
		int gameSizeX =0;
		if(networkSrc.isGameHost) {
			gameSizeX = Screen.width - playerSizeX - mapSizeX-40;
		} else {
			gameSizeX = Screen.width - playerSizeX - 30;
		}

		int gameSizeY = (Screen.height-loadPannelSizeY-20)/2;
		int gamePosX = playerSizeX+20;
		int gamePosY = 10;
		Rect gameMenu = new Rect(gamePosX, gamePosY, gameSizeX, gameSizeY);		
		
		int mapPosX = playerSizeX+20+gameSizeX+10;
		int mapPosY = 10;
		Rect mapMenu = new Rect(mapPosX, mapPosY, mapSizeX, mapSizeY);	
		
		int chatSizeX = Screen.width - playerSizeX - 30;
		int chatSizeY = gameSizeY;
		int chatPosX = gamePosX;
		int chatPosY = gameSizeY+20;
		Rect chatMenu = new Rect(chatPosX, chatPosY, chatSizeX, chatSizeY);		
		int chatButtonSizeX = 100;
		int chatButtonSizeY = 20;
		int chatFieldPosY = chatSizeY-sizeLabelY;
		int chatFieldSizeX = chatSizeX-20-chatButtonSizeX-10;
		int chatFieldSizeY = 20;		
		int chatButtonPosX = chatFieldSizeX+20;				
		chatContentSizeY = chatSizeY-chatFieldSizeY-60;
		
		int loadSizeX =  Screen.width - playerSizeX - 30;
		int loadSizeY = gameSizeY;
		int loadPosX = gamePosX;
		int loadPosY = chatSizeY+gameSizeY+10;
		Rect loadMenu = new Rect(loadPosX, loadPosY, loadSizeX, loadSizeY);
		int loadButtonSizeX = 150;			
		int loadButtonPosX =loadSizeX - loadButtonSizeX;
		
		int returnSizeX = playerSizeX + 30;
		int returnSizeY = gameSizeY;
		int returnPosX = gamePosX;
		int returnPosY = chatSizeY+gameSizeY+10;
		Rect returnMenu = new Rect(returnPosX, returnPosY, returnSizeX, returnSizeY);
		int returnButtonSizeX = 150;	
		
		
		int exitSizeX = 450;
		int exitSizeY =160 ;
		int exitPosX =  Screen.width/2-(exitSizeX/2);
		int extiPosY = Screen.height/2-(exitSizeY/2);
		Rect exitMenu = new Rect(exitPosX, extiPosY, exitSizeX, exitSizeY);
		int exitButtonSizeX = 150;
		int exitButtonSizeY = 30;
		
		/********************* PLAYER LIST PANEL**********************/
		GUI.BeginGroup(playerMenu, "");
		GUI.Box(new Rect(0,0,playerSizeX, playerSizeY), playerListTitle);
		float sizeY = 20;
		GUI.Label(new Rect(20,sizeY,playerSizeX, sizeLabelY),"");
		sizeY+=10;
		
		playerScroll = GUI.BeginScrollView (new Rect (0,sizeY,playerSizeX,playerSizeY-sizeY-20), playerScroll, new Rect(0,0, playerSizeX-50, (networkSrc.playerList.Count*20)));
		sizeY = 0;
		
		
		// Display the player list
		for(int i = 0; i < networkSrc.playerList.Count; i++){
			string serverMessage="";
			if(networkSrc.playerList[i].isGameHost == 1) {
				serverMessage = playerListHostTxt;
			} else {
				serverMessage = playerListClientTxt;
			}
			string playerStatus ="";
			if(networkSrc.playerList[i].isPlayerInGame == 1){
				playerStatus =playerInGameTxt;
			} else {
				playerStatus =playerInWaitRoomTxt;
			}
			
			GUI.Box(new Rect(10,sizeY,playerSizeX-20, sizeLabelY), "");
			GUI.Label(new Rect(20,sizeY+5,playerSizeX, sizeLabelY),networkSrc.playerList[i].playerName+serverMessage+" : "+ playerStatus);
			sizeY+=35;		
		}
		
		GUI.EndScrollView();	
		
		GUI.EndGroup();
		
		/************************* GAME PARAMETERS PANEL ******************/
		GUI.BeginGroup(gameMenu, "");
		GUI.Box(new Rect(0,0,gameSizeX, gameSizeY), gamePanelTitle+networkSrc.gameName);
		sizeY = 20;
		GUI.Label(new Rect(10,sizeY,gameSizeX, sizeLabelY),"");
		sizeY+=10;	
		GUI.Label(new Rect(20,sizeY,gameSizeX, sizeLabelY), gamePanelPlayersNbrTxt+networkSrc.gameTotalPlayer+" / "+networkSrc.gameMaxPlayer);
		sizeY+= 20;	
		
		GUI.Label(new Rect(20,sizeY,gameSizeX, sizeLabelY), gamePanelMapTxt+maps[networkSrc.gameMapKey]);

		
		if(mapsScreen[networkSrc.gameMapKey] != null && displayMapScreen) {
			sizeY+= 40;	
			GUI.DrawTexture(new Rect(20,sizeY,mapScreenX, mapScreenY), mapsScreen[networkSrc.gameMapKey],  ScaleMode.ScaleAndCrop, true);
		} 
			
		GUI.EndGroup();
		
		// SELECT MAP MENU		
		if(networkSrc.isGameHost){ // If I'm the host :
			GUI.BeginGroup(mapMenu, "");
			GUI.Box(new Rect(0,0,mapSizeX, mapSizeY), gamePanelMapTitle);			
			sizeY = 30;
			
			mapScroll = GUI.BeginScrollView (new Rect (0,sizeY,mapSizeX,mapSizeY-sizeY-20), mapScroll, new Rect(0,0, mapSizeX-40, mapButtonY*maps.Length));
			sizeY = 0;			
			
			networkSrc.gameMapKey = GUI.SelectionGrid(new Rect(20, sizeY,mapButtonX, mapButtonY*maps.Length), networkSrc.gameMapKey, maps, 1);
			
			GUI.EndScrollView();			
			
			GUI.EndGroup();			
		}
	
		
		/*************************** CHAT PANEL ******************************/
		GUI.BeginGroup(chatMenu, "");
		GUI.Box(new Rect(0,0,chatSizeX, chatSizeY), chatTitle);
		sizeY = 20;		
		GUI.Label(new Rect(10,sizeY,gameSizeX, sizeLabelY),"");
		sizeY+=10;
		
		chatScroll = GUI.BeginScrollView (new Rect (0,sizeY,chatSizeX,chatContentSizeY), chatScroll, new Rect(0,0, chatSizeX-40, 20*networkSrc.chatContent.Count));
		sizeY = 0;
		
		for(int i = 0; i < networkSrc.chatContent.Count; i++){			
			GUI.Label(new Rect(10,sizeY,chatSizeX, sizeLabelY), networkSrc.chatContent[i]);
			sizeY+=20;
		}
		
		GUI.EndScrollView();

		textChat = GUI.TextField(new Rect(10,chatFieldPosY,chatFieldSizeX, chatFieldSizeY), textChat);
		
		if(GUI.Button(new Rect(chatButtonPosX,chatFieldPosY,chatButtonSizeX, chatButtonSizeY), chatButtonTxt)){
			if(textChat != "") {
				string message = playerDataSrc.nameInGame+" : "+textChat;				
				int maxChar = (int)Math.Round((chatSizeX-10) / 6.5f);
				if(message.Length > maxChar){
					message = message.Substring(0, maxChar);
				}
			
				ChatMessage(message);
				textChat = "";
			}
		}
		
		GUI.EndGroup();		
	
		/****************************** EXIT BUTTON *************************/
		GUI.BeginGroup(returnMenu);		
		if(GUI.Button (new Rect(0, 20, returnButtonSizeX, loadButtonSizeY), exitButtonTxt)){
			exitGame = true;			
		}
		GUI.EndGroup();		
		
		// If we clicked on Exit button :
		if(exitGame){
			GUI.BeginGroup(exitMenu);	
			GUI.Box(new Rect(0,0,exitSizeX, exitSizeY),exitTitle);
			sizeY = 40;
			GUI.Label(new Rect(10,sizeY, exitSizeX,exitSizeY), exitTxt);
			sizeY+=40;
			if(GUI.Button(new Rect(10, sizeY, exitButtonSizeX, exitButtonSizeY), exitConfirmButtonTxt)){
				Destroy(networkObj); // Destoy the NetworkManager
				if(networkSrc.isOnlineGame){
					playerDataSrc.ExitGame(); // Exit game on DataBase
				}
				Application.LoadLevel(networkSrc.mainMenuName); // Load the main menu
			}
			if(GUI.Button(new Rect(exitButtonSizeX+30, sizeY, exitButtonSizeX, exitButtonSizeY), exitCancelButtonTxt)){
				exitGame = false;
			}
			GUI.EndGroup();	
		}	
		
		
		/****************************** START BUTTON *************************/
		GUI.BeginGroup(loadMenu);		
			
		if(networkSrc.isGameHost) { // If I am the host ;
			loadGameButton = gamePanelLoadButton;
		}else if (networkSrc.canJoinStartedGame && networkSrc.isGameStarted){
			// If I'm not the host, but  the game is started :
			loadGameButton = gamePanelJoinButton;
		} else { // Else :
			loadGameButton = gamePanelWaitButton;
		}
		
		if(!isLoading) { // If the game is non loading
			// If I'm not the host, and the game is not started :
			if(!networkSrc.isGameHost && !networkSrc.isGameStarted){
				GUI.enabled = false; // Disabled the load button (start disabled)
			}
			
			// Load button
			if(GUI.Button(new Rect(loadButtonPosX, 20, loadButtonSizeX, loadButtonSizeY), loadGameButton)){				
				// If I click on button, and I'm the host, and the game is not started :
				if(networkSrc.isGameHost && !networkSrc.isGameStarted){
					// Display load panel on the others players
					networkView.RPC("LoadGame", RPCMode.All, networkSrc.gameMapKey);	
					if(useLoadTimer) { // If we use a load timer :
						// Display the load timer on the others players
						networkView.RPC("PrivateChatMessage", RPCMode.All, gamePanelLoadMessage);	
					}
				} else {
					// Else : if I'm not the host, but I have the load button enabled :
					// = if I join a started game
					networkSrc.gameMapName = maps[networkSrc.gameMapKey]; // Save the map
					networkSrc.StartGame(true); // Load the game for me
				}
				isLoading = true;
			}
			
			// If I'm not the host, and the game is not started :
			if(!networkSrc.isGameHost && !networkSrc.isGameStarted){
				GUI.enabled = true;// Disabled the load button (end disabled)
			}
			
		} else {
			GUI.enabled = false;
			GUI.Button(new Rect(loadButtonPosX, 20, loadButtonSizeX, loadButtonSizeY), loadGameCount);
			GUI.enabled = true;	
		}	
		
		GUI.EndGroup();	
		
		// If GUI changed and if we are the host and if the game is not started
		 if(GUI.changed && networkSrc.isGameHost && !networkSrc.isGameStarted){
			// Send the map modification to the others players
			networkView.RPC("ChangeMap", RPCMode.Others, networkSrc.gameMapKey);
		}
	}	

	// Display the loadPanel
	private IEnumerator LoadPanel(){
		for(int i = loadTimerTime; i > 0; i--){	
			yield return new WaitForSeconds(1);		
			loadGameCount = i.ToString();
			if(useLoadChatTimer){		
				PrivateChatMessage(i.ToString());	
			}
			
		}		
		yield return new WaitForSeconds(1);	
		// When the timer is finish : load the game
		networkSrc.StartGame(true);
	}
	
	// Send message only for me
	[RPC]
	void PrivateChatMessage(string message){
		networkSrc.chatContent.Add (message);
		chatScroll = new Vector2(0, (20*(networkSrc.chatContent.Count+1)) - chatContentSizeY);
	}
	
	// Send message from
	private void ChatMessage(string message){
		networkSrc.SendChatMessage(message);
	}

	// Change the map : call from server to clients when a choose a new map
	[RPC]
	void ChangeMap(int newMapKey){
		networkSrc.gameMapKey = newMapKey;
		networkSrc.gameMapName = maps[newMapKey];
	}
	
	// Load the game : call from server to the clients when he click on "Start game+
	[RPC]
	void LoadGame(int newMapKey){
		networkSrc.gameMapKey = newMapKey;
		networkSrc.gameMapName = maps[newMapKey];
		isLoading = true;
		if(useLoadTimer) { // If we use timer : start it
			StartCoroutine(LoadPanel());
		} else { // Else : start the game directly
			networkSrc.StartGame(true);
		}
	}
}
