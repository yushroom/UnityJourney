<?php 
	include 'functions/functions.php';

	$user = new MOUser();
	if($user->saveUser(urldecode($_REQUEST['userName']), urldecode($_REQUEST['mail']), 
			urldecode($_REQUEST['pass']), urldecode($_REQUEST['privateIP']), urldecode($_REQUEST['sendMail']))){
		echo 'Success';
	} else {
		echo $user->errorMessage;
	}
?>