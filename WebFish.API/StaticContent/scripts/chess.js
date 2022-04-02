var board, moveList, statusMessage, app;
const delay = ms => new Promise(res => setTimeout(res, ms));

window.onload = function () {

    board = new Board();
    moveList = new MoveList();
    statusMessage = "Welcome to WebFish. Good luck, it is your turn...";
    board.startPosition();
    app = new Vue({
        el: '#gameContainer',
        data: {
            board: board,
            moveList: moveList,
            statusMessage: statusMessage
        }
    });

    $(document).on('click', '.piece:not(.black)', (e) => {
        let x = e.target.getAttribute('data-x');
        let y = e.target.getAttribute('data-y');
        var result = app.board.selectPiece(x, y);
        if (result) {
            let mask = board.generateMoveMask(x, y);
            if (mask) {
                board.applyMoveMask(mask);
            }
        }

        
    });

    

    $(document).on('click', '.boardCell', async (e) => {
        let x = e.target.getAttribute('data-x');
        let y = e.target.getAttribute('data-y');
        if (app.board.isPieceSelected() && this.board.squares[y][x].isEligibleMove()) {
            let selectedPiece = this.board.getSelectedPiece();
            if (selectedPiece != null) {
                this.board.movePiece(selectedPiece.X, selectedPiece.Y, x, y);
                this.moveList.appendMove(selectedPiece.pieceClassName(), selectedPiece.getLabel() + this.board.squares[y][x].getLabel());
                let serviceClient = new ServiceClient();
                const result = await serviceClient.submitMove(board.buildFENString());
                if (result != null && result.success === true) {

                    let complete = false;
                    let move = null;
                    while (!complete) {
                        const check = await serviceClient.checkMove($('#gameId').val(), result.moveId);
                        if (check.completed) {
                            complete = true;
                            move = check.move;
                        }
                        else {
                            await delay(500 + (1000 * check.wait));
                        }
                    }

                    if (move) {
                        let indexes = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
                        let startX, startY, destX, destY;
                        for (let i = 0; i < indexes.length; i++) {
                            if (move[0] == indexes[i]) {
                                startX = i;
                            }
                        }
                        startY = 8 - parseInt(move[1]);
                        for (let i = 0; i < indexes.length; i++) {
                            if (move[2] == indexes[i]) {
                                destX = i;
                            }
                        }
                        destY = 8 - parseInt(move[3]);

                        if (startX != null && startY != null && destX != null && destY != null) {
                            board.squares[destY][destX].id = board.squares[startY][startX].id;
                            board.squares[startY][startX].id = 0;
                        }
                        else {
                            console.log('bad move config');
                            console.log(move);
                            console.log('startX: ' + startX + ' startY: ' + startY + ' destX: ' + destX + ' destY: ' + destY);
                        }
                    }
                    else {
                        console.log('bad move');
                    }

                }
            }
        }
    });

}


class Board {
    constructor() {
        this.squares = [];
        for (let i = 0; i < 8; i++) {
            let row = [];
            for (let ii = 0; ii < 8; ii++) {
                row.push(new Square(ii,i, null));
            }
            this.squares.push(row);
        }
        this.isWhiteTurn = true;
        this.halfTurnCount = 0;
        this.fullTurnCount = 0;
        this.wcq = true;
        this.wck = true;
        this.bcq = true;
        this.bck = true;
    }

    startPosition() {
        this.squares[0][0].id = 8;
        this.squares[0][1].id = 9;
        this.squares[0][2].id = 10;
        this.squares[0][3].id = 11;
        this.squares[0][4].id = 12;
        this.squares[0][5].id = 10;
        this.squares[0][6].id = 9;
        this.squares[0][7].id = 8;

        for (let i = 0; i < this.squares[1].length; i++) {
            this.squares[1][i].id = 7;
        }

        for (let i = 0; i < this.squares[6].length; i++) {
            this.squares[6][i].id = 1;
        }

        this.squares[7][0].id = 2;
        this.squares[7][1].id = 3;
        this.squares[7][2].id = 4;
        this.squares[7][3].id = 5;
        this.squares[7][4].id = 6;
        this.squares[7][5].id = 4;
        this.squares[7][6].id = 3;
        this.squares[7][7].id = 2;
    }

    selectPiece(x, y) {
        if (x < 8 && y < 8 && x > -1 && y > -1) {
            if (!this.squares[y][x].isSelected()) {
                let square = this.squares[y][x];
                if (square == null || square.id == null) return false;
                if ((this.isWhiteTurn && square.id > 6) || (!this.isWhiteTurn && square.id < 7)) return false;
                this.clearSelection();
                this.squares[y][x].selected = true;
                return true;
            }
        }

        return false;
    }

    clearSelection() {
        for (let i = 0; i < 8; i++) {
            for (let ii = 0; ii < 8; ii++) {
                if (this.squares[i][ii].selected) {
                    this.squares[i][ii].selected = false;
                }
                if (this.squares[i][ii].eligibleMove) {
                    this.squares[i][ii].eligibleMove = false;
                }
            }
        }
    }

