// font thanks to https://www.1001fonts.com/fantaisieartistique-font.html

let map;
let fellowship;
let fantasyFont;
let frodo;
let lineNum = 0;
let fellowPos = {};
let path = [];
let pathSize = 5;
let pathScale = .2;

function preload(){
    map = loadImage("assets/lotrMap.jpg");
    fellowship = loadStrings("assets/fellowship.txt", () => {console.log("done loading text")});
    fantasyFont = loadFont("assets/fantasyfont.ttf");
    frodo = loadImage("assets/frodo.png");
}

function setup(){
    createCanvas(map.width * 2, map.height * 2);
    background(100);

    frameRate(10);

    imageMode(CENTER);
    image(map, width / 2, height / 2, map.width * 2, map.height * 2);
    textFont(fantasyFont);
    textAlign(CENTER, CENTER);
    textSize(height / 20);
    strokeWeight(8);
    fellowPos.x = width / 2;
    fellowPos.y = height / 3;
}

function draw() {
    //refresh background
    image(map, width / 2, height / 2, map.width * 2, map.height * 2);
  
    //go to next line in the text
    let textLine = fellowship[lineNum];
    let words = split(textLine, " ");
    if((lineNum + 1) < fellowship.length){
        lineNum++;
    }
    
    //sample the line for the random words
    let x1 = random(words);
    let y1 = random(words);
    let x2 = random(words);
    let y2 = random(words);
    // console.log(random(split(x1,"")), y1, x2, y2);
    let x1Val = x1.charCodeAt(random(split(x1,""))) * -1;
    let x2Val = x2.charCodeAt(random(split(x2,"")));
    let y1Val = y1.charCodeAt(random(split(y1,""))) * -1;
    let y2Val = y2.charCodeAt(random(split(y2,"")));
    // console.log(x1Val, x2Val, y1Val, y2Val);

    //could add prevention of huge jumps, but kind of funny, those are the eagles
    if (x1Val <= 0) {
    } else {
        x1Val = -4;
    }
    if (x2Val >= 0) {
    } else {
        x2Val = 4;
    }
    if (y1Val <= 0) {
    } else {
        y1Val = -4;
    }
    if (y2Val >= 0) {
    } else {
        y2Val = 4;   
    }

    // console.log(x1Val, x2Val, y1Val, y2Val);

    //update new position
    // fellowPos.x += random(-4, 4);
    // fellowPos.y += random(-4, 4);
    fellowPos.x += random(x1Val * pathScale, x2Val * pathScale);
    fellowPos.y += random(y1Val * pathScale, y2Val * pathScale);
    if(fellowPos.x < 0) {
        fellowPos.x = 0;
    }
    if(fellowPos.x > width){
        fellowPos.x = width;
    }
    if(fellowPos.y < 0){
        fellowPos.y = 0;
    }
    if(fellowPos.y > height){
        fellowPos.y = height;
    }

    let newPos = {x: fellowPos.x, y: fellowPos.y};
    path.push(newPos);

    //draw path thus far
    // noStroke();
    fill(0);
    // for(let part of path){
    //     ellipse(part.x, part.y, pathSize);
    // }
    strokeWeight(8);
    stroke(0);
    for (let i = 1; i < path.length; i++){
        line(path[i-1].x, path[i-1].y, path[i].x, path[i].y);
    };


    //text at bottom
    stroke(0);
    strokeWeight(8);
    fill(255);
    text(textLine, width / 2, 11 * height / 13);

    //current fellowship position
    stroke(0, 255, 100);
    strokeWeight(2);
    fill(255);
    // ellipse(fellowPos.x, fellowPos.y, 20, 20);
    image(frodo, fellowPos.x, fellowPos.y, 50, 70);




}