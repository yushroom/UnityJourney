using UnityEngine;
using System;
using MultiPlayer;

public class MMenu : MonoBehaviour {
	
	/******************* GLOBAL PARAMETERS ***********************/	
	public bool canJoinStartedGame; 	
	public string waitRoomName = "WaitRoom"; // waitroom scene	
	public int defaultPort = 25565; // default port	
	
	/********* LAN & ONLINE PARAMETERS **************/
	public bool useOnline = false;
	public bool useLan = false;
	public bool lanUseJoinIP = false;
	public bool lanUseList = false;
	
	// USE IT ONLY IF YOU HAVE MULTILAN : 
	//private MLMenu menuLan;
	
	// USE IF ONLY IF YOU HAVE MULTIONLINE : 
	private MOMenu menuOnline;
		
	/*********** SERVER PARAMETERS ***************/
	public int serverGameId;
	public string serverGameName;
	public int serverPort;	
	public int serverMaxPlayer;
	public int serverTotalPlayer;
	public string serverGameRegister;
	public string serverGameRegisterDate;
	public bool serverUsePassword;
	public string serverPassword;
	public bool serverIsPrivateGame;
	public bool serverIsOnlineGame;
	public bool serverNetworkHost;	
	public string serverConnectIp;	

	/************ SCRIPTS AND OBJECTS**************/
	public GameObject networkPrefab;	
	public MNetwork networkSrc;	
	public GameObject networkObj;
	
	public GameObject playerDataPrefab;
	public MPlayerData playerDataSrc;
	public GameObject playerDataObj;	
			
	
	/************* GUI PARAMETERS **************/
	// --- Size parameters
	public int sizeMenuButtonX = 150;
	public int sizeMenuButtonY = 40;
	
	public int sizeSubMenuButtonX = 125;
	public int sizeSubMenuButtonY = 40;
	
	public int sizePageButtonX = 150;
	public int sizePageButtonY = 40;
				
	public int sizeTitleX = 250;
	public int sizeTitleY = 30;
				
	public int sizeButtonX = 100;
	public int sizeButtonY = 20;
	public int sizeFieldX = 200;
	public int sizeSmallFieldX = 60;
	public int sizeFieldY = 20;		
	public int sizeLabelX = 150;
	public int sizeLabelY = 30;
		
	public int menuSizeX;
	public int menuSizeY;
	public float menuPosX = 10;
	public float menuPosY = 10;
	public Rect mainMenu;	
					
	public int menuPageSizeX;
	public int menuPageSizeY;
	public float menuPagePosX;
	public float menuPagePosY = 10;	
	public Rect menuPage;		
	public int sizeY;	
	
	public int backMenuButtonY;
			
	/******************  MESSAGES ******************/	
	public string waitMessage = "... Wait...";	
	public string errorIncompleteFields = "please fill in all fields. ";	
	public string createChooseTxt ="Choose the game type :";
	public string[] networkJoinMessage;
	public string[] networkCreateMessage;
	
	/********************* MAIN MENU BUTTONS ***********************/
	public string menuTitle = "MENU";	
	public string menuProfilButton = "Profil";
	public string profilTitle = "PROFIL";
	public string menuMultiButton = "Multiplayer";
	public string mutliTitle = "MUTLIPLAYER";
	public string menuExitButton = "Exit";
	public bool profilButton = false;
	public bool multiButton = false;
	public bool createLanGame = false;
	public bool createOnlineGame = false;
	
	/****** SPECIAL BUTTON FOR USING OF MULTLAN & MULTIONLINE ******/
	public string[] subMenuProfiButtons = new string[2]{"Player", "Online account"};
	public int subMenuProfiButton;
	
	public string[] multiButtonsOptions = new string[4]{"Online games", "Network games", "Join game from IP", "Create game"};	
	public string[] subMenuMultiButtons;	
	public int subMenuMultiButton;
	
	public string multiCreateLanButton = "NETWORK GAME";
	public string multiCreateOnlineButton = "ONLINE GAME";
	
	void Start () {	
		networkJoinMessage = new string[2]{null,null};
		networkCreateMessage = new string[2]{null,null};
		
		// USE IT ONLY IF YOU HAVE MULTILAN : 
		// Search if we've got the MLMenu script
		/*try{
			menuLan = this.GetComponent<MLMenu>();
			if(menuLan != null && menuLan.enabled != false){
				// If we have it :
				useLan = true; // Put useLan on true
				lanUseJoinIP = menuLan.useJoinIP;
				lanUseList = menuLan.useNetworkGames;
			}
		} catch(NullReferenceException){}	*/
		
		// USE IF ONLY IF YOU HAVE MULTIONLINE : 
		// Search if we've got the MOMenu script
		try{
			menuOnline = this.GetComponent<MOMenu>();
			if(menuOnline != null && menuOnline.enabled != false){
				// If we have it :
				useOnline = true; // Put useOnline on true			
			}
		} catch(NullReferenceException){}
		
		
		if(useOnline && useLan){								
			if(!lanUseJoinIP && lanUseList){
				subMenuMultiButtons = new string[3]{multiButtonsOptions[0], multiButtonsOptions[1], multiButtonsOptions[3]};
			// If we don't use the menu "View network games"
			} else if(lanUseJoinIP && !lanUseList){		
				subMenuMultiButtons = new string[3]{multiButtonsOptions[0], multiButtonsOptions[2], multiButtonsOptions[3]};	
			// If we don't use the menus "Join game from IP" and  "View network games"
			}else if(!lanUseJoinIP && !lanUseList){		
				subMenuMultiButtons = new string[2]{multiButtonsOptions[0], multiButtonsOptions[3]};		
			} else {
				subMenuMultiButtons = new string[4]{multiButtonsOptions[0], multiButtonsOptions[1], multiButtonsOptions[2], multiButtonsOptions[3]};		
			}			
			
		} else if(useLan && !useOnline){
			if(!lanUseJoinIP && lanUseList){
				subMenuMultiButtons = new string[2]{multiButtonsOptions[1], multiButtonsOptions[3]};		
			// If we don't use the menu "View network games"
			} else if(lanUseJoinIP && !lanUseList){		
				subMenuMultiButtons = new string[2]{multiButtonsOptions[2], multiButtonsOptions[3]};		
			// If we don't use the menus "Join game from IP" and  "View network games"
			}else if(!lanUseJoinIP && !lanUseList){		
				subMenuMultiButtons = new string[1]{multiButtonsOptions[3]};	
			} else {
				subMenuMultiButtons = new string[3]{multiButtonsOptions[1], multiButtonsOptions[2], multiButtonsOptions[3]};	
			}	
		} else if(!useLan && useOnline){
			subMenuMultiButtons = new string[2]{multiButtonsOptions[0], multiButtonsOptions[3]};	
		}
	}	
	
