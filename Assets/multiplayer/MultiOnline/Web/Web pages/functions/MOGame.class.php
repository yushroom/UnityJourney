<?php 
// MOGame : manage all interactions with the games MySQL table
class MOGame extends MOUser{

	private $gameId;
	private $gameName;
	private $gamePort;
	private $gameMaxPlayer;
	private $gameTotalPlayer;
	private $gameStatus;
	private $gameUsePass;
	private $gameRegister;	
	private $gameRegisterDate;
	private $XMLoutput;
	
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
	
	// Save a new game
	public function saveGame($userId, $loginKey, $gameName, $port, $maxPlayer, $usePass){
		if($this->checkUser($userId, $loginKey)){
			if(!empty($gameName) && !empty($port) && !empty($maxPlayer)){
				if($this->nameFilter($gameName) && is_numeric($maxPlayer) && is_numeric($port)){
					if($usePass == 0){
						$usePass = false;
					} else {
						$usePass = true;
					}
					
					$this->gameName = $gameName;
					$this->gamePort = $port;
					$this->gameMaxPlayer = $maxPlayer;
					$this->gameStatus = '0';
					$this->gameUsePass = $usePass;
					$this->gameTotalPlayer = 1;
					
					$array = array(':id' => '',
						':userHost_id' => $this->userId,
						':name' => secure_prepare_db($this->gameName),
						':port' => secure_prepare_db($this->gamePort),
						':maxPlayer' => secure_prepare_db($this->gameMaxPlayer),		
						':usePass' => $this->gameUsePass,
						':status' => secure_prepare_db($this->gameStatus));
					
					$this->prepare_exec('INSERT INTO games VALUES(:id, :userHost_id,
						:name, :port, :maxPlayer, :usePass, :status, NOW())', $array);
					$this->gameId = $this->lastId();
					
					$getDate = $this->query_fetchObject('SELECT DATE_FORMAT(dateReg, \'%H:%i\') AS gameRegister,	
					dateReg AS gameRegisterDate FROM games WHERE id=\''.$this->gameId.'\'');
					
					$this->gameRegister = $getDate->gameRegister;		
					$this->gameRegisterDate =$getDate->gameRegisterDate;
					
					$array2 = array(':user_id' => $this->userId, 
						':game_id' => $this->gameId);
					$this->prepare_exec('INSERT INTO users_has_games VALUES(:user_id, :game_id)', $array2);
					return true;
				} else {
					if(!$this->nameFilter($gameName)){
						$this->errorMessage.= '|errorGameName';
					}					
					if(!is_numeric($maxPlayer)){
						$this->errorMessage.= '|errorMaxPlayer';
					}
					if(!is_numeric($port)){
						$this->errorMessage.= '|errorPort';
					}
				}
			} else {
				if(empty($gameName)){
					$this->errorMessage.= '|emptyGameName';
				}
				if(empty($port)){
					$this->errorMessage.= '|emptyPort';
				}				
				if(empty($maxPlayer)){
					$this->errorMessage.= '|emptyMaxPlayer';
				}
			} 
		}
		return false;
	}
	
