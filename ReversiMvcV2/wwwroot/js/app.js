"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }

// -------------------------------GAME----------------------------------
var Game = function (apiUrl) {
    var configMap = {
        apiUrl: undefined,
        gameToken: gameToken,
        playerToken: playerToken,
        interval: undefined
    };
    var stateMap = {
        gameState: undefined
    };
    var win = function win() {
        Game.Data.post("localhost:7184/Spelcontroller/win");
        clearInterval(configMap.interval);
    }

    var lose = function lose() {
        Game.Data.post("localhost:7184/Spelcontroller/lose");
        clearInterval(configMap.interval);
    }

    var draw = function draw() {
        Game.Data.post("localhost:7184/Spelcontroller/draw");
        clearInterval(configMap.interval);
    }

    var privateInit = function privateInit() {
        configMap.apiUrl = apiUrl;
        Game.Model.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken);
        Game.Data.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken);
        Game.Reversi.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken, win, draw, lose);
        Game.Stats.init(configMap.apiUrl, configMap.gameToken, configMap.playerToken);
        window.addEventListener("beforeunload", Game.Reversi.doLeave);
        var interval = setInterval(() => { _getCurrentGameState(interval) }, 20);
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
        });

        Game._getCurrentGameState();
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
        }).then(function () {
            _getCurrentGameState();
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
    };

    var updateGameState = function updateGameState(gameState) {
        updateBoard(gameState.bord);
        updateBeurt(gameState.bord, gameState.aanDeBeurt, gameState.zetMogelijk, gameState.afgelopen, gameState.gewonnen);
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
        playerToken: undefined
    };

    var privateInit = function privateInit(url, gameToken, playerToken) {
        configMap.apiUrl = url;
        configMap.gameToken = gameToken;
        configMap.playerToken = playerToken;
    };

    var _getStats = function _getStats() {
        var body = {
            spelToken: configMap.gameToken,
            spelerToken: configMap.playerToken
        };
        var Promise = Game.Data.post("".concat(configMap.apiUrl, "api/spel/Stats"), body);
        var data = Promise.then(function (response) {
            return response.json().then(function (x) {
                return x;
            });
        });
        return data;
    };

    return {
        init: privateInit,
        getStats: _getStats
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