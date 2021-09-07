// https://natureofcode.com/book/chapter-9-the-evolution-of-code/

let saint;
let arrow;
let lifetime = 250;
let timeElapsed = 0;
let population;
let mutationRate = 0.02;
let arrowSlots;

function preload(){
    saint = loadImage("assets/saint.jpg");
    arrow = loadImage("assets/arrow4.png");
}

function setup(){
    createCanvas(windowHeight, windowHeight);
    background(202, 137, 240);
    imageMode(CENTER);
    rectMode(CENTER);
    ellipseMode(CENTER);
    textAlign(CENTER, CENTER);
    textSize(30);

    population = new Population(mutationRate, 100, lifetime);
    arrowSlots = [createVector(width/4, height/8), createVector(width / 2, height/8), createVector(3 * width/4, height / 8)];

}

function draw() {
    background(202, 137, 240);
    push();
    translate(width/2, height/8);
    rotate(3 * PI /2);
    image(saint, 0, 0, height / 4, width);
    pop();
    text(`GENERATION: ${population.generation}`, width / 8, 3 * height / 4);
    if (timeElapsed < lifetime) {
        population.live(arrow);
        // push();
        // // rotate(3*PI/2);
        // for (let agent of population.agents) {
        //     // translate(agent.position.x, agent.position.y);
        //     // image(arrow, 0, 0, 100, 20);
        //     image(arrow, agent.position.x, agent.position.y, 100, 20);
        //     ellipse(agent.position.x, agent.position.y, 20);
        // }
        // pop();
        timeElapsed++;
    } else {
        timeElapsed = 0;
        population.fitness(random(arrowSlots));
        // for (let agent of population.agents){
        //     let d = p5.Vector.dist(agent.position, random(arrowSlots));
        //     agent.fitness = pow(1 / d, 2);
        // }
        population.selection();
        population.reproduction();
        population.generation++;
    }
}