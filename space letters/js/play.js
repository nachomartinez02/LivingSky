let playState = {
    preload: preloadPlay,
    create: createPlay,
    update: updatePlay
};
const TIMER=Phaser.Timer.SECOND;
const CRAFT_VELOCITY=150;

let medium_Enemy1=false;
let big_Enemy1=false;
let medium_Enemy=false;
let big_Enemy=false;
let endedRound=false;
let timer_activated=false;
let deadSpacecraft=false;
let fixed_Word=false; 
let bigSpaceShipDead=false;
let mediumSpaceshipdead=false;

let spaceShip_array=[];
let words_array=[];  
let uniqueletters=[];

let CounterMediumSpaceship=0;
let CounterBigSpaceship=0;
let level=0;
let counter=0;
let score=0;
let counterSpaceShips=0;
let counterSpaceShipsWaves=0;
let rightsLetters=0;
let rightsLetterswaves=0;
let writtenLetters=0;
let time=0;
let sum=0;

let objetive="";

let timeText;
let scoreText;

let nave;
let Spaceships;
let spaceCraft;
let background;
let blasts;
let Spaceshiplimit;
let SpaceshipLimit2;
let SpaceshipsVelocity;
let fireButton;
let w;
let h;
let SpaceshipWord;
let musicInGame;
let musicenabled=false;

let DefaultFont = {fontSize: '18px', font: 'Courier new', fill: '#FFFFFF'};
let DefaultFontEndRound = {fontSize: '50px', font: 'Courier new',fill: '#FFFFFF'};
let DefaultFontEndRound2 = {fontSize: '30px', font: 'Courier new',fill: '#FFFFFF'};

let CounterSpaceships;
let eventSpaceships;
let eventSpaceshipsMediums;
let eventSpaceshipsBigs;

function preloadPlay() {
    game.load.image('background', 'assets/imgs/espacio1.jpg');
    game.load.image('background2', 'assets/imgs/espacio2.jpg');
    game.load.image('mesh', 'assets/imgs/malla1.png');
    game.load.image('ufo', 'assets/imgs/ufo.png');
    game.load.image('alienNormal', 'assets/imgs/alien.png');
    game.load.image('alienmedium', 'assets/imgs/alienbig.png');
    game.load.image('alienbig', 'assets/imgs/alienbigbig.png');
    game.load.image('alienmini', 'assets/imgs/alienmini.png');
    game.load.image('laser', 'assets/imgs/laser.png');
    game.load.image('black', 'assets/imgs/negro.jpg');

    game.load.spritesheet('blast', 'assets/imgs/blast2.png', 127, 127);
    game.load.spritesheet('selectionated', 'assets/imgs/laseranimacion.png', 189, 189);
    
    
    game.load.text('partA','assets/parts/partA.json', true);
    game.load.text('partB','assets/parts/partB.json', true);
    game.load.text('partC','assets/parts/partC.json', true);
    
    game.load.audio('partAMusic', 'assets/snds/partamusica.wav');
    game.load.audio('partBMusic', 'assets/snds/partbmusica.wav');
    game.load.audio('partCMusic', 'assets/snds/partcmusica.wav');
    game.load.audio('laser', 'assets/snds/laser.wav');
    game.load.audio('sndblast', 'assets/snds/blast2.wav');
    game.load.audio('blip', 'assets/snds/blip.wav');
    game.load.audio('finalround2', 'assets/snds/finalronda2.wav');
    game.load.audio('finalround', 'assets/snds/finalronda.wav');
    game.load.audio('error', 'assets/snds/error.wav');
}

function createPlay() {
    createConfig();
    createParallax();
    createSpaceCraft();
    createSounds();
    createSpaceships();
    createBlasts(30);
    createKeyControls();
    createHUD();
   
}
function createConfig(){
    w= game.world.width;
    h=game.world.height;
    levelConfig = JSON.parse(game.cache.getText('partA'));
    levelConfigB = JSON.parse(game.cache.getText('partB'));
    levelConfigC = JSON.parse(game.cache.getText('partC'));
    if (partA==true){//PARTE A 
        background=game.add.tileSprite(0,0,w,h,'background');
        Spaceshiplimit=(levelConfig.waves[level].spaceships)
        
    }
    else if (partB==true) {//PARTE B
        background=game.add.tileSprite(0,0,w,h,'background2');
        Spaceshiplimit=(levelConfigB.waves[level].spaceships);
        
        
    }
    if (partC==true){//PARTE C 
        background=game.add.tileSprite(0,0,w,h,'background2');
        Spaceshiplimit=(levelConfigC.waves[level].spaceships)
        
    }
    SpaceshipLimit2=Spaceshiplimit;

}
function createParallax(){ //Creates background
    w= game.world.width;
    h=game.world.height;
    mesh=game.add.tileSprite(0,0,w,h,'mesh');
}

