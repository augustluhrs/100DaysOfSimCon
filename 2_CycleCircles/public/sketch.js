
let poops = [];
let plants = [];
let rabbits = [];
let wolves = [];
let corpses = [];
let world = [corpses, poops, plants, rabbits, wolves];
let timeElapsed = 0;
let hasSecondPassed = false;
let handOfGod = 0;
let plantMax = 10;
let plantsMax = 5000;
let rabbitMax = 60;
let rabbitsMax = 40;
let wolfMax = 200;
let wolvesMax = 12;

function preload(){

}

function setup(){
    createCanvas(windowHeight, windowHeight);
    background(92, 250, 166);
    imageMode(CENTER);
    textAlign(CENTER, CENTER);

    noStroke();
}

function draw() {
    background(92, 250, 166);

    //update time
    timeElapsed += deltaTime;
    hasQuarterSecondPassed = false;
    if(timeElapsed >= 250){ //this is weird but whatever
        timeElapsed -= 250;
        hasQuarterSecondPassed = true;
        // fill(0); //just testing rate visually
        // ellipse(100, 100, 100, 100);
    }




    //decay all the stuff
    if(hasQuarterSecondPassed) {
        for (let life of world) {
            for (let [i, thing] of life.entries()) {
                let [isDead, product] = thing.decay();
                if (isDead) {
                    life.splice(i, 1);
                }
                if (product != undefined){
                    if(product instanceof Plant){
                        plants.push(product);
                    }
                    if(product instanceof Corpse){
                        corpses.push(product);
                    }
                    if(product instanceof Poop){
                        poops.push(product);
                    }
                }
            }
        }
    }

    //move the animals
    for (let wolf of wolves){
        wolf.move();
        wolf.eat();
    }
    for (let rabbit of rabbits){
        rabbit.move();
        rabbit.eat();
    }
   
    //mate check
    for (i = rabbits.length - 1; i >= 0; i--){
        if(rabbits[i].eggs > 0){
            for (j = i - 1; j >= 0; j--){
                if(rabbits[j].eggs > 0){
                    let d = dist(rabbits[i].pos.x, rabbits[i].pos.y, rabbits[j].pos.x, rabbits[j].pos.y);
                    if (d < rabbits[i].size / 2){
                        console.log("rabbit mate" + d);
                        rabbits[i].eggs--;
                        rabbits[j].eggs--;
                        rabbits.push(new Rabbit(rabbits[i].pos.x, rabbits[j].pos.y));
                    }
                }
            }
        }
    }
    for (i = wolves.length - 1; i >= 0; i--){
        if(wolves[i].eggs > 0){
            for (j = i - 1; j >= 0; j--){
                if(wolves[j].eggs > 0){
                    let d = dist(wolves[i].pos.x, wolves[i].pos.y, wolves[j].pos.x, wolves[j].pos.y);
                    if (d < wolves[i].size / 2){
                        console.log("wolf mate" + d);
                        wolves[i].eggs--;
                        wolves[j].eggs--;
                        wolves.push(new Wolf(wolves[i].pos.x, wolves[j].pos.y));
                    }
                }
            }
        }
    }

    //draw all the stuff
    for (let life of world) {
        for (let thing of life) {
            thing.draw();
        }
    }


}

function mousePressed() {
    if(handOfGod == 0){
        corpses.push(new Corpse(mouseX, mouseY, 10));
    } else if (handOfGod == 1) {
        poops.push(new Poop(mouseX, mouseY));
    } else if (handOfGod == 2) {
        plants.push(new Plant(mouseX, mouseY, plantMax));
    } else if (handOfGod == 3) {
        rabbits.push(new Rabbit(mouseX, mouseY));
    } else if (handOfGod == 4) {
        wolves.push(new Wolf(mouseX, mouseY));
    }
}

function keyPressed(){
    if (keyCode == ENTER){
        if(handOfGod + 1 < world.length){
            handOfGod++;
        } else {
            handOfGod = 0;
        }
    }
}

class Corpse {
    constructor(x, y, size) {
        this.x = x;
        this.y = y;
        this.size = size;
        this.life = 30;
        this.color = color(0, 150);
    }

    draw(){
        fill(this.color);
        ellipse(this.x, this.y, this.size);
    }

    decay(){
        this.life -= 1;
        this.color = color(255 - (255 * (this.life / 30)), 150);
        let isDead = false;
        let product;
        if (this.life == 0) {
            isDead = true;
            product = new Plant(this.x, this.y, plantMax);
        }
        return [isDead, product];
    }
}

class Poop {
    constructor(x, y){
        this.x = x;
        this.y = y;
        this.size = 10;
        this.life = 16;
        this.color = color(100, 50, 0);
    }

    draw(){
        fill(this.color);
        ellipse(this.x, this.y, this.size);
    }

    decay(){
        this.life -= 1;
        let isDead = false;
        let product;
        if (this.life == 0) {
            isDead = true;
            product = new Plant(this.x, this.y, plantMax);
        }
        return [isDead, product];
    }

    // grow(){
    //     return new plant(this.x, this.y, 60);
    // }
}

class Plant {
    constructor(x, y, life){
        this.x = x;
        this.y = y;
        this.size = 5;
        this.life = life;
        this.color = color(12, ((this.life / (plantMax / 2)) * 200), 10); 
    }

    draw(){
        fill(this.color);
        ellipse(this.x, this.y, this.size);
    }

