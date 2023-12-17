

let gameOverState = {
    preload: preloadGameover,
    create: createGameover,
    update: updateGameover
};

function preloadGameover(){
    game.load.image('background', 'assets/imgs/espacio1.jpg');
    game.load.image('backtomenu', 'assets/imgs/BACK_TO_MENU.png');
    game.load.image('backtomenu2', 'assets/imgs/BACK_TO_MENU2.png');
    game.load.image('ufo', 'assets/imgs/ufo_lloron.png');
    game.load.audio('gameovermsc', 'assets/snds/gameover.wav');
}

function createGameover(){

    createParallaxGameOver();
    createSoundsGameOver();
    
}
function createParallaxGameOver(){
    let w= game.world.width;
    let h=game.world.height;
    fondo=game.add.tileSprite(0,0,w,h,'background');

    btnBacktomenuNar = game.add.button(120, 500, 'backtomenu2', initPlay);
    btnBacktomenuNar.scale.setTo(0.10);
    btnBacktomenuNar.inputEnabled = true;
    btnBacktomenu = game.add.image(120, 500, 'backtomenu');
    btnBacktomenu.scale.setTo(0.10);
    btnBacktomenu.visible = false;
    
    let credits = 'GAME OVER';
    let styleI = {font:'70px Kenxo', fill:'#dedede'};
    game.add.text(30, 60, credits, styleI);

    let cg = 'YOU LOST';
    let styleII = {font:'40px Kenxo', fill:'#69e5ea'};
    game.add.text(140, 175, cg, styleII);

    let posX = game.world.width/2.7;
    let posY = game.world.height/2.5;
    imgUfo = game.add.image(posX, posY, 'ufo');
    imgUfo.scale.setTo(0.05);

}

function createSoundsGameOver(){
    gameovermusic=game.add.audio('gameovermsc')
    gameovermusic.volume=music.volume;
    gameovermusic.play();
    
}
function p(pointer) {
    console.log(pointer.event);
}

function initPlay(){
    fixed_Word=false
    rightsLetterswaves=0;
    level=0;
    score=0;
    timer_activated=false;
    time=0;
    counterSpaceShips=0;
    counterSpaceShipsWaves=0;
    spaceShip_array=[];
    words_array=[];  
    uniqueletters=[];
    deadSpacecraft=false;
    console.log(level);
    endedRound=false;
    partA=false;
    partB=false;
    partC=false;
    medium_Enemy=false;
    big_Enemy=false;
    medium_Enemy1=false;
    big_Enemy1=false;
    bigSpaceShipDead=false;
    mediumSpaceSipDead=false;
    
    musicenabled=false;
    gameovermusic.stop();
    musicplayed=false;
    game.state.start('init');
}

function updateGameover(){
    fondo.tilePosition.y+=0.3;
    btnBTM();
}

function btnBTM() {
    if (btnBacktomenuNar.input.pointerOver())
    {
        btnBacktomenu.visible = false;
    }
    else
    {
        btnBacktomenu.visible = true;
    }
    
}