class ServiceClient {
    constructor(apiKey) {
        this.apiKey = apiKey;
        this.startFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    }

    submitMoveToAPI = async function (gameId, fen) {
        const response = await fetch('/webfish/move', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*.*',
                'User-Agent': 'WebFish ServiceClient'
            },
            body: JSON.stringify({ GameId: gameId, FEN: fen })
        })
            .then(res => res.json());

        return response;
    }

    submitMove = async function (fen) {
        let gameId = $('#gameId').val();
        return await this.submitMoveToAPI(gameId, fen);
    }

    checkMoveFromAPI = async function(gameId, moveId) {
        const response = await fetch('/webfish/move?gameId=' + gameId + '&moveId=' + moveId, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': '*.*',
                'User-Agent': 'WebFish ServiceClient'
            },
        })
            .then(res => res.json());

        return response;
    }

    checkMove = async function (gameId, moveId) {
        const response = await this.checkMoveFromAPI(gameId, moveId);
        return response;
    }
}