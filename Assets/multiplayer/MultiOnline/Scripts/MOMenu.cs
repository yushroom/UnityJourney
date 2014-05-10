using UnityEngine;
using System;
using System.Collections.Generic;
using MultiPlayer;
using Filter;
	
public class MOMenu : MonoBehaviour {
	
	/************ SCRIPTS AND OBJECTS**************/
	private MMenu m;		
	public MOServer server;		
	private bool isLogin;
	/***************** PARAMETERS ******************/
	public bool sendRegisterMail = false;
	public int sizeLoginButtonX = 100;
	public int sizeLoginButtonY = 25;
	
	
	/******************** LOGIN / REGISTER ***************************/
	// --- Text	
	public string[] loginButtons = new string[3]{"Login", "Regsiter", "Forgot login"};
	private int loginButton;	
	public string loginTxt = "You must be login to access on this menu";
	public string loginNameTxt = "UserName : ";
	public string loginMailTxt = "E-mail : ";
	public string loginPassTxt = "Password : ";
	public string loginPassConfTxt = "Confirm : ";
	public string loginLogButton = "Login";
	public string registerButton = "Regsiter";
	public string forgotLoginButton = "Receive login";
	public string loginaveDataTxt = " Remember me";
	private string loginMessage;
	private bool viewLoginMessage = false;
	private string registerMessage;
	private bool viewRegisterMessage = false;
	public string loginForgotMailTxt = "E-mail used on your account creation : ";
	public string forgotLoginNameTxt = " Forgot userName";
	public string forgotLoginPassTxt = " Forgot password";
	private string forgotLoginMessage;
	private bool viewForgotLoginMessage = false;
	// --- Form fields
	public string formLoginName;
	public string formLoginPass;
	public bool formLoginSaveData;
	public string formRegisterName;
	public string formRegisterMail;
	public string formRegisterPass;
	public string formRegisterPassConf;
	public string formForgotLoginMail;
	public bool formForgotLoginName;
	public bool formForgotLoginPass;
	// Error & info messages
	public string errorName = "Your userName must contain only alphanumeric characters or underscrores. ";
	public string errorPass = "Your password must contain only alphanumeric characters and underscores and at least 4 character. ";
	public string errorMail = "Invalid e-mail format. ";
	public string errorLoginPass = "Error on your password. ";
	public string errorLoginUserName = "Error on your userName. ";
	public string errorLoginAlreadyLog = "This player is already login. ";
	public string errorNameExist = "This userName is already used, please choose another. ";
	public string errorMailExist = "This e-mail address is already used, please choose another. ";
	public string errorPassConf = "Error on your password confirmation. ";
	public string loginFailed = "Login failed : ";
	public string registerFailed = "Register failed : ";
	public string forgotLoginFailed = "Error : ";
	public string registerSuccess = "Registration completed successfully. You can login now.";
	public string forgotLoginSuccessUserName = "You will receive an e-mail with your userName. ";
	public string forgotLoginSuccessPass = "You will receive an e-mail with your new password. ";
	public string errorLoginMail = "Error, there is not account with this e-mail. ";
	
	
	/******************** PROFIL ****************************/
	// --- Text	
	public string profilTitle = "PROFIL SETTINGS";
	public string profilSave = "Save";
	public string profilMailTxt = "E-mail: ";
	public string profilNameTxt = "UserName: ";
	public string profilMessage;
	public bool viewProfilMessage;
	// --- Form fields
	public string formProfilName;
	public string formProfilMail;
	// --- Error & info messages
	public string errorLoginData= "Login error, impossible to change profil data. ";
	public string profilSuccess = "Your profil have been successful modified. ";
	public string profilFailed = "Change profil failed : ";
	
	/******************* CHANGE PASSWORD *************************/
	// --- Text	
	public string passwordTilte = "CHANGE PASSWORD";
	public string passwordButton = "Save";
	public string passwordTxt = "Current password : ";
	public string passwordNewTxt = "New password : ";
	public string passwordNewConfTxt = "Confirm : ";
	public string passwordMessage;
	public bool viewPasswordMessage;
	//--- Form fields
	public string formPassword;
	public string formPasswordNew;
	public string formPasswordNewConf;
	// --- Error & info messages
	public string errorCurrentPass = "Error on you current password. ";
	public string errorNewPass = "Your new password must contain only alphanumeric characters and underscores and at least 4 character. ";
	public string errorNewPassConf = "Error on your new password confirmation. ";
	public string passwordSuccess = "Your password have been modified. ";
	public string passwordFailed = "The modification of your password failed : ";
		
	/********************* LOGOUT *******************************/
	// --- Text
	public string logoutTitle = "LOGOUT";
	public string logoutButton = "LOGOUT";
	/***************** MULTIPLAYER VIEW GAMES ****************************/	
	// --- Text
	public string viewTilte ="VIEW GAMES";
	public string viewMessage;
	public string viewErrorMessage;
	public string viewRefreshButton ="Refresh";
	public string viewHostTxt =" host by : ";
	public string viewPlayersTxt ="Players : ";
	public string viewStatusTxt =" | Status : ";
	public string viewPasswordTxt =" | Password : ";
	public string viewJoinButton ="JOIN";
	public string viewStatusWait ="waiting";
	public string viewStatusGame ="in game";
	// --- Form fields	
	public Vector2 viewScroll = Vector2.zero;
	public string formViewPass;
	public string[] formViewPassArray;
	// --- Messages
	public string viewSearch = "... Search games...";
	public string viewNoGame = "No game was found";
	public string viewFailed = " Games search failed : ";
	public string viewOneGame = "One game was found";
	public string viewManyGame = " games was found";	
	public string errorViewPassword = "invalid password";
	public string errorView = "Error : ";
	public string errorViewEmptyPassword ="enter the password for join this game";
	
