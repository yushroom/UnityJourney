<?php 
	include 'functions/functions.php';

	$game = new MOGame();
	if(!empty($_REQUEST['saveRehosted'])){
		if($game->saveRehostedGame(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']),
				 urldecode($_REQUEST['gameId']), urldecode($_REQUEST['gameName']), urldecode($_REQUEST['gamePort']),
				urldecode($_REQUEST['maxPlayer']), urldecode($_REQUEST['usePass']),
				urldecode($_REQUEST['started']), urldecode($_REQUEST['register']))) {
			echo 'Success';					
		} else {
			echo $game->errorMessage;
		}
	} else {
		if($game->saveGame(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']),			
				 urldecode($_REQUEST['gameName']), urldecode($_REQUEST['gamePort']),
				 urldecode($_REQUEST['maxPlayer']), urldecode($_REQUEST['usePass']))) {
			echo 'Success|'
					.$game->gameId.'|'
					.$game->gameTotalPlayer.'|'
					.$game->gameStatus.'|'
					.$game->gameRegister.'|'
					.$game->gameRegisterDate;
		} else {
			echo $game->errorMessage;
		}
	}
?>