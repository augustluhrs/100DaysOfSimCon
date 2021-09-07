let movers = [];
let me;

function preload(){
    me = loadImage("assets/justFace.jpeg");
}

function setup(){
    createCanvas(windowHeight, windowHeight);
    background(100);
    imageMode(CENTER);
    rectMode(CENTER);
    ellipseMode(CENTER);
    textAlign(CENTER, CENTER);

    for (let i = 0; i < 4; i++) {
        movers.push(new Mover(random(0, width), random(0, height)));
    }
}

function draw() {
    background(140, 100, 0, 100);
    image(me, mouseX, mouseY, me.width / 10, me.height / 10);
    let target = createVector(mouseX, mouseY);
    for (let mover of movers){
        mover.seek(target);
        mover.separate(movers);
        mover.update();
        mover.display();
    }
}

function mousePressed() {
    movers.push(new Mover(width - mouseX, height - mouseY));
}