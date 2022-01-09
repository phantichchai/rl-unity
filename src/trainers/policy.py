from mlagents.trainers.torch.layers import LayerNorm, LinearEncoder, linear_layer, lstm_layer
from torch import nn
from mlagents.trainers.torch.attention import EntityEmbedding, ResidualSelfAttention
import torch



class NeuralNetwork(nn.Module):
    def __init__(self):
        super(NeuralNetwork, self).__init__()
        self.fc_input1 = linear_layer(174, 128)
        self.fc_input2 = linear_layer(15, 128)
        self.fc_input3 = linear_layer(22, 128)
        self.entity_embed = EntityEmbedding(128, 128, 128)
        self.residual = ResidualSelfAttention(16, num_heads=2)
        self.dense = LinearEncoder(256, 1, 256)
        self.lstm = lstm_layer(256, 256)

    def forward(self, input1, input2, input3):
        fc_input1 = self.fc_input1(input1)
        fc_input2 = self.fc_input2(input2)
        fc_input3 = self.fc_input3(input3)
        entity_embed1 = self.entity_embed(fc_input1, fc_input2)
        entity_embed2 = self.entity_embed(fc_input1, fc_input3)
        concate = torch.cat([entity_embed1, entity_embed2]).reshape(16, 16)
        residual = self.residual(concate, [entity_embed1, entity_embed2]).reshape(1, -1)
        dense = self.dense(residual)
        lstm = self.lstm(dense.reshape(1, 1, 256))
        return lstm




if __name__ == "__main__":
    # python src/main.py
    # python src/trainers/policy.py
    model = NeuralNetwork()

    print(model)
