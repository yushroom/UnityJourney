using UnityEngine;
using System.Collections;

namespace MultiPlayer {
	public class MGame {		
		public int id;
		public string name;
		public int port;
		public int totalPlayer;
		public int maxPlayer;	
		public bool isUsePassword;
		public string status;
		public bool isStarted;		
		public string register;		
		public string registerDate;
		public bool isOnline;		
		
		public int hostId;
		public string hostName;
		public string hostPrivateIp;
		public string hostPublicIp;
		
			
		
		public MGame(){}
		
		// Consctructor for MultiLan
		public MGame(string name,
			int port,
			int totalPlayer,
			int maxPlayer,
			bool isStarted,
			string hostName,
			string hostPrivateIp,
			string hostPublicIp){
			
			this.name = name; 
			this.port = port;	
			this.totalPlayer = totalPlayer;
			this.maxPlayer = maxPlayer;			
			this.isStarted = isStarted;				
			this.hostName = hostName; 
			this.hostPrivateIp = hostPrivateIp; 
			this.hostPublicIp = hostPublicIp; 
		}
		
		// Consctructor for MultiOnline
		public MGame(string id,
			string name,
			string port,
			string totalPlayer,
			string maxPlayer,
			string isUsePassword,
			string status,
			string register,
			string registerDate,
			string hostId,
			string hostName,
			string hostPrivateIp,
			string hostPublicIp,
			bool isOnline){
			
			this.id = int.Parse(id);			
			this.name = name; 
			this.port = int.Parse(port);		
			
			this.totalPlayer = int.Parse(totalPlayer);
			this.maxPlayer = int.Parse(maxPlayer);
			if(isUsePassword.Equals("1")){
				this.isUsePassword = true;
			} else {
				this.isUsePassword = false;
			}
			
			this.status = status; 
			if(status.Equals("1")){
				this.isStarted = true;	
			} else {
				this.isStarted = false;	
			}
			this.register = register; 
			this.registerDate = registerDate; 
			this.hostId = int.Parse(hostId); 
			this.hostName = hostName; 
			this.hostPrivateIp = hostPrivateIp; 
			this.hostPublicIp = hostPublicIp; 
			this.isOnline = isOnline; 
		}
		
		
	}
}