function createSpaceCraft(){//Creates our character

    let x = game.width/2-25;
    let y = game.height-75;


    spaceCraft = game.add.sprite(x, y, 'ufo');
    spaceCraft.alpha=1;
    
    //Fading in
    tween = game.add.tween(spaceCraft).to({
                
        alpha: 1
        
    }, 750, Phaser.Easing.Linear.None);
    tween.start(); 

    //Fading out
    backgroundblack=game.add.image(0,0,'black')
    tween = game.add.tween(backgroundblack).to({
                
        alpha: 0
        
    }, 650, Phaser.Easing.Linear.None);
    tween.start(); 

    spaceCraft.anchor.setTo(0.1,0.1);
    spaceCraft.scale.setTo(0.15,0.15);

    game.physics.arcade.enable(spaceCraft);
    spaceCraft   .body.collideWorldBounds = true;
    
}

function createSounds(){
    
    soundLaser=game.add.audio('laser')
    soundBlast = game.add.audio('sndblast');
    blip=game.add.audio('blip');
    finalround=game.add.audio('finalround');
    finalround2=game.add.audio('finalround2');
    error=game.add.audio('error');

    soundLaser.volume=music.volume;
    soundBlast.volume=music.volume;
    blip.volume=music.volume;
    finalround.volume=music.volume;
    finalround2.volume=music.volume;
    error.volume=music.volume;

    music.stop();
    //Music for each part
    if (partA==true && musicenabled==false){
        musicInGame=game.add.audio('partAMusic');
        musicInGame.play();
        musicenabled=true;
    }
    else if (partB==true && musicenabled==false) {
        musicInGame=game.add.audio('partBMusic');
        musicInGame.play();
        musicenabled=true;
    }
    if (partC==true && musicenabled==false){
        musicInGame=game.add.audio('partCMusic');
        musicInGame.play();
        musicenabled=true;
    }
    musicInGame.volume=music.volume;
}

function createSpaceships(){//Creates enemies
    CounterSpaceships=0
    Spaceships = game.add.group();
    Spaceships.enableBody=true;

    Spaceships.callAll('anchor.SetTo','anchor',0.5,1.0);
    
    Spaceships.setAll('checkWorldBounds',true);
    
    //Velocity for each part
    if (partA==true){
        SpaceshipsVelocity= levelConfig.waves[level].velocity;
    }
    if (partB==true){
        SpaceshipsVelocity= levelConfigB.waves[level].velocity;
    }
    if (partC==true){
        SpaceshipsVelocity= levelConfigC.waves[level].velocity;
    }
    if (timer_activated==false){
        game.time.events.loop(TIMER,timer,this);
    }
    eventSpaceships=game.time.events.loop(TIMER,activateSpaceships,this);
}

function createBlasts(number) {//Creates explosions group
    blasts = game.add.group();
    blasts.createMultiple(number, 'blast');
    blasts.forEach(setupBlast, this);
}

function createKeyControls(){ 
    cursors=game.input.keyboard.createCursorKeys();
    fireButton=game.input.keyboard.addKey(Phaser.Keyboard.SPACEBAR);
}

function createHUD() {//In game information
    let scoreX = 5;
    let levelX = game.world.width -45;
    let allY = game.world.height - 25;
    timeText=game.add.text(5, allY-20, 'Time: ' + time, DefaultFont);
    scoreText = game.add.text(scoreX, allY, 'Score: ' + score, DefaultFont);
    levelText = game.add.text(levelX, allY, 'Wave: ' + (level+1), DefaultFont);
    levelText.anchor.setTo(0.5, 0);
   
}

function timer(){
    time++;
}

function gradesToRadians(angle){
    angle=angle*Math.PI/180;
    return angle;
}

