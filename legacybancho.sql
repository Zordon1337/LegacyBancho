-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: 31. Des, 2023 01:04 AM
-- Tjener-versjon: 10.4.32-MariaDB
-- PHP Version: 8.1.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `legacybancho`
--

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `scores`
--

CREATE TABLE `scores` (
  `Checksum` text NOT NULL,
  `onlineId` int(11) NOT NULL,
  `playerName` text NOT NULL,
  `totalScore` int(11) NOT NULL,
  `maxCombo` int(11) NOT NULL,
  `count50` int(11) NOT NULL,
  `count100` int(11) NOT NULL,
  `count300` int(11) NOT NULL,
  `countMiss` int(11) NOT NULL,
  `countKatu` int(11) NOT NULL,
  `countGeki` int(11) NOT NULL,
  `perfect` tinyint(1) NOT NULL,
  `enabledMods` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `AvatarFileName` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `users`
--

CREATE TABLE `users` (
  `UserId` int(11) NOT NULL,
  `Accuracy` double NOT NULL,
  `CurrentRank` int(11) NOT NULL,
  `Score` int(11) NOT NULL,
  `Password` text NOT NULL,
  `Username` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
