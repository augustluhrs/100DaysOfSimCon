// let woes = ["woe", "pain", "challenge", "failure"];
let woes = ["woe", "pain", "fuck", "fuck", "stress", "nope"];


//based on Shiffman's https://natureofcode.com/book/chapter-6-autonomous-agents/
class Mover {
    constructor (x, y) {
        this.position = createVector(x, y);
        this.velocity = createVector(0, 0);
        this.acceleration = createVector(0, 0);
        this.word = random(woes);
        this.maxSpeed = 4;
        this.maxForce = .1;
        this.maxSeparationSpeed = 40;
        this.maxSeparationForce = 1;
        this.size = random(20, 50);
        this.blue = random(40, 255);
    }

    update(){
        this.velocity.add(this.acceleration);
        this.velocity.limit(this.maxSpeed);
        this.position.add(this.velocity);
        this.acceleration.mult(0);
    }

    applyForce(force){
        this.acceleration.add(force);
    }

    seek(target){
        let desired = p5.Vector.sub(target, this.position);
        desired.normalize();
        desired.mult(this.maxSpeed);

        let steer = p5.Vector.sub(desired, this.velocity);
        steer.limit(this.maxForce);
        this.applyForce(steer);
    }

    separate(movers){
        let desiredSeparation = this.size * 2;
        let sum = createVector(0, 0);
        let count = 0;

        for (let mover of movers) {
            if (mover != this) {
                let d = p5.Vector.dist(this.position, mover.position);
                // console.log(d);
                if ((d > 0) && (d < desiredSeparation)) {
                    let diff = p5.Vector.sub(this.position, mover.position);
                    diff.normalize();
                    diff.div(d);
                    sum.add(diff);
                    count++;
                }
            }
        }
        if (count > 0) {
            sum.div(count);
            sum.normalize();
            // console.log(sum);
            // sum.mult(this.maxSeparationSpeed);
            sum.mult(this.maxSpeed);
            // console.log(sum);
            let steer = p5.Vector.sub(sum, this.velocity);
            // steer.limit(this.maxSeparationForce);
            steer.limit(this.maxForce);
            this.applyForce(steer);
        }
    }

    display(){
        let theta = this.velocity.heading() + PI/2;
        push();
        stroke(100, 100, 255);
        fill(30, 30, this.blue);
        textSize(this.size);
        translate(this.position.x, this.position.y);
        rotate(theta);
        text(this.word, 0, 0);
        pop();
    }
}