const Dao = require("./dao.js");

module.exports = class DaoWrapper extends Dao {
  constructor(pool) {
    super(pool);
  }

  createOneRaw(json, callback) {
    var valRaw = [json.player_id, json.try_nr, json.point1, json.point2, json.point3,
                  json.point4, json.point5];
    super.query(
      "insert into 9dotproblemRaw (player_id, try_nr, point1, point2, point3, " +
      "point4, point5) values (?,?,?,?,?,?,?)",
      valRaw,
      callback
    );
  }
  createOneConv(json, callback) {
    var valConv = [json.player_id, json.try_nr, json.node1, json.node2, json.node3,
                  json.node4, json.node5, json.accepted == 'True' ? 1 : 0];
    super.query(
      "insert into 9dotproblemConv (player_id, try_nr, node1, node2, node3, " +
      "node4, node5, accepted) values (?,?,?,?,?,?,?,?)",
      valConv,
      callback
    );
  }
};
