<h1 align="center">ARGUMENT GAME <i>[DISCONTINUED]</i></h1>
<h4 align="center">using C# Unity <br>by Marek Krasiński<br>ONLY IN POLISH LANGUAGE</h4>
<h3 align="center"><i>Game in which you need your logic skills to progress the story</i> --- inspired by DANGANRONPA V3</h3>

<b align="center">DISCLAIMER: Some of the assets used in this project are not of my authorship, and I do not possess full copyright. These assets have been used solely for narrative and demonstrative purposes. All copyrights belong to their rightful owners. If you are the copyright owner of any of the used assets and wish for them to be removed or appropriately identified, please get in touch with me. </b> 

<br>
<p>
  The game consists of two scenes repeatedly switching with different narratives every time:
</p>

* Argument
* DialogBox

<h1>Argument Scene</h1>
<div align="center">
  <img src="https://github.com/MroseQ/Argument-Game/assets/46853552/c046943c-ff6e-4c54-abfd-64f9148908dc/argument1" width="80%"><br>
</div>

<h2>UI Elements</h2>

* <b> Audio Settings </b> (upper left corner) - changing the position on a slider results in a lower or higher volume of music or character sounds.
* <b> Health </b> (lower left corner) - information about how many tries the player has left.
* <b> Progress Nodes </b> (lower right corner) - information about which chunk of an argument is currently playing.

<h2>Player Capabilities</h2>

* changing the value of Audio Settings
* pressing <b>any ctrl button</b> will speed up the dialogues.
* clicking on <b>any yellow text</b> will start a checking sequence of whether the clicked text is a lie.

<h2>How the Scene plays out</h2>

1. All chunks of a specific plot-related part are loaded.
2. The first chunk starts playing (showing the speaker, typewriting the spoken text, and playing the speaker's voice) representing three different dialogue topics on different screen parts.
3. After a specific time without player interaction the chunk will disappear (characters and texts will fade out) while starting the next chunk.
4. If every chunk has finished playing, the game retracts to the first chunk and repeats the process until the player progresses or loses. 
5. If the player clicks a false yellow text block (not a lie), the game will push the player to the first chunk of an argument and lower the player's health number (lowering the number to 0 results in the player losing the game).
6. If the player clicks on a true yellow text block (a lie), the game will start a changing scene sequence resulting in a switch to a <b>"DialogBox"</b> scene.

<div align="center">
  <img src="https://github.com/MroseQ/Argument-Game/assets/46853552/86d16bcb-8594-4790-b00c-8938245c47f5/argument2" width="80%"><br>
</div>

<p align="center"><i> The visualization of a changing sequence </i></p>


<br>

<h1>DialogBox Scene</h1>
<div align="center">
  <img src="https://github.com/MroseQ/Argument-Game/assets/46853552/0060a25e-5ad6-4914-9aa7-d732c15a527d/argument3" width="80%"><br>
</div>

<h2>UI Elements</h2>

* <b> Audio Settings </b> (upper left corner NOT VISIBLE ON SCREEN) - changing the position on a slider results in a lower or higher volume of music or character sounds.
* <b> DialogBox </b> (lower center part) - representing the speaker sprite, speaker name, and spoken text.

<h2>Player Capabilities</h2>

* changing the value of Audio Settings
* clicking on the screen after the typewritten text has finished its animation will change to another dialogue line or start changing to <b>"Argument"</b> scene.

<h2>How the Scene plays out</h2>

1. Loading the first dialogue line.
2. Awaiting the player interaction.
3. The game iteratively presents dialogue lines, pausing to await player interaction, until all intended dialogue lines for this part of the story are completed.
4. Changing to the scene.

<h3 align="center">The game's code was created solely by myself.</h3>

<h2 align="center">Game and project view available on request.</h2>
