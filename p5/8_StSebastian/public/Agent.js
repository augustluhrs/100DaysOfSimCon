class Agent {
    constructor(x, y, lifetime, dna){
        this.position = createVector(x, y);
        this.velocity = createVector(0, 0);
        this.acceleration = createVector(0, 0);
        this.maxForce = .5;
        this.DNA = dna || new DNA(lifetime, this.maxForce);
        this.geneCount = 0;
        this.fitness = 0;
    }

    accelerate(force){
        this.acceleration.add(force);
    }

    update() {
        this.velocity.add(this.acceleration);
        this.position.add(this.velocity);
        this.acceleration.mult(0);
    }

    run(){
        this.accelerate(this.DNA.genes[this.geneCount]);
        this.geneCount++;
        this.update();
    }
}