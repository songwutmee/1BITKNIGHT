<div align="center">

# 1-Bit Knight


<a href="https://www.youtube.com/watch?v=8siNsZ8Pb-A" target="_blank">
  <img src="https://img.youtube.com/vi/8siNsZ8Pb-A/0.jpg" width="100%" alt="1BitKnight Gameplay">
</a>

</div>

### Game Concept
A vertical slice of a boss-rush game featuring minimalist 1-bit visuals and a tight, responsive Souls-like combat loop. The experience centers on pattern recognition, stamina management, and precision timing within a fully polished, repeatable encounter.

### My Work & Technical Approach
As  programmer, I focused on building the game with solid architectural patterns to keep the project organized and easy to work on.

- **Event-Driven System:** I used Scriptable Object events so that different parts of the game (like the Player and the UI) could communicate without being directly tied together. This made it really easy to add new feedback effects like camera shake or damage flashes without touching the character code.

- **State Pattern for AI and Player:** To keep the character logic clean, I used the State Pattern. Every action, like the boss's `ChaseState` or the player's `AttackState`, is its own separate class. This prevented messy `if/else` statements in the `Update()` loop and made the AI's behavior predictable and easy to debug.

- **Data-Driven Design:** All game balance values, from character stats to the skill tree layout, are stored in Scriptable Objects. This was a huge time-saver, as it allowed me to tweak and balance the game entirely in the Unity editor.

- **Shader Troubleshooting:** The game's 1-bit look comes from a custom shader. Getting it to work in the final game build was a challenge, and I learned a lot about solving complex URP compatibility issues that only show up after building.
