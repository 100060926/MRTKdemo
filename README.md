HoloLens AR Escape Room – MRTK 3 Project
========================================
A Mixed Reality experience developed using Unity and MRTK 3 for Microsoft HoloLens.

This project is an interactive escape room set in a virtual office, leveraging the advanced input and spatial features of the HoloLens device.

Overview
--------
- Built with Unity and Mixed Reality Toolkit (MRTK 3)
- Explores advanced MR features like gaze, hand gestures, spatial mapping, spatial audio
- Developed as part of a class assignment for learning immersive AR design


Game Summary
------------
- Setting: Virtual office with cement walls, shelves, desks, and a talking drone
- Objective: Solve piano puzzles using clues from hidden music sheets
- Core mechanics:
  - Find 3 music sheets placed around the room
  - Return to the piano and play the correct sequence of notes from each sheet
  - Receive one digit of the final lock code after each correct sequence
  - Input the full code into the keypad to win the game

Key Features Used
-----------------
- Spatial Audio Sound Emitter (directional audio cues)
- PressableButton scripts (for keypad and piano)
- Object Manipulator (for grabbing/interacting with sheets)
- "Follow Me" behavior (for UI visibility)
- Voice commands: "Show" / "Hide" to control panel visibility
- Custom scripts for:
  - Piano note detection
  - Interaction logic with gaze and pinch only

Interaction Highlights
----------------------
- Gaze + Pinch: Natural and immersive input method for selecting piano keys
- Spatial Audio: Helps guide players to clues using 3D sound direction
- Voice Feedback: Verbal encouragement from the drone after each puzzle
- Sequence Tracking: Note counter and real-time feedback (Try Again / Correct)
- Immersive flow: Prevents puzzle skipping and ensures proper sequence

Challenges Faced
----------------
- Gaze direction issues and ray misalignment
- Pressable buttons needing InputAdapter fixes
- Collider issues with interactive elements (e.g., box triggers on padlock)
- Testing fatigue due to frequent headset use
- Long setup and build cycles per test


