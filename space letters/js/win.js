
let winState = {
    preload: preloadWin,
    create: createWin,
    update: updateWin
};

function preloadWin(){
    game.load.image('background', 'assets/imgs/espacio1.jpg');
    game.load.image('backtomenu', 'assets/imgs/BACK_TO_MENU.png');
    game.load.image('backtomenu2', 'assets/imgs/BACK_TO_MENU2.png');

    game.load.audio('winmusic', 'assets/snds/winsound.wav');
}

function createWin(){

    createParallaxWin();
    createSoundsWin();
    
}
function createParallaxWin(){
    let w= game.world.width;
    let h=game.world.height;
    background=game.add.tileSprite(0,0,w,h,'background');

    btnBacktomenuNar = game.add.button(120, 500, 'backtomenu2', initPlayWin);
    btnBacktomenuNar.scale.setTo(0.10);
    btnBacktomenuNar.inputEnabled = true;
    btnBacktomenu = game.add.image(120, 500, 'backtomenu');
    btnBacktomenu.scale.setTo(0.10);
    btnBacktomenu.visible = false;
    
    let credits = 'YOU WON';
    let styleI = {font:'70px Kenxo', fill:'#dedede'};
    game.add.text(70, 60, credits, styleI);

    let cg = 'Accuracy: ';
    let styleII = {font:'40px Kenxo', fill:'#69e5ea'};
    game.add.text(100, 175, cg + Math.floor(100*rightsLetters/writtenLetters)+"%", styleII);
}

function createSoundsWin(){
    winMusic=game.add.sound("winmusic");
    winMusic.play();

}
function p(pointer) {
    console.log(pointer.event);
}

function initPlayWin(){
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
    endedRound=false;
    partA=false;
    partB=false;
    partC=false;
    big_Enemy=false;
    medium_Enemy1=false;
    big_Enemy1=false;
    bigSpaceShipDead=false;
    mediumSpaceSipDead=false;
    musicenabled=false;
    winMusic.stop();
    musicplayed=false;
    game.state.start('init');
}

function updateWin(){
    background.tilePosition.y+=0.3;
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