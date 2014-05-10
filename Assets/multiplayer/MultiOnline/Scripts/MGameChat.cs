using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MGameChat : MonoBehaviour {
	private MNetwork networkSrc;
	
	public Vector2 chatScroll = Vector2.zero;	
	public int chatContentSizeY;
	
	public string textChat;	
	public string chatButtonTxt = "Send";
	
	private bool openChat = false;
		
	// Use this for initialization
	void Start () {
		networkSrc = GameObject.Find ("NetworkManager(Clone)").GetComponent<MNetwork>();
		chatScroll = new Vector2(0, (20*(networkSrc.chatContent.Count+1)) - chatContentSizeY);
	}
	
	void OnGUI(){	
		if(!networkSrc.isGameServerRebuild && !networkSrc.isGameServerRebuildFailed && !networkSrc.isPlayerExitGame && networkSrc.isGameStarted) {
			
			// Positions settings
			int margin = 5;
			int chatSizeX = 250;
			int chatSizeY = 140;
			int chatSizeButtonX = 80;
			int chatSizeButtonY = 20;
			int chatPosX = margin;
			int chatPosY = Screen.height-chatSizeY-margin;
			Rect chatMenu = new Rect(chatPosX, chatPosY, chatSizeX, chatSizeY);		
			
			int chatButtonSizeX = 40;
			int chatButtonSizeY = 20;		
			int chatFieldSizeX = chatSizeX-chatButtonSizeX-(margin*3);
			int chatFieldSizeY = 20;	
			int chatFieldPosY = chatSizeY-chatFieldSizeY-(margin);
			int chatButtonPosX = chatFieldSizeX+(margin*2);				
			chatContentSizeY = chatSizeY-chatFieldSizeY-(margin*3);
			int sizeY = 0;			
			
			int chatMiniX = 250;
			int chatMiniY = 30;
			int chatCloseX = margin;
			int chatCloseY = Screen.height-chatSizeButtonY;
			
			if(openChat){
				if(GUI.Button(new Rect(chatPosX,chatPosY-chatSizeButtonY,chatSizeButtonX, chatSizeButtonY), "Close Chat")){
					openChat = false;
				}
				GUI.BeginGroup(chatMenu, "");			
				GUI.Box(new Rect(0,0,chatSizeX, chatSizeY), "");				
				sizeY=margin;					
			
				chatScroll = GUI.BeginScrollView (new Rect (0,sizeY,chatSizeX,chatContentSizeY), chatScroll, new Rect(0,0, chatSizeX-40, 20*networkSrc.chatContent.Count+margin));						
				for(int i = 0; i < networkSrc.chatContent.Count; i++){
					GUI.Label(new Rect(10,sizeY,chatSizeX, 30), networkSrc.chatContent[i]);
					sizeY+=20;
				}
								
				GUI.EndScrollView();
											
				textChat = GUI.TextField(new Rect(margin, chatFieldPosY,chatFieldSizeX, chatFieldSizeY), textChat);
				
				if(GUI.Button(new Rect(chatButtonPosX,chatFieldPosY,chatButtonSizeX, chatButtonSizeY), chatButtonTxt)){
					if(textChat != "") {
						string message = networkSrc.playerDataSrc.nameInGame+" : "+textChat;
						int maxChar = (int)Math.Round((chatSizeX-10) / 6.5f);
						if(message.Length > maxChar){
							message = message.Substring(0, maxChar);
						}
						ChatMessage(message);
						textChat = "";
					}
				}
				
				GUI.EndGroup();	 	
			} 
			else {
				if(GUI.Button(new Rect(chatCloseX,chatCloseY-chatMiniY-margin,chatSizeButtonX, chatSizeButtonY), "Open Chat")){
					openChat = true;				
				}
				GUI.Box(new Rect(chatCloseX,chatCloseY-margin*3,chatMiniX, chatMiniY), "");	
				string chatText="";
				try {
					if(networkSrc.chatContent[networkSrc.chatContent.Count-1] != null){
						chatText = networkSrc.chatContent[networkSrc.chatContent.Count-1];
					}
				} catch(ArgumentOutOfRangeException) {}
				GUI.Label(new Rect(10,chatCloseY-margin*2,chatMiniX, chatMiniY), chatText);
				
			}
		}
	}	
	
	private void ChatMessage(string message){
		networkSrc.SendChatMessage(message);		
	}
}