    decay(){
        // this.life -= 1;
        this.color = color(12, ((this.life / (plantMax / 2)) * 200), 10); 
        let isDead = false;
        let product;

        if(this.life % 4 == 0 && this.life <= plantMax && plants.length < plantsMax){
            let offset = {
                x: random(-5, 5),
                y: random(-5, 5)
            }
            if (random(1) < 0.5){
                product = new Plant(this.x + offset.x, this.y + offset.y, this.life + 1);
            } else {
                product = new Plant(this.x + offset.x, this.y + offset.y, this.life - 1);
            }
            // this.life -= 5; 
            this.life / 2; 

        } else {
            this.life--;
        }

        if (this.life == 0) {
            isDead = true;
        }
        return [isDead, product];
    }
}

class Rabbit {
    constructor(x, y){
        // this.x = x;
        // this.y = y;
        this.pos = createVector(x, y);
        this.xOff = random(1000);
        this.yOff = random(1000);
        this.maxSpeed = 10;
        this.size = 20;
        this.life = rabbitMax;
        this.belly = 10;
        this.bellyMax = 12;
        this.color = color(0, 0, 255);
        this.eggs = 1;
    }

    draw(){
        fill(this.color);
        ellipse(this.pos.x, this.pos.y, this.size);
    }

    move(){
        //perlin noise movement
        let vx = map(noise(this.xOff), 0, 1, -this.maxSpeed, this.maxSpeed);
        let vy = map(noise(this.yOff), 0, 1, -this.maxSpeed, this.maxSpeed);
        let velocity = createVector(vx, vy);
        this.xOff += 0.01;
        this.yOff += 0.01;
        this.pos.add(velocity);

        if (this.pos.x < -this.size) this.pos.x = width + this.size;
        if (this.pos.y < -this.size) this.pos.y = height + this.size;
        if (this.pos.x > width + this.size) this.pos.x = -this.size;
        if (this.pos.y > height + this.size) this.pos.y = -this.size;
    }

    eat(){
        if(this.belly < this.bellyMax){
            for(let [i, food] of plants.entries()){
                if (dist(food.x, food.y, this.pos.x, this.pos.y) < this.size / 2){
                    plants.splice(i, 1);
                    // this.belly++;
                    if (this.belly < this.bellyMax){
                        this.belly++;
                    }
                }
            }
        }
    }

    decay(){
        // this.life -= 1;
        this.color = color(0, 0, (255 - rabbitMax) + this.life); 
        let isDead = false;
        let product;

        if(this.belly == 0){
            this.life--;
        } else {
            this.belly--;
            if(this.life < rabbitMax){
                this.life++;
            }
        }

        if (this.belly == this.bellyMax && this.life % this.belly == 0){
            product = new Poop(this.pos.x + random(-2, 2), this.pos.y + random(-2, 2));
            product = new Poop(this.pos.x + random(-2, 2), this.pos.y + random(-2, 2));
            product = new Poop(this.pos.x + random(-2, 2), this.pos.y + random(-2, 2));

        }

        if (this.life == 0) {
            isDead = true;
            product = new Corpse(this.pos.x, this.pos.y, this.size);
        }
        return [isDead, product];
    }
}

class Wolf {
    constructor(x, y){
        this.pos = createVector(x, y);
        this.xOff = random(1000);
        this.yOff = random(1000);
        this.maxSpeed = 20;
        this.size = 60;
        this.life = wolfMax;
        this.belly = 10;
        this.bellyMax = 20;
        this.color = color(255, 0, 0);
        this.eggs = 1;
    }

    draw(){
        fill(this.color);
        ellipse(this.pos.x, this.pos.y, this.size);
    }

    move(){
        //perlin noise movement
        let vx = map(noise(this.xOff), 0, 1, -this.maxSpeed, this.maxSpeed);
        let vy = map(noise(this.yOff), 0, 1, -this.maxSpeed, this.maxSpeed);
        let velocity = createVector(vx, vy);
        this.xOff += 0.01;
        this.yOff += 0.01;
        this.pos.add(velocity);

        if (this.pos.x < -this.size) this.pos.x = width + this.size;
        if (this.pos.y < -this.size) this.pos.y = height + this.size;
        if (this.pos.x > width + this.size) this.pos.x = -this.size;
        if (this.pos.y > height + this.size) this.pos.y = -this.size;
    }

    eat(){
        if(this.belly < this.bellyMax){
            for(let [i, food] of rabbits.entries()){
                if (dist(food.pos.x, food.pos.y, this.pos.x, this.pos.y) < this.size / 2){
                    rabbits.splice(i, 1);
                    if (this.belly < this.bellyMax){
                        this.belly += 10;
                    }
                    poops.push(new Poop(this.pos.x, this.pos.y));
                    poops.push(new Poop(this.pos.x + 1, this.pos.y));
                    poops.push(new Poop(this.pos.x - 1, this.pos.y));
                    poops.push(new Poop(this.pos.x , this.pos.y + 1));
                    poops.push(new Poop(this.pos.x - 1, this.pos.y + 1));
                    poops.push(new Poop(this.pos.x + 1, this.pos.y + 1));
                    poops.push(new Poop(this.pos.x - 1, this.pos.y - 1));
                }
            }
        }
    }

    decay(){
        this.color = color((255 - wolfMax) + this.life, 0, 0); 
        let isDead = false;
        let product;

        if(this.belly == 0){
            this.life--;
        } else {
            this.belly--;
            if(this.life < wolfMax){
                this.life++;
            }
        }

        // if (this.belly == this.bellyMax && this.life % this.belly == 0){
        //     product = new Poop(this.pos.x, this.pos.y);
        // }

        if (this.life == 0) {
            isDead = true;
            product = new Corpse(this.pos.x, this.pos.y, this.size);
        }
        return [isDead, product];
    }
}