function activateSpaceships( ){//Activates all enemies
    if (deadSpacecraft==false && counterSpaceShipsWaves<SpaceshipLimit2){ //if the word is already in screen
        if (partA==true){ 
            SpaceshipWord=comproveLetter(1);
        }
        if (partB==true || partC==true){
            SpaceshipWord=comproveLetter(2);
        }
        x=Math.floor((Math.random()*450));
        
        if (medium_Enemy1==true){ //if the spaceship is an OWP replicator
            nave=Spaceships.create(x,200,'alienmedium');
            CounterMediumSpaceship=counterSpaceShipsWaves;
            eventSpaceshipsMediums=game.time.events.loop(TIMER*4,generateMediumSpaceships,this);
        }
        else if (big_Enemy1==true){//if the spaceship is an Fan generator
            nave=Spaceships.create(x,200,'alienbig');
            CounterBigSpaceship=counterSpaceShipsWaves; 
            eventSpaceshipsBigs=game.time.events.loop(TIMER*6.5,generateMiniSpaceships,this);
        }
        else{ //else creates a normal one
            nave=Spaceships.create(x,200,'alienNormal');
        }
        nave.checkWorldBounds = true;
        nave.events.onOutOfBounds.add(resetMember2, this);
        nave.scale.setTo(0.1);
       
        d=Math.sqrt((x-spaceCraft.x)**2+(-spaceCraft.y)**2)
        angle=((-1*((x-spaceCraft.x)/d))/(-1*((-50-spaceCraft.y)/d)));
        angle=(Math.atan(angle)*180/Math.PI);
        anglealeatorio=((Math.floor(Math.random()*20) )-10);
        angle=angle+anglealeatorio;  
        nave.angle-=angle;
        nave.reset(x,-25);
        vx=((Math.cos(gradesToRadians(angle-90)))*SpaceshipsVelocity);
        vy=-1*((Math.sin(gradesToRadians(angle-90)))*SpaceshipsVelocity);

        //save the enemy
        spaceShip_array.push(nave);
       
        //velocities for each enemy
        if (medium_Enemy1==true){
            vx=vx/2;
            vy=vy/2; 
            medium_Enemy1=false;
        }
        else if (big_Enemy1==true){
            vx=vx/3;
            vy=vy/3;
            big_Enemy1=false;
        }

        nave.body.velocity.x=vx;
        nave.body.velocity.y=vy;
        counterSpaceShipsWaves++;
        word=game.add.text(nave.x,nave.y,SpaceshipWord,DefaultFont);

        //if we are in partC it adds the visual effects
        if (partC==true){
            efectsPartC();
        }
       
        words_array.push(word);
    
        CounterSpaceships+=1;
        game.time.events.loop(1,moveText,this);
    }
}

function efectsPartC(){//they are random, if 0 just the first appear, if 1, just the second, if 2 both of them and if 3 or more none
    whichone=Math.floor(Math.random()*levelConfigC.waves[level].probability+2);
    if(whichone==0 || whichone==2){
        tween = game.add.tween(word).to({
            
            alpha: 0,
            
        }, 1500, Phaser.Easing.Linear.None)
        .to({

            alpha:1 

        }, 1500, Phaser.Easing.Linear.None)
        tween.loop(true);
        tween.start();
    }

    if (whichone==1 || whichone==2){
        tween = game.add.tween(word).to({

            angle: 90,

        }, 1500, Phaser.Easing.Linear.None)
        .to({

            angle: 0,

        }, 1500, Phaser.Easing.Linear.None);
        tween.loop(true);
        tween.start();
    }
}

function generateMiniSpaceships(){//generates 5 spaceships changing the angle wach time so they appear as we want
    
    if(bigSpaceShipDead==false && deadSpacecraft==false){ 
        angle=-20;
        for (var c=0;c<5;c++)   {
            SpaceshipWord = levelConfig.elements[Math.floor(Math.random()*26)].letter;
            x=spaceShip_array[CounterBigSpaceship].x;
            y=spaceShip_array[CounterBigSpaceship].y;
            
            nave=Spaceships.create(x,y,'alienmini');
            nave.checkWorldBounds = true;
            nave.events.onOutOfBounds.add(resetMember3, this);
            nave.scale.setTo(0.1);
            nave.reset(x+30+sum,y+35);
            vx=((Math.cos(gradesToRadians(angle-90)))*SpaceshipsVelocity);
            vy=-1*((Math.sin(gradesToRadians(angle-90)))*SpaceshipsVelocity);

            spaceShip_array.push(nave);
            nave.body.velocity.x=vx/1.5;
            nave.body.velocity.y=vy/1.5;
            word=game.add.text(nave.x,nave.y-60,SpaceshipWord,DefaultFont);
            if (partC==true){
                efectsPartC(); 
            }
            
            words_array.push(word);
            game.time.events.loop(1,moveText,this);
            
            angle=angle+10
            CounterSpaceships+=1;
            Spaceshiplimit++;
            
        }
    }
}

