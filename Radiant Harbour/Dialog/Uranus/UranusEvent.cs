using System.Collections.Generic;
using System.Linq;

namespace FredAndRadience.Radiant_Shipyard;

internal static class UranusEvent
{
	internal static void Inject()
	{
            DB.story.all["ShopRefillNukes_A"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "Do us a favor and don't tell anyone."
                    },
                }
            };
            DB.story.all["ShopRefillNukes_B"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "If anyone finds out.... I know how to find you."
                    },
                }
            };
            DB.story.all["ShopRefillNukes_C"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        loopTag = "explains",
                        Text = "Repairs complete."
                    },
                    new CustomSay()
                    {
                        who = "comp",
                        loopTag = "squint",
                        Text = "Repairs?"
                    },
                    new CustomSay()
                    {
                        who = "nerd",
                        flipped = true,
                        Text = "\"Repairs\"."
                    }
                }
            };
            DB.story.all["ShopRefillNukes_D"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        loopTag = "neutral",
                        Text = "You know nothing about this ship is legal, right?"
                    },
                    new SaySwitch
                    {
                        lines = [
                            new CustomSay()
                            {
                                who = Deck.riggs.Key(),
                                loopTag = "nervous",
                                Text = "Yes."
                            },
                            new CustomSay()
                            {
                                who = Deck.goat.Key(),
                                loopTag = "squint",
                                Text = "Don't remind me."
                            },
                            new CustomSay()
                            {
                                who = Deck.eunice.Key(),
                                loopTag = "sly",
                                Text = "Never stopped me."
                            },
                        ]
                    }
                }
            };
            DB.story.all["ShopRefillNukes_E"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "None of this is going on the record."
                    },
                }
            };
            DB.story.all["ShopRefillNukes_F"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "Aaaand I'll just scratch that from my ledger."
                    },
                }
            };
            DB.story.all["ShopRefillNukes_G"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "If I hear someone's onto you, I'll personally end your timeloop."
                    },
                }
            };
            DB.story.all["ShopRefillNukes_H"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                allPresent = [ Deck.dizzy.Key() ],
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        who = Deck.dizzy.Key(),
                        loopTag = "squint",
                        Text = "How long have you been holding onto these?"
                    },
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        loopTag = "nervous",
                        Text = "..."
                    }
                }
            };
            DB.story.all["ShopRefillNukes_I"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                allPresent = [ Deck.hacker.Key() ],
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        who = Deck.hacker.Key(),
                        loopTag = "squint",
                        Text = "Where did you even..."
                    },
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        loopTag = "neutral",
                        Text = "Another word and you're toast."
                    }
                }
            };
            DB.story.all["ShopRefillNukes_J"] = new StoryNode()
            {
                lookup = new HashSet<string>() { "shopRefillNukes" },
                allPresent = [ Deck.eunice.Key() ],
                type = NodeType.@event,
                bg = "BGShop",
                lines = new List<Instruction>()
                {
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        loopTag = "neutral",
                        Text = "I'm not even gonna ask where you got this ship."
                    },
                    new CustomSay()
                    {
                        who = Deck.eunice.Key(),
                        loopTag = "squint",
                        Text = "Why not?"
                    },
                    new CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        loopTag = "neutral",
                        Text = "Because I already know."
                    },
                }
            };
    }
}