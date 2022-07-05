"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ReversiHub").build();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }

function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }

function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }

function afterInit() {
  console.log('Game init voltooid');
} // -------------------------------GAME----------------------------------


var Game = function (url) {
  console.log(url);
  var configMap = {
    apiUrl: url
  };
  var stateMap = {
    gameState: undefined
  };

  var privateInit = function privateInit(callbackFunction) {
    console.log(configMap.apiUrl);
    setInterval(_getCurrentGameState, 2000);
    callbackFunction();
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    stateMap.gameState = Game.Model.getGameState();
  };

  return {
    init: privateInit
  };
}('/api/url'); // --------------------------------DATA-----------------------------------


Game.Data = function () {
  console.log('hallo vanuit module Data');
  var configMap = {
    apiKey: "9f5181271bb7622332fe45cbd7963f2d",
    mock: [{
      url: "api/Spel/Beurt",
      data: {
        ID: 1,
        Omschrijving: "Omschrijvinkje",
        Token: "XXXXX",
        Speler1Token: "1TOKEN",
        Speler2Token: "2TOKEN",
        Bord: "8883WB33BW3888",
        AandeBeurt: 1
      }
    }]
  };
  var stateMap = {
    environment: "development"
  };

  var privateInit = function privateInit(environment) {
    setEnvironment(environment);
    /*
    if(stateMap.environment == `production`){
        serverRequest()
    }
    if(stateMap.environment == 'development'){
        getMockData()
    }
       */
  };

  var setEnvironment = function setEnvironment(environment) {
    var environments = ["production", "development"];

    if (environments.includes(environment)) {
      stateMap.environment = environment;
    } else {
      throw new Error("Environment doesn't exist");
    }
  };

  var serverRequest = function serverRequest() {};

  var get = function get(url) {
    // returns a promise
    if (stateMap.environment == "production") {
      return $.get(url + configMap.apiKey).then(function (r) {
        return r;
      })["catch"](function (e) {
        console.log(e.message);
      });
    } else if (stateMap.environment == "development") {
      return getMockData(url);
    }
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock.find(function (data) {
      return data.url == url;
    }).data;
    return new Promise(function (resolve, reject) {
      if (mockData != undefined) {
        resolve(mockData);
      } else {
        reject("Url not in mock");
      }
    });
  };

  return {
    init: privateInit,
    get: get
  };
}(); // -----------------------------------------------REVERSI-----------------------------


Game.Reversi = function () {
  console.log('hallo vanuit module reversi');
  var configMap = {};

  var privateInit = function privateInit() {
    console.log(configMap.apiUrl);
  };

  return {
    init: privateInit
  };
}(); // ---------------------------------Model-----------------------------


Game.Model = function () {
  console.log('hallo vanuit module Model');
  var configMap = {};

  var privateInit = function privateInit() {
    console.log(configMap.apiUrl);
  };

  var getWeather = function getWeather() {
    var url = "http://api.openweathermap.org/data/2.5/weather?q=zwolle&apikey=";
    Game.Data.get(url);
  };

  var _getGameState = /*#__PURE__*/function () {
    var _ref = _asyncToGenerator( /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
      var result, turn;
      return regeneratorRuntime.wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              _context.next = 2;
              return Game.Data.get("api/Spel/Beurt");

            case 2:
              result = _context.sent;
              //"/game/{id}"
              turn = result.AandeBeurt;

              if (turn == 0 || turn == 1 || turn == 2) {
                _context.next = 6;
                break;
              }

              throw new Error("Can't determine turn order");

            case 6:
              return _context.abrupt("return", result);

            case 7:
            case "end":
              return _context.stop();
          }
        }
      }, _callee);
    }));

    return function _getGameState() {
      return _ref.apply(this, arguments);
    };
  }();

  return {
    init: privateInit,
    getGameState: _getGameState
  };
}();

function toggleFeedback(elementId) {
  var x = document.getElementById(elementId);

  if (x.style.display === "none") {
    x.style.display = "block";
  } else {
    x.style.display = "none";
  }
}

$("button").on("click", function () {
  alert("The button was clicked.");
});

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
  }

  _createClass(FeedbackWidget, [{
    key: "elementId",
    get: function get() {
      //getter, set keyword voor setter methode
      return this._elementId;
    }
  }, {
    key: "getCounterKey",
    value: function getCounterKey() {
      return this._elementId + "counter";
    }
  }, {
    key: "getCounter",
    value: function getCounter() {
      if (localStorage.getItem(this.getCounterKey())) {
        return parseInt(localStorage.getItem(this.getCounterKey()));
      }

      return null;
    }
  }, {
    key: "generateLogKey",
    value: function generateLogKey(counter) {
      return this._elementId + counter;
    }
  }, {
    key: "getLog",
    value: function getLog(id) {
      return localStorage.getItem(this.generateLogKey(id));
    }
  }, {
    key: "checkIfSuccess",
    value: function checkIfSuccess(type) {
      if (type === "success") {
        return type;
      }

      return "error";
    }
  }, {
    key: "negativeToZero",
    value: function negativeToZero(number) {
      if (number < 0) {
        return 0;
      }

      return number;
    }
  }, {
    key: "negativeToOne",
    value: function negativeToOne(number) {
      if (number < 1) {
        return 1;
      }

      return number;
    }
  }, {
    key: "show",
    value: function show(message, type) {
      var x = document.getElementById(this._elementId);
      $(x).text(message);

      if (type === "success") {
        $(x).attr('class', 'alert alert-success');
      } else {
        $(x).attr('class', 'alert alert-danger');
      }

      x.style.display = "block";
      var xx = {
        message: message,
        type: type
      };
      this.log(xx);
    }
  }, {
    key: "hide",
    value: function hide() {
      var x = document.getElementById(this._elementId);
      x.style.display = "none";
    }
  }, {
    key: "log",
    value: function log(message) {
      var counterNumber = 1;
      var counterKey = this.getCounterKey();

      if (this.getCounter() != null) {
        counterNumber = this.getCounter();
      } else {
        localStorage.setItem(counterKey, 1);
      }

      var string = JSON.stringify(message);
      localStorage.setItem(this.generateLogKey(counterNumber), string);
      counterNumber++;
      localStorage.setItem(counterKey, counterNumber);

      if (counterNumber > 10) {
        this.removeLog(counterNumber - 11);
      }
    }
  }, {
    key: "removeLog",
    value: function removeLog(idNumber) {
      localStorage.removeItem(this.generateLogKey(idNumber));
    }
  }, {
    key: "logArrayToString",
    value: function logArrayToString(array) {
      var string = "";
      array.forEach(function (element) {
        if (string != "") {
          string += " \n ";
        }

        var type = "error";

        if (element.type === "success") {
          type = "success";
        }

        string += "| ".concat(type, " |");
        string += " - ";
        string += "".concat(element.message);
      });
      return string;
    }
  }, {
    key: "history",
    value: function history() {
      if (this.getCounter() == null) {
        return;
      }

      var logs;
      var logNumberMax = this.getCounter();
      var logNumberMin = this.negativeToOne(logNumberMax - 10);

      for (var i = logNumberMin; i < logNumberMax; i++) {
        if (logs == null) {
          logs = "[";
        }

        logs += this.getLog(i);

        if (i != logNumberMax - 1) {
          logs += ",";
        }
      }

      logs += "]";
      var message = JSON.parse(logs);
      this.show(this.logArrayToString(message), "success");
    }
  }]);

  return FeedbackWidget;
}();