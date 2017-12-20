# Space Is The Place
(To test this project out, open ExampleBuilds/Build_1 and play!)

<img src="/Docs/Intro.png"/>
<img src="/Docs/Flanking.png"/>

I created this project to test out my knowledge of model-based agents and AI behavioral systems in games.
The enemy AI system has two top-level behavior styles, followers and leaders.

Leaders have three behaviors, Retreat, Attack, and Search. Followers have four behaviors, Retreat, Attack, Follow, and Search.

All behaviors are created from Unity's Scriptable Object Class and assigned based on the Follower boolean each time an enemy is created.
To see the implementation of model-based agent system, look at the Update function of Assets/_Scripts/EnemyController.cs
