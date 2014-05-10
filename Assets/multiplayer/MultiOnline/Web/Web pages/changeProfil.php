<?php 
	include 'functions/functions.php';
	$user = new MOUser();
	if(!empty($_REQUEST['changeData'])){
		if($user->changeProfil(urldecode($_REQUEST['id']), 
				urldecode($_REQUEST['loginKey']),
				urldecode($_REQUEST['username']),
				urldecode($_REQUEST['mail']))){
			echo 'Success';
		} else {
			echo $user->errorMessage;
		}
	} elseif(!empty($_REQUEST['changePass'])){
		if($user->changePassword(urldecode($_REQUEST['id']),
				urldecode($_REQUEST['loginKey']),
				urldecode($_REQUEST['currentPassword']),
				urldecode($_REQUEST['newPassword']))){
			echo 'Success';
		} else {
			echo $user->errorMessage;
		}
	}
?>