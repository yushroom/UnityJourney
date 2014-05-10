using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class NetworkScript {
	private static NetworkScript instance;
	private Socket socket;
	private string ip = "127.0.0.1";
	private int port = 10100;

	public static NetworkScript getInstance() {
		if (instance == null) {
			instance = new NetworkScript();
		}
		return instance;
	}

	public void init() {
		try{
			socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect (ip, port);
			Debug.Log("Sever connection successfully established!");
		} catch {
			Debug.Log("Connection failed!");
		}
	}
}
