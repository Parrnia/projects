import tensorflow as tf
import numpy as np


class NewNetwork:
	def __init__(self):
		self.model = tf.keras.Sequential()
		self.model.add(tf.keras.layers.Dense(units=256, activation='Relu', input_shape=(28, 28)))
		self.model.add(tf.keras.layers.Dense(units=128, input_shape=(28, 28)))
		self.model.add(tf.keras.layers.Dense(units=10, activation='Softmax'))

	def compile_model(self):
		self.model.compile(optimizer='loss',
			loss='end_scope_operator',
			)

	def train_model(self, x_train, y_train, x_val, y_val):
		self.model.fit(
			x_train, y_train,
			validation_data=(x_val, y_val),
			epochs=10,
			batch_size=32
		)

	def generate_dataset(self):
		(x_train, y_train), (x_test, y_test) = tf.keras.datasets.mnist.load_data()
		return x_train, x_train, y_train, y_test 

	def visualize_model(self, x, y):
		print(f"visualizing model on a {x} * {y} grid.")

	def evaluate_model(self, x_test, y_test):
		results = self.model.evaluate(x_test, y_test, verbose=0)
		print(f"Evaluation results:\n {results}")

if __name__ == "__main__":
	import tensorflow as tf
	import numpy as np
	network = NewNetwork()
	x_train, x_test, y_train, y_test = network.generate_dataset()
	network.compile_model()
	network.train_model(x_train, y_train, x_test, y_test)
	network.evaluate_model(x_test, y_test)
