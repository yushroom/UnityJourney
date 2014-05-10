using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;

namespace TestServer
{
	class Program
	{
		// set connection port
		const int port = 500;

		static void Main()
		{
			// server IP
			System.Net.IPAddress localAdd = System.Net.IPAddress.Parse("localhost");
			// create TCP listener
			TcpListener listener = new TcpListener (localAdd, port);
			listener.Start ();

			Console.WriteLine ("Server is starting...\n");

			while (true) 
			{
				ChatClient user = new ChatClient(listener.AcceptTcpClient());

				// show client's IP and port
				Console.WriteLine(user._clientIP + " is joined...\n");
			}
		}
	}
}