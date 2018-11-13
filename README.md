# Neck 'n' Neck

A 2 player giraffe fighting game! Choose from 5 different playable characters and swing your neck to victory!

Compatible with 2 joystick controllers. Use the left joystick to move your giraffe and the right to swing your neck! Slam your head into your opponent to deal damage! The faster your head is moving upon impact, the more damage you will deal!

Credits: Character art: Caro Maryn
	Scene art: John Kim
	UI, sound: Jaime Tous
	Polish: Cassidy Kammerer
	Character control, combat: Joshua Newell

For this project my focus was primarily on the combat systems and character control. Swinging the head in a spherical manner around the neck base proved to be an interested challenge. The implemented solution involved using quaternions to prevent gimbal lock, creating a predicted target location after user input using a raycast from the neck base, and using quaternion math to actually move the neck to the target location.

This provided a lot of flexibility for adjusting the neck length and tuning the controls. Constraints were placed to keep the head from entering the giraffes body.

The neck itself was also a point of focus for aesthetics sake. Since it was the primary method of combat it had to look and feel just right. The goofy cartoonized style we chose helped make the task a bit easier. The neck is a series of capsuled connected by joints that are free to spin around their own vertical axis. This makes the neck look seamless while also giving a funny effect spinning in place.

The tongue was implemented similarly adding some stylistic consistency and more goofiness to the giraffes. The challenge here was with Unity’s engine. The way rigidbody components and parenting objects works with Unity made giving the tongue independent physics while moving with the head a challenge. The tongues were ultimately made independent objects which were persistent in the fight stage level that sought the player objects by tag and followed a set relative position on the head every frame to appear as if they were attached.

The combat was designed to be first, very silly and awkward, similar to QWOP, and second, surprisingly deep. There are many factors to determine the amount of damage inflicted with each hit including the neck speed, factoring depth more than x,y movement to reward players for using the more difficult axis to play, the player body speed at a smaller factor than the neck itself again to reward the more difficult playstyle than simply ramming your opponent (though that does still work), and the character stats.
