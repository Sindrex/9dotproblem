DROP TABLE IF EXISTS 9dotproblem.9dotproblemRaw;
DROP TABLE IF EXISTS 9dotproblem.9dotproblemConv;
DROP SCHEMA IF EXISTS 9dotproblem;

CREATE SCHEMA 9dotproblem;

CREATE TABLE 9dotproblem.9dotproblemRaw (
	sequence_nr INT NOT NULL AUTO_INCREMENT,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	player_id VARCHAR(11) NOT NULL,
	try_nr INT NOT NULL,
	point1 VARCHAR(32),
	point2 VARCHAR(32),
	point3 VARCHAR(32),
	point4 VARCHAR(32),
	point5 VARCHAR(32),
	timer1 FLOAT,
	timer2 FLOAT,
	timer3 FLOAT,
	timer4 FLOAT,
	timer5 FLOAT,
	timer6 FLOAT,
	timer7 FLOAT,
	timer8 FLOAT,
	totalTime FLOAT,
	hasTabbedOut INT,
	tabbedOutAmount INT,
	tabbedOutTime FLOAT,
	PRIMARY KEY (sequence_nr)
);

CREATE TABLE 9dotproblem.9dotproblemConv (
	sequence_nr INT NOT NULL AUTO_INCREMENT,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	player_id VARCHAR(11) NOT NULL,
	try_nr INT NOT NULL,
	node1 VARCHAR(32),
	node2 VARCHAR(32),
	node3 VARCHAR(32),
	node4 VARCHAR(32),
	node5 VARCHAR(32),
	accepted INT,
	PRIMARY KEY (sequence_nr)
);
