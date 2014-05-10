<?php
/* MODb : this PHP class is used for the connections and queries to MySQL
 * This class is the mother of all others
 */
 
class MODb {
	
	/**************** YOU MUST COMPLETE THESE PARAMETERS ********************/
	// Enter here your database username :
	private $user = 'Username';
	// Enter here your database passwords :
	private $pass = 'password';
	// Enter here your database server (it's often "localhost") :
	private $host = 'localhost';
	// Enter here the name of your database :
	private $dbName = 'database name';	
	/*********************************************************************/
	
	private $connect;
	protected $dbResult;
		 
	// Connect to MySQL	
	function __construct(){
		try{
			$dns = 'mysql:host='.$this->host.';dbname='.$this->dbName;
			$bdd = new PDO($dns, $this->user, $this->pass);
			$this->connect = $bdd;
		}catch(PDOExeption $e){
			$this->getError($e);
		}							
	}
			
	function __destruct(){
		$this->close();
	}
		
	// Close connect
	public function close(){
	 	$this->connect = NULL;
	}
		
	// Get error
	private function getError($e){
		echo 'Error ! :'.$e->getMessage().'<br>';
		echo 'N° : '.$e->getCode();
		exit();
	}
		
	// Simple query to MySQL
	public function query($query){
		try{
			$data = $this->connect->query($query);
			return $data;
		}catch(PDOExeption $e){
			$this->getError($e);
		}	
	}		
		
	// Get the last saved ID
	public function lastId(){
		return $this->connect->lastInsertId();
	} 
		
	public function fetch($query){
		try{
			$data = $query->fetch(PDO::FETCH_ASSOC);
			$query->closeCursor();
			return $data;			
		}catch(PDOExeption $e){
			$this->getError($e);
		}	
	}
			
	// Return the result of a query with fetchObject 
	public function fetchObjet($query){
		try{
			$data = $query->fetchObject();					
			$query->closeCursor();
			return $data;			
		}catch(PDOExeption $e){
			$this->getError($e);
		}	
	}
		
	// Return the result of a query with fetchAll
	public function fetchAll($query){
		try{
			$data = $query->fetchAll(PDO::FETCH_ASSOC);
			$query->closeCursor();
			return $data;
		}catch(PDOExeption $e){
			$this->getError($e);
		}	
	}
		
	// Simpe exec 
	public function exec($var){
		try{
			$query = $this->connect->exec($var);
		}catch(PDOExeption $e){
			$this->getError($e);
		}	
	}
	
	// Query MySQL return with fetchAll
	public function query_fetchAll($var){		 
		try{
			$query = $this->connect->query($var);
			$data = $query->fetchAll(PDO::FETCH_ASSOC);
			$query->closeCursor();
			 return $data;
		}catch(PDOExeption $e){
			$this->getError($e);
		}	
	}
	
	// Query MySQL return with fetchObject
	public function query_fetchObject($var){		 
		try{
			$query = $this->connect->query($var);
			$data = $query->fetchObject();
			$query->closeCursor();
			return $data;
		}catch(PDOExeption $e){
			$this->getError($e);
		}	
	}
			
	// Pepare and execute a query
	public function prepare_exec($var, $var_array){
		try{
			$query = $modele = $this->connect->prepare($var);
			$modele->execute($var_array);
		}catch(PDOExeption $e){
			$this->getError($e);
		}
	}	
}	
 ?>