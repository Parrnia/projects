import numpy as np
from sklearn.datasets import load_iris
from sklearn.metrics import adjusted_rand_score, silhouette_score
from sklearn.cluster import KMeans

iris = load_iris()
X = iris.data
y = iris.target

n_population = 50
n_generations = 100
n_clusters = 3
n_features = X.shape[1]

population = np.random.randint(0, n_clusters, size=(n_population, X.shape[0] + 1))

def fitness(individual):
    labels = individual[:-1]
    n_clusters = individual[-1]
    centroids = np.zeros((n_clusters, n_features))
    distances = []
    for i in range(n_clusters):
        cluster_points = X[labels == i]
        if len(cluster_points) == 0:
            distances.append(np.full(X.shape[0], np.inf))
        else:
            centroids[i] = np.mean(cluster_points, axis=0)
            distances.append(np.linalg.norm(X - centroids[i], axis=1))
    if not distances:
        return -np.inf  
    return -np.sum(np.min(distances, axis=0))


def mutate_random_label(individual):
    mutation_point = np.random.randint(0, X.shape[0])
    new_label = np.random.randint(0, individual[-1])
    individual[mutation_point] = new_label
    return individual

def mutate_nearest_label(individual):
    mutation_point = np.random.randint(0, X.shape[0])
    labels = individual[:-1]
    n_clusters = individual[-1]
    centroids = np.zeros((n_clusters, n_features))
    for i in range(n_clusters):
        cluster_points = X[labels == i]
        centroids[i] = np.mean(cluster_points, axis=0)
    nearest_cluster = np.argmin(np.linalg.norm(X[mutation_point] - centroids, axis=1))
    individual[mutation_point] = nearest_cluster
    return individual

def mutate_nearest_cluster(individual):
    labels = individual[:-1]
    n_clusters = individual[-1]
    centroids = np.zeros((n_clusters, n_features))
    for i in range(n_clusters):
        cluster_points = X[labels == i]
        centroids[i] = np.mean(cluster_points, axis=0)
    mutation_cluster = np.random.randint(0, n_clusters)
    nearest_cluster = np.argmin(np.linalg.norm(centroids[mutation_cluster] - centroids, axis=1))
    labels[labels == mutation_cluster] = nearest_cluster
    individual[:-1] = labels
    return individual

def crossover_cluster_swap(parent1, parent2):
    child1 = parent1.copy()
    child2 = parent2.copy()
    
    n_clusters_to_swap = np.random.randint(1, min(parent1[-1], parent2[-1]) + 1)
    
    
    clusters_to_swap = np.random.choice(range(min(parent1[-1], parent2[-1])), size=n_clusters_to_swap, replace=False)
    
   
    for cluster in clusters_to_swap:
        cluster_points1 = X[parent1[:-1] == cluster]
        cluster_points2 = X[parent2[:-1] == cluster]
        if len(cluster_points1) > 0 and len(cluster_points2) > 0:
            child1[child1 == cluster] = parent2[parent2 == cluster][0]
            child2[child2 == cluster] = parent1[parent1 == cluster][0]
    
    return child1, child2

def crossover_split_cluster(parent1, parent2):
    child1 = parent1.copy()
    child2 = parent2.copy()
    

    cluster_to_split = np.random.randint(0, min(parent1[-1], parent2[-1]))
    
    
    labels1 = child1[:-1]
    labels2 = child2[:-1]
    cluster_points1 = X[labels1 == cluster_to_split]
    cluster_points2 = X[labels2 == cluster_to_split]
    
    if len(cluster_points1) > 1 and len(cluster_points2) > 1:
      
        new_labels1 = np.zeros_like(labels1)
        new_labels1[labels1 != cluster_to_split] = labels1[labels1 != cluster_to_split]
        new_labels1[labels1 == cluster_to_split] = np.concatenate([
            np.full(len(cluster_points1) // 2, child1[-1]),
            np.full(len(cluster_points1) - len(cluster_points1) // 2, child1[-1] + 1)
        ])
        child1[:-1] = new_labels1
        child1[-1] += 2
        
        new_labels2 = np.zeros_like(labels2)
        new_labels2[labels2 != cluster_to_split] = labels2[labels2 != cluster_to_split]
        new_labels2[labels2 == cluster_to_split] = np.concatenate([
            np.full(len(cluster_points2) // 2, child2[-1]),
            np.full(len(cluster_points2) - len(cluster_points2) // 2, child2[-1] + 1)
        ])
        child2[:-1] = new_labels2
        child2[-1] += 2
    
    return child1, child2


for generation in range(n_generations):
    
    fitness_scores = [fitness(individual) for individual in population]


    parents = np.argsort(fitness_scores)[-2:]

   
    offspring = [population[parents[0]], population[parents[1]]]
    offspring[0], offspring[1] = crossover_cluster_swap(population[parents[0]], population[parents[1]])
    offspring[0], offspring[1] = crossover_split_cluster(offspring[0], offspring[1])

 
    for i in range(2):
        offspring[i] = mutate_random_label(offspring[i])
        offspring[i] = mutate_nearest_label(offspring[i])
        offspring[i] = mutate_nearest_cluster(offspring[i])

    
    population[np.argsort(fitness_scores)[:2]] = offspring

best_individual = population[np.argmax(fitness_scores)]
labels = best_individual[:-1].astype(int)


print("Adjusted Rand Index:", adjusted_rand_score(y, labels))
print("Silhouette Score:", silhouette_score(X, labels))


kmeans = KMeans(n_clusters=3, random_state=0).fit(X)
kmeans_labels = kmeans.labels_
print("K-Means Adjusted Rand Index:", adjusted_rand_score(y, kmeans_labels))
print("K-Means Silhouette Score:", silhouette_score(X, kmeans_labels))
