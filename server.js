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
  host: config.DatabaseHost,
  user: config.DatabaseUser,
  password: config.DatabasePass,
  database: config.DatabaseDB,
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
  console.log("json: " + req.body);
  new DaoWrapper(pool).createOneRaw(req.body, (status, data) => {
    if(status == 200){
      new DaoWrapper(pool).createOneConv(req.body, (status, data) => {
        res.status(status);
        res.json(data);
      });
    }
    else{
      res.status(status);
      res.json(data);
    }
  });
});

//get config
app.get("/game/StreamingAssets", (req, res) => {
  console.log("/game/StreamingAssets got GET request from client");
  res.status(200);
  //res.json(config.URL + ":" + (process.env.PORT || 3000) + "/api/9dotproblem");
  res.json({
      "URL":config.URL + "/api/9dotproblem",
      "MAX_SEC":config.MAX_SEC,
      "SHOW_TIMER":config.SHOW_TIMER,
      "HELP_TEXT":config.HELP_TEXT,
      "REDIRECT_URL":config.REDIRECT_URL,
      "REDIRECT_TIME":config.REDIRECT_TIME
  });
  console.log("Sent URL: " + config.URL + "/api/9dotproblem");
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
