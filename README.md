# 9 DOT PROBLEM
By Sindre "Sindrex" Haugland Paulshus

For BI

Summer 2019

## About
This project is a digitalization of the "9 Dot Problem", a puzzle used to analyze psychological phenomena. The user must think "outside the box" to be able to solve the puzzle.

The project consists of:
 * A configurable NodeJS HTTP server.
 * A Unity WebGL exported game simulating the puzzle.
 * Source Code for the Unity game.
 * Initialization script for the database.
 * Diagram of the game space and data nodes.

Trello board: https://trello.com/b/rl18dEEW/bi-9-dot-problem

## Installation and running
### Download and install
Download the project or clone the repo:
`npm i https://github.com/Sindrex/9dotproblem`  

Install the necessary dependencies using:
`npm install`

### Configuration
There is a single JSON formatted file called "config.json". It has the following which need to be configured for the server to work:
 * **DATABASE_HOST:** The URL to the database host.
 * **DATABASE_USER:** The user for the database.
 * **DATABASE_PASS:** The password for the user.
 * **DATABASE_DB:** Which database to use within this user and host.
 * **URL:** The URL which the server is hosted on. Domain name or IP address. E.g. "https://ninedotproblem.herokuapp.com".
   Note that you might need ":PORT" added to the end of the URL, where PORT is the port the server is listening on. This is not always the case, so you will have to test it out for yourself. For example "http://localhost:3000".
 * **MAX_SEC:** The number of *seconds* a user has to do the test. Must be an integer larger than 0.
 * **SHOW_TIMER:** Whether or not the timer should be shown. Set with either *"true"* or *"false"*.
 * **HELP_TEXT:** What text the help button should show. A text string of maximum 450 characters including spaces.

 *Note: the database needs to be an SQL database, preferrably using Mysql.*

### Running server
Run `npm start` from the top folder.

## Dependencies
Programs used to run:
 * Git bash or equivalent
 * NPM 6.4.1 or equivalent
 * NodeJS 8.12.0 or higher

## Folder hierarchy guide
The has a simple structure, due to its low size.
 * In the top folder you will find the server, the config file, dao files, the DB initialization script ("sqltableinit.txt").
 * Under "/public" you will find the index.html file hosted by the server. This file has an iframe hosting the game. You will find the Unity WebGL exported game under "/public/game".
 * Under "/unity_source" you will find the game's source code. This should be able to be opened by the Unity editor (version 2017.3.1f1).

## Understanding the database
The database consists of 2 tables:
 * Raw data: Stored in the 9dotproblemRaw table. Coordinates (x,y) of the points in game space clicked by the user. The game space is a coordinate system where (0,0) is in the bottom-left corner and (17.9, 17.6) is the top-right corner. *Note that this is the full area the user can draw in, and not just what is seen from the camera during gameplay.* The dots have a radius of 0.4 and their coordinates are as follows (see data diagram for dots' identities):
   * 1: (6.9, 6.6)
   * 2: (8.9, 6.6)
   * 3: (10.9, 6.6)
   * 4: (6.9, 8.6)
   * 5: (8.9, 8.6)
   * 6: (10.9, 8.6)
   * 7: (6.9, 10.6)
   * 8: (8.9, 10.6)
   * 9: (10.9, 10.6)
 * Converted data: Stored in the 9dotproblemConv table. Contains the raw data converted to what area they fit in (each area is called a 'node'). See data diagram for all the nodes. Also contains an 'accepted' field to say if the user managed to draw through all nine dots.