	// Save a game after host migration
	public function saveRehostedGame($userId, $loginKey, $gameId, $gameName, $port, $maxPlayer, $usePass, $started, $register){
		if($this->checkUser($userId, $loginKey)){
			if(!empty($gameId) &&!empty($gameName) && !empty($port) && !empty($maxPlayer)){
				if($this->nameFilter($gameName)&& is_numeric($gameId) && is_numeric($maxPlayer) && is_numeric($port)){
					if($usePass == 0){
						$usePass = false;
					} else {
						$usePass = true;
					}
					
					if($started == 0){
						$started = false;
					} else {
						$started = true;
					}

					$check = $this->query_fetchObject('SELECT id FROM games WHERE id=\''.secure_db($gameId).'\'');
					if(!empty($check->id)){
						$this->exec('DELETE FROM games WHERE id=\''.$check->id.'\'');
					}
					
					$this->gameId = $gameId;
					$this->gameName = $gameName;
					$this->gamePort = $port;
					$this->gameMaxPlayer = $maxPlayer;
					$this->gameStatus = $started;
					$this->gameUsePass = $usePass;
					$this->gameRegister = $register;
					
					$array = array(':id' => $this->gameId,
							':userHost_id' => $this->userId,
							':name' => secure_prepare_db($this->gameName),
							':port' => secure_prepare_db($this->gamePort),
							':maxPlayer' => secure_prepare_db($this->gameMaxPlayer),
							':usePass' => $this->gameUsePass,
							':status' => secure_prepare_db($this->gameStatus),
							':dateReg' => secure_prepare_db($this->gameRegister));
						
					$this->prepare_exec('INSERT INTO games VALUES(:id, :userHost_id,
						:name, :port, :maxPlayer, :usePass, :status, :dateReg)', $array);				
						
					$array2 = array(':user_id' => $this->userId,
							':game_id' => $this->gameId);
					$this->prepare_exec('INSERT INTO users_has_games VALUES(:user_id, :game_id)', $array2);
					return true;
				} 
			} 
		}
		return false;
	}
	
	// Exit game : delete the player on the users_has_games table
	// if the player is the last on of the game, it delete the game too
	public function exitGame($userId, $loginKey){
		if($this->checkUser($userId, $loginKey)){
			$games = $this->query_fetchAll('SELECT game_id FROM users_has_games  					
				WHERE user_id=\''.$this->userId.'\'');
			
			foreach($games AS $value){
				if(!empty($value['game_id'])){
					$game = $this->query_fetchObject('SELECT 
						COUNT(uhg.game_id) AS totalPlayer,
						g.userHost_id AS hostId 
						FROM games g INNER JOIN users_has_games uhg
						ON (g.id = uhg.game_id)
						 WHERE g.id=\''.$value['game_id'].'\'');
					
					if($game->totalPlayer <= 1 || $game->hostId == $this->userId){
						$this->exec('DELETE FROM games WHERE id=\''.$value['game_id'].'\'');
					} 					
				}
			}
			$this->exec('DELETE FROM users_has_games WHERE user_id=\''.$this->userId.'\'');
			return true;
		} else {
			return false;			
		}
	}
	
