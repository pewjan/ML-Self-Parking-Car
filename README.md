# ML Self Parking Bus using Unity MLAgents (framework) 

![image](https://user-images.githubusercontent.com/62914735/176587688-edc8fe86-1cb2-482e-82bc-0d111cf2244d.png)

# Technologies
1) Unity3D
2) [MLAgents](https://github.com/Unity-Technologies/ml-agents)
3) C#

# Project OverView

Using Unity and MLAgents, it is a simulation model created to make a vechicle (bus in this instance) park in a given parking space. The reason for the creation was to use Reinforment Learning on a vechicle on a Unity Project. 

-> First Part of the Project was creating an environment using Unity3D which I could apply the reinforcement learning. 

-> Second Part was writing the code in C# and train the model using the RL. 

# Observations
1) Dot Product of Active Parking Position and Bus
2) Bus Position
3) Active Parking Position
4) Small Goals to aid into making the agent proceed into the right area

# Actions
Using Discrete Actions, two array were given
First Array
0) Don't go anywhere
1) Go Forward
2) Go Backwards
Second Array
0) Don't go left or right
1) Go Left
2) Go Right

# Rewards

1) Negative Reward depending on how far car is to the park position
2) Positive Reward if car is looking at the park and if it is rewarded how close it is to the parking space
3) Position Reward for getting the small goals
4) Positive Reward if Car makes it to the parking space

# Inspirations

[Self Parking Car](https://www.youtube.com/watch?v=VMp6pq6_QjI&t=265s)

[Car that stops for pedasterian](https://www.youtube.com/watch?v=7jeXZLVvg-4&t=54s)

# Example of Car Parking


https://user-images.githubusercontent.com/62914735/176595251-46089283-837a-4b26-9503-89e24a04ce93.mp4



https://user-images.githubusercontent.com/62914735/176613384-b2a93097-5711-4d3f-94f9-a41ed5a3b7be.mp4










