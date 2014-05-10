<?php
// MOUser : manage all interactions with the users MySQL table

class MOUser extends MODb{
	
	/**************** YOU CAN CHANGE THESE PARAMETERS ********************/
	// Sender e-mail :
	private $mailSender = 'Sender e-mail';	
	
	// Email for account create :
	// Subject :
	private $mailRegisterSubject = 'Your account have been created !';
	// Message (in HTML)
	private $mailRegisterContent = 'Hello<br/><br />Welcome on SuperGame, you are now registred with the following userName :<br /><b>$userName</b><br/><br /> Best regards, <br /> SuperGame team';
	
	// Email for "forgotten username" :
	// Subject :
	private $mailUserNameSubject = 'Your userName';
	// Message (in HTML)
	private $mailUserNameContent = 'Hello<br/><br />You are registred on SuperGame with the following userName :<br /><b>$userName</b><br/><br /> Best regards, <br /> SuperGame team';
	
	// Email for "forgotten password" :
	// Subject :
	private $mailNewPassSubject= 'Your password';
	// Message (in HTML) :
	private $mailNewPassContent = 'Hello<br/><br />This is your new password for login on SuperGame :<br /><b>$newPass</b><br/><br />Once you\'ll be login, you could change this password from the profil settings.<br/><br/>  Best regards, <br /> SuperGame team';
	
	/************************************************************/
	protected $errorMessage=null;
	protected $userName;
	protected $userMail;
	protected $userGameStatus;
	protected $userId;
	protected $loginKey;
	protected $privateIP;
	protected $publicIP;
	
	// Constructor
	function __construct(){
		parent::__construct();
	}
	
	// Destructor
	function __destruct(){
		parent::__destruct();
	}
	
	// Getter
	public function __get($nom){
		if(isset($this->$nom)){
			return $this->$nom;
		}
	}
	
