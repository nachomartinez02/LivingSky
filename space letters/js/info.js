let infoState = {
    preload: preloadInfo,
    create: createInfo,
    update: updateInfo
};

function preloadInfo(){
    game.load.image('background', 'assets/imgs/espacio1.jpg');
    game.load.image('backtomenu', 'assets/imgs/BACK_TO_MENU.png');
    game.load.image('backtomenu2', 'assets/imgs/BACK_TO_MENU2.png');
}

function createInfo(){
    createParallaxInfo();
}

function createParallaxInfo(){
    let w= game.world.width;
    let h=game.world.height;
    fondo=game.add.tileSprite(0,0,w,h,'background');

    btnBacktomenuNar = game.add.button(120, 500, 'backtomenu2', initPlayInfo);
    btnBacktomenuNar.scale.setTo(0.10);
    btnBacktomenuNar.inputEnabled = true;
    btnBacktomenu = game.add.image(120, 500, 'backtomenu');
    btnBacktomenu.scale.setTo(0.10);
    btnBacktomenu.visible = false;
    
    let credits = 'CREDITS';
    let styleI = {font:'50px Kenxo', fill:'#dedede'};
    game.add.text(135, 60, credits, styleI);

    let cg = 'CONCEPT, GRAPHICS,';
    let styleII = {font:'30px Kenxo', fill:'#69e5ea'};
    game.add.text(90, 175, cg, styleII);

    let pu = 'PROGRAMMING, UX,';
    game.add.text(95, 200, pu, styleII);

    let adm = 'ADITIONAL DESING & MUSIC';
    game.add.text(30, 225, adm, styleII);

    let pha = 'Pedro Hernández Alcón';
    let styleIII = {font:'25px Kenxo', fill:'#dedede'};
    game.add.text(120, 300, pha, styleIII);    

    let imr = 'Ignacio Martínez Ruíz';
    game.add.text(125, 330, imr, styleIII);

    let dvs = 'David Ventas Sánchez';
    game.add.text(125, 360, dvs, styleIII);
}

function p(pointer) {
    console.log(pointer.event);
}

function initPlayInfo(){
    
    game.state.start('init');
}

function updateInfo(){
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