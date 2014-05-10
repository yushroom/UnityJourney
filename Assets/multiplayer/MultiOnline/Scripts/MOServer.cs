using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;

namespace MultiPlayer {
	public class MOServer  {
	
		// Main URL with a slash in the end
		// Example : http://www.yourdomaine.com/
		// OR : http://www.yourdomaine.com/folder/
		public string url = "URL OF YOUR WEBSITE";
		// Register user URL :
		public string registerUrl = "saveUser.php";
		// Login user URL :
		public string logUrl = "logUser.php";
		// Forgot Login user URL :
		public string forgotLoginUrl = "forgotLogin.php";
		// Logout user URL :
		public string logoutUrl = "logoutUser.php";
		// Change user profil URL :
		public string changeProfilUrl = "changeProfil.php";
		// Register new game URL :
		public string registerGameUrl = "saveGame.php";
		// Update game parameters URL :
		public string updateGameUrl = "updateGame.php";
		// Search game URL :
		public string searchGameUrl = "searchGame.php";
		// Exit game URL :
		public string exitGameUrl = "exitGame.php";
			
		// User data
		public string userName;
		public string userMail;
		public string userId;
		public string userPublicIP;
		public string loginKey;
		public string userGameStatus;
		
		// Game data
		public string gameId;
		public string gameTotalPlayer;
		public string gameStatus;		
		public string gameRegister;
		public string gameRegisterDate;
		
		// Game list
		public List<MGame> gameList;		
		private bool success=false;
		public string[] errorMessages;
		
