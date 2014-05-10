using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;

namespace MultiPlayer {
	class MCheckPort {
		
		TcpListener server=null;		
		
		// Try to open a connexion
		public bool OpenConnexion(string ip, Int32 port) {
			IPAddress serverIp = IPAddress.Parse(ip);
			
			// Try to open the port
			try  {	  
			    server = new TcpListener(serverIp, port);		
		    	server.Start(); 				
				return true; // Return true if the port is open
				
		    } catch(SocketException) {
		     	return false; //  Return false if the port opening is failed 
		    }	
		}
		
		// Close a connexion
		public void CloseConnexion(){
			if(server != null){
				 server.Stop(); // Close the connexion
			}
		}
		
		// Try to connect on an IP
		public static bool TryConnexion(string ip, Int32 port) {
			IPAddress serverIp = IPAddress.Parse(ip);			
			TcpClient tcpClient = new TcpClient();
			
			// Try to connexion on the server
	        try {
				tcpClient.Connect(serverIp, port);
				 return true; // Return true if the connexion works
			}catch (Exception){
				return false; // Return true if the connexion fail
			}			
		}
	}
}