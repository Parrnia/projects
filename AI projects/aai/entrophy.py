import numpy as np
import pandas as pd
from sklearn.calibration import LabelEncoder
from sklearn.model_selection import train_test_split ,cross_val_score
from sklearn.metrics import accuracy_score
from sklearn.tree import DecisionTreeClassifier, export_graphviz
from sklearn.tree import export_graphviz

class DecisionTree:
    def __init__(self, max_depth=None):
        self.max_depth = max_depth
        self.tree = None

    def entropy(self, y):
        _, counts = np.unique(y, return_counts=True)
        probabilities = counts / len(y)
        entropy = -np.sum(probabilities * np.log2(probabilities))
        return entropy

    def information_gain(self, X, y, feature_index, split_value):
        left_mask = X.iloc[:, feature_index] <= split_value
        right_mask = X.iloc[:, feature_index] > split_value

        left_entropy = self.entropy(y[left_mask])
        right_entropy = self.entropy(y[right_mask])

        left_weight = len(y[left_mask]) / len(y)
        right_weight = len(y[right_mask]) / len(y)

        information_gain = self.entropy(y) - (left_weight * left_entropy + right_weight * right_entropy)
        return information_gain

    def best_split(self, X, y):
        best_feature = None
        best_split_value = None
        best_information_gain = 0

        for feature_index in range(X.shape[1]):
            unique_values = np.unique(X.iloc[:, feature_index])
            for split_value in unique_values:
                information_gain = self.information_gain(X, y, feature_index, split_value)
                if information_gain > best_information_gain:
                    best_information_gain = information_gain
                    best_feature = feature_index
                    best_split_value = split_value

        return best_feature, best_split_value

    def build_tree(self, X, y, depth=0):
        if depth == self.max_depth or len(np.unique(y)) == 1:
            return {'class': np.argmax(np.bincount(y))}

        best_feature, best_split_value = self.best_split(X, y)
        left_mask = X.iloc[:, best_feature] <= best_split_value
        right_mask = X.iloc[:, best_feature] > best_split_value

        left_subtree = self.build_tree(X[left_mask], y[left_mask], depth + 1)
        right_subtree = self.build_tree(X[right_mask], y[right_mask], depth + 1)

        return {'feature': best_feature, 'split_value': best_split_value,
                'left': left_subtree, 'right': right_subtree}
    def fit(self, X, y):
         self.tree = self.build_tree(X, y)

    def predict_instance(self, x, tree):
        if 'class' in tree:
            return tree['class']
        if x.iloc[tree['feature']] <= tree['split_value']:
            return self.predict_instance(x, tree['left'])
        else:
            return self.predict_instance(x, tree['right'])

    def predict(self, X):
        if self.tree is None:
            raise ValueError('The model has not been trained yet.')
        predictions = []
        for _, x in X.iterrows():
            prediction = self.predict_instance(x, self.tree)
            predictions.append(prediction)
        return np.array(predictions)

    def print_tree(self):
      self._print_node(self.tree)

    def _print_node(self, node, indent=""):
       if 'class' in node:
           print(f"{indent}Predicted class: {node['class']}")
       else:
           feature_name = X.columns[node['feature']]
           split_value = node['split_value']
           print(f"{indent}Split on feature '{feature_name}' at value {split_value}:")
           print(f"{indent}|--> Left subtree:")
           self._print_node(node['left'], indent + "    ")
           print(f"{indent}|--> Right subtree:")
           self._print_node(node['right'], indent + "    ")

if __name__ == '__main__':
     data = pd.read_csv("onlinefraud.csv")[['step','nameOrig','oldbalanceOrg','newbalanceOrig','nameDest','amount', 'isFraud', 'oldbalanceDest', 'newbalanceDest']].head(2000).dropna()
     nameOrig_counts = data.groupby('nameOrig')['isFraud'].count()
     grouped = data.groupby('step')['isFraud'].mean()
     print(grouped)
     grouped1 = data.groupby('newbalanceOrig')['isFraud'].mean()
     print(grouped1)
  
     nameDest_counts = data.groupby('nameDest')['nameDest'].count()
     print("Counts for nameOrig:")
     print(nameOrig_counts)
     print("Counts for nameDest:")
     print(nameDest_counts)
     grouped = data.groupby('step')['isFraud'].mean()

     print(grouped)
   
     label_encoder = LabelEncoder()
     data['nameOrig_encoded'] = label_encoder.fit_transform(data['nameOrig'])
     data['nameDest_encoded'] = label_encoder.fit_transform(data['nameDest'])

X = data[[ 'amount' , 'nameOrig_encoded', 'oldbalanceOrg', 'newbalanceOrig', 'oldbalanceDest', 'newbalanceDest']]
y = data['isFraud']

X_train, X_test, y_train, y_test = train_test_split(X.iloc[:2000], y.iloc[:2000], test_size=0.2, random_state=42)


decision_tree = DecisionTree(max_depth=5)
decision_tree.fit(X_train, y_train)

decision_tree.print_tree()

y_pred = decision_tree.predict(X_test)
accuracy = accuracy_score(y_test, y_pred)
print("Accuracy:", accuracy)

decision_tree = DecisionTreeClassifier(max_depth=3, min_samples_split=100)

decision_tree.fit(X_train, y_train)


cv_scores = cross_val_score(decision_tree, X, y, cv=5)

print("Cross-validation scores:", cv_scores)
print("Mean cross-validation score:", cv_scores.mean())
