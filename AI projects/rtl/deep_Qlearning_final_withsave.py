import os
import random
import keras
import tensorflow as tf
from keras import models, layers
from keras.optimizers import Adam
import gym
import numpy as np
import pandas as pd
from collections import deque
from tqdm import tqdm

M = 250  # number of episodes
T = 210  # number of iterations of inner loop
batch_size = 24

# Initialize the environment
env = gym.make("CartPole-v1")

class DQNAgent:
    def __init__(self, state_size, action_size, alpha=0.001, gamma=0.95, epsilon=1.0, epsilon_min=0.001, epsilon_decay=0.995):
        self.state_size = state_size
        self.action_size = action_size
        self.alpha = alpha
        self.gamma = gamma
        self.epsilon = epsilon
        self.epsilon_min = epsilon_min
        self.epsilon_decay = epsilon_decay
        self.memory = deque(maxlen=2500)
        self.model = self.build_model()

    def build_model(self):
        model = models.Sequential([
            layers.Dense(24, input_dim=self.state_size, activation='relu'),
            layers.Dense(24, activation='relu'),
            layers.Dense(self.action_size, activation='linear')
        ])
        model.compile(loss='mse', optimizer=Adam(learning_rate=self.alpha))
        return model

    def act(self, state):
        if np.random.rand() <= self.epsilon:
            return env.action_space.sample()
        act_values = self.model.predict(state)
        return np.argmax(act_values[0])

    def remember(self, state, action, reward, next_state, done):
        self.memory.append((state, action, reward, next_state, done))

    def replay(self):
        minibatch = random.sample(self.memory, batch_size)
        states = np.array([i[0] for i in minibatch])
        actions = np.array([i[1] for i in minibatch])
        rewards = np.array([i[2] for i in minibatch])
        next_states = np.array([i[3] for i in minibatch])
        dones = np.array([i[4] for i in minibatch])

        states = np.squeeze(states)
        next_states = np.squeeze(next_states)

        targets = rewards + self.gamma * (np.amax(self.model.predict_on_batch(next_states), axis=1)) * (1 - dones)
        targets_full = self.model.predict_on_batch(states)

        ind = np.array([i for i in range(batch_size)])
        targets_full[[ind], [actions]] = targets

        self.model.fit(states, targets_full, epochs=1, verbose=0)
        if self.epsilon > self.epsilon_min:
            self.epsilon *= self.epsilon_decay

if __name__ == "__main__":
    state_size = env.observation_space.shape[0]
    action_size = env.action_space.n
    agent = DQNAgent(state_size, action_size)

    results = pd.DataFrame(columns=['Episode', 'Score', 'Epsilon', 'Loss'])

    for episode in tqdm(range(M)):
        state = env.reset()
        state = np.reshape(state, [1, state_size])
        score = 0
        for t in range(T):
            action = agent.act(state)
            next_state, reward, done, _ = env.step(action)
            next_state = np.reshape(next_state, [1, state_size])
            agent.remember(state, action, reward, next_state, done)
            score += reward
            state = next_state
            if done or t == T - 1:
                loss = 0
                if len(agent.memory) >= batch_size:
                    agent.replay()
                    loss = agent.model.evaluate(state, agent.model.predict(state), verbose=0)
                new_row = pd.DataFrame([{
                    'Episode': episode,
                    'Score': score,
                    'Epsilon': agent.epsilon,
                    'Loss': loss
                }])
                results = pd.concat([results, new_row], ignore_index=True)
                print(f"Episode: {episode}, Score: {score}, Epsilon: {agent.epsilon}, Loss: {loss}")
                break

        if (episode + 1) % 25 == 0:
            model_filename = f'dqn_model_episode_{episode + 1}.h5'
            agent.model.save(model_filename)
            print(f"Model saved as {model_filename}")
            results.to_csv(f'dqn_training_results_{episode + 1}.csv', index=False)

    results.to_csv('dqn_training_results.csv', index=False)
    agent.model.save('dqn_model.h5')