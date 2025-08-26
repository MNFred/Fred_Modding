using System.Linq;

namespace FredAndRadience.Radiant_Shipyard;

internal static class ChangelingCombat
{
	internal static void Inject()
	{
        DB.story.all[$"Changeling_RunStart_Books_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Haha! Again! Again!",
					loopTag = "paws"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Books_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Weeeeee! It's like a magic fun-ride!",
					loopTag = "stoked"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.peri.Key(),
                            Text = "Books, you are precious. Never change.",
                            loopTag = "anime"
                        },
                        new CustomSay()
                        {
                            who = Deck.riggs.Key(),
                            Text = "Haha! Yeah, you're right!",
                            loopTag = "neutral"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "If you say so.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "That's one way to look at it.",
                            loopTag = "squint"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Changeling_RunStart_Riggs_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.riggs.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "Woah! Whiplash! Good thing I secured my boba!",
					loopTag = "neutral"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Riggs_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.riggs.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "Ha! Wow! This is kinda fun! Strap in everyone!",
					loopTag = "neutral"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Peri_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Hurk! Buckle up everyone! This is gonna be a wild ride.",
					loopTag = "panic"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Peri_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "I've got a spreadsheet for all the possible layouts. It's a lot.",
					loopTag = "neutral"
				},
                new CustomSay()
                {
                    who = Deck.hacker.Key(),
                    Text = "Did CAT help you do that?",
                    loopTag = "squint"
                }
			}
		};
        DB.story.all[$"Changeling_RunStart_Drake_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.eunice.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Huuurk! I think I'm gonna be sick. Where's the vac tube?",
					loopTag = "panic"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Drake_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.eunice.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Huuurk! Flaming fireballs, this is not fun.",
					loopTag = "panic"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Isaac_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.goat.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Wh- where did the bathroom just go?",
					loopTag = "panic"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Isaac_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.goat.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Huh. Wow. This- this is so disorienting.",
					loopTag = "squint"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "You're telling me! Let me off!",
                            loopTag = "panic"
                        },
                        new CustomSay()
                        {
                            who = Deck.shard.Key(),
                            Text = "It's fun though!",
                            loopTag = "paws"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Changeling_RunStart_Max_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.hacker.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Wow. The hallway just vanished.",
					loopTag = "intense"
				},
			}
		};
        DB.story.all[$"Changeling_RunStart_Max_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.hacker.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "How does this ship even function? What's running it?",
					loopTag = "squint"
				},
                new CustomSay()
                {
                    who = Deck.dizzy.Key(),
                    Text = "My readings show it's some kind of warp drive.",
                    loopTag = "shrug"
                }
			}
		};
        DB.story.all[$"Changeling_RunStart_Dizzy_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.dizzy.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Welp. That was more unpleasant than anticipated.",
					loopTag = "squint"
				},
                new CustomSay()
                {
                    who = Deck.eunice.Key(),
                    Text = "Number one understatement of this timeloop.",
                    loopTag = "mad"
                }
			}
		};
        DB.story.all[$"Changeling_RunStart_Dizzy_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.dizzy.Key() ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Wow. I think I'm getting dizzy.",
					loopTag = "intense"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "Was... Was that a pun?",
                            loopTag = "mad"
                        },
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "Me too... Hang on.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "No kidding... Wait.",
                            loopTag = "squint"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Changeling_RunStart_CAT_1"] = new()
		{
			type = NodeType.combat,
            allPresent = [ "comp" ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "Everybody get used to this. It's gonna happen a lot.",
					loopTag = "intense"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.hacker.Key(),
                            Text = "Oof.",
                            loopTag = "gameover"
                        },
                        new CustomSay()
                        {
                            who = Deck.goat.Key(),
                            Text = "I'll try.",
                            loopTag = "squint"
                        },
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "I don't know if I can.",
                            loopTag = "squint"
                        }
                    ]
                }
			}
		};
        DB.story.all[$"Changeling_RunStart_CAT_2"] = new()
		{
			type = NodeType.combat,
            allPresent = [ "comp" ],
			oncePerRun = true,
            lookup = new() { "Change_StartRun" },
			oncePerRunTags = new() { "Change_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "You know, this kind of tickles",
					loopTag = "neutral"
				},
                new SaySwitch
                {
                    lines = [
                        new CustomSay()
                        {
                            who = Deck.eunice.Key(),
                            Text = "I envy you right now.",
                            loopTag = "mad"
                        },
                        new CustomSay()
                        {
                            who = Deck.dizzy.Key(),
                            Text = "Fascinating.",
                            loopTag = "neutral"
                        }
                    ]
                }
			}
		};
    }
}