<?php 
	include 'functions/functions.php';
	$game = new MOGame();
	if(!empty($_REQUEST['changeStatus'])){
		if($game->updateGameStatus(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']), 
				urldecode($_REQUEST['gameId']), urldecode($_REQUEST['gameStatus']))){
			echo 'Success';
		} else {
			echo $game->errorMessage;
		}
	}
	
	if(!empty($_REQUEST['addPlayer'])){
		if($game->addPlayer(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']),
				urldecode($_REQUEST['gameId']))){
			echo 'Success';
		} else {
			echo $game->errorMessage;
		}
	}
	
	if(!empty($_REQUEST['updateHost'])){
		if($game->updateHost(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']),
				urldecode($_REQUEST['gameId']))){
			echo 'Success';
		} else {
			echo $game->errorMessage;
		}
	}
	
	
?>