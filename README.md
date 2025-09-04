# Card-Match Game Prototype

## Overview
This is a fully functional card-match game prototype developed in Unity. The project features smooth gameplay with animated card flips, multiple difficulty levels with dedicated buttons, scoring and combo mechanics, and a save/load system. Audio feedback enhances the user experience for both gameplay and UI actions.

## Features
- Animated card flipping and matching
- Multiple difficulty levels: Easy, Medium, Hard, Insane (via buttons)
- Scoring system with combo multiplier
- Save and Load game progress
- UI displaying score, combo, and pairs
- Win panel with final score
- Audio feedback for card flips, matches, mismatches, game over, save/load, new game, and difficulty changes
- Landscape mode optimized for Android
- Smooth performance and bug-free gameplay

## Installation & Play
1. Clone or download the repository from GitHub.  
2. Open the project in **Unity 2021 LTS**.  
3. Open the scene `MainScene` (or your main playable scene).  
4. Press **Play** in the Unity Editor or build for Android/iOS.  

### Android Build
- Landscape orientation enabled.  
- Canvas scales automatically for different screen sizes.  
- Tested on an Android device; graphics and audio perform correctly.  

## Controls
- **Tap** a card to flip it.  
- **Restart / New Game** to reset progress.  
- **Save / Load** to persist game state.  
- **Difficulty Buttons**: click Easy, Medium, Hard, or Insane to change difficulty and play the corresponding sound.  

## Notes
- AudioManager singleton handles all sound effects.  
- SaveSystem persists matched cards and score between sessions.  
- Project is fully tested; no errors or warnings in the console.  
