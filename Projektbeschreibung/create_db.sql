-- Drop and create database

DROP DATABASE IF EXISTS Poker;
CREATE DATABASE Poker;
USE Poker;

-- Create tables

CREATE TABLE User (
	Id int PRIMARY KEY AUTO_INCREMENT,
	Username varchar(30) NOT NULL,
	Salt varchar(50) NOT NULL,
	PwHash varchar(128) NOT NULL,
	Is_Deleted TINYINT(1) NOT NULL DEFAULT 0,

	UNIQUE (Username)
);

SHOW COLUMNS FROM User;
