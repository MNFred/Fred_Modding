using System.Linq;

namespace FredAndRadience.Radiant_Shipyard;

internal static class UranusCombat
{
	internal static void Inject()
	{
        DB.story.all[$"Uranus_RunStart_Books"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerRun = true,
            lookup = new() { "Uranus_StartRun" },
			oncePerRunTags = new() { "Uranus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "S-should I be on this ship?",
					loopTag = "gameover"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.riggs.Key(),
                            Text = "I had the same question.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "Just keep focused on the task ahead. We'll be fine.",
                            loopTag = "explains"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Who cares? Let's just reach the cobalt and blow it up.",
                            loopTag = "squint"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_RunStart_Riggs"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.riggs.Key() ],
			oncePerRun = true,
            lookup = new() { "Uranus_StartRun" },
			oncePerRunTags = new() { "Uranus_RunStart" },
            priority = true,
            maxTurnsThisCombat = 1,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "I'm not sure I'm comfortable flying this ship.",
					loopTag = "nervous"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "You know nothing we did aboard the Cobalt was legal, right?",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "I'm not comfortable either. But we have to stay focused.",
                            loopTag = "neutral"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Seriously? I feel right at home.",
                            loopTag = "sly"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_RunStart_Drake"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.eunice.Key() ],
			oncePerRun = true,
            lookup = new() { "Uranus_StartRun" },
			oncePerRunTags = new() { "Uranus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Ah, the comfortable life of the outlaw.",
					loopTag = "sly"
				},
                new CustomSay()
                {
                    who = Deck.hacker.Key(),
                    Text = "I'm pretty sure this is way more illegal than what you were doing before.",
                    loopTag = "squint"
                }
			}
		};
        DB.story.all[$"Uranus_RunStart_Peri"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerRun = true,
            lookup = new() { "Uranus_StartRun" },
			oncePerRunTags = new() { "Uranus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "For the record, I am not okay with being on this ship.",
					loopTag = "squint"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "Noted.",
                            loopTag = "serious"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Baby.",
                            loopTag = "sly"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_RunStart_CAT"] = new()
		{
			type = NodeType.combat,
            allPresent = [ "comp" ],
			oncePerRun = true,
            lookup = new() { "Uranus_StartRun" },
			oncePerRunTags = new() { "Uranus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "Let's keep this adventure between the three of us.",
					loopTag = "intense"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.riggs.Key(),
                            Text = "Got it!",
                            loopTag = "neutral"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "No promises.",
                            loopTag = "sly"
                        },
                        new CustomSay()
                        {
                            who = Deck.shard.Key(),
                            Text = "O-okay!",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "No need to tell me.",
                            loopTag = "intense"
                        },
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "Yeah, sounds good.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "Noted.",
                            loopTag = "serious"
                        },
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "I imagine we'll have to.",
                            loopTag = "squint"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_RunStart_Isaac"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.goat.Key() ],
			oncePerRun = true,
            lookup = new() { "Uranus_StartRun" },
			oncePerRunTags = new() { "Uranus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "This... this is so illegal.",
					loopTag = "squint"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "Yes.",
                            loopTag = "serious"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "We know.",
                            loopTag = "intense"
                        },
                        new CustomSay()
                        {
                            who = Deck.riggs.Key(),
                            Text = "...",
                            loopTag = "nervous"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Wimp.",
                            loopTag = "sly"
                        },
                        new CustomSay()
                        {
                            who = Deck.shard.Key(),
                            Text = "...",
                            loopTag = "intense"
                        },
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "It's... we have to make due.",
                            loopTag = "panic"
                        },
                        new CustomSay()
                        {
                            who = "comp",
                            Text = "As long as no one finds out...",
                            loopTag = "worried"
                        },
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_NukeMissed_Peri"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerCombat = true,
			oncePerCombatTags = new() { "Uranus_NukeMiss" },
            lookup = new() { "Uranus_NukeMissed" },
            priority = true,
            oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "I... what did we just do?",
					loopTag = "panic"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "I'm not sticking around to find out.",
                            loopTag = "panic"
                        },
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "Let's hope no one runs into it.",
                            loopTag = "panic"
                        },
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "It'll be fine... probably.",
                            loopTag = "intense"
                        },
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_NukeMissed_Riggs"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.riggs.Key() ],
			oncePerCombat = true,
			oncePerCombatTags = new() { "Uranus_NukeMiss" },
            lookup = new() { "Uranus_NukeMissed" },
            priority = true,
            oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "Uh... what does that mean?",
					loopTag = "nervous"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Someone's gonna have a bad day.",
                            loopTag = "sly"
                        },
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "We have to hope for minimal collateral.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "Hopefully no one will run into it.",
                            loopTag = "intense"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_NukeMissed_CAT_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ "comp" ],
			oncePerCombat = true,
            oncePerRun = true,
			oncePerCombatTags = new() { "Uranus_NukeMiss" },
            lookup = new() { "Uranus_NukeMissed" },
            priority = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "Uh oh... . That's really bad.",
					loopTag = "worried"
				},
                new CustomSay()
                {
                    who = Deck.goat.Key(),
                    Text = "Yeah... no kidding...",
                    loopTag = "squint"
                }
			}
		};
        DB.story.all[$"Uranus_NukeMissed_CAT_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ "comp" ],
			oncePerCombat = true,
            oncePerRun = true,
			oncePerCombatTags = new() { "Uranus_NukeMiss" },
            lookup = new() { "Uranus_NukeMissed" },
            priority = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "We... may now be guilty of several major war crimes.",
					loopTag = "worried"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "Only if it blows up near people.",
                            loopTag = "explains"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "I'm already beyond saving.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "I didn't sign up for this.",
                            loopTag = "mad"
                        },
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "Great... as if my career wasn't already in jeopardy.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "We'll manage... Somehow.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.riggs.Key(),
                            Text = "hhhahaha what?",
                            loopTag = "nervous"
                        },
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_NukeMissed_Books"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerCombat = true,
			oncePerCombatTags = new() { "Uranus_NukeMiss" },
            priority = true,
            oncePerRun = true,
            lookup = new() { "Uranus_NukeMissed" },
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "N-no one's gonna get hurt by that, right?",
					loopTag = "intense"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "We have to hope.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.riggs.Key(),
                            Text = "Yes? No?",
                            loopTag = "nervous"
                        },
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "Hope not!",
                            loopTag = "intense"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "Sure. Yeah.",
                            loopTag = "intense"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Uranus_NukeMissed_Max"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.hacker.Key() ],
			oncePerCombat = true,
            oncePerRun = true,
			oncePerCombatTags = new() { "Uranus_NukeMiss" },
            priority = true,
            lookup = new() { "Uranus_NukeMissed" },
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Crap. That's really bad.",
					loopTag = "intense"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "You're telling me!",
                            loopTag = "panic"
                        },
                        new CustomSay()
                        {
                            who = Deck.shard.Key(),
                            Text = "Really...?",
                            loopTag = "intense"
                        },
                        new CustomSay()
                        {
                            who = Deck.riggs.Key(),
                            Text = "...",
                            loopTag = "nervous"
                        },
                    ]
                }
			}
		};
    }
}