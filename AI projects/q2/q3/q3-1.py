import os
import cv2
import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import LabelEncoder
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Flatten, Dense
from tensorflow.keras.utils import to_categorical


def load_data():
    images = []
    labels = []
    faces_dir = "./Dataset/Faces/Faces"
    labels_data = pd.read_csv("./Dataset/Dataset.csv")
    label_encoder = LabelEncoder()

    for filename in os.listdir(faces_dir):
        img = cv2.imread(os.path.join(faces_dir, filename))
        if img is not None:
            img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
            img = cv2.resize(img, (32, 32))  
            images.append(img)
            label = labels_data[labels_data["id"] == filename]["label"].values[0]
            labels.append(label)

    labels = label_encoder.fit_transform(labels)
    labels = to_categorical(labels)
    X_train, X_test, y_train, y_test = train_test_split(images, labels, test_size=0.2, random_state=42)
    X_train = np.array(X_train) / 255.0
    X_test = np.array(X_test) / 255.0

    return X_train, X_test, y_train, y_test


def build_model(input_shape, num_classes):
    model = Sequential()
    model.add(Flatten(input_shape=input_shape))
    model.add(Dense(256, activation='relu'))
    model.add(Dense(num_classes, activation='softmax'))
    model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
    return model


def train_model(X_train, X_test, y_train, y_test):
    input_shape = X_train[0].shape
    num_classes = y_train.shape[1]

    model = build_model(input_shape, num_classes)
    model.fit(X_train, y_train, validation_data=(X_test, y_test), epochs=10, batch_size=32)

    return model


def evaluate_model(model, X_test, y_test):
    _, accuracy = model.evaluate(X_test, y_test)
    print("Accuracy: {:.2f}%".format(accuracy * 100))

def main():
    X_train, X_test, y_train, y_test = load_data()
    model = train_model(X_train, X_test, y_train, y_test)
    evaluate_model(model, X_test, y_test)

if __name__ == '__main__':
    main()