    isPieceSelected() {
        return (this.getSelectedPieceId() != null);
    }

    getSelectedPieceId() {
        let result = this.getSelectedPiece();
        return (result != null ? result.id : null);
    }

    getSelectedPiece() {
        for (let i = 0; i < 8; i++) {
            for (let ii = 0; ii < 8; ii++) {
                if (this.squares[i][ii].selected) {
                    return this.squares[i][ii];
                }
            }
        }
        return null;
    }

    movePiece(startX, startY, destX, destY) {
        if (!this.isPieceSelected) {
            return false;
        }

        this.squares[destY][destX].id = this.getSelectedPieceId();
        this.squares[startY][startX].id = null;
        this.clearSelection();

    }

    applyMoveMask(mask) {
        for (let i = 0; i < 8; i++) {
            for (let ii = 0; ii < 8; ii++) {
                if (mask[i][ii] == 1) {
                    this.squares[i][ii].eligibleMove = true;
                }
            }
        }
    }

    generateMoveMask(startX, startY) {
        startX = parseInt(startX);
        startY = parseInt(startY);
        let pieceId = this.squares[startY][startX].id;
        let mask = this.zeroMask();

        if (pieceId == 1) { //Pawn
            this.addPawnMoveMask(mask, startX, startY);
        }
        else if (pieceId == 2) { //Rook
            this.addCardinalNorthToMask(mask, startX, startY);
            this.addCardinalSouthToMask(mask, startX, startY);
            this.addCardinalEastToMask(mask, startX, startY);
            this.addCardinalWestToMask(mask, startX, startY);
        }
        else if (pieceId == 3) { //Knight

        }
        else if (pieceId == 4) { //Bishop
            this.addCardinalNorthEastToMask(mask, startX, startY);
            this.addCardinalSouthEastToMask(mask, startX, startY);
            this.addCardinalSouthWestToMask(mask, startX, startY);
            this.addCardinalNorthWestToMask(mask, startX, startY);
        }
        else if (pieceId == 5) { //Queen
            this.addCardinalNorthToMask(mask, startX, startY);
            this.addCardinalSouthToMask(mask, startX, startY);
            this.addCardinalEastToMask(mask, startX, startY);
            this.addCardinalWestToMask(mask, startX, startY);
            this.addCardinalNorthEastToMask(mask, startX, startY);
            this.addCardinalSouthEastToMask(mask, startX, startY);
            this.addCardinalSouthWestToMask(mask, startX, startY);
            this.addCardinalNorthWestToMask(mask, startX, startY);
        }
        else if (pieceId == 6) { //King

        }

        //this.addCardinalNorthToMask(mask, startX, startY);
        //this.addCardinalSouthToMask(mask, startX, startY);
        //this.addCardinalEastToMask(mask, startX, startY);
        //this.addCardinalWestToMask(mask, startX, startY);
        return mask;
    }

    addCardinalNorthToMask(mask, startX, startY) {
        if (startY > 0) {
            for (let i = startY - 1; i >= 0; i--) {
                let cell = this.squares[i][startX];

                if (cell.isOccupied()) {
                    if (cell.isOccupiedByBlack()) {
                        mask[i][startX] = 1;
                    }
                    return;
                }
                else {
                    mask[i][startX] = 1;
                }
            }
        }
    }

    addCardinalSouthToMask(mask, startX, startY) {
        if (startY < 7) {
            for (let i = startY + 1; i <= 7; i++) {
                let cell = this.squares[i][startX];

                if (cell.isOccupied()) {
                    if (cell.isOccupiedByBlack()) {
                        mask[i][startX] = 1;
                    }
                    return;
                }
                else {
                    mask[i][startX] = 1;
                }
            }
        }
    }

    addCardinalWestToMask(mask, startX, startY) {
        if (startX > 0) {
            for (let i = startX - 1; i >= 0; i--) {
                let cell = this.squares[startY][i];

                if (cell.isOccupied()) {
                    if (cell.isOccupiedByBlack()) {
                        mask[startY][i] = 1;
                    }
                    return;
                }
                else {
                    mask[startY][i] = 1;
                }
            }
        }
    }

    addCardinalEastToMask(mask, startX, startY) {
        if (startX < 7) {
            for (let i = startX + 1; i <= 7; i++) {
                let cell = this.squares[startY][i];

                if (cell.isOccupied()) {
                    if (cell.isOccupiedByBlack()) {
                        mask[startY][i] = 1;
                    }
                    return;
                }
                else {
                    mask[startY][i] = 1;
                }
            }
        }
    }

    addCardinalNorthEastToMask(mask, startX, startY) {
        let x = startX + 1;
        let y = startY - 1;
        while (this.continueMove(mask, x, y)) {
            x += 1;
            y -= 1;
        }
    }

    addCardinalSouthEastToMask(mask, startX, startY) {
        let x = startX + 1;
        let y = startY + 1;
        while (this.continueMove(mask, x, y)) {
            x += 1;
            y += 1;
        }
    }