	void OnGUI(){
		
		// Define GUI positions settings
		menuSizeX = sizeMenuButtonX+20;
		menuSizeY = Screen.height-20;
		mainMenu = new Rect(menuPosX, menuPosY, menuSizeX, menuSizeY);
		menuPageSizeX = Screen.width - menuSizeX - 30;
		menuPageSizeY = Screen.height-20;
		menuPagePosX = menuSizeX + 20;
		menuPage = new Rect(menuPagePosX, menuPagePosY, menuPageSizeX, menuPageSizeY);	
		backMenuButtonY = menuSizeY-sizeMenuButtonY-10;
		
		// SMEBAR MENU
		GUI.BeginGroup(mainMenu, "");
		GUI.Box(new Rect(0,0,menuSizeX, menuSizeY), menuTitle);
		int sizeY = 40;		
		
		// VIEW PROFIL				
		if(GUI.Button(new Rect(10,sizeY,sizeMenuButtonX,sizeMenuButtonY), menuProfilButton)){
			profilButton = true;
			multiButton = false;
			createLanGame = false;
			createOnlineGame = false;
		} 
		sizeY+=sizeMenuButtonY+5;		
			
		// VIEW MUTLIPLAYER
		if(GUI.Button(new Rect(10,sizeY,sizeMenuButtonX,sizeMenuButtonY), menuMultiButton)){
			multiButton = true;
			profilButton = false;
			createLanGame = false;
			createOnlineGame = false;
		} 
		sizeY+=sizeMenuButtonY+5;					
		
			// EXIT
		if(GUI.Button(new Rect(10,sizeY,sizeMenuButtonX,sizeMenuButtonY), menuExitButton)){
			Application.Quit();
		} 
		GUI.EndGroup();			
		
		// USE IF ONLY IF YOU HAVE MULTILAN :
		// Display MultiLan menu		
		/*if(menuLan != null && useLan){		
			menuLan.DisplayMenu();
		}*/
		
		// USE IF ONLY IF YOU HAVE MULTIONLINE : 
		// Display MultiOnline menu
		if(menuOnline != null && useOnline){
			menuOnline.DisplayMenu();
		}
	}
		
	// START NETWORK
	// Instantiate the server by the networkPrefab object to create or join a party
	public void StartNetwork(bool isGameHost){	
		if(Network.peerType != NetworkPeerType.Disconnected){
			Network.Disconnect();
		}
		
		if(networkObj != null) {
			networkSrc = networkObj.GetComponent<MNetwork>();
			networkSrc.CleanGame();
			Destroy(networkObj);
		}		
		
		networkObj = Instantiate(networkPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		networkSrc = networkObj.GetComponent<MNetwork>();	
		
		networkSrc.isOnlineGame = serverIsOnlineGame;
		networkSrc.gameHostId = playerDataSrc.id;
		networkSrc.gameId = serverGameId;		
		networkSrc.gameName = serverGameName;
		networkSrc.gameMaxPlayer = serverMaxPlayer;
		networkSrc.gameTotalPlayer = serverTotalPlayer;
		networkSrc.gameRegister = serverGameRegister;
		networkSrc.gameRegisterDate = serverGameRegisterDate;
		networkSrc.isGameUsePassword = serverUsePassword;
		networkSrc.gamePassword = serverPassword;
		networkSrc.gamePort = serverPort;	
		networkSrc.waitRoomName = waitRoomName;
		networkSrc.canJoinStartedGame = canJoinStartedGame;	
		networkSrc.isGamePrivate = serverIsPrivateGame;
		
		if(isGameHost) {				
			networkSrc.isGameHost = true;	
		} else {			
			networkSrc.gameHostConnectIp = serverConnectIp;	
			networkSrc.isGameHost = false;	
			networkSrc.isOnNetwork = serverNetworkHost;		
		}	
	}	
	
	public void LanGetNetworkGames(bool arg){
		// USE IT ONLY IF YOU HAVE MULTILAN
		/*if(useLan && menuLan != null){
			menuLan.GetNetworkGames(arg);
		}*/
	}
		
}
