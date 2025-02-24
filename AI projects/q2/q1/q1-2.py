import numpy as np
import matplotlib.pyplot as plt
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense

def nonlinear_function(x):
    equation = 3*np.sin(2*np.pi*x/5)+2*x-19+10*x -20*np.sin(4*np.pi*x/10)-3*x+-20 + np.sin(-6*np.pi*x/15)-x*x
    return equation

x = np.linspace(-10, 10, 100)


y_original = nonlinear_function(x)

noise = np.random.normal(0, 10, size=x.shape)
y_train = y_original + noise


model = Sequential()
model.add(Dense(10, input_dim=1, activation='relu'))
model.add(Dense(1))


model.compile(loss='mse', optimizer='adam')
model.fit(x, y_train, epochs=1000, verbose=0)


y_approx = model.predict(x)


plt.plot(x, y_original, label='Nonlinear Function')
plt.scatter(x, y_train, label='Noisy Data')
plt.plot(x, y_approx, label='Approximation')
plt.legend()
plt.show()