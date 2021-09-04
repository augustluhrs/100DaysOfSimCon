//day 3 -- social flowers game of life
//next interactive selection or sliders for vibe or seed/death spore

let colors = [];

// let numRows;
let cellSize; 
let garden = [];

let spontaneousRate = 0.01;
let timeElapsed = 0;

function setup(){
    createCanvas(windowHeight, windowHeight);
    background(92, 58, 10, 200);
    imageMode(CENTER);
    rectMode(CENTER);
    ellipseMode(CENTER);
    textAlign(CENTER, CENTER);
    noStroke();

    // let colors = ["green",           "blue",             "pink",          "yellow",    "orange",           "teal",         "purple"];
    // colors = [color("#53DD6C"), color("#1B9AAA"), color("#EE4266"), color("#F7F052"), color("#F17300"), color("#0AD3FF"), color("#5C3A7A")];
    // colors = [color("#EE4266"), color("#F7F052"), color("#F17300"), color("#0AD3FF")];
    colors = [color("#EE4266"), color("#F7F052"), color("#0AD3FF")];


    // numRows = 30;
    cellSize = windowHeight / 30;

    for (let i = cellSize / 2; i < width; i += cellSize) {
        let row = [];
        for (let j = cellSize / 2; j < height; j += cellSize) {
            row.push(new Plot(i, j));
        }
        garden.push(row);
    }
}

function draw() {
    background(92, 58, 10, 100);

    //update time
    timeElapsed += deltaTime;
    // if(timeElapsed >= 250){ //this is weird but whatever
    //     timeElapsed -= 250;

        let newGarden = [];
        //check neighbors and update according to vibe
        for (let col = 0; col < garden[0].length; col++ ) {
            let newRow = [];
            for (let row = 0; row < garden.length; row++) {
                if (garden[col][row].flower instanceof Flower) { //only check vibes of flowers
                    let neighbors = [];
                    for (let c = 0; c < colors.length; c++) {
                        neighbors.push(0);
                    }
                    for (let i = -1; i <= 1; i++) {
                        for (let j = -1; j <= 1; j++) {
                            if (col + i >= 0 && col + i < garden[0].length && row + j >= 0 && row + j < garden.length) {
                                // if(garden[col + i][row + j] == undefined) {
                                //     console.log(col, i, row, j);
                                // }
                                if (garden[col + i][row + j].flower.colorNum != null) { // ignore grass
                                    neighbors[garden[col + i][row + j].flower.colorNum]++;
                                }
                            }
                        }
                    }
                    newRow.push(garden[col][row].update(neighbors)); 
                } else {
                    newRow.push(garden[col][row])
                }
            }
            newGarden.push(newRow);
        }

        garden = newGarden;
    // }

    //draw everything
    for (let row of garden) {
        for (let plot of row){
            plot.flower.draw();
        }
    }
}

class Plot {
    constructor (i, j, flower) {
        // this.col = i;
        // this.row = j;
        // this.x = this.col * cellSize;
        // this.y = this.row * cellSize;
        this.x = i;
        this.y = j;
        this.flower = flower || undefined;

        //what's in the plot? small chance of nothing
        if (this.flower == undefined) {
            if (random() < .2) {
                this.flower = new NoFlower(this.x, this.y);
            } else {
                this.flower = new Flower(this.x, this.y);
            }
        }
        //no seeds / death yet
    }

    update(neighbors){
        this.flower.checkVibe(neighbors);

        return new Plot(this.x, this.y, this.flower);
    }
}

class NoFlower {
    constructor (x, y) {
        this.x = x;
        this.y = y;
        this.vibe = "empty";
        // this.colorNum = colors.length; //just to fall at end for checking
        this.colorNum = null;
    }

    draw() {
        fill(61, 130, 8);
        rect(this.x, this.y, cellSize - (cellSize / 10));
    }
}

class Flower {
    constructor (x, y) {
        this.x = x;
        this.y = y;
        this.size = random(cellSize / 3, cellSize + (cellSize / 4))
        this.vibe = this.generateVibe();
        // this.color = random(colors);
        this.colorNum = floor(random(colors.length));
        this.color = colors[this.colorNum];
        // this.state = floor(random(3));
    }

