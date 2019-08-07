var express = require("express");
var mysql = require("mysql");
var bodyParser = require("body-parser");
var app = express();
var apiRoutes = express.Router();
app.use(bodyParser.json()); // for å tolke JSON
app.use(express.static('./public'));

//app.use(express.static(public_path));
var config = require('./config');

const DaoWrapper = require("./daowrapper.js");

var pool = mysql.createPool({
  connectionLimit: 5,
  host: config.DATABASE_HOST,
  user: config.DATABASE_USER,
  password: config.DATABASE_PASS,
  database: config.DATABASE_DB,
  debug: false,
  multipleStatements: true
});

//Basic endpoint
app.get("/id=:id", (req, res) => {
  console.log("/id=:id got GET request from client: id=" + req.params.id);
  res.sendfile("./public/index.html");
});

//post data
app.post("/api/9dotproblem", (req, res) => {
  console.log("/api/9dotproblem got POST request from client");
  new DaoWrapper(pool).createOne(req.body, (status, data) => {
    res.status(status);
    res.json(data);
  });
});

//get URL
app.get("/game/StreamingAssets", (req, res) => {
  console.log("/game/StreamingAssets got GET request from client");
  res.status(200);
  //res.json(config.URL + ":" + (process.env.PORT || 3000) + "/api/9dotproblem");
  res.json(config.URL + "/api/9dotproblem");
  console.log("Sent URL: " + config.URL);
});

//get max seconds
app.get("/api/maxsec", (req, res) => {
  console.log("/api/maxsec got GET request from client");
  res.status(200);
  res.json(config.MAX_SEC);
  console.log("Sent max sec: " + config.MAX_SEC);
});

//get config options
app.get("/api/config", (req, res) => {
  console.log("/api/config got GET request from client");
  res.status(200);
  res.json({
    config.MAX_SEC,
    config.SHOW_TIMER,
    config.HELP_TEXT
  });
  console.log("Sent max sec: " + config.MAX_SEC);
});

//TESTING--------------------------
app.post("/9dotproblem", (req, res) => {
  console.log("/9dotproblem got POST request from client");
  new DaoWrapper(pool).createOne(req.body, (status, data) => {
    res.status(status);
    res.json(data);
  });
});

//Testing: Logging
var fs = require("fs");

app.post("/9dotproblem/write", (req, res) => {
  console.log("/9dotproblem/write got POST request from client");
  var data = req.body.text;
  console.log("Got data: " + data);

  var prevData = "";
  fs.readFile("./logging/log.txt", "utf-8", (err, readData) => {
    if(err) {
      console.log(err);
      fs.writeFile("./logging/log.txt", data, (err) => {
        if(err) console.log(err);
        else console.log("Successfully written to file!");
      });
    }
    else{
      prevData = readData;
      fs.writeFile("./logging/log.txt", prevData + data, (err) => {
        if(err) console.log(err);
        else console.log("Successfully written to file!");
      });
    }
  });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, function() {
  console.log(`App listening on port ${PORT}`);
});
