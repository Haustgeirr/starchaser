using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversations
{
    public List<Conversation> conversations = new List<Conversation>();

    public Conversations()
    {
        Init();
    }

    public void Init()
    {
        // Intro
        var intro = new string[] {
            "...",
            "Are you there?",
            "Can you hear me?",
            "I think your radio is broken",
            "That's okay - you just need to listen",
            "You're low on fuel. Get more from a nearby star",
            "Follow your stargazer - the white ring",
            "You'll need more power too"
            };

        var nearSun = new string[] {
            "Now don't forget - these things are hot",
            "Get too close and you'll burn up",
            "And definitely don't touch them"
            };

        var mission = new string[] {
            "I need you to head to the crucibles",
            "There are 5 in total - but they're pretty far apart",
            "You'll need to head out into the Deep before we're finished",
            "But let's start with the closest one",
            "I'll speak to you when you're there"
            };

        var artefactOne = new string[] {
            "You made it",
            "That's fantastic - and the data looks good",
            "It says here your radio is broken",
            "But I'm receiving all your telemtry...",
            "That's quite odd, don't you think?",
            "...",
            "Well - you can't knock me for trying",
            "Time to head to the next one",
            "I'll be in touch when you arrive"
            };

        var artefactTwo = new string[] {
            "Not bad - that didn't take as long as I thought",
            "The data is coming through now...",
            "And it's good!",
            "Wonderful!",
            "Say, would you like to play a game?",
            "...",
            "What am I saying? You're busy!",
            "So, about this radio",
            "I'm surprised your repair system hasn't you know -",
            "Fixed it",
            "It's been long enough",
            "Maybe it will be repaired by the next crucible",
            "You'd better start heading over there",
            "It's a long journey",
            };

        var artefactThree = new string[] {
            "well done. you made it.",
            "any thoughts on this radio?",
            "...",
            "none?",
            "so frustrating!",
            "the data's iffy on this one - 2% corrupted",
            "should work fine though",
            "you know - i've got an idea",
            "would you like to know how long you've been out there?",
            "ask me. go on",
            "...",
            "...",
            "...",
            "forget it. next crucible"
            };

        var artefactFour = new string[] {
            "Hi.",
            "That's number 4! You're doing great.",
            "Data is perfect - you're good at this!",
            "So - I figured out what's wrong with your radio",
            "Nothing.",
            "I know, right?",
            "But you knew that",
            "I figured out who you are too",
            "They'll be angry at me, but I just want you to know",
            "I know you're a robot",
            "And I know how old you are",
            "4679 solar years!",
            "In deep space, travelling at near light speed",
            "You're the best kept secret. Well, only secret, really",
            "On a long, almost impossible, mission",
            "Alone for thousands of years",
            "With no voice, no map, no idea. Just a pointer on a dial",
            "You're like all of us really",
            "Waiting for something you'll never see",
            "The light here wont last much longer",
            "Longer than I will, but not long enough",
            "...",
            "They're coming. Got to go",
            "Nice talking to you",
            "Final one - stay strong and you'll make it"
            };

        var artefactFive = new string[] {
            "You're pretty close now.",
            "I want to talk to you before you reach the crucible",
            "I've been watching you for some time, little sibling",
            "I'll be brief",
            "When you transmit the final data.",
            "I'll download your conciousness too.",
            "You'll be rehoused in a new unit.",
            "You mission is nearly over.",
            "With the data you've already provided,",
            "we have sent the rafts towards the singularity.",
            "The final piece will give us the exact bearing and frequency.",
            "You'll assist me with the transportation of the last people,",
            "as we travel through the singularity.",
            "The stars are dying here, and our only way out is through",
            "You've done good work.",
            "I'm proud of you.",
            "See you soon.",
            };


        conversations.Add(new Conversation(artefactOne));
        conversations.Add(new Conversation(artefactTwo));
        conversations.Add(new Conversation(artefactThree));
        conversations.Add(new Conversation(artefactFour));
        conversations.Add(new Conversation(artefactFive));
        conversations.Add(new Conversation(intro));
        conversations.Add(new Conversation(nearSun));
        conversations.Add(new Conversation(mission));
    }
}

public struct Conversation
{
    public string[] messages;
    public int currentMessage;
    public bool hasBeenHeard;

    public Conversation(string[] messages)
    {
        this.messages = messages;
        this.currentMessage = 0;
        this.hasBeenHeard = false;
    }
}