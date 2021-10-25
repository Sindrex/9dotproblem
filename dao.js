module.exports = class Dao {
  constructor(pool) {
    // Dependency Injection
    this.pool = pool;
  }

  query(sql, params, callback) {
    this.pool.getConnection((err, connection) => {
      console.log("dao: connecting to database");
      if (err) {
        console.log("dao: error connecting");
        console.log(err);
        callback(500, { error: "error with connecting" });
      } else {
        console.log("dao: running sql: " + sql);
        connection.query(sql, params, (err, rows) => {
          connection.release();
          if (err) {
            console.log(err);
            callback(500, { error: "error querying" });
          } else {
            console.log("dao: returning rows");
            callback(200, rows);
          }
        });
      }
    });
  }
};