		// Save a new user on the dataBase
		public IEnumerator SaveUser(string userName, string mail, string pass, bool sendMail, MOMenu menuSrc){
			string mailParam = "";
			if(sendMail){
				mailParam ="&sendMail=1";
			}
			
			WWW www = new WWW(url+registerUrl+"?userName="+System.Uri.EscapeUriString(userName)
				+"&mail="+System.Uri.EscapeUriString(mail)+"&pass="+System.Uri.EscapeUriString(pass)
				+"&privateIP="+System.Uri.EscapeUriString(Network.player.ipAddress)+mailParam);
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}
				menuSrc.SaveUserDone(success);
			} 
		}
		
		// Save a new user on the dataBase
		public IEnumerator ForgotLogin(string mail, bool forgotUserName, bool forgotPass, MOMenu menuSrc){
			string sendUserName = "0";
			if(forgotUserName){
				sendUserName = "1";
			}
			
			string newPass = "0";
			if(forgotPass){
				newPass = "1";
			}
			
			WWW www = new WWW(url+forgotLoginUrl+"?mail="+System.Uri.EscapeUriString(mail)
				+"&sendUserName="+System.Uri.EscapeUriString(sendUserName)
				+"&forgotPass="+System.Uri.EscapeUriString(newPass));
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}
				menuSrc.ForgotLoginDone(success);
			} 
		}
		
		
		// Try to login user (check login informations on the dataBase)
		public IEnumerator LogUser(string userName, string pass, MOMenu menuSrc){
			WWW www = new WWW(url+logUrl+"?userName="+System.Uri.EscapeUriString(userName)
				+"&pass="+System.Uri.EscapeUriString(pass)+"&privateIP="+System.Uri.EscapeUriString(Network.player.ipAddress));
			yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				string[] text = trimText.Split('|');
				if(text[0] == "Success") {
					this.userId = text[1];
					this.userName = text[2];	
					this.userMail = text[3];
					this.userPublicIP = text[4];
					this.loginKey = text[5];					
					success = true;			
				} else {
					errorMessages = text;
					success = false;	
				}
				menuSrc.LogUserDone(success);
			} 
		}
		
		// Logout user on the dataBase
		public IEnumerator LogOut(int id, string loginKey){
			WWW www = new WWW(url+logoutUrl+"?id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey));
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}
			}
		}
		
		// Modify the user's userName or e-mail on the dataBase
		public IEnumerator ChangeUserData(int id, string loginKey, string username, string mail, MOMenu menuSrc){
			WWW www = new WWW(url+changeProfilUrl+"?changeData=1&id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey)
				+"&username="+System.Uri.EscapeUriString(username)
				+"&mail="+System.Uri.EscapeUriString(mail));
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}
				menuSrc.ChangeProfilDone(success);
			}
		}
		
		// Modify the user's password on the dataBase
		public IEnumerator ChangeUserPassword(int id, string loginKey, string currentPassword, string newPassword, MOMenu menuSrc){
			WWW www = new WWW(url+changeProfilUrl+"?changePass=1&id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey)
				+"&currentPassword="+System.Uri.EscapeUriString(currentPassword)
				+"&newPassword="+System.Uri.EscapeUriString(newPassword));
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}
				menuSrc.ChangePasswordDone(success);
			}
		}
		
		// Register a new game on the dataBase
		public IEnumerator RegisterGame(int id, string loginKey,string gameName,int port,int maxPlayer,bool usePass, MOMenu menuSrc){			
			string pass;
			if(usePass){
				pass = "1";
			} else {
				pass = "0";
			}			
			WWW www = new WWW(url+registerGameUrl+"?id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey)
				+"&gameName="+System.Uri.EscapeUriString(gameName)
				+"&gamePort="+System.Uri.EscapeUriString(port.ToString())
				+"&maxPlayer="+System.Uri.EscapeUriString(maxPlayer.ToString())
				+"&usePass="+System.Uri.EscapeUriString(pass));
			 yield return www;
			 if(www.isDone){			
				string TrimText = www.text.Trim();
				string[] text = TrimText.Split('|');
				if(text[0] == "Success") {						
					success = true;	
					this.gameId = text[1];
					this.gameTotalPlayer = text[2];
					this.gameStatus = text[3];
					this.gameRegister = text[4];
					this.gameRegisterDate = text[5];
				} else {
					errorMessages = text;
					success = false;	
				}
				menuSrc.RegisterGameDone(success);
			}
		}
		
		// Exit game : logout a use and make he quit the games where he is register on the dataBase
		// If he is the last player on a game (so the host), it destroy the game on the fataBase
		public IEnumerator ExitGame(int id, string loginKey, bool logout){
			string logoutParam = null;
			if(logout){
				logoutParam = "&logout=1";
			}
			
			WWW www = new WWW(url+exitGameUrl+"?id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey)+logoutParam);
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}							
			}
		} 
		
		// Search open games on the dataBase
		public IEnumerator SearchGames(int id, string loginKey, bool canJoinStartedGame, MOMenu menuSrc){			
			gameList = new List<MGame>();
			
			WWW www = new WWW(url+searchGameUrl+"?id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey));
	        yield return www;
			if(www.isDone){				
				string trimText = www.text.Trim();
				string[] text = trimText.Split('|');
				if(text[0] != "ERROR") {
					ParseGames(trimText, canJoinStartedGame, menuSrc);
				} else {
					errorMessages = trimText.Split('|');
					menuSrc.SearchGamesDone(false);		
				}		
			}
		}
		
		// If SearchGames success : parse the XML result and put it on the gameList List
		private void ParseGames(string XMLData, bool canJoinStartedGame, MOMenu menuSrc){
			XmlDocument doc = new XmlDocument();				
			try {
				doc.LoadXml(XMLData);
			} catch(XmlException){
				return;
			}				
			
			XmlNodeList nodeList = doc.GetElementsByTagName("game");
			for(int i = 0; i < nodeList.Count; i++) {	
				if(canJoinStartedGame || 
					(!canJoinStartedGame &&
					nodeList[i].SelectNodes("status").Item(0).InnerText.Equals("0"))) {				
					MGame currentGame = new MGame(
						nodeList[i].Attributes["id"].Value, 
						nodeList[i].SelectNodes("name").Item(0).InnerText,
						nodeList[i].SelectNodes("port").Item(0).InnerText,
						nodeList[i].SelectNodes("currentPlayers").Item(0).InnerText,
						nodeList[i].SelectNodes("maxPlayer").Item(0).InnerText,
						nodeList[i].SelectNodes("usePass").Item(0).InnerText,
						nodeList[i].SelectNodes("status").Item(0).InnerText,
						nodeList[i].SelectNodes("register").Item(0).InnerText,
						nodeList[i].SelectNodes("registerDate").Item(0).InnerText,
						nodeList[i].SelectNodes("hostId").Item(0).InnerText,
						nodeList[i].SelectNodes("hostName").Item(0).InnerText,
						nodeList[i].SelectNodes("hostPrivateIp").Item(0).InnerText,
						nodeList[i].SelectNodes("hostPublicIp").Item(0).InnerText,
						true);
					gameList.Add(currentGame);
				}
			}
			menuSrc.SearchGamesDone(true);	
		}
		
		// Modify the status of the game in the dataBase
		public IEnumerator RefreshGameStatus(int id, string loginKey, int gameId, string gameStatus){
			WWW www = new WWW(url+updateGameUrl+"?changeStatus=1&id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey)
				+"&gameId="+System.Uri.EscapeUriString(gameId.ToString())
				+"&gameStatus="+System.Uri.EscapeUriString(gameStatus));
	        yield return www;
			if(www.isDone){
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}			
			}
		}
		
		// Add a new player in a game on the database
		public IEnumerator AddPlayerInGame(int id, string loginKey, int gameId){
			WWW www = new WWW(url+updateGameUrl+"?addPlayer=1&id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey)
				+"&gameId="+System.Uri.EscapeUriString(gameId.ToString()));
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}			
			}
		}
		
		// Add a new player in a game on the database
		public IEnumerator SaveRehostedGame(int id, string loginKey, int gameId, string gameName, int gamePort, int maxPlayer, bool usePass, bool isStarted, string register){
			string pass="";
			if(usePass){
				pass = "1";
			} else {
				pass = "0";
			}	
			
			string started ="";
			if(isStarted){
				started = "1";
			} else {
				started = "0";
			}
			WWW www = new WWW(url+registerGameUrl+"?saveRehosted=1&id="+System.Uri.EscapeUriString(id.ToString())
				+"&loginKey="+System.Uri.EscapeUriString(loginKey)
				+"&gameId="+System.Uri.EscapeUriString(gameId.ToString())
				+"&gameName="+System.Uri.EscapeUriString(gameName)
				+"&gamePort="+System.Uri.EscapeUriString(gamePort.ToString())
				+"&maxPlayer="+System.Uri.EscapeUriString(maxPlayer.ToString())
				+"&usePass="+System.Uri.EscapeUriString(pass)
				+"&started="+System.Uri.EscapeUriString(started)
				+"&register="+System.Uri.EscapeUriString(register));
	        yield return www;
			if(www.isDone){	
				string trimText = www.text.Trim();
				if(trimText == "Success") {						
					success = true;			
				} else {
					errorMessages = www.text.Split('|');
					success = false;	
				}			
			}
		}
	}		
}
		 