    draw(){
        fill(61, 130, 8, 100);
        rect(this.x, this.y, cellSize - (cellSize / 10));
        // fill(this.color);
        // ellipse(this.x, this.y, this.size);
        //vibe visual
        push();
        if (this.vibe == "aloof") {
            stroke(0);
            strokeWeight(2);
            fill(this.color);
            ellipse(this.x, this.y, this.size);
        }
        if (this.vibe == "spontaneous") {
            fill(this.color);
            ellipse(this.x, this.y, this.size);
            fill(255, 100);
            ellipse(this.x, this.y, this.size / 1.5);
        }
        if (this.vibe == "conformist") {
            fill(this.color);
            ellipse(this.x, this.y, this.size);
            
        }
        if (this.vibe == "rebel") {
            fill(this.color);
            ellipse(this.x, this.y, this.size);
            fill(0, 200);
            ellipse(this.x, this.y, this.size / 4);
        }
        pop();


        //petals TODO
        // push();
        // // stroke(0);
        // fill(this.color, 200);
        // ellipse(this.x + random(-this.size, this.size), this.y + random(-this.size, this.size), random(this.size / 3, this.size / 1.5));
        // fill(this.color, 100);
        // ellipse(this.x + random(-this.size, this.size), this.y + random(-this.size, this.size), random(this.size / 4, this.size / 2));
        // pop();
    }

    checkVibe(neighbors) {
        //total flowers to check for aloof
        if (this.vibe == "aloof") {
            let totalNeighbors;
            for (let c of neighbors) {
                totalNeighbors += c;
            }
            for (let [i, o] of neighbors.entries()) { //i = color, c = occurance
                if (o == totalNeighbors) {
                    this.colorNum = i;
                    this.color = colors[i];
                }
            }
        }

        if (this.vibe == "rebel") {
            for (let c of neighbors) {
                if (c >= 6){
                    this.colorNum = floor(random(colors.length));
                    this.color = colors[this.colorNum]; 
                }
            }
        }

        if (this.vibe == "conformist") {
            let mostCommon = 0;
            for (let i = 1; i < neighbors.length; i++){
                if (neighbors[mostCommon] < neighbors[i]) {
                    mostCommon = i;
                } else if (neighbors[mostCommon] == neighbors[i]){
                    if (random() < .5) {
                        mostCommon = i;
                    }
                }
            }
            if (neighbors[mostCommon] != 0){
                this.colorNum = mostCommon;
                this.color = colors[this.colorNum];
            }
        }





        if (this.vibe == "spontaneous") {
            if (random() < spontaneousRate) {
                this.colorNum = floor(random(colors.length));
                this.color = colors[this.colorNum];
            }
        }
        
        
        



    }

    generateVibe(){
        // set random preferences -- for now just the colorVibe
        // mutually exclusive:
        // rebel = changes color to random color if 3 or more neighbors are the same color
        // conformist = changes color to most common color of neighbors
        // spontaneous = changes color randomly
        // aloof = doesn't change, doesn't care, unless every neighbor is the same color
        // let colorVibes = ["rebel", "conformist", "spontaneous", "aloof"];
        let colorVibes = ["conformist", "conformist", "conformist", "spontaneous"];
        // let colorVibes = ["conformist", "conformist", "conformist", "conformist", "conformist", "conformist", "rebel", "aloof", "spontaneous"];

        let colorVibe = colorVibes[floor(random(colorVibes.length))];

        // let vibes = {
        //     colorVibe: colorVibe
        // };

        return colorVibe;
    }
}

// 
// https://stackoverflow.com/questions/3730510/javascript-sort-array-and-return-an-array-of-indices-that-indicates-the-positio
// 
// function sortWithIndeces(toSort) {
//     for (var i = 0; i < toSort.length; i++) {
//       toSort[i] = [toSort[i], i];
//     }
//     toSort.sort(function(left, right) {
//       return left[0] < right[0] ? -1 : 1;
//     });
//     toSort.sortIndices = [];
//     for (var j = 0; j < toSort.length; j++) {
//       toSort.sortIndices.push(toSort[j][1]);
//       toSort[j] = toSort[j][0];
//     }
//     return toSort;
// }
  
//   var test = ['b', 'c', 'd', 'a'];
//   sortWithIndeces(test);
//   console.log(test.sortIndices.join(","));