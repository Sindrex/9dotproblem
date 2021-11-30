const Dao = require("./dao.js");

module.exports = class DaoWrapper extends Dao {
  constructor(pool) {
    super(pool);
  }

  createOneRaw(json, callback) {
    var valRaw = [
      json.created_at, json.player_id, json.try_nr, json.point1, json.point2, json.point3,
      json.point4, json.point5, json.timer1, json.timer2, json.timer3, json.timer4, json.timer5, json.timer6, json.timer7, json.timer8, json.totalTime,
      json.hasTabbedOut, json.tabbedOutAmount, json.tabbedOutTime
    ];
    super.query(
      "insert into 9dotproblem.9dotproblemRaw (" +
      "created_at, player_id, try_nr, point1, point2, point3, point4, point5, timer1, timer2, timer3, timer4, timer5, timer6, timer7, timer8, totalTime, hasTabbedOut, tabbedOutAmount, tabbedOutTime" +
      ") values (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
      valRaw,
      callback
    );
  }
  createOneConv(json, callback) {
    var valConv = [
      json.created_at, json.player_id, json.try_nr, json.node1, json.node2, json.node3,
      json.node4, json.node5, json.accepted
    ];
    super.query(
      "insert into 9dotproblem.9dotproblemConv (created_at, player_id, try_nr, node1, node2, node3, " +
      "node4, node5, accepted) values (?,?,?,?,?,?,?,?,?)",
      valConv,
      callback
    );
  }
  getAllRaw(callback) {
    super.query(
      "select * from 9dotproblem.9dotproblemraw;",
      null,
      callback
    );
  }
  getAllConv(callback) {
    super.query(
      "select * from 9dotproblem.9dotproblemconv;",
      null,
      callback
    );
  }
};
