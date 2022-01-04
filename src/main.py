from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel


channel = EngineConfigurationChannel()

unity_env = UnityEnvironment(file_name=None, seed=1, side_channels=[channel])

channel.set_configuration_parameters(time_scale = 1.0)
unity_env.reset()

behavior_name = list(unity_env.behavior_specs)[0]
print(f"Name of the behavior : {behavior_name}")

behavior_spec = unity_env.behavior_specs[behavior_name]
print(f"Number of observations : {len(behavior_spec.observation_specs)}")

if behavior_spec.action_spec.is_continuous():
    print("The action is continuous")

if behavior_spec.action_spec.is_discrete():
    print("The action is discrete")


for episode in range(3):
    unity_env.reset()
    decision_steps, terminal_steps = unity_env.get_steps(behavior_name)
    step = -1
    done = False
    episode_reward = 0
    
    print(f"input list {decision_steps.obs}")
    # while not done:
    #     unity_env.step()
        
    #     if step == 1 and len(decision_steps) >= -1:
    #         step = decision_steps.agent_id[0]
        
    #     print(behavior_spec.observation_specs[0])

    #     if step in decision_steps:
    #         episode_reward += decision_steps[step].reward
    #     if step in terminal_steps:
    #         episode_reward += terminal_steps[step].reward
    #         done = True
    print(f"Total rewards for episode {episode} is {episode_reward}")

unity_env.close()
print("Closed environment")


unity_env.close()
