let settingsState = {
    preload: preloadSettings,
    create: createSettings,
    update: updateSettings,
};

function preloadSettings(){
    game.load.image('background', 'assets/imgs/espacio1.jpg');
    game.load.image('backtomenu', 'assets/imgs/BACK_TO_MENU.png');
    game.load.image('backtomenu2', 'assets/imgs/BACK_TO_MENU2.png');
    game.load.image('menos', 'assets/imgs/MENOS.png');
    game.load.image('menos2', 'assets/imgs/MENOS2.png');
    game.load.image('mas', 'assets/imgs/MAS.png');
    game.load.image('mas2', 'assets/imgs/MAS2.png');
}
let styleII = {font:'30px Kenxo', fill:'#69e5ea'};
let volumen2;

function createSettings(){
    createParallaxSettings();
}

function createParallaxSettings(){
    let w= game.world.width;
    let h=game.world.height;
    fondo=game.add.tileSprite(0,0,w,h,'background');

    btnBacktomenuNar = game.add.button(120, 500, 'backtomenu2', initPlaySettings);
    btnBacktomenuNar.scale.setTo(0.10);
    btnBacktomenuNar.inputEnabled = true;
    btnBacktomenu = game.add.image(120, 500, 'backtomenu');
    btnBacktomenu.scale.setTo(0.10);
    btnBacktomenu.visible = false;

    let settings = 'SETTINGS';
    let styleI = {font:'50px Kenxo', fill:'#dedede'};
    game.add.text(120, 90, settings, styleI);

    let volumen = 'VOLUME';
    
    game.add.text(177, 250, volumen, styleII);

    btnMenosNar = game.add.button(160, 300, 'menos2', changeVolume);
    btnMenosNar.scale.setTo(0.05);
    btnMenosNar.inputEnabled = true;
    btnMenos = game.add.image(160, 300, 'menos');
    btnMenos.scale.setTo(0.05);
    btnMenos.visible = false;

    btnMasNar = game.add.button(300, 300, 'mas2', changeVolume);
    btnMasNar.scale.setTo(0.05);
    btnMasNar.inputEnabled = true;
    btnMas = game.add.image(300, 300, 'mas');
    btnMas.scale.setTo(0.05);
    btnMas.visible = false;

    volume = music.volume * 100 + '%';
    
    volumen2=game.add.text(205, 320, volume, styleII);
}

function p(pointer) {
    console.log(pointer.event);
}

function changeVolume(pointer) {

    if (btnMasNar.input.pointerOver())
    {
        if (music.volume < 1)
        {
            music.volume += 0.1;
            volume = music.volume * 100 + '%';
            button.volume=music.volume;
            button.play();
            
        }
    }
    else if (btnMenosNar.input.pointerOver())
    {
        if (music.volume > 0.1)
        {
            music.volume -= 0.1;
            volume = music.volume * 100 + '%';
            button.volume=music.volume;
            button.play();
        }
    }
    
    volume = Math.floor(music.volume * 100) + 100+ '%';
    
}

function initPlaySettings(){
    
    game.state.start('init');
}

function updateSettings(){
    fondo.tilePosition.y+=0.3;
    btnBMasMus();
    btnBMenMus();
    btnBTM();
    volume = music.volume * 100 + '%';
    volumen2.setText(Math.floor(music.volume * 100) + '%');
    
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

function btnBMenMus() {
    if (btnMenosNar.input.pointerOver())
    {
        btnMenos.visible = false;
        
    }

    else
    {
        btnMenos.visible = true;
        
    }
    
}

function btnBMasMus() {
    if (btnMasNar.input.pointerOver())
    {
        btnMas.visible = false;
    }
    else
    {
        btnMas.visible = true;
    }
   
}