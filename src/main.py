from mlagents_envs.base_env import ActionSpec
import torch.onnx
import torch
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
from trainers.policy import NeuralNetwork

channel = EngineConfigurationChannel()

unity_env = UnityEnvironment(file_name=None, seed=1, side_channels=[channel])

channel.set_configuration_parameters(time_scale = 2.0)
unity_env.reset()

behavior_name = list(unity_env.behavior_specs)[0]
print(f"Name of the behavior : {behavior_name}")

behavior_spec = unity_env.behavior_specs[behavior_name]
print(f"Number of observations : {len(behavior_spec.observation_specs)}")

if behavior_spec.action_spec.is_continuous():
    print("The action is continuous")

if behavior_spec.action_spec.is_discrete():
    print("The action is discrete")

device = 'cuda' if torch.cuda.is_available() else 'cpu'
model = NeuralNetwork()

num_episode = 1
max_step = 1
for episode in range(num_episode):
    unity_env.reset()
    decision_steps, terminal_steps = unity_env.get_steps(behavior_name)
    step = -1
    done = False
    episode_reward = 0
    
    for step in range(max_step):
        
        action = behavior_spec.action_spec.random_action(len(decision_steps))

        input1 = torch.Tensor(decision_steps.obs[0])
        input2 = torch.Tensor(decision_steps.obs[1])
        input3 = torch.Tensor(decision_steps.obs[2])

        print(f"action input1 dim: {input1.shape}")
        print(f"action input2 dim: {input2.shape}")
        print(f"action input3 dim: {input3.shape}")

        pred = model.forward(input1, input2, input3)
        print(f"forward: {pred}")

        unity_env.set_actions(behavior_name, action)
        unity_env.step()
        
    print(f"Total rewards for episode {episode} is {episode_reward}")


decision_steps, terminal_steps = unity_env.get_steps(behavior_name)
input1 = torch.Tensor(decision_steps.obs[0])
input2 = torch.Tensor(decision_steps.obs[1])
input3 = torch.Tensor(decision_steps.obs[2])

torch.onnx.export(model, (input1, input2, input3), "neuralnetwork.onnx")


unity_env.close()
print("Closed environment")
