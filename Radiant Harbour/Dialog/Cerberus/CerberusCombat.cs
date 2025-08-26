using System.Linq;

namespace FredAndRadience.Radiant_Shipyard;

internal static class CerberusCombat
{
	internal static void Inject()
	{
        DB.story.all[$"Cerberus_RunStart_Drake"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.eunice.Key() ],
			oncePerRun = true,
            lookup = new() { "Cerberus_StartRun" },
			oncePerRunTags = new() { "Cerberus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "The cannons! It's beautiful! Reminds me of the Fireball.",
					loopTag = "neutral"
				},
                new CustomSay()
                {
                    who = "comp",
                    Text = "Isn't the Fireball a lot smaller?",
                    loopTag = "squint"
                }
			}
		};
        DB.story.all[$"Cerberus_RunStart_Books"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.shard.Key() ],
			oncePerRun = true,
            lookup = new() { "Cerberus_StartRun" },
			oncePerRunTags = new() { "Cerberus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "W-wow. So many cannons! Do we get to use them all at once?",
					loopTag = "stoked"
				},
                new CustomSay()
                {
                    who = Deck.peri.Key(),
                    Text = "Not unless we aim very carefully.",
                    loopTag = "neutral"
                }
			}
		};
        DB.story.all[$"Cerberus_RunStart_Peri"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.peri.Key() ],
			oncePerRun = true,
            lookup = new() { "Cerberus_StartRun" },
			oncePerRunTags = new() { "Cerberus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "I'll do my best to keep my aim consistent.",
					loopTag = "neutral"
				},
                new CustomSay()
                {
                    who = Deck.riggs.Key(),
                    Text = "Counting on you!",
                    loopTag = "neutral"
                }
			}
		};
        DB.story.all[$"Cerberus_RunStart_CAT"] = new()
		{
			type = NodeType.combat,
            allPresent = [ "comp" ],
			oncePerRun = true,
            lookup = new() { "Cerberus_StartRun" },
			oncePerRunTags = new() { "Cerberus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "Keep these cannons on target and we'll be blowing up the Cobalt in no time!",
					loopTag = "neutral"
				},
                new CustomSay()
                {
                    who = Deck.riggs.Key(),
                    Text = "Roger!",
                    loopTag = "neutral"
                }
			}
		};
        DB.story.all[$"Cerberus_RunStart_Riggs"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.riggs.Key() ],
			oncePerRun = true,
            lookup = new() { "Cerberus_StartRun" },
			oncePerRunTags = new() { "Cerberus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
                new CustomSay()
                {
                    who = Deck.riggs.Key(),
                    Text = "I'll try to keep our cannons lined up with the enemy, but no promises!",
                    loopTag = "neutral"
                }
			}
		};
        DB.story.all[$"Cerberus_RunStart_Max"] = new()
		{
			type = NodeType.combat,
            allPresent = [ Deck.hacker.Key() ],
			oncePerRun = true,
            lookup = new() { "Cerberus_StartRun" },
			oncePerRunTags = new() { "Cerberus_RunStart" },
            maxTurnsThisCombat = 1,
            priority = true,
            turnStart = true,
            
			lines = new()
			{
                new CustomSay()
                {
                    who = Deck.hacker.Key(),
                    Text = "Keep a steady hand with these cannons and this should be a fun ride.",
                    loopTag = "smile"
                }
			}
		};
    }
}