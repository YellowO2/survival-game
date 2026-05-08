I was initially implement a mini game thats somewhat outlined as the follows:
```
A turn-based, physics-puzzle arcade game where the player uses pool-like shooting mechanics to trigger massive same-color chain reactions, surviving an endlessly filling board.
Core Gameplay Loop
Aim & Shoot: The player aims and fires the Player Ball into a crowd of enemy balls.
Color Switching: The Player Ball alternates between Red, Green, and Blue. It will ONLY damage and trigger chain reactions with enemies of the matching color! This helps focus your strategy each turn.
Chain Reactions (The Hook): Enemies have 1+ HP. A direct hit from the matching Player Ball deals 1 damage, and knocking same-color balls into each other causes bonus damage and physics knockback.
Resolve & Spawn: Physics settle, dead enemies are destroyed, and a new "clump" of same-colored enemies spawns on the table.
Survive: Repeat to rack up the highest score possible before the table gets too crowded.
Setup & Flow
The Break: Every game starts with balls in a fixed shape (like a pool triangle) with randomized colors.
Endless Spawns: New enemies spawn in clusters every turn so the player always has "color puzzle" targets to aim at.
Special Balls: Bomb Balls and Spike Balls exist to create massive, satisfying board-clears.
The Stakes (Win/Loss)
Game Over Condition: If the total number of enemies on the board exceeds the maxEnemiesAllowed (e.g., 25-30 balls), the player loses.
Progression: No upgrade menus. As the player's score increases, the game auto-scales (e.g., maxEnemiesAllowed increases, or player strike power increases).
```
BUT THEN I TRIED IMPLEMENTING THE ABOVE BUT REALISE IT ISNT FUN, BECAUSE THERE IS NO SENSE OF GROWTH, OR NOT ENOUGH GROWTH AND PRESSURE FOR AN ENDLESS GAME TO BE FUN. The above idea would only maybe work if it was modified to be more realtime/stressful so not turn based, or made to be level and puzzle like.

Hence i am giving up the idea. I have thought of a new idea that would be the best state of action for me, such that i might be able to acheive a playable game mechanic without too much changes. My initial ideas is as follows:

- There are only player ball, black balls(i guess can be seen as enemy) and white balls. They all have associated hitpoints.
- The player can hit the white ball and make the white ball bounce, similar to a pool ball, which is same as current ball hitting mechanic. If player hit black balls, they will not have physics collision but just like "touch" it and then both parties get min(hp of both) damange. If player health < 0, die. Obviously the black ball that got hit will dissapear too.
- If the white ball hit by player touches another white ball, they will merge to be a next tier white ball(wperhaps just a ball with more hitpoints). Currently im not sure if tier 1 ball can only merge with tier 1, or should it be like tier X can mix with tier Y? give advice. Im also thinking perhaps the act of merging will reward the player with 1 hp too, dk if that makes sense. 
- If white ball hits the black ball, and they are of a lower tier, they get swallowed up. And the black ball somehow becomes stronger. Just i dont have idea what is a good way of stronger, similar to white balls merging i also dk whats good for "stronger". I mean i can only think of hitpoint and tier maybe but idk how to use them concretely.
- Then player loses either when black balls are strong to certain conditions, or maybe just like player hitpoint < 0.


