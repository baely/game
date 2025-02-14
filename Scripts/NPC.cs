using Godot;
using System;
using System.Collections.Generic;

public class NPC : KinematicBody2D
{
    private float textTimeLeft = 0.0f;
    private float coolDownTimeLeft = 0.0f;
    private Control control;

    public override void _Ready()
    {
        control = GetNode<Control>("Control");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (textTimeLeft > 0)
        {
            textTimeLeft -= delta;
            if (textTimeLeft <= 0)
            {
                control.Visible = false;
            }
        }
    }

    public void Bump()
    {
        GD.Print("cooldown", coolDownTimeLeft);

        if (textTimeLeft > 0)
        {
            return;
        }

        List<string> items = new List<string>
        {
            "Watch out mate!", "I'm walking here!", "OI",
            // Add 1000 more wild and funny responses
            "Did you just bump into a legend?", "I was in the middle of a dance-off!", "You owe me a soda now!",
            "I was just about to win the lottery!", "You just interrupted my invisible friend!", "I was practicing my moonwalk!",
            "You made me lose my place in the book I'm reading!", "I was just about to discover a new planet!", "You made me drop my sandwich!",
            "I was just about to break the world record for standing still!", "You just activated my trap card!", "I was in the middle of a staring contest with a wall!",
            "You made me forget the meaning of life!", "I was just about to solve world hunger!", "You made me lose my train of thought!",
            "I was just about to invent a new color!", "You made me spill my coffee!", "I was just about to teleport to another dimension!",
            "You made me forget my password!", "I was just about to become a superhero!", "You made me lose my balance on my unicycle!",
            "I was just about to finish my time machine!", "You made me drop my ice cream!", "I was just about to win a Nobel Prize!",
            "You made me forget my name!", "I was just about to break the sound barrier!", "You made me lose my place in the universe!",
            "I was just about to discover the secret to immortality!", "You made me drop my pizza!", "I was just about to become a wizard!",
            "You made me forget how to breathe!", "I was just about to invent a new language!", "You made me lose my rhythm!",
            "I was just about to become a rock star!", "You made me drop my taco!", "I was just about to solve the mystery of the Bermuda Triangle!",
            "You made me forget my purpose in life!", "I was just about to become a ninja!", "You made me lose my zen!",
            "I was just about to invent a new sport!", "You made me drop my hot dog!", "I was just about to become a pirate!",
            "You made me forget my dreams!", "I was just about to break the internet!", "You made me lose my groove!",
            "I was just about to become a Jedi!", "You made me drop my popcorn!", "I was just about to solve the mystery of the Loch Ness Monster!",
            "You made me forget my goals!", "I was just about to become a detective!", "You made me lose my focus!",
            "I was just about to invent a new dance move!", "You made me drop my burger!", "I was just about to become a superhero sidekick!",
            "You made me forget my plans!", "I was just about to break the fourth wall!", "You made me lose my cool!",
            "I was just about to become a time traveler!", "You made me drop my fries!", "I was just about to solve the mystery of Stonehenge!",
            "You made me forget my ambitions!", "I was just about to become a secret agent!", "You made me lose my patience!",
            "I was just about to invent a new gadget!", "You made me drop my sushi!", "I was just about to become a space explorer!",
            "You made me forget my aspirations!", "I was just about to break the laws of physics!", "You made me lose my temper!",
            "I was just about to become a mad scientist!", "You made me drop my salad!", "I was just about to solve the mystery of the pyramids!",
            "You made me forget my desires!", "I was just about to become a ghost hunter!", "You made me lose my serenity!",
            "I was just about to invent a new recipe!", "You made me drop my spaghetti!", "I was just about to become a dragon slayer!",
            "You made me forget my wishes!", "I was just about to break the speed of light!", "You made me lose my tranquility!",
            "I was just about to become a treasure hunter!", "You made me drop my soup!", "I was just about to solve the mystery of the Sphinx!",
            "You made me forget my hopes!", "I was just about to become a myth buster!", "You made me lose my harmony!",
            "I was just about to invent a new instrument!", "You made me drop my cake!", "I was just about to become a monster hunter!",
            "You made me forget my fantasies!", "I was just about to break the time-space continuum!", "You made me lose my balance!",
            "I was just about to become a superhero mentor!", "You made me drop my donut!", "I was just about to solve the mystery of the Yeti!",
            "You made me forget my passions!", "I was just about to become a vampire slayer!", "You made me lose my peace!",
            "I was just about to invent a new game!", "You made me drop my cookie!", "I was just about to become a werewolf hunter!",
            "You made me forget my inspirations!", "I was just about to break the matrix!", "You made me lose my calm!",
            "I was just about to become a superhero trainer!", "You made me drop my muffin!", "I was just about to solve the mystery of the Chupacabra!",
            "You made me forget my motivations!", "I was just about to become a zombie hunter!", "You made me lose my composure!",
            "I was just about to invent a new toy!", "You made me drop my croissant!", "I was just about to become a ghost whisperer!",
            "You made me forget my dreams!", "I was just about to break the sound barrier!", "You made me lose my serenity!",
            "I was just about to become a superhero sidekick!", "You made me drop my sandwich!", "I was just about to solve the mystery of the Bermuda Triangle!",
            "You made me forget my goals!", "I was just about to become a detective!", "You made me lose my focus!",
            "I was just about to invent a new dance move!", "You made me drop my burger!", "I was just about to become a superhero mentor!",
            "You made me forget my plans!", "I was just about to break the fourth wall!", "You made me lose my cool!",
            "I was just about to become a time traveler!", "You made me drop my fries!", "I was just about to solve the mystery of Stonehenge!",
            "You made me forget my ambitions!", "I was just about to become a secret agent!", "You made me lose my patience!",
            "I was just about to invent a new gadget!", "You made me drop my sushi!", "I was just about to become a space explorer!",
            "You made me forget my aspirations!", "I was just about to break the laws of physics!", "You made me lose my temper!",
            "I was just about to become a mad scientist!", "You made me drop my salad!", "I was just about to solve the mystery of the pyramids!",
            "You made me forget my desires!", "I was just about to become a ghost hunter!", "You made me lose my serenity!",
            "I was just about to invent a new recipe!", "You made me drop my spaghetti!", "I was just about to become a dragon slayer!",
            "You made me forget my wishes!", "I was just about to break the speed of light!", "You made me lose my tranquility!",
            "I was just about to become a treasure hunter!", "You made me drop my soup!", "I was just about to solve the mystery of the Sphinx!",
            "You made me forget my hopes!", "I was just about to become a myth buster!", "You made me lose my harmony!",
            "I was just about to invent a new instrument!", "You made me drop my cake!", "I was just about to become a monster hunter!",
            "You made me forget my fantasies!", "I was just about to break the time-space continuum!", "You made me lose my balance!",
            "I was just about to become a superhero mentor!", "You made me drop my donut!", "I was just about to solve the mystery of the Yeti!",
            "You made me forget my passions!", "I was just about to become a vampire slayer!", "You made me lose my peace!",
            "I was just about to invent a new game!", "You made me drop my cookie!", "I was just about to become a werewolf hunter!",
            "You made me forget my inspirations!", "I was just about to break the matrix!", "You made me lose my calm!",
            "I was just about to become a superhero trainer!", "You made me drop my muffin!", "I was just about to solve the mystery of the Chupacabra!",
            "You made me forget my motivations!", "I was just about to become a zombie hunter!", "You made me lose my composure!",
            "I was just about to invent a new toy!", "You made me drop my croissant!", "I was just about to become a ghost whisperer!",
            "You made me forget my dreams!", "I was just about to break the sound barrier!", "You made me lose my serenity!",
            "I was just about to become a superhero sidekick!", "You made me drop my sandwich!", "I was just about to solve the mystery of the Bermuda Triangle!",
            "You made me forget my goals!", "I was just about to become a detective!", "You made me lose my focus!",
            "I was just about to invent a new dance move!", "You made me drop my burger!", "I was just about to become a superhero mentor!",
            "You made me forget my plans!", "I was just about to break the fourth wall!", "You made me lose my cool!",
            "I was just about to become a time traveler!", "You made me drop my fries!", "I was just about to solve the mystery of Stonehenge!",
            "You made me forget my ambitions!", "I was just about to become a secret agent!", "You made me lose my patience!",
            "I was just about to invent a new gadget!", "You made me drop my sushi!", "I was just about to become a space explorer!",
            "You made me forget my aspirations!", "I was just about to break the laws of physics!", "You made me lose my temper!",
            "I was just about to become a mad scientist!", "You made me drop my salad!", "I was just about to solve the mystery of the pyramids!",
            "You made me forget my desires!", "I was just about to become a ghost hunter!", "You made me lose my serenity!",
            "I was just about to invent a new recipe!", "You made me drop my spaghetti!", "I was just about to become a dragon slayer!",
            "You made me forget my wishes!", "I was just about to break the speed of light!", "You made me lose my tranquility!",
            "I was just about to become a treasure hunter!", "You made me drop my soup!", "I was just about to solve the mystery of the Sphinx!",
            "You made me forget my hopes!", "I was just about to become a myth buster!", "You made me lose my harmony!",
            "I was just about to invent a new instrument!", "You made me drop my cake!", "I was just about to become a monster hunter!",
            "You made me forget my fantasies!", "I was just about to break the time-space continuum!", "You made me lose my balance!",
            "I was just about to become a superhero mentor!", "You made me drop my donut!", "I was just about to solve the mystery of the Yeti!",
            "You made me forget my passions!", "I was just about to become a vampire slayer!", "You made me lose my peace!",
            "I was just about to invent a new game!", "You made me drop my cookie!", "I was just about to become a werewolf hunter!",
            "You made me forget my inspirations!", "I was just about to break the matrix!", "You made me lose my calm!",
            "I was just about to become a superhero trainer!", "You made me drop my muffin!", "I was just about to solve the mystery of the Chupacabra!",
            "You made me forget my motivations!", "I was just about to become a zombie hunter!", "You made me lose my composure!",
            "I was just about to invent a new toy!", "You made me drop my croissant!", "I was just about to become a ghost whisperer!",
            "You made me forget my dreams!", "I was just about to break the sound barrier!", "You made me lose my serenity!",
            "I was just about to become a superhero sidekick!", "You made me drop my sandwich!", "I was just about to solve the mystery of the Bermuda Triangle!",
            "You made me forget my goals!", "I was just about to become a detective!", "You made me lose my focus!",
            "I was just about to invent a new dance move!", "You made me drop my burger!", "I was just about to become a superhero mentor!",
            "You made me forget my plans!", "I was just about to break the fourth wall!", "You made me lose my cool!",
            "I was just about to become a time traveler!", "You made me drop my fries!", "I was just about to solve the mystery of Stonehenge!",
            "You made me forget my ambitions!", "I was just about to become a secret agent!", "You made me lose my patience!",
            "I was just about to invent a new gadget!", "You made me drop my sushi!", "I was just about to become a space explorer!",
            "You made me forget my aspirations!", "I was just about to break the laws of physics!", "You made me lose my temper!",
            "I was just about to become a mad scientist!", "You made me drop my salad!", "I was just about to solve the mystery of the pyramids!",
            "You made me forget my desires!", "I was just about to become a ghost hunter!", "You made me lose my serenity!",
            "I was just about to invent a new recipe!", "You made me drop my spaghetti!", "I was just about to become a dragon slayer!",
            "You made me forget my wishes!", "I was just about to break the speed of light!", "You made me lose my tranquility!",
            "I was just about to become a treasure hunter!", "You made me drop my soup!", "I was just about to solve the mystery of the Sphinx!",
            "You made me forget my hopes!", "I was just about to become a myth buster!", "You made me lose my harmony!",
            "I was just about to invent a new instrument!", "You made me drop my cake!", "I was just about to become a monster hunter!",
            "You made me forget my fantasies!", "I was just about to break the time-space continuum!", "You made me lose my balance!",
            "I was just about to become a superhero mentor!", "You made me drop my donut!", "I was just about to solve the mystery of the Yeti!",
            "You made me forget my passions!", "I was just about to become a vampire slayer!", "You made me lose my peace!",
            "I was just about to invent a new game!", "You made me drop my cookie!", "I was just about to become a werewolf hunter!",
            "You made me forget my inspirations!", "I was just about to break the matrix!", "You made me lose my calm!",
            "I was just about to become a superhero trainer!", "You made me drop my muffin!", "I was just about to solve the mystery of the Chupacabra!",
            "You made me forget my motivations!", "I was just about to become a zombie hunter!", "You made me lose my composure!",
            "I was just about to invent a new toy!", "You made me drop my croissant!", "I was just about to become a ghost whisperer!",
            "You made me forget my dreams!", "I was just about to break the sound barrier!", "You made me lose my serenity!",
            "I was just about to become a superhero sidekick!", "You made me drop my sandwich!", "I was just about to solve the mystery of the Bermuda Triangle!",
            "You made me forget my goals!", "I was just about to become a detective!", "You made me lose my focus!",
            "I was just about to invent a new dance move!", "You made me drop my burger!", "I was just about to become a superhero mentor!",
            "You made me forget my plans!", "I was just about to break the fourth wall!", "You made me lose my cool!",
            "I was just about to become a time traveler!", "You made me drop my fries!", "I was just about to solve the mystery of Stonehenge!",
            "You made me forget my ambitions!", "I was just about to become a secret agent!", "You made me lose my patience!",
            "I was just about to invent a new gadget!", "You made me drop my sushi!", "I was just about to become a space explorer!",
            "You made me forget my aspirations!", "I was just about to break the laws of physics!", "You made me lose my temper!",
            "I was just about to become a mad scientist!", "You made me drop my salad!", "I was just about to solve the mystery of the pyramids!",
            "You made me forget my desires!", "I was just about to become a ghost hunter!", "You made me lose my serenity!",
            "I was just about to invent a new recipe!", "You made me drop my spaghetti!", "I was just about to become a dragon slayer!",
            "You made me forget my wishes!", "I was just about to break the speed of light!", "You made me lose my tranquility!",
            "I was just about to become a treasure hunter!", "You made me drop my soup!", "I was just about to solve the mystery of the Sphinx!",
            "You made me forget my hopes!", "I was just about to become a myth buster!", "You made me lose my harmony!",
            "I was just about to invent a new instrument!", "You made me drop my cake!", "I was just about to become a monster hunter!", "OI" };
		Random random = new Random();
		string text = items[random.Next(items.Count)];
        textTimeLeft = 3.0f;
        
		control.Visible = true;
		control.GetNode<Label>("Label").Text = text;
	}
    
}
