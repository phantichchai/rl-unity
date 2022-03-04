# Battle Agent

![](https://github.com/phantichchai/rl-unity/blob/main/Image/battle-agent-v3.png)

[Link video](https://youtu.be/saH_OosGbnU): Battle Agent v3

## Neural Networks
Algorithm (POCA) 
- Fully connect
- Self-attention
- Self-play

## Description
- Disruptor Agent can do all task but can't win the game a long training time passed
- Collector Agent can do all task as well and win the game always

### **# Observation Input**
For each agent:
- Raycast observation vector for return distance between agent and other object (2*20+1) * (4+1+1)
- Raycast observation vector for jump (2*1+1) * (3+1+1)
- Raycast observation vector for back (2*1+1) * (3+1+1)
- Raycast observation vector for enemie (2*20+1) * (1+1+1)
- Observation vector (17)
  + Float Dot product between vector velocity with forward axis
  + Float Dot product between vector velocity with right axis  
  + boolean agent can jump
  + boolean agent is stun
  + boolean agent dash cooldown
  + Vector3 agent position
  + Vector3 agent enler angles
  + Vector3 item position
  + Vector3 destination position

### **# Action**
Discrete action (13)  
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
  - Dash [one-hot vector size 2] (argmax in one-hot [action])
    - 0 [no action]
    - 1 [dash]
