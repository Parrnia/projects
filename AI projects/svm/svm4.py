import numpy as np
from sklearn.datasets import fetch_20newsgroups
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.svm import LinearSVC
from sklearn.metrics import accuracy_score
import matplotlib.pyplot as plt
from sklearn.metrics import classification_report, ConfusionMatrixDisplay

newsgroups_train = fetch_20newsgroups(subset='train')
newsgroups_test = fetch_20newsgroups(subset='test')

tfidf_vectorizer = TfidfVectorizer()
X_train = tfidf_vectorizer.fit_transform(newsgroups_train.data)
X_test = tfidf_vectorizer.transform(newsgroups_test.data)
y_train = newsgroups_train.target
y_test = newsgroups_test.target

clf = LinearSVC()
clf.fit(X_train, y_train)

y_pred = clf.predict(X_test)
accuracy = accuracy_score(y_test, y_pred)
print(f"Accuracy: {accuracy:.2f}")

cm = ConfusionMatrixDisplay.from_estimator(clf, X_test, y_test)
plt.title("Confusion Matrix")
plt.show()

print(classification_report(y_test, y_pred, target_names=newsgroups_test.target_names))

plt.figure(figsize=(12, 8))
plt.title("Classification Report")
plt.text(0.01, 1.25, str('Classification Report'), {
    'fontsize': 10}, fontproperties='monospace')
plt.text(0.01, 0.05, str(classification_report(y_test, y_pred, target_names=newsgroups_test.target_names)), {
    'fontsize': 10}, fontproperties='monospace')
plt.axis('off')
plt.tight_layout()
plt.show()