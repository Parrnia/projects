import os


class DeepDSLCodeGenerator:
    generated_rules = []
    def __init__(self):
        self.non_operands = [
            'layer',
            'activation',
            'input_shape',
            'training',
            'dataset',
            'visualize',
            'evaluate',
            'metric_choice',
        ]

        self.operand_stack = []
        self.code_stack = []
        self.shape_stack = []
        self.aux_stack = []

    def is_operand(self, item):
        return item not in self.non_operands

    @staticmethod
    def generate_initial(self):
        imports = "import tensorflow as tf\n" + \
                  "import numpy as np\n\n\n"

        class_def = f"class NewNetwork:\n" + \
                    f"\tdef __init__(self):\n" + \
                    f"\t\tself.model = tf.keras.Sequential()\n"
        return imports + class_def
    @staticmethod
    def generate_instance(self):
        code_string = ("\nif __name__ == \"__main__\":"
                       "\n\timport tensorflow as tf"
                       "\n\timport numpy as np"
                       "\n\tnetwork = NewNetwork()\n")
        if "generate_dataset" in self.generated_rules:
            code_string += "\tx_train, x_test, y_train, y_test = network.generate_dataset()\n"
        else:
             code_string += "\t(x_train, y_train), (x_test, y_test) = tf.keras.datasets.mnist.load_data()\n"
        if "compile_model" in self.generated_rules:
            code_string += "\tnetwork.compile_model()\n"
        if "train_model" in self.generated_rules:
            code_string += "\tnetwork.train_model(x_train, y_train, x_test, y_test)\n"
        if "evaluate_model" in self.generated_rules:
            code_string += "\tnetwork.evaluate_model(x_test, y_test)\n"

        return code_string

    def generate_code(self, post_order_array):
        for item in post_order_array:
            label = item["label"]
            if not self.is_operand(label):
                self.generate_code_based_on_non_operand(label)
            else:
                self.operand_stack.append(label)

        result = ''
        for code in self.code_stack:
            if code is not None:
                result += code

        output_file = os.path.join('output', 'generated_output.py')
        with open(output_file, "w") as f:
            f.write(self.generate_initial(self))
            f.write(result)
            f.write(self.generate_instance(self))
        return result

    def generate_code_based_on_non_operand(self, item):
        if item == "layer":
            self.generate_layer()
        elif item == "activation":
            self.generate_activation()
        elif item == "input_shape":
            self.generate_input_shape()
        elif item == "training":
            self.generate_training()
        elif item == "metric_choice":
            self.generate_metric_choice()
        elif item == "evaluate":
            self.generate_evaluate()
        elif item == "visualize":
            self.generate_visualize()
        elif item == "begin_scope_operator":
            self.generate_begin_scope_operator()
        elif item == "end_scope_operator":
            self.generate_end_scope_operator()
        elif item == "dataset":
            self.generate_dataset()

    def generate_input_shape(self):
        y = self.operand_stack.pop()
        x = self.operand_stack.pop()
        code_string = f"input_shape=({x}, {y})"
        self.aux_stack.append(("input_shape", code_string))

    def generate_layer(self):
        layer_units = self.operand_stack.pop()
        layer_type = self.operand_stack.pop()

        layer_input_shape = ''
        if len(self.aux_stack) > 0 and self.aux_stack[-1][0] == "input_shape":
            layer_input_shape = f', {self.aux_stack.pop()[1]}'

        layer_activation = ''
        if len(self.aux_stack) > 0 and self.aux_stack[-1][0] == "activation":
            layer_activation = f', {self.aux_stack.pop()[1]}'

        layer_def = f"\t\tself.model.add(tf.keras.layers.{layer_type}(units={layer_units}{layer_activation}{layer_input_shape}))\n"
        self.code_stack.append(layer_def)

    def generate_activation(self):
        activation = self.operand_stack.pop()
        code_string = f"activation=\'{activation}\'"
        self.aux_stack.append(("activation", code_string))

    def generate_training(self):
        self.generated_rules.append("compile_model")
        self.generated_rules.append("train_model")

        validation_split = self.operand_stack.pop()
        batch_size = self.operand_stack.pop()
        epochs = self.operand_stack.pop()

        metrics = ''
        if len(self.aux_stack) > 0 and self.aux_stack[-1][0] == "metric_choice":
            metrics = self.aux_stack.pop()[1]

        loss = self.operand_stack.pop()
        optimizer = self.operand_stack.pop()

        code_string = "\n\tdef compile_model(self):\n" + \
                      f"\t\tself.model.compile(optimizer=\'{optimizer}\',\n" + \
                      f"\t\t\tloss=\'{loss}\',\n" + \
                      f"\t\t\t{metrics})\n"

        code_string += "\n\tdef train_model(self, x_train, y_train, x_val, y_val):\n" + \
            "\t\tself.model.fit(\n" + \
            "\t\t\tx_train, y_train,\n" + \
            "\t\t\tvalidation_data=(x_val, y_val),\n" + \
            f"\t\t\tepochs={epochs},\n" + \
            f"\t\t\tbatch_size={batch_size}\n" + \
            "\t\t)\n"

        self.code_stack.append(code_string)

    def generate_metric_choice(self):
        metrics = []
        current_code = self.code_stack.pop()
        if current_code != '##COMPILER_PARAM:::scope:::end_scope_operator':
            self.code_stack.append(current_code)
            return
        while current_code != '##COMPILER_PARAM:::scope:::begin_scope_operator':
            metrics.append(self.aux_stack.pop())
            current_code = self.code_stack.pop()
        code_string = f"metrics = ["
        for metric in metrics:
            code_string += f"\'{metric}\',"
        code_string = code_string[:-1] + ']'
        self.aux_stack.append(("metric_choice", code_string))

    def generate_evaluate(self):
        metrics = ''
        self.generated_rules.append("evaluate_model")
        if len(self.aux_stack) > 0 and self.aux_stack[-1][0] == "metric_choice":
            metrics = self.aux_stack.pop()[1]

        code_string = "\n\tdef evaluate_model(self, x_test, y_test):\n" + \
                      "\t\tresults = self.model.evaluate(x_test, y_test, verbose=0)\n" + \
                      "\t\tprint(f\"Evaluation results:\\n {results}\")\n"
        self.code_stack.append(code_string)

    def generate_visualize(self):
        y = self.operand_stack.pop()
        x = self.operand_stack.pop()

        code_string = "\n\tdef visualize_model(self, x, y):\n" + \
                      "\t\tprint(f\"visualizing model on a {x} * {y} grid.\")\n"

        self.code_stack.append(code_string)

    def generate_dataset(self):
        self.generated_rules.append("generate_dataset")
        preprocessing = self.operand_stack.pop()
        source = self.operand_stack.pop()
        code_string = "\n\tdef generate_dataset(self):\n"
        if source:
            code_string += f"\t\t(x_train, y_train), (x_test, y_test) = tf.keras.datasets.mnist.load_data()"
        else:
            code_string += f"\t\tdata = pd.read_csv({source})" +\
                            "\n\t\tx_train, x_train, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)"
        code_string += "\n\t\treturn x_train, x_train, y_train, y_test \n"
        self.code_stack.append(code_string)



    def generate_begin_scope_operator(self):
        self.code_stack.append("##COMPILER_PARAM:::scope:::begin_scope_operator")

    def generate_end_scope_operator(self):
        self.code_stack.append("##COMPILER_PARAM:::scope:::end_scope_operator")


