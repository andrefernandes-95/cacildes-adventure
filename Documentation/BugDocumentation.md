04/04/2025

Spent an hour debugging AIPursueTargetState because AI would spin around player or run towards infinite. After several checks, I noticed
I had the warnings disabled and when I enabled them, I had a damage animation named wrong -> "Hit Left" was "Hit Leg". This caused an animation
state bug and the animator would break, making the character buggy and completely ignore the player.
