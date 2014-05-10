<?php 
	include 'functions/functions.php';

	$user = new MOUser();
	if($user->logoutUser(urldecode($_REQUEST['id']), urldecode($_REQUEST['loginKey']))){
		echo 'Success';
	} else {
		echo $user->errorMessage;
	}
?>