function generateMediumSpaceships(){ //just make a normal spaceship which goes direct to us
    if(mediumSpaceshipdead==false&&deadSpacecraft==false){ 
        if (partA==true){
            SpaceshipWord=comproveLetter(1);
        }
        if (partB==true || partC==true){
            SpaceshipWord=comproveLetter(2);
        }
        x=spaceShip_array[CounterMediumSpaceship].x;
        y=spaceShip_array[CounterMediumSpaceship].y;
        
        nave=Spaceships.create(x,y,'alienNormal');

        nave.checkWorldBounds = true;
        nave.events.onOutOfBounds.add(resetMember2, this);
        
        nave.scale.setTo(0.1);
        
        d=Math.sqrt((x-spaceCraft.x)**2+(y-spaceCraft.y)**2)
        angle=((-1*((x-spaceCraft.x)/d))/(-1*((y-spaceCraft.y)/d)));
        angle=(Math.atan(angle)*180/Math.PI);
        
        angle=angle; 
        nave.angle-=angle;
        nave.reset(x+28,y+35);
        vx=((Math.cos(gradesToRadians(angle-90)))*SpaceshipsVelocity);
        vy=-1*((Math.sin(gradesToRadians(angle-90)))*SpaceshipsVelocity);

        spaceShip_array.push(nave);
        
        nave.body.velocity.x=vx;
        nave.body.velocity.y=vy;
        
        word=game.add.text(nave.x,nave.y,SpaceshipWord,DefaultFont);

        if (partC==true){//visual effects

            efectsPartC();
            
        }
       
        words_array.push(word);

        CounterSpaceships+=1;
        Spaceshiplimit++;
        
        game.time.events.loop(1,moveText,this);
    }
}

function newRound(){ //each round these variables need to restart
    medium_Enemy=false;
    big_Enemy=false;
    medium_Enemy1=false;
    big_Enemy1=false;
    rightsLetterswaves=0;
    level++;
    timer_activated=true;
    time=time-3;
    counterSpaceShips=0;
    counterSpaceShipsWaves=0;
    spaceShip_array=[];
    words_array=[];  
    uniqueletters=[];
    game.time.events.remove(eventSpaceships);
    game.time.events.remove(eventSpaceshipsMediums);
    game.time.events.remove(eventSpaceshipsBigs);
    endedRound=false;   
    bigSpaceShipDead=false;
    mediumSpaceshipdead=false;
    createPlay();
}

function createKeyWord(extrac){ 
    
    if(extrac==2){ //if extrac==2 it can appear an owp replicator and fan generator
        var tiponave=Math.floor(Math.random()*5);
        if(tiponave>=2 && medium_Enemy==false){
            medium_Enemy=true;
            medium_Enemy1=true;
            return levelConfigB.elements[Math.floor(Math.random()*26)].measures[Math.floor(Math.random()*3)].words[0];
        }else if(tiponave>2 && big_Enemy==false){
            big_Enemy=true;
            big_Enemy1=true;
            return levelConfigB.elements[Math.floor(Math.random()*26)].measures[Math.floor(Math.random()*2)+3].words[0];
        }

        return levelConfig.elements[Math.floor(Math.random()*26)].measures[Math.floor(Math.random()*4)].words[Math.floor(Math.random()*3)];

    }
    else{
        return levelConfig.elements[Math.floor(Math.random()*26)].measures[Math.floor(Math.random()*4)].words[Math.floor(Math.random()*3)];
    }

}