	/***************** MULTIPLAYER  CREATE GAMES *************************/
	// ---Text
	public string createTilte ="CREATE GAME";
	public string createNameTxt = "Game name :";
	public string createPortTxt = "Port : ";
	public string createPlayersTxt = "Maximum players : ";
	public string createUsePassTxt = "use password : ";
	public string createPassTxt = "Password : ";
	public string createPassConfTxt = "Confirm : ";
	public string createButton = "Create game";
	public string createMessage;
	public bool viewCreateMessage;	
	// --- Form fields	
	public string formCreateName;
	public string formCreatePort;
	public string formCreatePlayers;
	public bool formCreateUsePass;
	public string formCreatePass;
	public string formCreatePassConf;
	// --- Error & info messages
	public string errorCreateFailed = "Error : "; 
	public string errorCreateName = "the game name must contain only alphanumeric characters or underscrores. ";
	public string errorCreatePort ="the port is not numeric types. ";	
	public string errorCreatePlayers = "the maximum players is not in numeric type. ";
	public string errorCreatePass ="the password must contain only alphanumeric characters and underscores and at least 4 character. ";
	public string errorCreatePassConf ="error on the password confirmation. ";
	
	private int sizeY;
	
	
	/*********************  FUNCTIONS **********************/
	// Use this for initialization
	void Start () {
		m = this.GetComponent<MMenu>();		
		server = new MOServer();
		
		// If we have already the playerData obj :
		try{
			m.playerDataObj =  GameObject.Find ("PlayerData(Clone)");
			m.playerDataSrc = m.playerDataObj.GetComponent<MPlayerData>();
			
			// If we are login :
			if(m.playerDataSrc.isLogin){
				isLogin = true;
				
				// Fill profil fields
				formProfilName= m.playerDataSrc.userName; 
				formProfilMail= m.playerDataSrc.mail;
				formLoginPass = "";		
				
				// Search games
				StartCoroutine(server.SearchGames(m.playerDataSrc.id, m.playerDataSrc.loginKey, m.canJoinStartedGame, this.GetComponent<MOMenu>()));
				viewMessage = viewSearch;
			}
		} catch(NullReferenceException){
			// ELSE :			
			// Instantiate an empty playerDataObj
			m.playerDataObj = Instantiate(m.playerDataPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			// Get playerDataObj script
			m.playerDataSrc = m.playerDataObj.GetComponent<MPlayerData>();
			
		}	
		
		//Fill the forms fields by the defalut/saved values
		formCreatePort = m.defaultPort.ToString();
		formLoginName = PlayerPrefs.GetString("playerUserName");
		formLoginPass = PlayerPrefs.GetString("PlayerPass");
		string playerSaveData = PlayerPrefs.GetString("PlayerSaveData");
		if(playerSaveData.Equals("true")){
			formLoginSaveData = true;
		} else {
			formLoginSaveData = false;
		}
	}
		
	// Logout function
	private void Logout(){
		// Logout the player on the web dataBase
		StartCoroutine(server.LogOut(m.playerDataSrc.id, m.playerDataSrc.loginKey)); 
		// Fill login form with the player's parameters (if he saved it)
		formLoginName = PlayerPrefs.GetString("playerUserName"); 
		formLoginPass = PlayerPrefs.GetString("PlayerPass");
		
		// Destroy the player data about online account
		m.playerDataSrc.isLogin = false; 
		m.playerDataSrc.isOnline = false; 
		m.playerDataSrc.id = 0;
		m.playerDataSrc.userName = null;
		m.playerDataSrc.isInGame = false;
		m.playerDataSrc.mail = null;
		m.playerDataSrc.loginKey = null;
		
		isLogin = false; // Turn isLogin on false
	
	}
	
	/****************** GUI DISPLAY FUNCTION **********************/
	// Call from the ONGui function of MMenu.cs
	
	// DisplayMenu : manage the display of all the MOMenu
	public void DisplayMenu(){		
		if(m.profilButton){ // If we clicked on "Profil" button
			viewErrorMessage=null;
			viewCreateMessage=false;						
			DisplayProfil(); // Display profil
			
		} else if(m.multiButton){ // If we clicked on "Multiplayer" button
			viewPasswordMessage=false;	
			viewProfilMessage=false;			
			DisplayMultiPlayer();// Display multiplayer				
		} 
	}
	
	// DisplayLogin : manage the display of the login panels
	private void DisplayLogin(){	
	
		GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,m.menuPageSizeY), loginTxt);
		sizeY+= 20;	
		loginButton = GUI.SelectionGrid(new Rect(20,sizeY,sizeLoginButtonX*loginButtons.Length,sizeLoginButtonY), loginButton, loginButtons, loginButtons.Length);
		sizeY+= m.sizeSubMenuButtonY+20;
		
