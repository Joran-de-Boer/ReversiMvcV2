"use strict";

// -------------------------------GAME----------------------------------
var Game = function (apiUrl) {
  var configMap = {
    apiUrl: undefined,
    gameToken: gameToken,
    playerToken: playerToken,
    interval: undefined //updateStaticsCall: updateStatistics

  };
  var stateMap = {
    gameState: undefined
  };

  var win = function win() {
    Game.Data.post("localhost:7184/Spelcontroller/win");
    clearInterval(configMap.interval);
  };

  var lose = function lose() {
    Game.Data.post("localhost:7184/Spelcontroller/lose");
    clearInterval(configMap.interval);
  };

  var draw = function draw() {
    Game.Data.post("localhost:7184/Spelcontroller/draw");
    clearInterval(configMap.interval);
  };

  var privateInit = function privateInit(updateStatisticsCall) {
    configMap.apiUrl = apiUrl;
    console.log(updateStatisticsCall);
    Game.Model.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken);
    Game.Data.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken);
    Game.Reversi.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken, win, draw, lose);
    Game.Stats.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken, updateStatisticsCall);
    window.addEventListener("beforeunload", Game.Reversi.doLeave);
    var interval = setInterval(function () {
      _getCurrentGameState(interval);
    }, 20);
    configMap.interval = interval;
  };

  var _getCurrentGameState = function _getCurrentGameState(interval) {
    var newGameState = Game.Model.getGameState();
    newGameState.then(function (response) {
      console.log(response);

      if (response.afgelopen) {
        clearInterval(interval);
      }

      if (response != stateMap.gameState) {
        stateMap.gameState = response;
        Game.Reversi.updateGameState(response);
      }
    });
  };

  return {
    init: privateInit
  };
}('https://localhost:7258/'); // --------------------------------DATA-----------------------------------


Game.Data = function () {
  var configMap = {
    apiUrl: undefined,
    gameToken: undefined,
    playerToken: undefined
  };

  var privateInit = function privateInit(url, gToken, pToken) {
    configMap.apiUrl = url;
    configMap.gameToken = gToken;
    configMap.playerToken = pToken;
  };

  var get = function get(url) {
    //api request from url
    return fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
  };

  var post = function post(url, data) {
    //api request from url
    return fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    });
  };

  var put = function put(url, data) {
    //api request from url
    return fetch(url, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    });
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
    get: get,
    post: post,
    put: put
  };
}(); // -----------------------------------------------REVERSI-----------------------------


Game.Reversi = function () {
  var configMap = {
    Board: undefined,
    Beurt: undefined,
    playerToken: undefined,
    apiUrl: undefined,
    win: undefined,
    draw: undefined,
    lose: undefined
  };

  var addChip = function addChip(x, y, color) {
    var tile = $("#".concat(x, "_").concat(y))[0];

    if (tile.classList.contains("Tile")) {
      var chip = document.createElement("div");
      chip.className = "Chip";

      if (color === "b") {
        chip.classList.add("Black");
      }

      if (color === "w") {
        chip.classList.add("White");
      }

      if (tile.children.length == 0) {
        tile.appendChild(chip);
      }
    }
  };

  $().ready(function () {
    $(".Tile").click(addChip);
  });

  var privateInit = function privateInit(url, gameToken, playerToken, win, draw, lose) {
    configMap.apiUrl = url;
    configMap.gameToken = gameToken;
    configMap.playerToken = playerToken;
    configMap.win = win;
    configMap.draw = draw;
    configMap.lose = lose;
  };

  var isNumber = function isNumber(_char) {
    if (typeof _char !== 'string') {
      return false;
    }

    if (_char.trim() === '') {
      return false;
    }

    return !isNaN(_char);
  };

  var updateBoard = function updateBoard(Board) {
    $(".Chip").remove();
    var x = 0;
    var y = 0;
    var BoardArr = Array.from(Board);
    BoardArr.forEach(function (_char2) {
      if (x == 8) {
        y++;
        x = 0;
      }

      if (_char2 == "W") {
        addChip(x, y, "w");
        x++;
      }

      if (_char2 == "B") {
        addChip(x, y, "b");
        x++;
      }

      if (isNumber(_char2)) {
        var num = parseInt(_char2);

        for (var i = 0; i < num; i++) {
          x++;
        }
      }
    });
  };

  var doTurn = function doTurn(event) {
    var tile = event.target;
    var x = tile.id.split("_")[0];
    var y = tile.id.split("_")[1];
    var body = {
      spelToken: configMap.gameToken,
      spelerToken: configMap.playerToken,
      zet: {
        rijZet: x,
        kolomZet: y
      }
    };
    Game.Data.put("".concat(configMap.apiUrl, "api/spel/zet"), body).then(function (response) {
      console.log(response);
    }); //Game._getCurrentGameState();
  };

  var doPass = function doPass() {
    var body = {
      spelToken: configMap.gameToken,
      spelerToken: configMap.playerToken
    };
    Game.Data.put("".concat(configMap.apiUrl, "api/spel/pass"), body).then(function (response) {
      console.log(response);
    })["catch"](function (error) {
      console.log(error);
    }).then(function () {//_getCurrentGameState();
    });
  };

  var doLeave = function doLeave() {
    var body = {
      SpelId: configMap.gameToken,
      SpelerId: configMap.playerToken
    };
    Game.Data.put("".concat(configMap.apiUrl, "api/spel/leave"), body);
  };

  var updateBeurt = function updateBeurt(bord, beurt, ZetMogelijk, Afgelopen, Gewonnen) {
    $(".Tile").off("click");
    $("#pass").off("click");

    if (beurt && !Afgelopen && ZetMogelijk) {
      $(".Tile").click("click", doTurn);
    }

    if (beurt && !Afgelopen && !ZetMogelijk) {
      $("#pass").click("click", doPass);
    }

    if (Afgelopen) {
      console.log("AFGELOPEN");
      var url = window.location.href;
      var parts = url.split("/");
      url = parts[0] + "//" + parts[2] + "/" + parts[3];
      $(".Tile").off("click");

      if (Gewonnen) {
        alert("Gewonnen");
        configMap.win();
        window.location.href = url;
        return;
      } else {
        alert("Verloren");
        configMap.lose();
        window.location.href = url;
        return;
      }

      return;
    }

    if (beurt && !ZetMogelijk) {
      $("#pass").click("click", doPass);
    }
  };

  var updateGameState = function updateGameState(gameState) {
    updateBoard(gameState.bord);
    updateBeurt(gameState.bord, gameState.aanDeBeurt, gameState.zetMogelijk, gameState.afgelopen, gameState.gewonnen);
    Game.Stats.updateStatistics(gameState);
  };

  return {
    init: privateInit,
    updateBoard: updateBoard,
    updateGameState: updateGameState,
    doLeave: doLeave,
    doPass: doPass
  };
}(); // ---------------------------------Model-----------------------------


