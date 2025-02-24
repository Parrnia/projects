import random


distance_matrix = [
   [16, 21, 20, 29, 0],
[28, 19, 15, 0, 29],
[25, 13, 0, 15, 20],
[17, 0, 13, 19, 21],
[0, 17, 25, 28, 16]
]


population_size = 50
mutation_rate = 0.01
num_generations = 100


def create_population(size):
    population = []
    for _ in range(size):
        individual = list(range(1, len(distance_matrix)))
        random.shuffle(individual)
        population.append(individual)
    return population

def calculate_distance(route):
    total_distance = 0
    for i in range(len(route)):
        total_distance += distance_matrix[route[i-1]][route[i]]
    total_distance += distance_matrix[route[-1]][0]
    return total_distance +1


def evaluate_population(population):
    fitness_scores = []
    for individual in population:
        distance = calculate_distance([0] + individual + [0])
        fitness = 1 / distance
        fitness_scores.append(fitness)
    return fitness_scores

def selection(population, fitness_scores):
    total_fitness = sum(fitness_scores)
    probabilities = [fitness / total_fitness for fitness in fitness_scores]
    parents = random.choices(population, weights=probabilities, k=2)
    return parents


def crossover(parent1, parent2):
    crossover_point = random.randint(0, len(parent1) - 1)
    offspring = parent1[:crossover_point]
    for gene in parent2:
        if gene not in offspring:
            offspring.append(gene)
    return offspring


def mutate(individual):
    idx1, idx2 = random.sample(range(len(individual)), 2)
    individual[idx1], individual[idx2] = individual[idx2], individual[idx1]
    return individual


def genetic_algorithm():
    population = create_population(population_size )

    for generation in range(num_generations):
        fitness_scores = evaluate_population(population)
        best_individual = population[fitness_scores.index(max(fitness_scores))]
        best_distance = calculate_distance(best_individual)
      

        new_population = [best_individual]

        while len(new_population) < population_size:
            parent1, parent2 = selection(population, fitness_scores)
            offspring = crossover(parent1, parent2)

            if random.random() < mutation_rate:
                offspring = mutate(offspring)

            new_population.append(offspring)

        population = new_population

    best_route = [0] + population[fitness_scores.index(max(fitness_scores))] + [0]
    best_distance = calculate_distance(best_route)
    print("\nBest Route:", "->".join(str(city) for city in reversed(best_route)))
    print("Best Distance:", best_distance)


genetic_algorithm()