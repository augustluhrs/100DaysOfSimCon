class DNA {
    constructor(lifetime, maxForce){
        this.genes = [];
        this.maxForce = maxForce;

        for (let i = 0; i < lifetime; i++) {
            this.genes[i] = p5.Vector.random2D();
            this.genes[i].mult(random(0, this.maxForce));
        } 
    }

    crossover(partner){
        let child = new DNA(this.genes.length, this.maxForce);
        let midpoint = floor(random(this.genes.length));
        for (let i = 0; i < this.genes.length; i++){
            if (i > midpoint) {
                child.genes[i] = this.genes[i];
            } else {
                child.genes[i] = partner.genes[i];
            }
        }
        return child;
    }

    mutate(mutationRate) {
        for (let i = 0; i < this.genes.length; i++) {
            if (random(1) < mutationRate) {
                this.genes[i] = p5.Vector.random2D();
                this.genes[i].mult(random(0, this.maxForce));
            }
        }
    }
}