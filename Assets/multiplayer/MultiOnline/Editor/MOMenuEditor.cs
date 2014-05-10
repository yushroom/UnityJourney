using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MOMenu))]
public class MOMenuEditor : Editor {
	private MOMenu edit;	
	private bool showSubmenu = false;
	private string[] loginButtons = new string[3]{"login", "register", "forgot login"};
	
	private bool showParameters = false;
	private bool showLogin = false;		
	private bool showProfil = false;
	private bool showPassword = false;
	private bool showGameList = false;	
	private bool showGameCreate = false;
	
	public void OnEnable(){
		edit = (MOMenu) target;	
	
	}
	
	public override void OnInspectorGUI () {
		EditorGUILayout.LabelField("ONLINE GAME MENU CONTENT", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Parameters", EditorStyles.boldLabel);
		showParameters = EditorGUILayout.Foldout(showParameters, "Show parameters");
		if(showParameters){
			edit.sendRegisterMail = EditorGUILayout.Toggle("Send registration e-mail" , edit.sendRegisterMail);
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Submenus", EditorStyles.boldLabel);
		showSubmenu = EditorGUILayout.Foldout(showSubmenu, "Show submenus content");
		if(showSubmenu) {		
			EditorGUILayout.LabelField("Login submenu", EditorStyles.boldLabel);
			for(int i = 0; i < edit.loginButtons.Length; i++){
				edit.loginButtons[i] = EditorGUILayout.TextField("Button \""+loginButtons[i]+"\": ", edit.loginButtons[i]);
			}			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Login & register panel", EditorStyles.boldLabel);
		showLogin = EditorGUILayout.Foldout(showLogin, "Show login & register panel content");
		if(showLogin){			
			EditorGUILayout.LabelField("Submenu buttons size", EditorStyles.boldLabel);
			edit.sizeLoginButtonX = EditorGUILayout.IntField("Button width: ", edit.sizeLoginButtonX);
			edit.sizeLoginButtonY = EditorGUILayout.IntField("Button height: ", edit.sizeLoginButtonY);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
			edit.loginTxt = EditorGUILayout.TextField("Login text: ", edit.loginTxt);
			edit.loginNameTxt = EditorGUILayout.TextField("UserName field: ", edit.loginNameTxt);
			edit.loginMailTxt = EditorGUILayout.TextField("E-mail field: ", edit.loginMailTxt);
			edit.loginForgotMailTxt = EditorGUILayout.TextField("E-mail field (forgot login): ", edit.loginForgotMailTxt);
			edit.loginPassTxt = EditorGUILayout.TextField("Pass field: ", edit.loginPassTxt);
			edit.loginPassConfTxt = EditorGUILayout.TextField("Confirm pass field: ", edit.loginPassConfTxt);
			edit.loginaveDataTxt = EditorGUILayout.TextField("Save login data box: ", edit.loginaveDataTxt);
			edit.forgotLoginNameTxt = EditorGUILayout.TextField("Forgot username box: ", edit.forgotLoginNameTxt);
			edit.forgotLoginPassTxt = EditorGUILayout.TextField("Save password box: ", edit.forgotLoginPassTxt);
			edit.loginLogButton = EditorGUILayout.TextField("Submit login button: ", edit.loginLogButton);
			edit.registerButton = EditorGUILayout.TextField("Submit register button: ", edit.registerButton);
			edit.forgotLoginButton = EditorGUILayout.TextField("Forgot login button: ", edit.forgotLoginButton);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Error & info messages", EditorStyles.boldLabel);
			edit.errorName = EditorGUILayout.TextField("Invalid userName format: ", edit.errorName);
			edit.errorPass = EditorGUILayout.TextField("Invalid password format: ", edit.errorPass);
			edit.errorMail = EditorGUILayout.TextField("Invalid e-mail format: ", edit.errorMail);
			edit.errorLoginPass = EditorGUILayout.TextField("Error on login password: ", edit.errorLoginPass);
			edit.errorLoginUserName = EditorGUILayout.TextField("Error on login userName: ", edit.errorLoginUserName );
			edit.errorLoginAlreadyLog = EditorGUILayout.TextField("User already login: ", edit.errorLoginAlreadyLog);
			edit.errorNameExist = EditorGUILayout.TextField("UserName already exists: ", edit.errorNameExist);
			edit.errorMailExist = EditorGUILayout.TextField("E-mail already exists: ", edit.errorMailExist);
			edit.errorPassConf = EditorGUILayout.TextField("Error on pass. confirm: ", edit.errorPassConf);
						edit.errorLoginMail = EditorGUILayout.TextField("Error on account email: ", edit.errorLoginMail);

			edit.loginFailed = EditorGUILayout.TextField("Login failed: ", edit.loginFailed);
			edit.registerFailed = EditorGUILayout.TextField("Register failed: ", edit.registerFailed);
			edit.registerFailed = EditorGUILayout.TextField("Send login info failed: ", edit.forgotLoginFailed);
			edit.forgotLoginFailed = EditorGUILayout.TextField("Register success: ", edit.registerSuccess);
			edit.forgotLoginSuccessUserName = EditorGUILayout.TextField("Sucess send username: ", edit.forgotLoginSuccessUserName);
			edit.forgotLoginSuccessPass = EditorGUILayout.TextField("Success send new pass ", edit.forgotLoginSuccessPass);
		
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Profil panel", EditorStyles.boldLabel);
		showProfil = EditorGUILayout.Foldout(showProfil, "Show profil panel content");
		if(showProfil){
			EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);			
			edit.profilTitle = EditorGUILayout.TextField("Title: ", edit.profilTitle);
			edit.profilNameTxt = EditorGUILayout.TextField("UserName field: ", edit.profilNameTxt);
			edit.profilMailTxt = EditorGUILayout.TextField("E-mail field: ", edit.profilMailTxt);
			edit.profilSave = EditorGUILayout.TextField("Save button: ", edit.profilSave);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Error & info messages", EditorStyles.boldLabel);
			edit.errorLoginData = EditorGUILayout.TextField("Error on login data: ", edit.errorLoginData);
			edit.profilSuccess = EditorGUILayout.TextField("Change profil success: ", edit.profilSuccess);
			edit.profilFailed = EditorGUILayout.TextField("Change profil failed: ", edit.profilFailed);

		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Change password panel", EditorStyles.boldLabel);
		showPassword = EditorGUILayout.Foldout(showPassword, "Show change password panel content");
		if(showPassword){
			EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
			edit.passwordTilte = EditorGUILayout.TextField("Title: ", edit.passwordTilte);
			edit.passwordTxt = EditorGUILayout.TextField("Current pass field: ", edit.passwordTxt);
			edit.passwordNewTxt = EditorGUILayout.TextField("New pass field: ", edit.passwordNewTxt);
			edit.passwordNewConfTxt = EditorGUILayout.TextField("Conf. new pass field: ", edit.passwordNewConfTxt);
			edit.passwordButton = EditorGUILayout.TextField("Save button: ", edit.passwordButton);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Error & info messages", EditorStyles.boldLabel);
			edit.errorCurrentPass = EditorGUILayout.TextField("Error on current pass: ", edit.errorCurrentPass);
			edit.errorNewPass = EditorGUILayout.TextField("Invalid new pass format: ", edit.errorNewPass);
			edit.errorNewPassConf = EditorGUILayout.TextField("Error on new pass conf.: ", edit.errorNewPassConf);
			edit.passwordSuccess = EditorGUILayout.TextField("Change pass success: ", edit.passwordSuccess);
			edit.passwordFailed = EditorGUILayout.TextField("Change pass failed: ", edit.passwordFailed);

		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("View games panel", EditorStyles.boldLabel);
		showGameList = EditorGUILayout.Foldout(showGameList, "Show view games panel content");
		if(showGameList){
			EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
			edit.viewTilte = EditorGUILayout.TextField("Title: ", edit.viewTilte);
			edit.viewRefreshButton = EditorGUILayout.TextField("Refresh button: ", edit.viewRefreshButton);
			edit.viewHostTxt = EditorGUILayout.TextField("Host name: ", edit.viewHostTxt);
			edit.viewPlayersTxt = EditorGUILayout.TextField("Player nbr: ", edit.viewPlayersTxt);
			edit.viewStatusTxt = EditorGUILayout.TextField("Game status: ", edit.viewStatusTxt);
			edit.viewStatusWait = EditorGUILayout.TextField("Game in waiting room txt: ", edit.viewStatusWait);
			edit.viewStatusGame = EditorGUILayout.TextField("Game in game text: ", edit.viewStatusGame);
			edit.viewPasswordTxt = EditorGUILayout.TextField("Password field: ", edit.viewPasswordTxt);
			edit.viewJoinButton = EditorGUILayout.TextField("Join game button: ", edit.viewJoinButton);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Error & info messages", EditorStyles.boldLabel);
			edit.viewSearch = EditorGUILayout.TextField("Searching games message: ", edit.viewSearch);
			edit.viewNoGame = EditorGUILayout.TextField("No game was found: ", edit.viewNoGame);
			edit.viewFailed = EditorGUILayout.TextField("Search failed: ", edit.viewFailed);
			edit.viewOneGame = EditorGUILayout.TextField("One game found: ", edit.viewOneGame);
			edit.viewManyGame  = EditorGUILayout.TextField("Many game found: ", edit.viewManyGame );
			edit.errorView = EditorGUILayout.TextField("Error: ", edit.errorView);
			edit.errorViewPassword  = EditorGUILayout.TextField("Error on password: ", edit.errorViewPassword );
			edit.errorViewEmptyPassword = EditorGUILayout.TextField("Password missing: ", edit.errorViewEmptyPassword);
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Create game panel", EditorStyles.boldLabel);
		showGameCreate = EditorGUILayout.Foldout(showGameCreate, "Show create game panel content");
		if(showGameCreate){
			EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
			edit.createTilte = EditorGUILayout.TextField("Title: ", edit.createTilte);
			edit.createNameTxt = EditorGUILayout.TextField("Game name field: ", edit.createNameTxt);
			edit.createPortTxt = EditorGUILayout.TextField("Port field: ", edit.createPortTxt);
			edit.createPlayersTxt = EditorGUILayout.TextField("Players field: ", edit.createPlayersTxt);
			edit.createUsePassTxt = EditorGUILayout.TextField("Use pass box: ", edit.createUsePassTxt);
			edit.createPassTxt = EditorGUILayout.TextField("Pass field: ", edit.createPassTxt);
			edit.createPassConfTxt = EditorGUILayout.TextField("Confirm pass field: ", edit.createPassConfTxt);
			edit.createButton = EditorGUILayout.TextField("Join game button: ", edit.createButton);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Error & info messages", EditorStyles.boldLabel);
			edit.errorCreateFailed = EditorGUILayout.TextField("Create game failed: ", edit.errorCreateFailed);
			edit.errorCreateName = EditorGUILayout.TextField("Invalid game name format: ", edit.errorCreateName);
			edit.errorCreatePort = EditorGUILayout.TextField("Invalid port format: ", edit.errorCreatePort);
			edit.errorCreatePlayers = EditorGUILayout.TextField("Invalid players format: ", edit.errorCreatePlayers);
			edit.errorCreatePass = EditorGUILayout.TextField("Invalid pass format: ", edit.errorCreatePass);
			edit.errorCreatePassConf  = EditorGUILayout.TextField("Error on pass confirm: ", edit.errorCreatePassConf );

		}
	}	
}
