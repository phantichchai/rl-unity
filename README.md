# Collect Resource Game with Reinforcement Learning

![](https://github.com/phantichchai/rl-unity/blob/main/Image/v0.1.3.png)

## Neural Networks
CollectAgent-4550370 (PPO) 
- Fully connect then lstm

## Description
### **# Problem**
Agent can't solve the problem by full connect neural network then lstm

Agent can keep one item and delivery to destination but environment have 3 items for keep

### **# Addition**
Add detection sensor to a lidar like around agent number of ray cast is 14.

Add more observation vector example local position, euler angles, velocity of agent and box local postion, velocity.

Add jump sensor.

## Behaviors
CollectAgent.yaml :arrow_right: CollectAgent behavior

## Game System Environment
![](https://github.com/phantichchai/rl-unity/blob/main/Image/environment.png)

**In Scence:**
- Fields
- Boxs
- Walls
- Elevator
- Items
- Collector Agent
- Disruptor Agent
