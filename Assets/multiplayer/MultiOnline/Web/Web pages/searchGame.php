<?php 
	include 'functions/functions.php';
	$game = new MOGame();
	if($game->searchGames(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']))){
		echo $game->XMLoutput;				
	} else {
		echo 'ERROR'.$game->errorMessage;
	}
?>