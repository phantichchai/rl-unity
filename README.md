# Collect Resource Game with Reinforcement Learning

![](https://github.com/phantichchai/rl-unity/blob/main/Image/v0.1.4.png)

link video agent v0.1.5 https://youtu.be/2VNZBbtnHtU

## Neural Networks
[Collect(Collector/Disruptor)Agent-24020000](https://github.com/phantichchai/rl-unity/blob/main/config/CollectAgent.yaml) (PPO) 
- Fully connect
- Multihead attention
- Curiosity reward

### Policy Architecture
![](https://github.com/phantichchai/rl-unity/blob/main/Image/polict-network.png)

## Description
- Disruptor Agent can't do task as well. 
- Collect Agent can collect all 3 items but can't delivery item.
- Swap step is too small cause updating each training round is not good enough.

### **# Observation Input**
- Raycast observation vector for return distance between agent and other object (174)  
  :arrow_right: (2 * 20[number of raycast] + 1) * (4[one-hot vector size equal detectable tag] + 1[boolean raycast hit something]  
   \+ 1[float distance between agent and other object])
- Raycast observation vector for return distance between agent and enemy (174)  
  :arrow_right: (2 * 20[number of raycast] + 1) * (1[one-hot vector size equal detectable tag] + 1[boolean raycast hit something]  
   \+ 1[float distance between agent and other object])
- Raycast observation vector for backward agent (15)  
  :arrow_right: (2 * 1[number of raycast] + 1) * (3[one-hot vector size equal detectable tag] + 1[boolean raycast hit something]  
   \+ 1[float distance between agent and other object])
- Raycast observation vector for jumping (15)  
  :arrow_right: (2 * 1[number of raycast] + 1) * (3[one-hot vector size equal detectable tag] + 1[boolean raycast hit something]  
   \+ 1[float distance between agent and other object])
- Observation vector (15)  
  + boolean agent can jump
  + Vector3 agent position
  + Vector3 agent enler angles
  + Float dot product between forward velocity with forward axis
  + Float dot product between right velocity with right axis
  + Vector3 destination position
  + Float agent dash cooldown
  + boolean agent is stun
  + int number of items

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

## Research
[Battle Agent](https://github.com/phantichchai/rl-unity/blob/main/doc/BattleAgent.md)
