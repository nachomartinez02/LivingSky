

let btnStart;
let imgUfo;
let musicplayed=false;
let music;
let button;
let winMusic;
let gameovermusic;
let initState = {
    preload: preloadInit,
    create: createInit,
    update: updateInit
};

function preloadInit () {
    game.load.image('newgame', 'assets/imgs/NEW_GAME.png');
    game.load.image('newgame2', 'assets/imgs/NEW_GAME2.png');
    game.load.image('settings', 'assets/imgs/SETTINGS.png');
    game.load.image('settings2', 'assets/imgs/SETTINGS2.png');
    game.load.image('ufo', 'assets/imgs/ufo.png');
    game.load.image('background', 'assets/imgs/espacio1.jpg');
    game.load.image('logo', 'assets/imgs/logo.png');
    game.load.image('info', 'assets/imgs/informacion.png');
    game.load.image('info2', 'assets/imgs/informacion2.png');

    game.load.audio('buttonsnd', 'assets/snds/updatebutton.wav');
    game.load.audio('music', 'assets/snds/musica.wav');
}

function createInit() {
    createSoundsInit();
    createParallaxInit();
    
}

function createSoundsInit(){
    if (musicplayed==false){
        music=game.add.audio('music');
        music.play();
        musicplayed=true;
    }
    button=game.add.audio('buttonsnd');
    button.volume=music.volume;

}

function createParallaxInit(){
    let w= game.world.width;
    let h=game.world.height;
    let posX = game.world.width/2;
    let posY = game.world.height/1.7;

    fondo=game.add.tileSprite(0,0,w,h,'background');
    logo=game.add.image(10,50,'logo');
    logo.scale.setTo(0.7,0.7);

    btnStartNar = game.add.button(120, posY, 'newgame2', startPreplay);
    btnStartNar.scale.setTo(0.10);
    btnStartNar.inputEnabled = true;    
    btnStart = game.add.image(120, posY, 'newgame');
    btnStart.scale.setTo(0.10);
    btnStart.visible = false;

    btnSettingsNar = game.add.button(120, posY+120, 'settings2', settingsPlay);
    btnSettingsNar.scale.setTo(0.10);
    btnSettingsNar.inputEnabled = true;    
    btnSettings = game.add.image(120, posY+120, 'settings');
    btnSettings.scale.setTo(0.10);
    btnSettings.visible = false;

    btninfoNar = game.add.button(410, 650, 'info2', infoPlay);
    btninfoNar.scale.setTo(0.01);
    btninfoNar.inputEnabled = true;    
    btninfo = game.add.image(410, 650, 'info');
    btninfo.scale.setTo(0.01);
    btninfo.visible = false;

    posY = game.world.height/2.5;
    imgUfo = game.add.image(posX, posY, 'ufo');
    imgUfo.anchor.setTo(0.5, 0.5);
    imgUfo.scale.setTo(0.3);

}

function p(pointer) {
    console.log(pointer.event);
    
}

function startPreplay() {
    game.state.start('preplay');
}

function infoPlay() {
    game.state.start('info');
}

function settingsPlay() {
    game.state.start('setting');
}

function updateInit(){
    fondo.tilePosition.y+=0.3;
    btnSta();
    btnSet();
    btnInf();
    
}

function btnSta() {
    if (btnStartNar.input.pointerOver())
    {
        btnStart.visible = false;
    }
    else
    {
        btnStart.visible = true;
    }
    button.play();
}

function btnSet() {
    if (btnSettingsNar.input.pointerOver())
    {
        btnSettings.visible = false;
    }
    else
    {
        btnSettings.visible = true;
    }
    button.play();
}

function btnInf() {
    if (btninfoNar.input.pointerOver())
    {
        btninfo.visible = false;
    }
    else
    {
        btninfo.visible = true;
    }
    button.play();
}

