DROP TABLE IF EXISTS 9dotproblem;
DROP TABLE IF EXISTS 9dotproblemRaw;
DROP TABLE IF EXISTS 9dotproblemConv;

CREATE TABLE 9dotproblemRaw (
	sequence_nr INT(11) NOT NULL AUTO_INCREMENT,
	player_id VARCHAR(11) NOT NULL,
	try_nr INT(11) NOT NULL,
	point1 VARCHAR(32),
	point2 VARCHAR(32),
	point3 VARCHAR(32),
	point4 VARCHAR(32),
	point5 VARCHAR(32),
	PRIMARY KEY (sequence_nr)
);

ALTER TABLE 9dotproblemRaw
  ADD CONSTRAINT 9dotproblemRaw UNIQUE(player_id, try_nr);

CREATE TABLE 9dotproblemConv (
	sequence_nr INT(11) NOT NULL AUTO_INCREMENT,
	player_id VARCHAR(11) NOT NULL,
	try_nr INT(11) NOT NULL,
	node1 VARCHAR(32),
	node2 VARCHAR(32),
	node3 VARCHAR(32),
	node4 VARCHAR(32),
	node5 VARCHAR(32),
	accepted BIT,
	PRIMARY KEY (sequence_nr)
);

ALTER TABLE 9dotproblemConv
  ADD CONSTRAINT 9dotproblemConv UNIQUE(player_id, try_nr);