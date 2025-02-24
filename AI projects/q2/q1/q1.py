import numpy as np
import matplotlib.pyplot as plt
from sklearn.neural_network import MLPRegressor
from sklearn.model_selection import train_test_split
from sklearn.model_selection import cross_val_score

def target_function_1(x):
    return np.sin(x)

def target_function_2(x):
    return x**2 + 2*x + 1

def target_function_3(x):
    return np.sqrt(np.abs(x))


num_points = 50
#num_points = 100  # تعداد نقاط آموزشی جدید
x_train = np.linspace(-2 * np.pi, 2 * np.pi, num_points)
#x_train = np.linspace(-3 * np.pi, 3 * np.pi, num_points)  # محدوده ورودی تغییر کند
y_train_1 = target_function_1(x_train) + np.random.normal(0, 0.1, num_points)
y_train_2 = target_function_2(x_train) + np.random.normal(0, 0.1, num_points)
y_train_3 = target_function_3(x_train) + np.random.normal(0, 0.1, num_points)


x_train, x_val, y_train_1, y_val_1, y_train_2, y_val_2, y_train_3, y_val_3 = train_test_split(
    x_train, y_train_1, y_train_2, y_train_3, test_size=0.4, random_state=42
)


hidden_layer_sizes = (100, 100, 50)
#hidden_layer_sizes = (200, 100, 50)  # تعداد نورون‌های هر لایه مخفی جدید
#hidden_layer_sizes = (50, 25, 10)  # تعداد نورون‌های هر لایه مخفی جدید
#max_iter = 10000
max_iter = 5000
mlp_1 = MLPRegressor(hidden_layer_sizes=hidden_layer_sizes, max_iter=max_iter)

mlp_2 = MLPRegressor(hidden_layer_sizes=hidden_layer_sizes, max_iter=max_iter)

mlp_3 = MLPRegressor(hidden_layer_sizes=hidden_layer_sizes, max_iter=max_iter)

cv_scores_1 = -cross_val_score(mlp_1, x_train.reshape(-1, 1), y_train_1, cv=5, scoring='neg_mean_squared_error')
cv_scores_2 = -cross_val_score(mlp_2, x_train.reshape(-1, 1), y_train_2, cv=5, scoring='neg_mean_squared_error')
cv_scores_3 = -cross_val_score(mlp_3, x_train.reshape(-1, 1), y_train_3, cv=5, scoring='neg_mean_squared_error')


print("Mean Squared Error :", np.mean(cv_scores_1))
print("Mean Squared Error :", np.mean(cv_scores_2))
print("Mean Squared Error :", np.mean(cv_scores_3))

mlp_1.fit(x_train.reshape(-1, 1), y_train_1)
mlp_2.fit(x_train.reshape(-1, 1), y_train_2)
mlp_3.fit(x_train.reshape(-1, 1), y_train_3)


#x_test = np.linspace(-3 * np.pi, 3 * np.pi, num_points)  # محدوده ورودی تغییر کند
x_test = np.linspace(-4 * np.pi, 4 * np.pi, 200)
y_test_1 = target_function_1(x_test)
y_test_2 = target_function_2(x_test)
y_test_3 = target_function_3(x_test)

y_pred_1 = mlp_1.predict(x_test.reshape(-1, 1))
y_pred_2 = mlp_2.predict(x_test.reshape(-1, 1))
y_pred_3 = mlp_3.predict(x_test.reshape(-1, 1))


mse_1 = np.mean((y_test_1 - y_pred_1) ** 2)
mse_2 = np.mean((y_test_2 - y_pred_2) ** 2)
mse_3 = np.mean((y_test_3 - y_pred_3) ** 2)

print("Mean Squared Error for Target Function 1:", mse_1)
print("Mean Squared Error for Target Function 2:", mse_2)
print("Mean Squared Error for Target Function 3:", mse_3)


plt.figure(figsize=(12, 8))

plt.subplot(311)
plt.plot(x_test, y_test_1, label='Target Function 1')
plt.plot(x_test, y_pred_1, label='Learned Function 1')
plt.scatter(x_train, y_train_1, color='red', label='Training Points')
plt.legend()
plt.xlabel('x')
plt.ylabel('y')
plt.title('Approximating Target Function 1 using MLP')

plt.subplot(312)
plt.plot(x_test, y_test_2, label='Target Function 2')
plt.plot(x_test, y_pred_2, label='Learned Function 2')
plt.scatter(x_train, y_train_2, color='red', label='Training Points')
plt.legend()
plt.xlabel('x')
plt.ylabel('y')
plt.title('Approximating Target Function 2 using MLP')

plt.subplot(313)
plt.plot(x_test, y_test_3, label='Target Function 3')
plt.plot(x_test, y_pred_3, label='Learned Function 3')
plt.scatter(x_train, y_train_3, color='red', label='Training Points')
plt.legend()
plt.xlabel('x')
plt.ylabel('y')
plt.title('Approximating Target Function 3 using MLP')

plt.tight_layout()
plt.show()