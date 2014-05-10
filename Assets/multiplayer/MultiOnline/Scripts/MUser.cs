using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MultiPlayer {
	public class MUser {
		
		public int id;
		public string privateIp;
		public string publicIp;		
		public string playerName;
		public int isPlayerInGame;
		public int isGameHost;
		public bool canHost;
		
		/******************* CONSTRUCTORS *******************/
		// Constructor whitout parameters
		public MUser(){}
		
		// Constructor with variable in string 
		public MUser(string id, string privateIp, string publicIp, string playerName, string isPlayerInGame, string isGameHost, string canHost){
			this.id = int.Parse(id);
			this.privateIp = privateIp;
			this.publicIp = publicIp;
			this.playerName = playerName;		
			this.isPlayerInGame = int.Parse(isPlayerInGame);	
			this.isGameHost = int.Parse(isGameHost);
			if(canHost == "True"){
				this.canHost = true;
			} else {
				this.canHost = false;
			}
		}
		
		// Constructor with variable in real types 
		public MUser(int id, string privateIp, string publicIp, string playerName, int isPlayerInGame, int isGameHost, bool canHost){
			this.id = id;
			this.privateIp = privateIp;
			this.publicIp = publicIp;
			this.playerName = playerName;
			this.isPlayerInGame = isPlayerInGame;			
			this.isGameHost = isGameHost;
			this.canHost = canHost;
		}
		
					
		/***********************************************************/
		
		// Transform the MUser object parameters on a string
		public string UserToString(){
			return id.ToString()+"$"+privateIp+"$"+publicIp+"$"+playerName+"$"+isPlayerInGame+"$"+isGameHost.ToString()+"$"+canHost.ToString();
		}
		
		// Create an MUser object with parameters from a string
		public static MUser UserToObject(string parseUser){
			string[] values = parseUser.Split('$');
			return new MUser(values[0], values[1], values[2],values[3], values[4], values[5], values[6]);
		}
		
		// Transform a userList (List<MUser>) on a string
		public static string ListToString(List<MUser> userList){
			string values = "";
			for(int i = 0; i < userList.Count; i++){
				values+= userList[i].UserToString();
				if(i < userList.Count-1){
					values+="#";
				}				
			}
			return values;
		}
		
		// Create a userList (List<MUser>) from a string
		public static List<MUser> ListToObject(string parseUser){
			List<MUser> userList = new List<MUser>();
			string[] values = parseUser.Split('#');
			for(int i = 0; i < values.Length; i++){
				userList.Add(UserToObject(values[i]));
			}
			return userList;
		}
		
		// Remove a user from his M - Return the modified userList
		public static List<MUser> RemoveFromId(List<MUser> userList, int id){
			MUser removeUser = null;
			for(int i = 0; i < userList.Count; i++){
				if(id == userList[i].id){
					removeUser = userList[i];
				}
			}			
			if(removeUser != null){
				userList.Remove(removeUser);
			}			
			return userList;
		}
		
		// Remove the server of the userList (call OnDisconnectedFromServer) - Return the modified userList
		public static List<MUser> RemoveServer(List<MUser> userList){
			MUser removeUser = null;
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].isGameHost == 1){
					removeUser = userList[i];
				}
			}			
			if(removeUser != null){
				userList.Remove(removeUser);
			}			
			return userList;
		}
				
		// Save the new status of a player - Return the modified userList
		public static List<MUser> SavePlayerStatus(int id, int isPlayerInGame, List<MUser> userList) {
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].id == id){
					userList[i].isPlayerInGame = isPlayerInGame;
				}
			}	
			return userList;
		}
		
		// Save a player hosting capacities - Return the modified userList
		public static List<MUser> SavePlayerHostingCapacity(int id, bool canHost, List<MUser> userList) {
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].id == id){
					userList[i].canHost = canHost;
				}
			}	
			return userList;
		}
		
		// Search a player from his ip (return him as MUser object)
		public static MUser SearchPlayer(int id, List<MUser> userList) {
			MUser searchUser = new MUser();
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].id == id){
					searchUser = userList[i];
				}
			}	
			return searchUser;
		}
		
		// Check if a specific player is on the userList
		public static bool inList(List<MUser> userList, int id){
			for(int i = 0; i < userList.Count; i++){
				if(userList[i].id == id){
					return true;
				}
			}	
			return false;
		}		
	}
}
