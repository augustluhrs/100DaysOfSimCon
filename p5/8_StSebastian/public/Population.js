class Population {
    constructor(mutationRate, numAgents, lifetime){
        this.mutationRate = mutationRate;
        this.agents = []
        this.matingPool = [];
        this.generation = 1;

        for (let i = 0; i < numAgents; i++) {
            this.agents.push(new Agent(random(width/4, 3 * width / 4), height, lifetime));
        }
    }

    live(arrow) {
        for (let i = 0; i < this.agents.length; i++) {
            let agent = this.agents[i];
            if(agent.position.y > height/8){
                agent.run();
            }
            push();
            translate(agent.position.x, agent.position.y);
            rotate(3 * PI/2);
            image(arrow, 0, 0, 100, 20);
            // image(arrow, agent.position.x, agent.position.y, 100, 20);
            // ellipse(agent.position.x, agent.position.y, 20);
            pop();
        }
    }

    // fitness(target){
    fitness(){
        for (let agent of this.agents){
            // let d = p5.Vector.dist(agent.position, target);
            let d = (height/8) - agent.position.y + 0.01;
            agent.fitness = pow(1 / d, 2);
        }
    }

    selection(){
        for (let i = 0; i < this.agents.length; i++) {
            let n = this.agents[i].fitness * 100;
            for (let j = 0; j < n; j++) {
                // console.log(n);
                this.matingPool.push(this.agents[i]);
            }
        }
    }

    reproduction(){
        let newAgents = [];
        for (let i = 0; i < this.agents.length; i++){
            let a = floor(random(this.matingPool.length));
            let b = floor(random(this.matingPool.length));
            let child = this.matingPool[a].DNA.crossover(this.matingPool[b].DNA);
            child.mutate(this.mutationRate);
            let baby = new Agent(random(width/4, 3 * width / 4), height, lifetime, child);
            newAgents.push(baby);
        }
        this.agents = newAgents;
        this.matingPool = [];
    }
}