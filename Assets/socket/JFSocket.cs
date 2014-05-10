using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class JFSocket {
	private Socket clientSocket;
	//public List<JFPackage.WorldPackage> worldpackage;

	// singleton pattern
	private static JFSocket instance;
	public static JFSocket GetInstance()
	{
		if (instance == null) 
		{
			instance = new JFSocket ();
		}
		return instance;
	}

	JFSocket()
	{
		clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPAddress ipAddress = IPAddress.Parse("localhost");
		IPEndPoint ipEndpoint = new IPEndPoint (ipAddress, 10060);

	}
}
