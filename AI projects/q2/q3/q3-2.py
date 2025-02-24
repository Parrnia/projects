import os
import cv2
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import LabelEncoder
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Conv2D, MaxPooling2D, Flatten, Dense
from tensorflow.keras.utils import to_categorical


def load_data():
    images = []
    labels = []
    original_dir = "./Dataset/Original_Images/Original_Images"

    labels_data = pd.read_csv("./Dataset/Dataset.csv")
    label_encoder = LabelEncoder()

    for person_dir in os.listdir(original_dir):
        person_path = os.path.join(original_dir, person_dir)
        if os.path.isdir(person_path):
            for filename in os.listdir(person_path):
                img = cv2.imread(os.path.join(person_path, filename))
                if img is not None:
                    img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
                    img = cv2.resize(img, (64, 64))  

                    images.append(img)
                    labels.append(person_dir)

    labels = label_encoder.fit_transform(labels)
    labels = to_categorical(labels)

    X_train, X_test, y_train, y_test = train_test_split(images, labels, test_size=0.4, random_state=42)
    X_val, X_test, y_val, y_test = train_test_split(X_test, y_test, test_size=0.5, random_state=42)

    X_train = np.array(X_train) / 255.0
    X_val = np.array(X_val) / 255.0
    X_test = np.array(X_test) / 255.0

    X_train = X_train.reshape(X_train.shape[0], X_train.shape[1], X_train.shape[2], 1)
    X_val = X_val.reshape(X_val.shape[0], X_val.shape[1], X_val.shape[2], 1)
    X_test = X_test.reshape(X_test.shape[0], X_test.shape[1], X_test.shape[2], 1)

    return X_train, X_val, X_test, y_train, y_val, y_test

def build_model(input_shape, num_classes):
    model = Sequential()
    model.add(Conv2D(32, (3, 3), activation='relu', input_shape=input_shape))
    model.add(MaxPooling2D((2, 2)))
    model.add(Conv2D(64, (3, 3), activation='relu'))
    model.add(MaxPooling2D((2, 2)))
    model.add(Flatten())
    model.add(Dense(256, activation='relu'))
    model.add(Dense(num_classes, activation='softmax'))
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    return model

def train_model(X_train, X_val, y_train, y_val):
    input_shape = X_train[0].shape
    num_classes = y_train.shape[1]

    model = build_model(input_shape, num_classes)
    model.fit(X_train, y_train, validation_data=(X_val, y_val), epochs=20, batch_size=64)

    return model

def evaluate_model(model, X_test, y_test):
    _, accuracy = model.evaluate(X_test, y_test)
    print("Accuracy: {:.2f}%".format(accuracy * 100))

def save_trained_model(model, model_path):
    model.save(model_path)
    print("Trained model saved successfully.")

def main():
    X_train, X_val, X_test, y_train, y_val, y_test = load_data()
    model = train_model(X_train, X_val, y_train, y_val)
    evaluate_model(model, X_test, y_test)

    save_trained_model(model, "./trained_model.h5")

    sample_images = X_test[:5]  
    sample_labels = y_test[:5]  

    fig, axes = plt.subplots(1, len(sample_images), figsize=(12, 3))

    for i, ax in enumerate(axes):
        ax.imshow(sample_images[i].squeeze(), cmap='gray')
        ax.set_title("Label: {}".format(np.argmax(sample_labels[i])))
        ax.axis('off')

    plt.tight_layout()
    plt.show()

if __name__ == '__main__':
    main()