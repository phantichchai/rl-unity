# Battle Agent

![](https://github.com/phantichchai/rl-unity/blob/main/Image/battle-agent-v1.png)

[Link video](https://youtu.be/MvmCw2683hg): Battle Agent v1

## Neural Networks
Algorithm (POCA) 
- Fully connect
- Self-attention
- Self-play

## Description
- Fail for first training agent can't do anything.

### **# Observation Input**
For each agent:
- Raycast observation vector for return distance between agent and other object (21)
- Raycast observation vector for Jump (8)
- Raycast observation vector for enemie (22)
- Observation vector (16)  
  + boolean agent can jump
  + Vector3 agent position
  + Vector3 agent enler angles
  + Vector3 agent velocity
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