function comproveLetter(extract){ //make us to know if the letter is already in screen
    var p=createKeyWord(extract);
    var again=0;
    for(let n=0;n<uniqueletters.length;n++){

        if(uniqueletters[n]==p.charAt(0)){
            again=1;
        }
    }
    if (again==1){ 
        console.log("REPEATED WORD") 
        if (medium_Enemy1==true){
            medium_Enemy=false;
        }
        if (big_Enemy1==true){
            big_Enemy=false;
        }
        p=comproveLetter(extract);

    }else{
        uniqueletters.push(p.charAt(0));
    }
    var randomNumber=Math.floor(Math.random()*levelConfigC.waves[level].probability)
        
    if (partC==true && randomNumber==1){
        var p2="";
        for(let n=0;n<p.length;n++){
            var letter=p.charAt(n);
            if(Math.floor(Math.random()*3)>1){
                letter=p.charAt(n).toUpperCase();
            }
            p2=p2+letter;
        }
        p=p2;
    }
    return p;
}

function letterDown(e){ 
    if(e.keyCode >= Phaser.Keyboard.A && e.keyCode <= Phaser.Keyboard.Z){
        
        if(fixed_Word==false && deadSpacecraft==false){// if we dont have a word avaiable we choose again
            objetive=look_word(e.key);  

            console.log("word selectionated: "+objetive); //show us the word that is fixed          
        }
        if(objetive!=""){ //fix the word
            fixed_Word=true;
            word=look_word2(objetive);
            if(e.key==objetive.charAt(0)){
                if (counter==0)
                {
                    actual=game.add.sprite(spaceShip_array[word[1]].position.x-65,spaceShip_array[word[1]].position.y-50,'selectionated');
                    blip.play();
                    anim=actual.animations.add('selectionated');
                    anim.play(20,false,true);
                    
                }
                word=look_word2(objetive);
                word[0].style.fill="#FF2D00"
                
                objetive=objetive.substring(1,objetive.length)// it will be decreasing
                words_array[word[1]].text=objetive;

                counter++;
                rightsLetterswaves++;
                rightsLetters++;
                writtenLetters++;
                
                score=100*rightsLetters/writtenLetters;

                //spaceShip_array[word[1]].position.y-=2;

                laserimg=game.add.image(spaceCraft.x+20,spaceCraft.y-25,'laser')
                tween = game.add.tween(laserimg).to({
                    x: spaceShip_array[word[1]].position.x+20,
                    y: spaceShip_array[word[1]].position.y+80 ,
                    alpha: 0
                    
                }, 500, Phaser.Easing.Linear.None);
                tween.start();
                soundLaser.play();


                

                if  (words_array[word[1]].text==""){
                    spaceShip_array[word[1]].kill();
                    displayBlast(spaceShip_array[word[1]]);
                    soundBlast.play();
                    uniqueletters.splice(word[1],word[1]+1);
                    if (word[1]==CounterBigSpaceship){
                        bigSpaceShipDead=true;
                    }
                    if (word[1]==CounterMediumSpaceship){
                        mediumSpaceshipdead=true;
                    }
                }
            }
            else{
                error.play();
                writtenLetters++;
                score=100*rightsLetters/writtenLetters;
            } 
        }
        if(objetive.length==0 && endedRound==false && deadSpacecraft==false){// if the word ends
            if (fixed_Word==true){
                counterSpaceShips++;
            }
            else{
                error.play();
                writtenLetters++;
                score=100*rightsLetters/writtenLetters;
            }
            fixed_Word=false;
            counter=0;
        }
    }
}

function changeRound(){ //information between waves
    tween = game.add.tween(spaceCraft).to({
                
        alpha: 0
        
    }, 500, Phaser.Easing.Linear.None);
    tween.start();

    backgroundblack=game.add.image(0,0,'black')
    backgroundblack.alpha=0;
    tween = game.add.tween(backgroundblack).to({
            
        alpha: 1
        
    }, 750, Phaser.Easing.Linear.None);
    tween.start(); 
    game.add.text(30, 150, '   ROUND ' + (level+1) + '\n COMPLETED!!!', DefaultFontEndRound);
    game.add.text(30, 350, 'letters typed right: ' + rightsLetterswaves, DefaultFontEndRound2);
    game.add.text(30, 400, 'deactivated OWP: ' + counterSpaceShips, DefaultFontEndRound2);
    if (partA==true){
        game.add.text(30, 500, 'actual part: Part A ' , DefaultFontEndRound2);
    }
    if (partB==true){
        game.add.text(30, 500, 'actual part: Part B ' , DefaultFontEndRound2);
    }
    if (partC==true){
        game.add.text(30, 500, 'actual part: Part C ' , DefaultFontEndRound2);
    }
    game.add.text(30, 450, 'elapsed time: ' + time,DefaultFontEndRound2);
    finalround.play();
    finalround2.play();
    endedRound=true;

    if (levelConfig.waves[level].n<5){
        game.time.events.add(3000, newRound, this);
    }
    else{
        game.time.events.add(3000, startWin, this);
    }
}

