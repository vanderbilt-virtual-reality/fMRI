# fmriexperience
fMRI Virtual Reality Experience Built on Unity for Oculus

## Description and Functions
Our project is a virtual representation of a person's brain and its activity as the person listened to an audio stimulus. We were given fMRI data from Dr. Chang's lab representing fMRI signals over the course of time.  We used that data to calculate changes in brain region connections over the course of the audio stimulus. Each sphere in the experience represents a different region of the brain, and the chords that appear represent connections between different regions of the brain. The chords will grow, shrink, and change color based on how strong the connection is. The music that the user hears in the headset is the same music that the person heard as their brain responded the way it did.User Instructions:
- To see what region of the brain a particular orb represents, point to the orb with the laser pointer connected to the right hand.
- Move forward, left, right, and backwards with the right joystick. You will have to repeatedly push it, not hold it down.
- You can enter the brain- that's the point!
- Use the A, B, X, and Y buttons to instantly transport to different outer views of the brain.
- Use the right grip button to move up in the scene and use the left grip button to move down in the scene.
- To make the laser change color so it is easier to see, hold down the right trigger. The right trigger will also toggle the text with information about the brain region in question on or off.
- The brain diagram at the bottom of the scene should help the user orient themselves if they ever get confused inside the brain.

## Location of Files
- Python Preprocessing files can be found in the pysrc directory
- Everything associated with the unity project is in fMRI Environment
- The current directory is the folder where all the executable builds can be found
- fMRI_Environment is the executable file