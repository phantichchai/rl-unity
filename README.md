# Collect Resource Game with Reinforcement Learning

![](https://github.com/phantichchai/rl-unity/blob/main/Image/v0.1.4.png)

link video agent v0.1.4 https://youtu.be/eSUhizSMniU

## Neural Networks
[CollectAgent-6591537](https://github.com/phantichchai/rl-unity/blob/main/config/CollectAgent.yaml) (PPO) 
- Fully connect
- Curiosity reward
- Curriculum learning while training

### Policy Architecture
![](https://github.com/phantichchai/rl-unity/blob/main/Image/CollectAgent-6591537.png)

## Description
- Agent can solve the problem by full connect neural network with curiosity reward and curriculum learning.
- Agent can collect all 3 items for delivery to destination.

### **# Observation Input**
- Raycast observation vector for return distance between agent and other object (174)  
  :arrow_right: (2 * 14[number of raycast] + 1) * (4[one-hot vector size equal detectable tag] + 1[boolean raycast hit something]  
   \+ 1[float distance between agent and other object])
- Raycast observation vector for detect wall (15)  
  :arrow_right: (2 * 1[number of raycast] + 1) * (3[one-hot vector size equal detectable tag] + 1[boolean raycast hit something]  
   \+ 1[float distance between agent and other object])
- Observation vector (22)  
  + boolean agent can jump
  + Vector3 agent position
  + Vector3 agent enler angles
  + Vector3 agent velocity
  + Vector3 * 3 item position
  + Vector3 destination position

### **# Action**
Discrete action (11)  
  - Move direction Z [one-hot vector size 3] (argmax in one-hot [action])
    - 0 [no action]
    - 1 [forward]
    - 2 [backward]
  - Rotate direction [one-hot vector size 3] (argmax in one-hot [action])
    - 0 [no action]
    - 1 [right rotate]
    - 2 [left rotate]
  - Move direction X [one-hot vector size 3] (argmax in one-hot [action])  
    - 0 [no action]
    - 1 [left]
    - 2 [right]
  - Jump [one-hot vector size 2] (argmax in one-hot [action])
    - 0 [no action]
    - 1 [jump]  

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