	// Search the games
	public function searchGames($userId, $loginKey){
		if($this->checkUser($userId, $loginKey)){
			$count = $this->query_fetchObject('SELECT COUNT(*) AS totalResult FROM games');
			if($count->totalResult  > 0){
			$games = $this->query_fetchAll('SELECT 
				g.id AS gameId,
				g.userHost_id AS hostId,
				g.name AS gameName,
				g.port AS gamePort,	
				g.maxPlayer AS gameMaxPlayer,	
				g.usePass AS gameUsePass,	
				g.status AS gameStatus,
				DATE_FORMAT(g.dateReg, \'%H:%i\') AS gameRegister,	
				g.dateReg AS gameRegisterDate,	
				u.userName AS hostName,
				u.privateIP AS hostPrivateIp,
				u.publicIP AS hostPublicIp			
				FROM games g INNER JOIN users u
				ON (g.userHost_id = u.id) 				
				ORDER BY g.dateReg DESC');
				
				$this->XMLoutput = null;					
				foreach($games as $value){
					$players = $this->query_fetchObject('SELECT COUNT(*) AS totalPlayers FROM
						users_has_games WHERE game_id=\''.$value['gameId'].'\'');
					if($players->totalPlayers < $value['gameMaxPlayer'] && $players->totalPlayers >0) {
						$this->XMLoutput.= '<game id="'.utf8_encode($value['gameId']).'">';
						$this->XMLoutput.= '<name>'.utf8_encode(display_db($value['gameName'])).'</name>';
						$this->XMLoutput.= '<port>'.utf8_encode(display_db($value['gamePort'])).'</port>';
						$this->XMLoutput.= '<currentPlayers>'.utf8_encode($players->totalPlayers).'</currentPlayers>';
						$this->XMLoutput.= '<maxPlayer>'.utf8_encode(display_db($value['gameMaxPlayer'])).'</maxPlayer>';
						$this->XMLoutput.= '<usePass>'.utf8_encode(display_db($value['gameUsePass'])).'</usePass>';
						$this->XMLoutput.= '<status>'.utf8_encode(display_db($value['gameStatus'])).'</status>';
						$this->XMLoutput.= '<register>'.utf8_encode(display_db($value['gameRegister'])).'</register>';
						$this->XMLoutput.= '<registerDate>'.utf8_encode(display_db($value['gameRegisterDate'])).'</registerDate>';
						$this->XMLoutput.= '<hostId>'.utf8_encode(display_db($value['hostId'])).'</hostId>';
						$this->XMLoutput.= '<hostName>'.utf8_encode(display_db($value['hostName'])).'</hostName>';
						$this->XMLoutput.= '<hostPrivateIp>'.utf8_encode(display_db($value['hostPrivateIp'])).'</hostPrivateIp>';
						$this->XMLoutput.= '<hostPublicIp>'.utf8_encode(display_db($value['hostPublicIp'])).'</hostPublicIp>';
						$this->XMLoutput.= '</game>';
					} /*else if($players->totalPlayers <= 0){
						$array = array(':id' => $value['gameId']);
						$this->prepare_exec('DELETE FROM games WHERE id=:id', $array);
					}	*/			
				}
				
				if($this->XMLoutput != null && $this->XMLoutput != ''){
					$this->XMLoutput = '<gameList>'.$this->XMLoutput.'</gameList>';
					return true;
				} else {
					$this->errorMessage = '|noGame';
				}				
			}
			$this->errorMessage = '|noGame';			
		}
		return false;
	}
	
	// Update a game status (when the game begin)
	public function updateGameStatus($userId, $loginKey, $gameId, $gameStatus){
		if($this->checkUser($userId, $loginKey)){
			$check = $this->query_fetchObject('SELECT id FROM games WHERE id=\''.secure_db($gameId).'\'');
			if(!empty($check->id) && ($gameStatus == '0' || $gameStatus == '1')){
				$array = array(':id' => $check->id, ':status' => secure_prepare_db($gameStatus));
				$this->prepare_exec('UPDATE games SET status=\''.secure_db($gameStatus).'\' WHERE id=\''.$check->id.'\'', $array);
				return true;
			} else {
				if(empty($check->id)){
					$this->errorMessage.= '|badGameId';
				}
				if($gameStatus != '0' && $gameStatus != '1'){
					$this->errorMessage = '|badGameStatus';
				}
			}			
		}
		return false;
	}
	
	// Add a player in users_has_games
	public function addPlayer($userId, $loginKey, $gameId){
		if($this->checkUser($userId, $loginKey)){
			$check = $this->query_fetchObject('SELECT id FROM games WHERE id=\''.secure_db($gameId).'\'');
			if(!empty($check->id)){
				$array = array(':user_id' => $this->userId, ':game_id' => $check->id);
				$this->prepare_exec('INSERT INTO users_has_games VALUES(:user_id, :game_id)', $array);
				return true;
			} else {				
				$this->errorMessage.= '|badGameId';
			}
		}
		return false;
	}
	
	// Update the game's host
	public function updateHost($userId, $loginKey, $gameId){
		if($this->checkUser($userId, $loginKey)){
			$check = $this->query_fetchObject('SELECT id FROM games WHERE id=\''.secure_db($gameId).'\'');
			if(!empty($check->id)){
				$array = array(':id' => $check->id, ':userHost_id' => $this->userId);
				$this->prepare_exec('UPDATE games SET userHost_id=:userHost_id WHERE id=:id', $array);
				return true;
			} else {
				$this->errorMessage.= '|badGameId';
			}
		}
		return false;
	}

}
?>