Game.Model = function () {
  var configMap = {
    apiUrl: undefined,
    gameToken: undefined,
    playerToken: undefined
  };

  var privateInit = function privateInit(url, gameToken, playerToken) {
    configMap.apiUrl = url;
    configMap.gameToken = gameToken;
    configMap.playerToken = playerToken;
  };

  var _getGameState = function _getGameState() {
    // const Promise = Game.Data.get(`${configMap.apiUrl}api/spel/GameToken/${configMap.gameToken}`);
    // let data = Promise.then(function(response){
    //     return response.json().then(x => {
    //         return x;
    //     });
    // });
    // return data;
    var body = {
      spelToken: configMap.gameToken,
      spelerToken: configMap.playerToken
    };
    var Promise = Game.Data.post("".concat(configMap.apiUrl, "api/spel/Gamestate"), body);
    var data = Promise.then(function (response) {
      return response.json().then(function (x) {
        return x;
      });
    });
    return data;
  };

  return {
    init: privateInit,
    getGameState: _getGameState
  };
}(); //--------------------------Stats-----------------------------


Game.Stats = function () {
  var configMap = {
    apiUrl: undefined,
    gameToken: undefined,
    playerToken: undefined,
    updateStaticsCall: undefined
  };
  var beurtAantal = 0;
  var currentBeurt = undefined;
  var aantalZwart = 2;
  var aantalWit = 2;
  var veroverdeFiches = {
    zwart: 0,
    wit: 0
  };
  var gameStateThroughHistory = [];
  var veroverdeFichesThroughHistory = [];

  var privateInit = function privateInit(url, gameToken, playerToken, updateStatisticsCall) {
    configMap.apiUrl = url;
    configMap.gameToken = gameToken;
    configMap.playerToken = playerToken;
    configMap.updateStaticsCall = updateStatisticsCall;
  };

  var _getStats = function _getStats() {
    console.log("STATS GOTTEN");
    return {
      gameStateThroughHistory: gameStateThroughHistory,
      veroverdeFichesThroughHistory: veroverdeFichesThroughHistory
    };
  };

  var isNumber = function isNumber(_char3) {
    if (typeof _char3 !== 'string') {
      return false;
    }

    if (_char3.trim() === '') {
      return false;
    }

    return !isNaN(_char3);
  };

  var _updateStatistics = function _updateStatistics(gamestate) {
    if (currentBeurt === undefined) {
      currentBeurt = gamestate.aanDeBeurt;
    } else if (currentBeurt === gamestate.aanDeBeurt) {
      return;
    }

    currentBeurt = gamestate.aanDeBeurt;
    beurtAantal++;
    var currentFiches = {
      zwart: 0,
      wit: 0
    };
    var Board = gamestate.bord;
    var x = 0;
    var y = 0;
    var BoardArr = Array.from(Board);
    BoardArr.forEach(function (_char4) {
      if (x == 8) {
        y++;
        x = 0;
      }

      if (_char4 == "W") {
        currentFiches.wit++;
        x++;
      }

      if (_char4 == "B") {
        currentFiches.zwart++;
        x++;
      }

      if (isNumber(_char4)) {
        var num = parseInt(_char4);

        for (var i = 0; i < num; i++) {
          x++;
        }
      }
    });
    var verslagenFiches = veroverdeFiches;
    var deltaWit = currentFiches.wit - aantalWit;
    var deltaZwart = currentFiches.zwart - aantalZwart;
    aantalWit = currentFiches.wit;
    aantalZwart = currentFiches.zwart;

    if (deltaWit < 0) {
      verslagenFiches.zwart -= deltaWit;
    }

    if (deltaZwart < 0) {
      verslagenFiches.wit -= deltaZwart;
    }

    veroverdeFiches = verslagenFiches;
    var aantalOnveroverd = 64 - aantalWit - aantalZwart;
    veroverdeFichesThroughHistory[beurtAantal] = verslagenFiches;
    gameStateThroughHistory[beurtAantal] = {
      zwart: aantalZwart,
      wit: aantalWit,
      onveroverd: aantalOnveroverd
    };
    configMap.updateStaticsCall(_getStats());
  };

  return {
    init: privateInit,
    updateStatistics: _updateStatistics
  };
}();