		// LOGIN FORM
		if(loginButton == 0){
			viewRegisterMessage = false;
			viewForgotLoginMessage = false;
			DispalyPlayerLogin();
		// REGISTER FORM
		} else 	if(loginButton == 1){
			viewLoginMessage = false;
			viewForgotLoginMessage = false;
			DisplayRegister();
		} else if(loginButton == 2){
			viewLoginMessage = false;
			viewRegisterMessage = false;
			DisplayForgotLogin();
		}
	}
	
	// Display login form GUI
	private void DispalyPlayerLogin(){		
			
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), loginNameTxt);
		formLoginName = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formLoginName);
		sizeY+= 40;
			
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), loginPassTxt);
		formLoginPass = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formLoginPass, '*');
		sizeY+= 30;
		formLoginSaveData = GUI.Toggle(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formLoginSaveData, loginaveDataTxt);
		sizeY+= 30;
		if(GUI.Button(new Rect(m.sizeLabelX, sizeY, m.sizeButtonX, m.sizeButtonY),loginLogButton)) {
			if(CheckLogin()){
				loginMessage = m.waitMessage;
				StartCoroutine(server.LogUser(formLoginName, formLoginPass, this.GetComponent<MOMenu>()));						
				if(formLoginSaveData){
					// Save player name
					PlayerPrefs.SetString("playerUserName", formLoginName);
					PlayerPrefs.SetString("PlayerPass", formLoginPass);
					PlayerPrefs.SetString("PlayerSaveData", "true");
					PlayerPrefs.Save();
				} else {
					PlayerPrefs.SetString("playerUserName", "");
					PlayerPrefs.SetString("PlayerPass", "");
					PlayerPrefs.SetString("PlayerSaveData", "false");
					PlayerPrefs.Save();
				}
			}
		}
		if(viewLoginMessage){
			sizeY+= 40;
			GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,200), loginMessage);
		}
	}
	
	// DisplayRegister GUI
	private void DisplayRegister(){
		
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), loginNameTxt);
		formRegisterName = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formRegisterName);
		sizeY+= 40;
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), loginMailTxt);
		formRegisterMail = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formRegisterMail);
		sizeY+= 40;
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), loginPassTxt);
		formRegisterPass = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formRegisterPass, '*');
		sizeY+= 40;
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), loginPassConfTxt);
		formRegisterPassConf = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formRegisterPassConf, '*');
		sizeY+= 40;
		
		if(GUI.Button(new Rect(m.sizeLabelX, sizeY, m.sizeButtonX, m.sizeButtonY), registerButton)) {		
			if(CheckRegister()){					
				registerMessage = m.waitMessage;	
				StartCoroutine(server.SaveUser(formRegisterName, formRegisterMail, formRegisterPass, sendRegisterMail, this.GetComponent<MOMenu>()));
			} 
		}
		if(viewRegisterMessage){
			sizeY+= 40;
			GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,200), registerMessage);
		}
	}
	
	// Display frogot login panel
	private void DisplayForgotLogin(){
		GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,m.sizeLabelY), loginForgotMailTxt);
		sizeY+= 20;
		formForgotLoginMail = GUI.TextField(new Rect(20,sizeY,m.sizeFieldX,m.sizeFieldY), formForgotLoginMail);
		sizeY+= 30;
		
		formForgotLoginName = GUI.Toggle(new Rect(20,sizeY,m.sizeFieldX,m.sizeFieldY), formForgotLoginName, forgotLoginNameTxt);
		sizeY+= 30;
		
		formForgotLoginPass = GUI.Toggle(new Rect(20,sizeY,m.sizeFieldX,m.sizeFieldY), formForgotLoginPass, forgotLoginPassTxt);
		sizeY+= 30;
		
		if(GUI.Button(new Rect(20, sizeY, m.sizeButtonX, m.sizeButtonY),forgotLoginButton)) {
			if(MFilter.CheckMail(formForgotLoginMail) && (formForgotLoginName || formForgotLoginPass)){			
				StartCoroutine(server.ForgotLogin(formForgotLoginMail, formForgotLoginName, formForgotLoginPass, this.GetComponent<MOMenu>()));						
			} else {				
				viewForgotLoginMessage=true;
				forgotLoginMessage=forgotLoginFailed;
				if(!MFilter.CheckMail(formForgotLoginMail)){
					forgotLoginMessage+= errorMail;
				} 
				if(!formForgotLoginName && !formForgotLoginPass){
					forgotLoginMessage+= m.errorIncompleteFields;
				}
			}
		}
		if(viewForgotLoginMessage){
			sizeY+= 40;
			GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,200), forgotLoginMessage);
		}
	}
	
	
	// DisplayProfil GUI, managne the display of the profil menu panels
	private void DisplayProfil(){		
		if(!m.useLan || (m.useLan && m.subMenuProfiButton == 1)){	
			
			GUI.BeginGroup(m.menuPage, "");
			GUI.Box(new Rect(0,0,m.menuPageSizeX, m.menuPageSizeY), m.profilTitle);
			sizeY = 40;	
			if(m.useLan && m.subMenuProfiButton == 1){			
				m.subMenuProfiButton = GUI.SelectionGrid(new Rect(20,sizeY,m.sizeSubMenuButtonX*m.subMenuProfiButtons.Length,m.sizeSubMenuButtonY), m.subMenuProfiButton, m.subMenuProfiButtons, m.subMenuProfiButtons.Length);			
				sizeY= m.sizeSubMenuButtonY+20;
				sizeY+= 40;	
			} 		
	
			if(isLogin){				
				DisplayProfilSettings();	
				sizeY+= 40;	
				DisplayPassword();
				sizeY+= 60;	
				DisplayLogout();
			} else {				
				DisplayLogin();
			}
		
			GUI.EndGroup();	
		}
	}
	
	// DisplayLogout GUI
	private void DisplayLogout(){
		if(GUI.Button(new Rect(20, sizeY, m.sizePageButtonX, m.sizePageButtonY), logoutButton)) {
			Logout();
		}
	}	
	
	// DisplayProfilSettings GUI
	private void DisplayProfilSettings(){		
		GUI.Label(new Rect(20,sizeY, m.sizeTitleX,m.sizeTitleY), profilTitle);	
		sizeY+= 40;
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), profilNameTxt);
		formProfilName = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formProfilName);
		sizeY+= 40;
		
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), profilMailTxt);
		formProfilMail = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formProfilMail);
		sizeY+= 40;
					
		if(GUI.Button(new Rect(m.sizeLabelX, sizeY, m.sizeButtonX, m.sizeButtonY), profilSave)) {
			if(CheckProfil()){				
				profilMessage=m.waitMessage;
				StartCoroutine(server.ChangeUserData(m.playerDataSrc.id, m.playerDataSrc.loginKey, formProfilName, formProfilMail, this.GetComponent<MOMenu>()));
			}
		} 		
		if(viewProfilMessage){
			sizeY+= 40;
			GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,200), profilMessage);
		}		
	}
	
	// DisplayPassword GUI
	private void DisplayPassword(){	
		GUI.Label(new Rect(20,sizeY, m.sizeTitleX,m.sizeTitleY), passwordTilte);	
		sizeY+= 40;
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), passwordTxt);
		formPassword = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formPassword, '*');
						
		sizeY+= 40;
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), passwordNewTxt);
		formPasswordNew = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formPasswordNew, '*');
		sizeY+= 40;
						
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), passwordNewConfTxt);
		formPasswordNewConf = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formPasswordNewConf, '*');
		sizeY+= 40;
						
		if(GUI.Button(new Rect(m.sizeLabelX, sizeY, m.sizeButtonX, m.sizeButtonY), passwordButton)) {
			if(CheckPassword()){				
				passwordMessage = m.waitMessage;
				StartCoroutine(server.ChangeUserPassword(m.playerDataSrc.id, m.playerDataSrc.loginKey, formPassword, formPasswordNew, this.GetComponent<MOMenu>()));
			} 					
			formPassword="";
			formPasswordNew="";
			formPasswordNewConf="";			
		}	
		if(viewPasswordMessage){
			sizeY+= 40;
			GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,200), passwordMessage);
		}

	}
	
	// DisplayMultiPlayer GUI, managne the display of the Multiplayer menu panels
	private void DisplayMultiPlayer(){
		// If we don't use MNetwork 
		// or if we use it and if we are on a MultiOnline menu
		if(!m.useLan || (m.useLan && 
			((m.lanUseList && m.lanUseJoinIP && (m.subMenuMultiButton==0 || m.subMenuMultiButton==3 && m.createOnlineGame))) ||
			((!m.lanUseList ^ !m.lanUseJoinIP) && (m.subMenuMultiButton==0 || m.subMenuMultiButton==2 && m.createOnlineGame)))
			){			
		
			GUI.BeginGroup(m.menuPage, "");
			GUI.Box(new Rect(0,0,m.menuPageSizeX, m.menuPageSizeY), m.mutliTitle);
			sizeY = 40;		
			
			if(m.useLan || isLogin) {							
				m.subMenuMultiButton = GUI.SelectionGrid(new Rect(20,sizeY,m.sizeSubMenuButtonX*m.subMenuMultiButtons.Length,m.sizeSubMenuButtonY), m.subMenuMultiButton, m.subMenuMultiButtons, m.subMenuMultiButtons.Length);
				sizeY+= m.sizeSubMenuButtonY+20;
			}	
			
			if(m.subMenuMultiButton == 0){	
				m.createLanGame = false;
				m.createOnlineGame = false;
				if(isLogin){
					viewCreateMessage=false;
					DisplayMultiView();
				} else {
					DisplayLogin();
				}
			} else if((m.subMenuMultiButton == 1 && m.subMenuMultiButtons[1].Equals(m.multiButtonsOptions[3]))
				|| (m.subMenuMultiButton == 2 && m.subMenuMultiButtons[2].Equals(m.multiButtonsOptions[3]))
				|| (m.subMenuMultiButton == 3 && m.subMenuMultiButtons[3].Equals(m.multiButtonsOptions[3]))){	
				if(isLogin){					
					viewErrorMessage=null;
					DisplayMultiCreate();
				} else {
					DisplayLogin();
				}
			}
			
			GUI.EndGroup();	
		}
			
	}
	
	// DisplayMultiView GUI
	private void DisplayMultiView(){		
		if(m.networkJoinMessage[1] != null && m.networkJoinMessage[0].Equals("True")){
			viewErrorMessage = m.networkJoinMessage[1];
			m.networkJoinMessage[0] = null;
			m.networkJoinMessage[1] = null;			
		}
		
		GUI.Label(new Rect(20,sizeY, m.sizeTitleX,m.sizeTitleY), viewTilte);	
		sizeY+= 30;		
		if(viewErrorMessage != null && viewErrorMessage!=""){
			GUI.Label(new Rect(20,sizeY, 400,m.sizeLabelY), viewErrorMessage);	
		} else {
			GUI.Label(new Rect(20,sizeY, 400,m.sizeLabelY), viewMessage);	
		}				
		sizeY+= 20;		
		if(GUI.Button(new Rect(20, sizeY, m.sizeButtonX, m.sizeButtonY), viewRefreshButton)){
			StartCoroutine(server.SearchGames(m.playerDataSrc.id, m.playerDataSrc.loginKey, m.canJoinStartedGame, this.GetComponent<MOMenu>()));
			viewMessage = viewSearch;			
		}	
		
		sizeY+= 40;	
		int pannelSizeY = (Screen.height-sizeY-40);
		bool useScroll = false;
		
		// If the gameList is not empty
		if(server.gameList != null){			
			if(((65*server.gameList.Count)-5) > pannelSizeY) {		
				useScroll = true;
				viewScroll = GUI.BeginScrollView (new Rect (0,sizeY,m.menuPageSizeX-20,pannelSizeY), viewScroll, new Rect(0,0, m.menuPageSizeX-60, (65*server.gameList.Count)-5));
				sizeY = 0;
			}		
			
			// Loop the gameList to display all the games
			for(int i = 0; i < server.gameList.Count; i++){					
				string status=null;
				if(server.gameList[i].status.Equals("1")){
					status = viewStatusGame;
				} else {
					status = viewStatusWait;
				}
				
				GUI.Box(new Rect(20,sizeY,m.menuPageSizeX-60, 60), "");
				GUI.Label(new Rect(30,sizeY+5, m.menuPageSizeX-60,m.sizeLabelY), 
					server.gameList[i].name+
					viewHostTxt+
					server.gameList[i].hostName);
				GUI.Label(new Rect(30,sizeY+m.sizeLabelY, m.menuPageSizeX-60,m.sizeLabelY), 
					viewPlayersTxt+
					server.gameList[i].totalPlayer+"/"+
					server.gameList[i].maxPlayer+
					viewStatusTxt+
					status);
				if(server.gameList[i].isUsePassword){
					GUI.Label(new Rect(220,sizeY+m.sizeLabelY, 80,20),viewPasswordTxt);	
					if(formViewPassArray[i] != null){
						formViewPassArray[i] = GUI.PasswordField(new Rect(220+80,sizeY+m.sizeLabelY, 80,20), formViewPassArray[i], '*');
					}
				}
				
				if(GUI.Button(new Rect(m.menuPageSizeX-60-100+15, sizeY+5, 100, 50), viewJoinButton)){
					if(CheckView(i)){
						m.serverIsOnlineGame = true;
						m.serverGameId = server.gameList[i].id;
						m.serverGameName = server.gameList[i].name;
						m.serverPort = server.gameList[i].port;	
						m.serverMaxPlayer = server.gameList[i].maxPlayer;
						m.serverTotalPlayer = server.gameList[i].totalPlayer;
						m.serverGameRegister = server.gameList[i].register;
						m.serverGameRegisterDate = server.gameList[i].registerDate;
						m.serverUsePassword = server.gameList[i].isUsePassword;
						m.serverPassword = formViewPassArray[i];
						
						if(server.gameList[i].hostPublicIp.Equals(m.playerDataSrc.publicIP)){
							m.serverConnectIp = server.gameList[i].hostPrivateIp;			
							m.serverNetworkHost = true;
						} else {			
							m.serverConnectIp = server.gameList[i].hostPublicIp;		
							m.serverNetworkHost = false;
						}					
						m.StartNetwork(false);						
					}					
				}
				sizeY+= 65;			
			}
			
			if(useScroll) {
				GUI.EndScrollView();
			}	
		}		
	}
	
	// DisplayMultiCreate GUI
	private void DisplayMultiCreate(){		
		GUI.Label(new Rect(20,sizeY, m.sizeTitleX,m.sizeTitleY), createTilte);	
		sizeY+= 40;	
		
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), createNameTxt);
		formCreateName = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formCreateName);
		sizeY+= 40;			
					
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), createPortTxt);			
		formCreatePort = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeSmallFieldX,m.sizeFieldY), formCreatePort);
		sizeY+= 40;		
					
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), createPlayersTxt);
		formCreatePlayers = GUI.TextField(new Rect(m.sizeLabelX,sizeY,m.sizeSmallFieldX,m.sizeFieldY), formCreatePlayers);
		sizeY+= 40;		
				
		GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), createUsePassTxt);
		formCreateUsePass = GUI.Toggle(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formCreateUsePass, "");
		sizeY+= 40;	
		if(formCreateUsePass){
			GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), createPassTxt);
			formCreatePass = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formCreatePass, '*');
			sizeY+= 40;	
			GUI.Label(new Rect(20,sizeY, m.sizeLabelX,m.sizeLabelY), createPassConfTxt);
			formCreatePassConf = GUI.PasswordField(new Rect(m.sizeLabelX,sizeY,m.sizeFieldX,m.sizeFieldY), formCreatePassConf, '*');
			sizeY+= 40;	
		}	
							
		if(GUI.Button(new Rect(m.sizeLabelX,sizeY, m.sizeButtonX,m.sizeButtonY), createButton)) {					
			if(CheckCreate()){				
				createMessage = m.waitMessage;				
				m.serverGameName = formCreateName;
				m.serverPort = int.Parse(formCreatePort);	
				m.serverMaxPlayer = int.Parse(formCreatePlayers);
				m.serverUsePassword = formCreateUsePass;
				m.serverPassword = formCreatePass;
				
				StartCoroutine(server.RegisterGame(
					m.playerDataSrc.id,m.playerDataSrc.loginKey,m.serverGameName,
					m.serverPort,m.serverMaxPlayer,m.serverUsePassword,this.GetComponent<MOMenu>()));
			}	
		}
		
		if(m.networkCreateMessage[1] != null && m.networkCreateMessage[1] != "" && m.networkCreateMessage[0].Equals("True")){
			viewCreateMessage = true;
			createMessage = m.networkCreateMessage[1];
			m.networkCreateMessage[0] = null;
			m.networkCreateMessage[1] = null;			
		}
		if(viewCreateMessage){
			sizeY+= 40;
			GUI.Label(new Rect(20,sizeY, m.menuPageSizeX,200), createMessage);
		}
	}
	

	/******************** CHECK FUNCTIONS *****************/
	//Check the forms before send it anywhere (call from the different Display functions)
	
	// Check the login form data
	private bool CheckLogin(){
		viewLoginMessage = true;
		loginMessage = loginFailed;
		
		if(!MFilter.CheckName(formLoginName)){			
			if(MFilter.error.Equals("empty")){
				loginMessage+= m.errorIncompleteFields;				
			} else {
				loginMessage+= errorName;
			}
			return false;
		}					
		if(!MFilter.CheckPass(formLoginPass)){
			if(MFilter.error.Equals("empty")){
				loginMessage+= m.errorIncompleteFields;
			} else {
				loginMessage+= errorPass;
			}
			return false;
		}
		loginMessage="";
		return true;
	}
	
	// Check the register form data
	private bool CheckRegister(){
		viewRegisterMessage = true;	
		registerMessage = registerFailed;
		
		if(!MFilter.CheckName(formRegisterName)){			
			if(MFilter.error.Equals("empty")){
				registerMessage+= m.errorIncompleteFields;
			} else {
				registerMessage+= errorName;
			}
			return false;
		}					
		if(!MFilter.CheckMail(formRegisterMail)){
			if(MFilter.error.Equals("empty")){							
				registerMessage+= m.errorIncompleteFields;
			} else {
				registerMessage+= errorMail;
			}
			return false;
		}
		if(!MFilter.CheckPass(formRegisterPass)){
			if(MFilter.error.Equals("empty")){
				registerMessage+= m.errorIncompleteFields;
			} else {
				registerMessage+= errorPass;
			}
			return false;
		}					
		if(!MFilter.CheckPass(formRegisterPassConf)){
			if(MFilter.error.Equals("empty")){
				registerMessage+= m.errorIncompleteFields;
			} else {
				registerMessage+= errorPass;
			}
			return false;
		}					
		if(!formRegisterPass.Equals(formRegisterPassConf)){
			registerMessage+= errorPassConf;
			return false;
		}
		registerMessage="";
		return true;
	}
	
	// Check the profil form data
	private bool CheckProfil(){
		viewPasswordMessage = false;
		viewProfilMessage = true;
		profilMessage = profilFailed;
		if(!MFilter.CheckName(formProfilName)){					
			if(MFilter.error.Equals("empty")){
				profilMessage+= m.errorIncompleteFields;
			} else {
				profilMessage+= errorName;
			}
			return false;
		}					
		if(!MFilter.CheckMail(formProfilMail)){
			if(MFilter.error.Equals("empty")){							
				profilMessage+= m.errorIncompleteFields;
			} else {
				profilMessage+= errorMail;
			}	
			return false;
		}
		profilMessage="";
		return true;
	}
	
	// Check the password form data
	private bool CheckPassword(){
		viewProfilMessage = false;
		viewPasswordMessage = true;
		passwordMessage=passwordFailed;
		
		if(!MFilter.CheckPass(formPassword)){
			if(MFilter.error.Equals("empty")){
				passwordMessage+= m.errorIncompleteFields;
			} else {
				passwordMessage+= errorCurrentPass;
			}
			return false;
		}
		if(!MFilter.CheckPass(formPasswordNew)){
			if(MFilter.error.Equals("empty")){
				passwordMessage+= m.errorIncompleteFields;
			} else {
				passwordMessage+= errorNewPass;
			}
			return false;
		}
		if(!MFilter.CheckPass(formPasswordNewConf)){
			if(MFilter.error.Equals("empty")){
				passwordMessage+= m.errorIncompleteFields;
			} else {
				passwordMessage+= errorNewPass;
			}
			return false;
		}
		if(!formPasswordNew.Equals(formPasswordNewConf)){
			passwordMessage+= errorNewPassConf;
			return false;
		}
		passwordMessage="";
		return true;
		
	}
	
	
		// Check the join form data (the password if there is one)
	private bool CheckView(int key){
		if(server.gameList[key].isUsePassword &&
			!MFilter.CheckPass(formViewPassArray[key])){
			viewErrorMessage = errorView;
			if(MFilter.error.Equals("empty")){
				viewErrorMessage+= errorViewEmptyPassword;
			} else {
				viewErrorMessage+= errorViewPassword;
			}
			return false;
		}
		return true;
	}
	
	
	
	private bool CheckCreate(){
		viewCreateMessage = true;
		if(!MFilter.CheckName(formCreateName)){
			if(MFilter.error.Equals("empty")){
				createMessage= m.errorIncompleteFields;
			} else {
				createMessage= errorCreateName;
			}
			return false;
		}
		if(!MFilter.CheckNumber(formCreatePort)){
			if(MFilter.error.Equals("empty")){
				createMessage= m.errorIncompleteFields;
			} else {
				createMessage= errorCreatePort;
			}
			return false;
		}
		if(!MFilter.CheckNumber(formCreatePlayers)){
			if(MFilter.error.Equals("empty")){
				createMessage= m.errorIncompleteFields;
			} else {
				createMessage= errorCreatePlayers;
			}
			return false;
		}
		if(formCreateUsePass && !MFilter.CheckPass(formCreatePass)){
			if(MFilter.error.Equals("empty")){
				createMessage= m.errorIncompleteFields;
			} else {
				createMessage= errorCreatePass;
			}
			return false;
		}
		if(formCreateUsePass && !formCreatePass.Equals(formCreatePassConf)){
			createMessage= errorCreatePassConf;
			return false;
		}
		return true;
	}
	

	/******************** DONE FUNCTIONS **************************/
	// Call from MOServer when the connexion with the website if finish
	
	// Call after login a user
	public void LogUserDone(bool success){
		viewLoginMessage=false;
		viewRegisterMessage=false;
		viewMessage="";
		if(success){	
			
			// Search if we have already the playerDataObj
			try{
				m.playerDataObj =  GameObject.Find ("PlayerData(Clone)");
				m.playerDataSrc = m.playerDataObj.GetComponent<MPlayerData>();
			} catch(NullReferenceException){
				// ELSE :
				// Instantiate playerDataObj
				m.playerDataObj = Instantiate(m.playerDataPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
				// Get playerDataObj script
				m.playerDataSrc = m.playerDataObj.GetComponent<MPlayerData>();
			}	
								
			// Fill m.playerDataSrc with the player's data
			m.playerDataSrc.id = int.Parse(server.userId);
			m.playerDataSrc.userName = server.userName;
			m.playerDataSrc.isInGame = false;
			m.playerDataSrc.mail = server.userMail;
			m.playerDataSrc.privateIP = Network.player.ipAddress;
			m.playerDataSrc.publicIP = server.userPublicIP;
			m.playerDataSrc.loginKey = server.loginKey;
			m.playerDataSrc.isOnline = true;
		
			// Turn m.playerDataSrc.isLogin and isLogin var on true
			m.playerDataSrc.isLogin = true; 
			isLogin = true; 
			
			// Fill profil fields
			formProfilName= m.playerDataSrc.userName; 
			formProfilMail= m.playerDataSrc.mail;
			formLoginPass = "";		
			
			// SEARCH ONLINE GAMES
			StartCoroutine(server.SearchGames(m.playerDataSrc.id, m.playerDataSrc.loginKey, m.canJoinStartedGame, this.GetComponent<MOMenu>()));
			viewMessage = viewSearch;
		
		} else {
			viewLoginMessage = true;
			loginMessage = loginFailed;
			for(int i = 0; i < server.errorMessages.Length; i++){
				if(server.errorMessages[i].Equals("errorPass")){
					loginMessage+= errorLoginPass;
				} else if(server.errorMessages[i].Equals("errorAlreadyLog")){
					loginMessage+= errorLoginAlreadyLog;
				} else if(server.errorMessages[i].Equals("errorName")){
					loginMessage+= errorLoginUserName;
				} else if(server.errorMessages[i].Equals("nameFilter")){
					loginMessage+= errorName;
				} else if(server.errorMessages[i].Equals("passFilter")){
					loginMessage+= errorPass;
				} else if(server.errorMessages[i].Equals("emptyName")){
					loginMessage+= m.errorIncompleteFields;
				} else if(server.errorMessages[i].Equals("emptyPass")){
					loginMessage+= m.errorIncompleteFields;
				}
			}
		}	
	}
	
	// Call after have change profil infos
	public void ChangeProfilDone(bool success){
		viewProfilMessage = true;
		profilMessage="";
		if(success){
			profilMessage = profilSuccess;
			m.playerDataSrc.userName = formProfilName;
			m.playerDataSrc.mail = formProfilMail;
		} else {	
			profilMessage = profilFailed;
			for(int i = 0; i < server.errorMessages.Length; i++){
				if(server.errorMessages[i].Equals("nameExist")){
					profilMessage+= errorNameExist;
				} else if(server.errorMessages[i].Equals("emptyName")){
					profilMessage+= m.errorIncompleteFields;
				} else if(server.errorMessages[i].Equals("errorName")){
					profilMessage+= errorName;
				} else if(server.errorMessages[i].Equals("mailExist")){
					profilMessage+= errorMailExist;
				} else if(server.errorMessages[i].Equals("emptyMail")){
					profilMessage+= m.errorIncompleteFields;
				} else if(server.errorMessages[i].Equals("errorMail")){
					profilMessage+= errorMail;
				} else if(server.errorMessages[i].Equals("badId")){
					profilMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("badLoginKey")){
					profilMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyId")){
					profilMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyLoginKey")){
					profilMessage+= errorLoginData;
				}
			}
		}
	}
	
		
	// Call after have register a new user
	public void SaveUserDone(bool success){
		viewRegisterMessage = true;
		registerMessage="";
		if(success){
			registerMessage = registerSuccess;
			formLoginName = formRegisterName;
			formLoginPass = formRegisterPass;
		} else {
			registerMessage = registerFailed;
			for(int i = 0; i < server.errorMessages.Length; i++){
				if(server.errorMessages[i].Equals("nameExist")){
					registerMessage+= errorNameExist;
				} else if(server.errorMessages[i].Equals("mailExist")){
					registerMessage+= errorMailExist;
				} else if(server.errorMessages[i].Equals("nameFilter")){
					registerMessage+= errorName;
				} else if(server.errorMessages[i].Equals("mailFilter")){
					registerMessage+= errorMail;
				} else if(server.errorMessages[i].Equals("passFilter")){
					registerMessage+= errorPass;
				} else if(server.errorMessages[i].Equals("emptyName")){
					registerMessage+= m.errorIncompleteFields;
				} else if(server.errorMessages[i].Equals("emptyMail")){
					registerMessage+= m.errorIncompleteFields;
				} else if(server.errorMessages[i].Equals("emptyPass")){
					registerMessage+= m.errorIncompleteFields;
				}
			}
		}	
	}
	
	// Call after have change the password of a user
	public void ChangePasswordDone(bool success){
		viewPasswordMessage = true;
		passwordMessage="";
		formPassword="";
		formPasswordNew="";
		formPasswordNewConf="";
		if(success){
			passwordMessage = passwordSuccess;
		} else {		
			passwordMessage=passwordFailed;
			for(int i = 0; i < server.errorMessages.Length; i++){
				if(server.errorMessages[i].Equals("errorPass")){
					passwordMessage+= errorCurrentPass;
				} else if(server.errorMessages[i].Equals("passFilter")){
					passwordMessage+= errorNewPass;
				} else if(server.errorMessages[i].Equals("badId")){
					passwordMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("badLoginKey")){
					passwordMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyId")){
					passwordMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyLoginKey")){
					passwordMessage+= errorLoginData;
				}
			}
		}
	}
	
	
	// Call after have send the informations when a user forgot his password or username
	public void ForgotLoginDone(bool success){
		viewForgotLoginMessage = true;
		if(success){
			if(formForgotLoginName){
				forgotLoginMessage = forgotLoginSuccessUserName;
			}
			if(formForgotLoginPass){
				forgotLoginMessage +=forgotLoginSuccessPass;
			}
			formForgotLoginMail = "";
			formForgotLoginName = false;
			formForgotLoginPass = false;
		} else {
			forgotLoginMessage=forgotLoginFailed;
			for(int i = 0; i < server.errorMessages.Length; i++){
				if(server.errorMessages[i].Equals("errorUserMail")){
					forgotLoginMessage+= errorLoginMail;
				} else if(server.errorMessages[i].Equals("mailFilter")){
					forgotLoginMessage+= errorMail;
				} else if(server.errorMessages[i].Equals("emptyMail")){
					forgotLoginMessage+= m.errorIncompleteFields;
				} 
			}
		}	
	}
	
	// Call after have search the gamelist
	public void SearchGamesDone(bool success){
		if(success){
			formViewPassArray = new string[server.gameList.Count];	
			for(int i = 0; i < formViewPassArray.Length; i++){
				formViewPassArray[i] = "";
			}			
			
			if(server.gameList.Count > 1){
				viewMessage = server.gameList.Count.ToString()+viewManyGame;
			} else {
				viewMessage = viewOneGame;
			}
		} else {
			for(int i = 0; i < server.errorMessages.Length; i++){			
				if(server.errorMessages[i].Equals("noGame")){
					viewMessage= viewNoGame;
				}else if(server.errorMessages[i].Equals("badId")){
					viewMessage= viewFailed+errorLoginData;
				} else if(server.errorMessages[i].Equals("badLoginKey")){
					viewMessage= viewFailed+errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyId")){
					viewMessage= viewFailed+errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyLoginKey")){
					viewMessage= viewFailed+errorLoginData;
				}
			}
		}
	}
	
	// Call after have create a new online game
	public void RegisterGameDone(bool success){
		viewCreateMessage = false;
		createMessage="";
		if(success){
			m.serverGameId = int.Parse(server.gameId);
			m.serverTotalPlayer = int.Parse(server.gameTotalPlayer);
			m.serverGameRegister = server.gameRegister;
			m.serverGameRegisterDate = server.gameRegisterDate;
			m.serverIsOnlineGame = true;
			m.serverIsPrivateGame = false;
			m.StartNetwork(true);			
		} else {
			viewCreateMessage = true;
			for(int i = 0; i < server.errorMessages.Length; i++){
				if(server.errorMessages[i].Equals("errorGameName")){
					createMessage+= errorCreateName;
				} else if(server.errorMessages[i].Equals("errorMaxPlayer")){
					createMessage+= errorCreatePlayers;
				} else if(server.errorMessages[i].Equals("errorPort")){
					createMessage+= errorCreatePort;
				} else if(server.errorMessages[i].Equals("emptyGameName")){
					createMessage+= m.errorIncompleteFields;
				}else if(server.errorMessages[i].Equals("emptyPort")){
					createMessage+= m.errorIncompleteFields;
				}else if(server.errorMessages[i].Equals("emptyGameType")){
					createMessage+= m.errorIncompleteFields;
				}else if(server.errorMessages[i].Equals("emptyMaxPlayer")){
					createMessage+= m.errorIncompleteFields;
				}else if(server.errorMessages[i].Equals("badId")){
					createMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("badLoginKey")){
					createMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyId")){
					createMessage+= errorLoginData;
				} else if(server.errorMessages[i].Equals("emptyLoginKey")){
					createMessage+= errorLoginData;
				}
				
			}
		}
	}

	
}
