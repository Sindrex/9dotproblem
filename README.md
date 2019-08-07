# 9 DOT PROBLEM
By Sindre "Sindrex" Haugland Paulshus

For BI

Summer 2019

## About
This project is a digitalization of the "9 Dot Problem", a puzzle used to analyze psychological phenomena. The user must think "outside the box" to be able to solve the puzzle.

The product consists of:
 * A configurable NodeJS HTTP server.
 * A Unity WebGL exported game simulating the puzzle.
 * Source Code for the Unity game.
 * Initialization script for the database.

Trello board: https://trello.com/b/rl18dEEW/bi-9-dot-problem

## Installation and Running
### Download and Install
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
 * Under "/unity_source" you will find the game's source code. This should be able to be opened by the Unity editor.
