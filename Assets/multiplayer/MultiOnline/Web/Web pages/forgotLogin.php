<?php 
	include 'functions/functions.php';

	$user = new MOUser();
	if($user->frogotLogin(urldecode($_REQUEST['mail']), urldecode($_REQUEST['sendUserName']), urldecode($_REQUEST['forgotPass']))){
		echo Success;
	} else {
		echo $user->errorMessage;
	}
?>