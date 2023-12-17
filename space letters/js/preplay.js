let preplayState = {
    preload: preloadPreplay,
    create: createPreplay,
    update: updatePreplay
};

function preloadPreplay() {
    game.load.image('background', 'assets/imgs/espacio1.jpg');
    game.load.image('partA', 'assets/imgs/PARTA.png');
    game.load.image('partB', 'assets/imgs/PARTB.png');
    game.load.image('partC', 'assets/imgs/PARTC.png');
    game.load.image('partA2', 'assets/imgs/PARTA2.png');
    game.load.image('partB2', 'assets/imgs/PARTB2.png');
    game.load.image('partC2', 'assets/imgs/PARTC2.png');
    game.load.image('backtomenu', 'assets/imgs/BACK_TO_MENU.png');
    game.load.image('backtomenu2', 'assets/imgs/BACK_TO_MENU2.png');
}

let partA=false;
let partB=false;
let partC=false;
function createPreplay() {
    createParallaxPreplay();
}
function createParallaxPreplay(){
    let w = game.world.width;
    let h = game.world.height;
    fondo=game.add.tileSprite(0,0,w,h,'background');

    btnPartANar = game.add.button(w/2, h/10, 'partA2', startPlayA);
    btnPartANar.anchor.setTo(0.5,0,5);
    btnPartANar.scale.setTo(0.5);
    btnPartANar.inputEnabled = true;
    btnPartA = game.add.image(w/2, h/10, 'partA');
    btnPartA.anchor.setTo(0.5,0,5);
    btnPartA.scale.setTo(0.5);
    btnPartA.visible = false;

    btnPartBNar = game.add.button(w/2, h/3.5, 'partB2', startPlayB);
    btnPartBNar.anchor.setTo(0.5,0,5);
    btnPartBNar.scale.setTo(0.5);
    btnPartBNar.inputEnabled = true;
    btnPartB = game.add.image(w/2, h/3.5, 'partB');
    btnPartB.anchor.setTo(0.5,0,5);
    btnPartB.scale.setTo(0.5);
    btnPartB.visible = false;

    btnPartCNar = game.add.button(w/2, h/2.1, 'partC2', startPlayC);
    btnPartCNar.anchor.setTo(0.5,0,5);
    btnPartCNar.scale.setTo(0.5);
    btnPartCNar.inputEnabled = true;
    btnPartC = game.add.image(w/2, h/2.1, 'partC');
    btnPartC.anchor.setTo(0.5,0,5);
    btnPartC.scale.setTo(0.5);
    btnPartC.visible = false;

    btnBacktomenuNar = game.add.button(120, 500, 'backtomenu2', initPlayPrePlay);
    btnBacktomenuNar.scale.setTo(0.10);
    btnBacktomenuNar.inputEnabled = true;
    btnBacktomenu = game.add.image(120, 500, 'backtomenu');
    btnBacktomenu.scale.setTo(0.10);
    btnBacktomenu.visible = false;
}

function p(pointer) {
    console.log(pointer.event);
}

function startPlayA() {
    partA=true;
    button.play();
    game.state.start('play');
}

function startPlayB() {
    partB=true;
    button.play();
    game.state.start('play');
}

function startPlayC() {
    partC=true;
    button.play();
    game.state.start('play');
}
function initPlayPrePlay(){
    
    game.state.start('init');
}


function updatePreplay(){
    fondo.tilePosition.y+=0.3;
    btnPA();
    btnPB();
    btnPC();
    btnBTM();
}

function btnPA() {
    if (btnPartANar.input.pointerOver())
    {
        btnPartA.visible = false;
    }
    else
    {
        btnPartA.visible = true;
    }
    
}

function btnPB() {
    if (btnPartBNar.input.pointerOver())
    {
        btnPartB.visible = false;
    }
    else
    {
        btnPartB.visible = true;
    }
    
}

function btnPC() {
    if (btnPartCNar.input.pointerOver())
    {
        btnPartC.visible = false;
    }
    else
    {
        btnPartC.visible = true;
    }
    
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