	// Save a new user
	public function saveUser($userName, $mail, $pass, $privateIP, $sendMail) {		
		if(!empty($userName) && !empty($mail) && !empty($pass)) {
			if($this->nameFilter($userName) && $this->mailFilter($mail) && $this->passFilter($pass)){
				
				$checkUserName = $this->query_fetchObject('SELECT id FROM
					users WHERE userName=\''.secure_db($userName).'\'');
				
				$checkMail = $this->query_fetchObject('SELECT id FROM
					users WHERE mail=\''.secure_db($mail).'\'');
				
				if(empty($checkUserName->id) && empty($checkMail->id)) {
					$savePass = crypt(secure_prepare_db($pass));	
					$array = array(
						':id' => '',
						':userName' => secure_prepare_db($userName),
						':mail' => secure_prepare_db($mail),
						':privateIP' => secure_prepare_db($privateIP),
						':publicIP' => secure_prepare_db($_SERVER['REMOTE_ADDR']),
						':pass' => $savePass,						
						':login' => false,
						':loginKey' => null,
						':lastLog' => null
					);					
					$this->prepare_exec('INSERT INTO users VALUES
						(:id,:userName,:mail,:privateIP,:publicIP,:pass,NOW(),:login,:loginKey,:lastLog)', $array);
					
					if($sendMail == 1){
						// Send e-mail
						$message = str_replace('$userName', $userName, $this->mailRegisterContent);
						mail($mail, $this->mailRegisterSubject, $message, "From: $this->mailSender\nContent-Type: text/html;charset=utf-8");
					}
					
					return true;
				} else {
					if(!empty($checkUserName->id)) {					
						$this->errorMessage.= '|nameExist';
					}
					if(!empty($checkMail->id)){
						$this->errorMessage.= '|mailExist';
					}
				}
				
			} else {
				if(!$this->nameFilter($userName)){
					$this->errorMessage.= '|nameFilter';
				}
				if (!$this->mailFilter($mail)) {
					$this->errorMessage.= '|mailFilter';
				}
				if (!$this->passFilter($pass)) {
					$this->errorMessage.= '|passFilter';
				}
			}
		} else {
			if(empty($userName)){
				$this->errorMessage.= '|emptyName';
			}
			if(empty($mail)){
				$this->errorMessage.= '|emptyMail';
			}
			if(empty($pass)){
				$this->errorMessage.= '|emptyPass';
			}
		}
		return false;
	}
	
	// Login a user
	public function logUser($userName, $pass, $privateIP){		
		if(!empty($userName) && !empty($pass)){
			if($this->nameFilter($userName) && $this->passFilter($pass)){
				$check = $this->query_fetchObject('SELECT id, mail, pass, login, 
						SECOND(lastLog) AS second,
						MINUTE(lastLog) AS minute,
						HOUR(lastLog) AS hour,
						MONTH(lastLog) AS month,	
						DAY(lastLog) AS day,						
						YEAR(lastLog) AS year FROM users WHERE userName=\''.secure_db($userName).'\'');
				if(!empty($check->id) && !empty($check->pass)){					
					$pass = crypt(secure_prepare_db($pass), display_db($check->pass));				
					if($pass == $check->pass && 
					(!$check->login ||
					mktime($check->hour, $check->minute, $check->second, $check->month, $check->day, $check->year) < time()-(3600*12))){
						/* Create the login key with the username and the current timestamp.
						 * The obtained string is after hashed with cryp() function.
						 *  In this way, we are quite sure that the login key is unique and irreproducible */
						$this->loginKey =  $userName.time();
						$this->loginKey = crypt($this->loginKey);
						
						$array = array(
							':id' => $check->id,
							':privateIP' => secure_prepare_db($privateIP),
							':publicIP' => secure_prepare_db($_SERVER['REMOTE_ADDR']),
							':login' => true,
							':loginKey' => secure_prepare_db($this->loginKey)
						);			
						$this->prepare_exec('UPDATE users SET privateIP=:privateIP, publicIP=:publicIP, 
								login=:login, loginKey=:loginKey, lastLog=NOW() WHERE id=:id', $array);
						$this->userId = $check->id;
						$this->userName = $userName;
						$this->privateIP = $privateIP;
						$this->publicIP = $_SERVER['REMOTE_ADDR'];
						$this->userMail = $check->mail;
						return true;
					} else {
						if($pass != $check->pass){
							$this->errorMessage.= '|errorPass';
						}
						if($check->login && 
						 mktime($check->hour, $check->minute, $check->second, $check->month, $check->day, $check->year) >= time()-(3600*12)){
							$this->errorMessage.= '|errorAlreadyLog';
						}
					}
				} else {
					$this->errorMessage.= '|errorName';
				}
			} else {
				if(!$this->nameFilter($userName)){
					$this->errorMessage.= '|nameFilter';
				}				
				if (!$this->passFilter($pass)) {
					$this->errorMessage.= '|passFilter';
				}
			}			
		} else {
			if(empty($userName)){
				$this->errorMessage.= '|emptyName';
			}			
			if(empty($pass)){
				$this->errorMessage.= '|emptyPass';
			}
		}
		return false;
	}
	
	// If a user have forgot his username or password
	public function frogotLogin($mail, $sendUserName, $forgotPass){
		if(!empty($mail) && $this->mailFilter($mail)){
			$check = $this->query_fetchObject('SELECT id, userName FROM users WHERE mail =\''.secure_db($mail).'\'');
			if(!empty($check->id)){
				if($sendUserName == 1){
					// Send e-mail
					$message = str_replace('$userName', display_db($check->userName), $this->mailUserNameContent);
					mail($mail, $this->mailUserNameSubject, $message, "From: $this->mailSender\nContent-Type: text/html;charset=utf-8");
				}
				if($forgotPass == 1){	
					// Define new password
					$newPass = rand(10000, 99999);
					$newPassCrypt = crypt($newPass);
					$array = array(':id' => $check->id, ':pass'=> secure_prepare_db($newPassCrypt));
					$this->prepare_exec('UPDATE users SET pass=:pass WHERE id=:id', $array);
					
					// Send e-mail
					$message = str_replace('$newPass',$newPass, $this->mailNewPassContent);
					mail($mail, $this->mailNewPassSubject, $message, "From: $this->mailSender\nContent-Type: text/html;charset=utf-8");
				}
				return true;
			} else {
				$this->errorMessage.= '|errorUserMail';
			}
		} else {
			if(empty($mail)){
				$this->errorMessage.= '|emptyMail';
			}
			if(!$this->mailFilter($mail)){
				$this->errorMessage.= '|mailFilter';
			}
		}
		return false;
	}

	// Logout user
	public function logoutUser($id, $loginKey){
		if($this->checkUser($id, $loginKey)){
			$array = array(':id'=> $this->userId, ':login'=>false, ':loginKey'=>null);
			$this->prepare_exec('UPDATE users SET login=:login,loginKey=:loginKey WHERE id=:id', $array);
			return true;
		} else {					
			return false;
		}
	}
	
	// Change the username or email of a user
	public function changeProfil($id, $loginKey, $userName=null, $mail=null){
		if($this->checkUser($id, $loginKey)){
			if(!empty($userName) && $this->nameFilter($userName)){
				$checkUserName = $this->query_fetchObject('SELECT id FROM
					users WHERE id <> \''.$this->userId.'\' AND userName=\''.secure_db($userName).'\'');
					
				if(empty($checkUserName->id)){
					$array = array(
							':id' => $this->userId,
							':userName' => secure_prepare_db($userName));
						
					$this->prepare_exec('UPDATE users SET userName=:userName WHERE id=:id', $array);
				} else {
					$this->errorMessage.= '|nameExist';
					return false;
				}
			} else {
				if(!empty($userName) && !$this->nameFilter($userName)){
					$this->errorMessage.= '|errorName';
					return false;
				}
			}
			if(!empty($mail) && $this->mailFilter($mail)){
				$checkMail = $this->query_fetchObject('SELECT id FROM
					users WHERE id <> \''.$this->userId.'\' AND mail=\''.secure_db($mail).'\'');
					
				if(empty($checkMail->id)){
					$array = array(
							':id' => $this->userId,
							':mail' => secure_prepare_db($mail));
						
					$this->prepare_exec('UPDATE users SET mail=:mail WHERE id=:id', $array);
				} else {
					$this->errorMessage.= '|mailExist';
					return false;
				}
			} else {
				if(!empty($mail) && !$this->mailFilter($mail)){
					$this->errorMessage.= '|errorMail';
					return false;
				}
			}
		} else {
			return false;
		}		
		return true;
	}
	
	// Change the password of a user
	public function changePassword($id, $loginKey, $password, $newPassword){
		if($this->checkUser($id, $loginKey)){
			$check = $this->query_fetchObject('SELECT pass FROM users WHERE id = \''.$this->userId.'\'');
			$pass = crypt(secure_prepare_db($password), display_db($check->pass));
			if($pass == $check->pass && $this->passFilter($newPassword)){
				$array = array(':id' => $this->userId, ':pass' => secure_prepare_db(crypt($newPassword)));
				$this->prepare_exec('UPDATE users SET pass=:pass WHERE id=:id', $array);
				return true;
			} else {
				if($pass != $check->pass){
					$this->errorMessage.= '|errorPass';
				}
				if(!$this->passFilter($newPassword)){
					$this->errorMessage.= '|passFilter';
				}
			}
		}
		return false;	
	}
		
	// Check if a user is login (call before do any action when we received a query for a user)
	protected function checkUser($id, $loginKey){
		if(!empty($id) && !empty($loginKey)){
			$check = $this->query_fetchObject('SELECT id, publicIP, loginKey from users WHERE id=\''.secure_db($id).'\'');
			if(!empty($check->id) && $check->loginKey == secure_db($loginKey)){
				$this->userId = $check->id;
				$this->publicIP = $check->publicIP;
				$this->loginKey = $check->loginKey;				
				return true;
			} else {
				if(empty($check->id)){
					$this->errorMessage.= '|badId';
				}
				if($check->loginKey != secure_db($loginKey)){
					$this->errorMessage.= '|badLoginKey';
				}
			}
		}else {
			if(empty($id)){
				$this->errorMessage.= '|emptyId';
			}
			if(empty($loginKey)){
				$this->errorMessage.= '|emptyLoginKey';
			}
		}
		return false;
	}
	
	/********************************** FILTERS *******************************/	
	protected function mailFilter($var){
		if(is_string($var) AND strlen($var) < 100){
			if(preg_match('#^[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]{2,}\.[a-zA-Z]{2,4}$#', $var)){
				return TRUE;
			}else	{
				return FALSE;
			}
		}else{
			return FALSE;
		}
	}
	
	protected function passFilter($var){
		if(is_string($var) AND strlen($var) < 100 AND strlen($var) > 3){
			return TRUE;
		}else{
			return FALSE;
		}
	}
	
	protected function nameFilter($var){
		if(is_string($var) AND strlen($var) < 100){
			if(preg_match('#^[a-zA-Z0-9_ ]+$#', $var)){
				return TRUE;
			}else	{
				return FALSE;
			}
		}else{
			return FALSE;
		}
	}
}
?>