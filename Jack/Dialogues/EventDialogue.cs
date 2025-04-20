using System.Linq;

namespace Fred.Jack;

internal static class EventDialogue
{
	internal static ModEntry Instance => ModEntry.Instance;
	private static IKokoroApi KokoroApi => Instance.KokoroApi;

	internal static void Inject()
	{
		string jack = Instance.Jack_Deck.UniqueName;
    DB.story.GetNode("AbandonedShipyard")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "Scanners are all static.",
			loopTag = "squint"
		});
		DB.story.GetNode("AbandonedShipyard_Repaired")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "This place gives me bad vibes.",
			loopTag = "squint"
		});
    DB.story.all[$"ChoiceCardRewardOfYourColorChoice_{jack}"] = new()
		{
			type = NodeType.@event,
			oncePerRun = true,
			allPresent = new() { jack },
			bg = "BGBootSequence",
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "I feel.... The same?",
					loopTag = "question"
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Energy readings are back to normal."
				}
			}
		};
    DB.story.GetNode("CrystallizedFriendEvent")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "Why is everyone looking at me?",
			loopTag = "question"
		});
    DB.story.all[$"CrystallizedFriendEvent_{jack}"] = new()
		{
			type = NodeType.@event,
			oncePerRun = true,
			allPresent = new() { jack },
			bg = "BGCrystalizedFriend",
			lines = new()
			{
				new Wait()
				{
					secs = 1.5
				},
				new CustomSay()
				{
					who = jack,
					Text = "Officer Jack. Ready for combat.",
					loopTag = "salute"
				}
			}
		};
    DB.story.GetNode("DraculaTime")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "I have zero clue who you are.",
			loopTag = "squint"
		});
		DB.story.GetNode("GrandmaShop")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "Brownies!",
			loopTag = "happy"
		});
    DB.story.GetNode("LoseCharacterCard")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "Aren't you supposed to keep us away from these things?!",
			loopTag = "serious"
		});
    DB.story.GetNode("LoseCharacterCard_No")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "I still blame the autopilot.",
			loopTag = "squint"
		});
    DB.story.all[$"LoseCharacterCard_{jack}"] = new()
		{
			type = NodeType.@event,
			oncePerRun = true,
			allPresent = new() { jack },
			bg = "BGSupernova",
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "Was there nothing less important?",
					loopTag = "question"
				}
			}
		};
    DB.story.all[$"ShopkeeperInfinite_{jack}"] = new()
		{
			type = NodeType.@event,
			lookup = new() { "shopBefore" },
			allPresent = new() { jack },
			bg = "BGShop",
			lines = new()
			{
				new CustomSay()
				{
					who = jack,
					Text = "You call this a shop?",
					loopTag = "question"
				},
        new CustomSay()
				{
					who = "nerd",
					Text = "Yes, and if you keep up this attitude, I'll kick you out.",
					loopTag = "neutral",
					flipped = true
				},
				new Jump()
				{
					key = "NewShop"
				}
			}
		};
    DB.story.all[$"{jack}_Intro"] = new()
		{
			type = NodeType.@event,
			lookup = new() { "zone_first" },
			allPresent = new() { jack },
			once = true,
			bg = "BGRunStart",
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "Alright, is everyone on board?",
          flipped = true,
					loopTag = "neutral"
				},
        new CustomSay()
        {
          who = jack,
          Text = "..........",
          loopTag = "sleep"
        },
        new CustomSay()
				{
					who = "comp",
					Text = "...",
          flipped = true,
					loopTag = "squint"
				},
        new CustomSay()
				{
					who = "comp",
					Text = "WAKE UP!!!",
          flipped = true,
					loopTag = "mad"
				},
        new CustomSay()
        {
          who = jack,
          Text = "SIR YES SIR!",
          loopTag = "salute"
        },
        new CustomSay()
        {
          who = jack,
          Text = "Wait... Where am I?",
          loopTag = "confused"
        },
        new CustomSay()
				{
					who = "comp",
					Text = "You're in the loop with us. Do you know who you are?",
          flipped = true,
					loopTag = "neutral"
				},
        new CustomSay()
        {
          who = jack,
          Text = "Of course. I'm.... uhh....",
          loopTag = "confused"
        },
        new CustomSay()
				{
					who = "comp",
					Text = "Jack, your name is Jack.",
          flipped = true,
					loopTag = "neutral"
				},
        new CustomSay()
        {
          who = jack,
          Text = "Oh yea, Jack, that's right.",
          loopTag = "neutral"
        },
        new CustomSay()
				{
					who = "comp",
					Text = "Alright, prepare yourself, there's an enemy approaching.",
          flipped = true,
					loopTag = "intense"
				},
			}
		};
    DB.story.GetNode("SogginsEscape_0")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "I'll have them confiscate all this when we get out of this mess.",
			loopTag = "squint"
		});
    DB.story.GetNode("Soggins_Infinite")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = jack,
			Text = "They're handing out weapons to anybody these days.",
			loopTag = "confused"
		});
  }
}