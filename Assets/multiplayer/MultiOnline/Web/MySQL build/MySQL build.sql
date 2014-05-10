-- phpMyAdmin SQL Dump
-- version 3.5.2.1
-- http://www.phpmyadmin.net
--
-- Client: localhost
-- Généré le: Jeu 01 Août 2013 à 14:30
-- Version du serveur: 5.0.84-log
-- Version de PHP: 5.2.17

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;


-- --------------------------------------------------------

--
-- Structure de la table `games`
--

CREATE TABLE IF NOT EXISTS `games` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `userHost_id` int(10) unsigned NOT NULL,
  `name` varchar(100) collate latin1_german1_ci NOT NULL,
  `port` varchar(10) collate latin1_german1_ci NOT NULL,
  `maxPlayer` tinyint(3) unsigned NOT NULL,
  `usePass` tinyint(1) default NULL,
  `status` enum('0','1') collate latin1_german1_ci NOT NULL,
  `dateReg` datetime NOT NULL,
  PRIMARY KEY  (`id`),
  KEY `user_id` (`userHost_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_german1_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Structure de la table `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `userName` varchar(100) collate latin1_german1_ci NOT NULL,
  `mail` varchar(100) collate latin1_german1_ci NOT NULL,
  `privateIP` varchar(100) collate latin1_german1_ci default NULL,
  `publicIP` varchar(100) collate latin1_german1_ci default NULL,
  `pass` varchar(100) collate latin1_german1_ci NOT NULL,
  `dateReg` timestamp NULL default NULL,
  `login` tinyint(1) default NULL,
  `loginKey` varchar(100) collate latin1_german1_ci default NULL,
  `lastLog` timestamp NULL default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_german1_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Structure de la table `users_has_games`
--

CREATE TABLE IF NOT EXISTS `users_has_games` (
  `user_id` int(10) unsigned NOT NULL,
  `game_id` int(10) unsigned NOT NULL,
  KEY `user_id` (`user_id`),
  KEY `game_id` (`game_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_german1_ci;

--
-- Contraintes pour les tables exportées
--

--
-- Contraintes pour la table `games`
--
ALTER TABLE `games`
  ADD CONSTRAINT `games_ibfk_1` FOREIGN KEY (`userHost_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Contraintes pour la table `users_has_games`
--
ALTER TABLE `users_has_games`
  ADD CONSTRAINT `users_has_games_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `users_has_games_ibfk_2` FOREIGN KEY (`game_id`) REFERENCES `games` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
