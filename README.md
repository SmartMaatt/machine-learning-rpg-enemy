<h1 align="center">Enemy AI system for cRPG games using Machine Learning</h1>

<p align="center">
  <a href="#overview">Overview</a> •
  <a href="#machine-learning-implementation">Implementation</a> •
  <a href="#screenshots">Screenshots</a> •
  <a href="#unity-3d-application-project">Application Project</a> •
  <a href="#launcher-project">Launcher</a> •
  <a href="#developer-requirements">Requirements</a> •
  <a href="#application-build">Build</a> •
  <a href="#license">License</a>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/License-MIT-yellow.svg" />
  <img src="https://img.shields.io/badge/Author-SmartMatt-blue" />
</p>

## Overview
This project aims to design and implement an AI system for antagonists in cRPG games using the Unity 3D engine, with a focus on machine learning technologies. A key feature of this project is the use of the Unity MLAgents plugin, which facilitates the integration of advanced machine learning techniques within the Unity environment. The priority is to ensure that enemy behaviors and abilities adapt to their environment, situational context, or the player's skill level. Additionally, the system is designed to evolve through experience gained in combat against the player or other enemies, enhancing its predictive abilities, decision-making, and action selection in response to various scenarios. The system learns through successive interactions with players, thereby improving its combat skills and responses, ultimately presenting an increasing challenge to the player.

## Machine Learning Implementation
Two types of Machine Learning are employed in this project:

1. **Behavioral Tree:** Used for decision-making and action execution based on the current state of the environment surrounding the non-playable character (NPC). Responses include following the player when in sight, fleeing to safety when health is critically low, and roaming freely in the absence of nearby enemies.

2. **Reinforcement Learning:** This approach provides the enemy AI with numerous observations about its current state and the surrounding environment, as well as the state machine values of the enemy or player. Based on this information, the model decides on actions such as casting spells or refraining from action. Actions are evaluated, and the accumulated points in each episode are used to optimize decision-making in subsequent encounters.

## Screenshots
![Battle](https://smartmatt.pl/github/enemy-ai/battle.png)
*Presentation of a battle between two learning bots.*

![Behavior tree](https://smartmatt.pl/github/enemy-ai/behaviour-tree.png)
*The behavioural tree system, which is used by bots as a support for machine learning.*

## Unity 3D Application Project
- **Unity Version:** The project is developed in Unity 2020.3.19f1 with the integration of the MLAgents plugin.
- **Project Location:** The content can be found in the folder `EnemyAI - Unity project`.

## Launcher Project
- **Development Language:** C# in the "Windows Forms Application" template.
- **Project Location:** The content can be found in the folder `EnemyAI- Launcher`.

## Developer Requirements
- Python version 3.6 or 3.7
- Installation of Python plugins from the file `EnemyAI - Unity project/requirements.txt`
- Unity 3D version 2020.3.19f1 or later, with MLAgents plugin installed

## Application Build
The build generation procedure is complex. Access to the latest available version can be found at the following link:
[Enemy AI Build](https://smartmatt.pl/enemy-ai-build)

### Build Requirements
Python version 3.6 or 3.7 (Python is only necessary in training modes, the application allows gameplay mode without this requirement.)

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---
&copy; 2023 Mateusz Płonka (SmartMatt). All rights reserved.
<a href="https://smartmatt.pl/">
    <img src="https://smartmatt.pl/github/smartmatt-logo.png" title="SmartMatt Logo" align="right" width="60" />
</a>

<p align="left">
  <a href="https://smartmatt.pl/">Portfolio</a> •
  <a href="https://github.com/SmartMaatt">GitHub</a> •
  <a href="https://www.linkedin.com/in/mateusz-p%C5%82onka-328a48214/">LinkedIn</a> •
  <a href="https://www.youtube.com/user/SmartHDesigner">YouTube</a> •
  <a href="https://www.tiktok.com/@smartmaatt">TikTok</a>
</p>
