<?php
	spl_autoload_register('load_class'); 	
	
	function load_class($class){
		// Real path of your functions folder 
		// (go on the page realPath.php with your web browser to know it)
		$path = 'Real path of your function folder';
		
		if(file_exists($path.$class.'.class.php') AND is_file($path.$class.'.class.php')){		
			require($path.$class.'.class.php'); 	
		}
	}

	// Secure data before save it on MySQL
	function secure_db($var){	 
		 $var = trim($var);
		 $var = preg_replace('#</?|>#', '', $var);
		 $var = addcslashes($var, '%_()');	
		 $var = htmlentities($var, ENT_QUOTES, 'UTF-8');	
		 $var = preg_replace('#</?script>#', '', $var);
		 $var = preg_replace('#java|javascript|ajax#i', '', $var);		
		 return $var;
	}
	
	/// Secure data before save it on MySQL (for prepared query)
	function secure_prepare_db($var){	 
		 $var = trim($var);
		 $var = preg_replace('#</?|>#', '', $var);
		 $var = addcslashes($var, '%_()');	
		 $var = htmlentities($var, ENT_QUOTES, 'UTF-8');	
		 $var = preg_replace('#</?script>#', '', $var);
		 $var = preg_replace('#java|javascript|ajax#i', '', $var);		
		 return $var;
	}
	
	//Prepare MySQL data to be displayed
	function display_db($var){
		 $var = stripslashes($var);
		 return $var;
	}
	
?>