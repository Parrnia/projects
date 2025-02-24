import keras
from keras.losses import mean_squared_error
import gym
import numpy as np

# Load the trained model
model = keras.models.load_model('dqn_model.h5', custom_objects={'mse': mean_squared_error})

# Initialize the environment with rendering
env = gym.make("CartPole-v1", render_mode="human")


def epsilon_greedy(state, model, epsilon=0):
    action_values = model.predict(state, verbose=0)[0]
    return np.argmax(action_values)


def test_model(model, episodes=10):
    for episode in range(episodes):
        state, info = env.reset()
        state = np.reshape(state, (1, -1))
        total_reward = 0
        
        for i in range(210):
            env.render()  # Render the environment
            action = epsilon_greedy(state, model)
            next_state, reward, done, _, _ = env.step(action)
            next_state = np.reshape(next_state, (1, -1))
            total_reward += reward
            state = next_state

            if done:
                print(f"Episode: {episode + 1}, Score: {total_reward}")
                break


if __name__ == "__main__":
    test_model(model, episodes=2)
    env.close()