function look_word(letter){ 
    var word="";
    for(let n=0;n<words_array.length;n++){
        if(letter==words_array[n].text.charAt(0)){
            word=words_array[n].text;
            return  word;
        }
    }
    if(word==""){
        return word;
    }
}

function look_word2(letter){ 
    let word="";
    for(let n=0;n<words_array.length;n++){
        if(letter==words_array[n].text){
            word=words_array[n];
            let lista = [word,n];
            return lista;
        }
    }
    if(word==""){
        return "";
    }
}

function moveText(){ //move text constanly (each frame)
    for(let n=0;n<words_array.length;n++){
        words_array[n].position.y=spaceShip_array[n].position.y+60;
        words_array[n].position.x=spaceShip_array[n].position.x;
    }
    

}

function quitwords(){ //makes the words disappear
    for(let n=0;n<CounterSpaceships;n++){
        words_array[n].text="";
    }

}

function setupBlast(blast) {
    blast.anchor.x = 0.5;
    blast.anchor.y = 0.5;
    blast.animations.add('blast');
    

}

function displayBlast(ship) { //explosions
    let blast = blasts.getFirstExists(false);
    
    let x = ship.body.center.x;
    let y = ship.body.center.y;
    blast.reset(x, y);
    blast.play('blast', 10, false, true);
   
}


function resetMember(item){
   
    item.kill();
}

function resetMember2(item2){//kill the item and the text wich has gone out of limits
    var number;
    for (let i=0; i<words_array.length;i++){
        if (spaceShip_array[i].x==item2.x){
            number=i;
        }
    }
    if (words_array[number].text==objetive){
        fixed_Word=false;
    }
    uniqueletters.splice(number,number+1);
    counter=0;
    if (spaceShip_array[number].key=='alienNormal'){ 
        counterSpaceShipsWaves--;//a new spaceship will appear at the top
    }
    else{
        Spaceshiplimit--;
    }

    item2.kill();
}
function resetMember3(item3){ //kills the mini spaceships of the fan generator

    item3.kill();
    Spaceshiplimit--;

}

function startGameOver() { 
    musicInGame.stop();
    game.state.start('gameOver');

}

function startWin() {
    musicInGame.stop();
    game.state.start('win');
}

function updatePlay() { 
    
    game.physics.arcade.overlap(spaceCraft,Spaceships,ufoHitnave,null,this);
    updateBackground();
    manageCraftMovements(); 
    manageTexts();
    if (counterSpaceShips==Spaceshiplimit && endedRound==false && deadSpacecraft==false){
        changeRound();
    }
    game.input.keyboard.onDownCallback = letterDown;
}

function ufoHitnave(nave,craft){ //collides and kill
    nave.kill();
    craft.kill();
    displayBlast (nave);
    displayBlast(craft);
    soundBlast.play();      
    Spaceships.forEach(resetMember, this);
    quitwords();
    game.input.enabled = true;
    deadSpacecraft=true;
    game.time.events.add(2000, startGameOver, this);
}

function updateBackground(){
    background.tilePosition.y+=0.5;
    mesh.tilePosition.y+=1;
    mesh.tilePosition.x+=0.2;
}

function manageCraftMovements() {
    spaceCraft.body.velocity.x = 0;
    if (cursors.left.isDown || game.input.speed.x < 0)
    spaceCraft.body.velocity.x = -CRAFT_VELOCITY;
    else if (cursors.right.isDown || game.input.speed.x > 0)
    spaceCraft.body.velocity.x = CRAFT_VELOCITY;
}

function manageTexts(){
    
    timeText.setText('Time: ' + time, DefaultFont)
    scoreText.setText('Score: ' + Math.floor(score)+' % ', DefaultFont)
}