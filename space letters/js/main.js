const GAME_STAGE_WIDTH = 480;
const GAME_STAGE_HEIGHT = 720;

let game = new Phaser.Game(GAME_STAGE_WIDTH, GAME_STAGE_HEIGHT, Phaser.CANVAS, 'gamestage');

// Entry point
window.onload = startGame;

function startGame() {
    game.state.add('init', initState);
    game.state.add('play', playState);
    game.state.add('preplay', preplayState);
    game.state.add('gameOver', gameOverState);
    game.state.add('win', winState);
    game.state.add('info', infoState);
    game.state.add('setting', settingsState);
    
    game.state.start('init');
}
