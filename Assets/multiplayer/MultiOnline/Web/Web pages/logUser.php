<?php 
	include 'functions/functions.php';

	$user = new MOUser();
	if($user->logUser(urldecode($_REQUEST['userName']), urldecode($_REQUEST['pass']), urldecode($_REQUEST['privateIP']))){
		echo 'Success|'.$user->userId.'|'.$user->userName.'|'.$user->userMail.'|'.$user->publicIP.'|'.$user->loginKey;
	} else {
		echo $user->errorMessage;
	}
?>