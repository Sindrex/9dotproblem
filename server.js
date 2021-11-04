var express = require("express");
var mysql = require("mysql");
var app = express();
var apiRoutes = express.Router();
app.use(express.json()); // for å tolke JSON
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
  multipleStatements: true,
  insecureAuth : true
});

//Basic endpoint
app.get("/id=:id", (req, res) => {
  console.log("/id=:id got GET request from client: id=" + req.params.id);
  res.sendFile("./public/index.html");
});

//post data
app.post("/api/9dotproblem", (req, res) => {
  console.log("/api/9dotproblem got POST request from client");
  console.log(req.body);
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
  res.json({
      "Url":config.Url + "/api/9dotproblem",
      "TimeLimitSeconds":config.TimeLimitSeconds,
      "ShowTimer":config.ShowTimer,
      "HelpText":config.HelpText,
      "ShowHelpText":config.ShowHelpText,
      "Title":config.Title,
      "RedirectUrl":config.RedirectUrl,
      "RedirectTime":config.RedirectTime,
      "ShowLineAmount":config.ShowLineAmount,
      "ShowTriesAmount":config.ShowTriesAmount,
      "ShowTrainingScreen":config.ShowTrainingScreen,
      "TrainingScreenText":config.TrainingScreenText
  });
  console.log("Sent URL: " + config.Url + "/api/9dotproblem");
});

app.get("/api/conv", (req, res) => {
  console.log("/conv got GET request from client");
  res.status(200);
  new DaoWrapper(pool).getAllConv((status, data) => {
    res.status(status);
    res.json(data);
  });
});

app.get("/api/raw", (req, res) => {
  console.log("/raw got GET request from client");
  res.status(200);
  new DaoWrapper(pool).getAllRaw((status, data) => {
    res.status(status);
    res.json(data);
  });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, function() {
  console.log(`App listening on port ${PORT}`);
});
