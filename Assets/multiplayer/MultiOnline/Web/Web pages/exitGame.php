<?php 
	include 'functions/functions.php';
	$game = new MOGame();
	if($game->exitGame(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']))){
		if(!empty($_REQUEST['logout'])){
			if($game->logoutUser(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']))){
				echo 'Success';
			}else {
				echo $game->errorMessage.'|';
			}
		} else {
			echo 'Success';
		}		
	} else {
		echo $game->errorMessage;
	}
?>