    addCardinalSouthWestToMask(mask, startX, startY) {
        let x = startX - 1;
        let y = startY + 1;
        while (this.continueMove(mask, x, y)) {
            x -= 1;
            y += 1;
        }
    }

    addCardinalNorthWestToMask(mask, startX, startY) {
        let x = startX - 1;
        let y = startY - 1;
        while (this.continueMove(mask, x, y)) {
            x -= 1;
            y -= 1;
        }
    }

    continueMove(mask, destX, destY) {
        if (destX >= 0 && destX <= 7 && destY >= 0 && destY <= 7) {
            let cell = this.squares[destY][destX];
            if (cell.isOccupied()) {
                if (cell.isOccupiedByBlack()) {
                    mask[destY][destX] = 1;
                }
                return false;
            }
            mask[destY][destX] = 1;
            return true;
        }
        return false;
    }

    addPawnMoveMask(mask, startX, startY) {
        if (startY > 0) {
            let cell = this.squares[startY - 1][startX];
            if (cell.isOccupied()) {
                if (cell.isOccupiedByBlack()) {
                    mask[startY - 1][startX] = 1;
                }
                return;
            }
            else {
                mask[startY - 1][startX] = 1;
                if (startY == 6) {
                    cell = this.squares[startY - 2][startX];
                    if (!cell.isOccupied() || cell.isOccupiedByBlack()){
                        mask[startY - 2][startX] = 1;
                    }
                }
            }
        }
    }

    zeroMask = function () {
        let retVal = []
        for (let i = 0; i < 8; i++) {
            let row = [];
            for (let ii = 0; ii < 8; ii++) {
                row.push(0);
            }
            retVal.push(row);
        }
        return retVal;
    }

    buildFENString() {
        let chars = ["", "P", "R", "N", "B", "Q", "K", "p", "r", "n", "b", "q", "k"];
        let fen = "";
        for (let ii = 0; ii < 8; ii++) {
            let zeroCounter = 0;
            for (let i = 0; i < 8; i++) {
                let squareId = this.squares[ii][i].id;
                let next = null;
                if (squareId != null && squareId != 0) {
                    next = chars[squareId];
                }
                else {
                    zeroCounter++;
                }

                if (next != null) {
                    if (zeroCounter > 0) {
                        next = zeroCounter + next;
                        zeroCounter = 0;
                    }
                }
                else {
                    if (i == 7) {
                        next = zeroCounter;
                    }
                }

                if (next != null) {
                    fen += next;
                    if (i == 7) {
                        fen += "/";
                    }
                }
            }
        }

        return fen;
    }
}

class Square {
    constructor(x, y, id) {
        this.X = x;
        this.Y = y;
        this.id = id;
        this.selected = false;
        this.eligibleMove = false;
    }

    
    isDark = function () {
        return this.colorMask[this.Y][this.X] == 1;
    }

    isOccupied = function () {
        return this.id != null;
    }

    isSelected = function () {
        return this.selected;
    }

    isOccupiedByBlack = function () {
        if (this.id == null) return false;
        else {
            return this.id > 6;
        }
    }

    isEligibleMove = function() {
        return this.eligibleMove;
    }

    getLabel = function () {
        let retVal = "";
        switch (this.X) {
            case (0):
                retVal = "a";
                break;
            case (1):
                retVal = "b";
                break;
            case (2):
                retVal = "c";
                break;
            case (3):
                retVal = "d";
                break;
            case (4):
                retVal = "e";
                break;
            case (5):
                retVal = "f";
                break;
            case (6):
                retVal = "g";
                break;
            case (7):
                retVal = "h";
                break;
        }
        return retVal + (8 - this.Y);
    }

    pieceClassName = function () {
        let retVal = "fas fa-chess-";
        if (this.id == 1 || this.id == 7) retVal += "pawn";
        else if (this.id == 2 || this.id == 8) retVal += "rook";
        else if (this.id == 3 || this.id == 9) retVal += "knight";
        else if (this.id == 4 || this.id == 10) retVal += "bishop";
        else if (this.id == 5 || this.id == 11) retVal += "queen";
        else if (this.id == 6 || this.id == 12) retVal += "king";
        return retVal;
    }

    colorMask = [
        [1, 0, 1, 0, 1, 0, 1, 0],
        [0, 1, 0, 1, 0, 1, 0, 1],
        [1, 0, 1, 0, 1, 0, 1, 0],
        [0, 1, 0, 1, 0, 1, 0, 1],
        [1, 0, 1, 0, 1, 0, 1, 0],
        [0, 1, 0, 1, 0, 1, 0, 1],
        [1, 0, 1, 0, 1, 0, 1, 0],
        [0, 1, 0, 1, 0, 1, 0, 1]];

}

class MoveList {
    constructor() {
        this.Moves = [new Move(null, "START")];
    }

    appendMove(className, fenString) {
        this.Moves.push(new Move(className, fenString));
    }
}

class Move {
    constructor(pieceClassName, fen) {
        this.FEN = fen;
        this.date = new Date()
        this.pieceClassName = pieceClassName;
    }
}

