# 9 DOT PROBLEM
By Sindre "Sindrex" Haugland Paulshus

For BI

Summer 2019 and Oct-Dec 2021

## About
This project is a digitalization of the "9 Dot Problem", a puzzle used to analyze psychological phenomena. The user must think "outside the box" to be able to solve the puzzle.

The project consists of:
 * A configurable NodeJS HTTP server.
 * A Unity WebGL exported game simulating the puzzle.
 * Source Code for the Unity game.
 * Initialization script for the database.
 * Diagram of the game space and data nodes.

## Installation and running
### Download and install
Download the project or clone the repo:
`npm i https://github.com/Sindrex/9dotproblem`  

Install the necessary dependencies using:
`npm install`

### Configuration
There is a single JSON formatted file called "config.json". It has the following which need to be configured for the server to work:
 * **DatabaseHost:** The URL to the database host.
 * **DatabaseUser:** The user for the database.
 * **DatabasePass:** The password for the user.
 * **DatabaseDB:** Which database to use within this user and host.
 * **Url:** The URL which the server is hosted on. Domain name or IP address. E.g. "https://ninedotproblem.herokuapp.com".
   Note that you might need ":PORT" added to the end of the URL, where PORT is the port the server is listening on. This is not always the case, so you will have to test it out for yourself. For example "http://localhost:3000".
 * **TimeLimitSeconds:** The number of *seconds* a user has to do the test. Must be an integer larger than 0.
 * **ShowTimer:** Whether or not the timer should be shown. Set with either *"true"* or *"false"*.
 * **HelpText:** What text the help button should show. A text string of maximum 450 characters including spaces.
 * **Title:** The title shown on the starting screen.
 * **RedirectUrl:** The URL to where the user will be redirected after the test is over. Set empty if you do not want the user to be directed. *Note: you also have to access the website with the parameter "redirect=true" for the user to be redirected.*
 * **RedirectTime:** The number of *seconds* from the test is finished till the user is redirected to the REDIRECT_URL. (DEPRECATED)
  * **ShowLineAmount:** Whether or not to show the text with amount of lines remaining for a given try.
  * **ShowTriesAmount:** Whether or not to show the text with amount of tries submitted.
  * **ShowTrainingScreen:** Whether or not to show the training screen (the screeb after start and before the main part).
  * **TrainingScreenText:** The text shown on the training screen. You can add "\n" for newline.

### Running server
Run `npm start` from the top folder.

### Open and try
`http://localhost:3000/?id=sindre&redirect=true`

## Dependencies
Programs used to run:
 * Git bash or equivalent
 * NPM 6.4.1 or equivalent
 * NodeJS 8.12.0 or higher
 * MySQL database.

## Folder hierarchy guide
The has a simple structure, due to its low size.
 * In the top folder you will find the server, the config file, dao files, the DB initialization script ("sqltableinit.txt").
 * Under "/public" you will find the index.html file hosted by the server. This file has an iframe hosting the game. You will find the Unity WebGL exported game under "/public/game".
 * Under "/unity_source" you will find the game's source code. This should be able to be opened by the Unity editor (version 2017.3.1f1).

## Understanding the database
The database consists of 2 tables:
 * Raw data: Stored in the 9dotproblemRaw table. Coordinates (x,y) of the points in game space clicked by the user. The game space is a coordinate system where (4.6, 4.2) is in the bottom-left corner and (13.2, 13) is the top-right corner. *Note that this is the area the user can draw in, boxed in by borders as seen during gameplay.* The dots have a radius of 0.5 and their coordinates are as follows (see data diagram for dots' identities):
   * 1: (6.9, 6.6)
   * 2: (8.9, 6.6)
   * 3: (10.9, 6.6)
   * 4: (6.9, 8.6)
   * 5: (8.9, 8.6)
   * 6: (10.9, 8.6)
   * 7: (6.9, 10.6)
   * 8: (8.9, 10.6)
   * 9: (10.9, 10.6)
 In addition 8 timers are stored. These are the time between different draw actions taken by the user.
   * timer1: When the first point is created (and the user starts drawing first line. Time after start of the try.).
   * timer2: When the second point is created (and the user stops drawing first line).
   * timer3: When the user starts drawing second line.
   * timer4: When the third point is created (and the user stops drawing the second line).
   * timer5: When the user starts drawing the third line.
   * timer6: When the fourth point is created (and the user stops drawing the third line).
   * timer7: When the user starts drawing the fourth line.
   * timer8: When the fifth point is created (and the user stops drawing the fourth and final line)
 * Converted data: Stored in the 9dotproblemConv table. Contains the raw data converted to what area they fit in (each area is called a 'node'). See data diagram for all the nodes. Also contains an 'accepted' field to say if the user managed to draw through all nine dots.

## Integration with Qualtrics
Valid as of 02.10.2019.

This system has support for Qualtrics in two ways:
 1. It can redirect the user to a Qualtrics survey at the end of their test and pass along their ID.
 2. Qualtrics can redirect to the system and pass along the user's ID and whether or not the system shall redirect at the end of the test.

Here's one way to integrate with Qualtrics so both of these options are possible. *Note: The system cannot redirect to the same survey as redirected to itself. There has to be two seperate surveys if you want to redirect to the system and want the system to redirect at the end of the test.*

### How to setup Qualtrics redirect to 9dotproblem
  1. Create a survey in Qualtrics.
  2. Open up "Survey Flow".
  3. Add a new element. Click "Embedded Data".
  5. From the dropdown choose "Survey Metadata" -> "ResponseID".
  6. Click "Save Flow".
  7. Open up "Survey Options".
  8. Under "Survey Termination", select "Redirect".
  9. In the redirect input box, enter the following: The URL the server is hosted on, e.g. "https://ninedotproblem.herokuapp.com/". *Remember to get the "/" at the end*. Then add the following field at the end of the URL: "?id=${e://Field/ResponseID}".
  10. OPTIONAL: If you want the 9dotproblem system to redirect to it's REDIRECT_URL, add "&redirect=true" at the end of the URL. *Note: remember to set the system's config REDIRECT_URL if you want it to redirect the user after a completed test.*
  11. Save, publish and you are done! This survey will now redirect to the 9dotproblem system.

### How to setup 9dotproblem redirect to Qualtrics
  1. Create a survey in Qualtrics.
  2. Open up "Survey Flow".
  3. Add a new element. Click "Embedded Data".
  4. From the dropdown choose "Survey Metadata" -> "ResponseID".
  5. Add a new field to the same block. Call it "id". *Do not set a custom value for it*.
  6. Add a new element. Click "Branch".
  7. Add a condition to the branch. Under "Question" select "Embedded Data". In the left inputfield, write "id". In the middle dropdown select "Is Empty".
  8. Add a new element underneath the branch (so that the branch goes there). Click "Embedded Data".
  9. From the dropdown choose "Existing Embedded Data" -> "id".
  10. Click "Set a Value Now" for "id". Write "${e://Field/ResponseID}"
  11. Click "Save Flow".
  12. Publish and you are done! This survey will now use the ID gotten from the 9dotproblem system.


## Supported URL params
* id: string. The participant's id.
* redirect: bool ("true" or "false"). Whether or not to redirect after finishing the test.
* showt: bool ("true" or "false"). Whether or not to show the countdown timer. Note that if the server config "ShowTimer" is true, this param does nothing.