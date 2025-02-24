# Generated from D:/University/Compiler Design/git/IUST-CompilerDesign-Project/Grammar/deepDSL.g4 by ANTLR 4.13.2
from antlr4 import *
if "." in __name__:
    from .deepDSLParser import deepDSLParser
else:
    from deepDSLParser import deepDSLParser

# This class defines a complete generic visitor for a parse tree produced by deepDSLParser.

class deepDSLVisitor(ParseTreeVisitor):

    # Visit a parse tree produced by deepDSLParser#start.
    def visitStart(self, ctx:deepDSLParser.StartContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#network.
    def visitNetwork(self, ctx:deepDSLParser.NetworkContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#layer.
    def visitLayer(self, ctx:deepDSLParser.LayerContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#training.
    def visitTraining(self, ctx:deepDSLParser.TrainingContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#dataset.
    def visitDataset(self, ctx:deepDSLParser.DatasetContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#visualize.
    def visitVisualize(self, ctx:deepDSLParser.VisualizeContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#evaluate.
    def visitEvaluate(self, ctx:deepDSLParser.EvaluateContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#optimizer.
    def visitOptimizer(self, ctx:deepDSLParser.OptimizerContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#optimizer_func.
    def visitOptimizer_func(self, ctx:deepDSLParser.Optimizer_funcContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#loss.
    def visitLoss(self, ctx:deepDSLParser.LossContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#loss_func.
    def visitLoss_func(self, ctx:deepDSLParser.Loss_funcContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#metric_choice.
    def visitMetric_choice(self, ctx:deepDSLParser.Metric_choiceContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#metrics.
    def visitMetrics(self, ctx:deepDSLParser.MetricsContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#epochs.
    def visitEpochs(self, ctx:deepDSLParser.EpochsContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#batch_size.
    def visitBatch_size(self, ctx:deepDSLParser.Batch_sizeContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#validation_split.
    def visitValidation_split(self, ctx:deepDSLParser.Validation_splitContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#source.
    def visitSource(self, ctx:deepDSLParser.SourceContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#preprocessing.
    def visitPreprocessing(self, ctx:deepDSLParser.PreprocessingContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#preprocessing_func.
    def visitPreprocessing_func(self, ctx:deepDSLParser.Preprocessing_funcContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#types.
    def visitTypes(self, ctx:deepDSLParser.TypesContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#units.
    def visitUnits(self, ctx:deepDSLParser.UnitsContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#activation.
    def visitActivation(self, ctx:deepDSLParser.ActivationContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#activation_func.
    def visitActivation_func(self, ctx:deepDSLParser.Activation_funcContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#input_shape.
    def visitInput_shape(self, ctx:deepDSLParser.Input_shapeContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#value.
    def visitValue(self, ctx:deepDSLParser.ValueContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#path.
    def visitPath(self, ctx:deepDSLParser.PathContext):
        return self.visitChildren(ctx)


    # Visit a parse tree produced by deepDSLParser#type.
    def visitType(self, ctx:deepDSLParser.TypeContext):
        return self.visitChildren(ctx)



del deepDSLParser