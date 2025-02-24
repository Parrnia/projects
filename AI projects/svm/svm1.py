import pandas as pd
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.svm import SVC
from sklearn.metrics import accuracy_score, roc_curve, auc
import matplotlib.pyplot as plt

data = pd.read_csv('SMSSpamCollection.csv', sep='\t', names=['label', 'message'])

X = data['message']
y = data['label'].map({'ham': 0, 'spam': 1})

tfidf = TfidfVectorizer()
X_tfidf = tfidf.fit_transform(X)


model = SVC(kernel='linear')
model.fit(X_tfidf, y)


y_pred = model.predict(X_tfidf)
accuracy = accuracy_score(y, y_pred)
print(f'Accuracy: {accuracy * 100:.2f}%')

fpr, tpr, thresholds = roc_curve(y, model.decision_function(X_tfidf))
roc_auc = auc(fpr, tpr)

plt.figure()
plt.plot(fpr, tpr, color='darkorange', lw=2, label='ROC curve (area = %0.2f)' % roc_auc)
plt.plot([0, 1], [0, 1], color='navy', lw=2, linestyle='--')
plt.xlim([0.0, 1.0])
plt.ylim([0.0, 1.05])
plt.xlabel('False Positive Rate')
plt.ylabel('True Positive Rate')
plt.title('Receiver Operating Characteristic (ROC) Curve')
plt.legend(loc="lower right")
plt.show()