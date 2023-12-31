-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: 31. Des, 2023 19:08 PM
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
-- Tabellstruktur for tabell `beatmaps`
--

CREATE TABLE `beatmaps` (
  `id` int(11) NOT NULL,
  `checksum` text NOT NULL,
  `status` int(11) NOT NULL,
  `submit_date` text NOT NULL,
  `ranked_data` text NOT NULL,
  `creator` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dataark for tabell `beatmaps`
--

INSERT INTO `beatmaps` (`id`, `checksum`, `status`, `submit_date`, `ranked_data`, `creator`) VALUES
(1, 'cf8bd375f2708f152562a919247ba09a', 2, '2007-XX-XX', '2007-XX-XX', 'peppy'),
(2, '974b72f33a25bd5ef297bd8682d7fa79', 2, '', '', ''),
(3, 'ea0df9f890e7e9e7ad4d3862a7823359', 2, '', '', ''),
(4, '774861583d38346a3876ade4116ebbc0', 2, '', '', '');

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
