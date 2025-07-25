﻿using System;
using System.Collections.Generic;
using System.Linq;
using AATool.Data.Categories;
using AATool.UI.Controls;
using Newtonsoft.Json;

namespace AATool.Configuration
{
    [JsonObject]
    public class PinnedObjectiveSet
    {
        public static readonly List<string> AllAA = new (){
            "EGap", "Trident", "NautilusShells", "WitherSkulls",
            "AncientDebris", "GoldBlocks", "Bees", "Sniffers",
            "Cats", "Foods", "Animals", "Monsters", "Biomes", "Cauldrons", "ArmorTrims", "HeavyCore"
        };

        public static readonly List<string> AllAB = new (){
            "Trident", "NautilusShells", "ShulkerShells", "WitherSkulls",
            "AncientDebris", "DeepslateEmerald", "SculkBlocks", "Mycelium", "RedSand", "Bees", "HeavyCore"
        };

        public static readonly List<string> AllAACH = new (){
            "EGap", "WitherSkulls", "GoldBlocks", "Biomes",
        };

        public static List<string> GetAllAvailable()
        {
            var available = new List<string>();
            if (Tracker.Category is AllBlocks)
                available.AddRange(AllAB);
            else if (Tracker.Category is AllAchievements)
                available.AddRange(AllAACH);
            else
                available.AddRange(AllAA);

            if (Version.TryParse(Tracker.CurrentVersion, out Version current))
            {
                if (current < new Version("1.21"))
                    available.Remove("HeavyCore");
                if (current < new Version("1.20"))
                {
                    available.Remove("ArmorTrims");
                    available.Remove("Sniffers");
                }
                if (current < new Version("1.19"))
                    available.Remove("SculkBlocks");
                if (current != new Version("1.17"))
                    available.Remove("Cauldrons");
                if (current < new Version("1.17"))
                    available.Remove("DeepslateEmerald");
                if (current < new Version("1.16"))
                    available.Remove("AncientDebris");
                if (current < new Version("1.15"))
                    available.Remove("Bees");
                if (current < new Version("1.14"))
                    available.Remove("Cats");
                if (current < new Version("1.13"))
                { 
                    available.Remove("Trident");
                    available.Remove("NautilusShells");
                }
            }
            else
            {
                //latest snapshot
                available.Remove("Cauldrons");
            }
            return available;
        }

        [JsonProperty]
        public Dictionary<string, List<string>> Pinned = new () {
            { "All Advancements 1.21.6", new () {
                "WitherSkulls", "NautilusShells", "Trident", "HeavyCore", "Sniffers", "ArmorTrims",
            }},
            { "All Advancements 1.21 v2", new () {
                "WitherSkulls", "NautilusShells", "Trident", "HeavyCore", "Sniffers", "ArmorTrims",
            }},
            { "All Advancements 1.21", new () {
                "WitherSkulls", "NautilusShells", "Trident", "Sniffers", "ArmorTrims",
            }},
            { "All Advancements 1.20.5", new () {
                "WitherSkulls", "NautilusShells", "Trident", "Sniffers", "ArmorTrims",
            }},
            { "All Advancements 1.20 v2", new () {
                "WitherSkulls", "NautilusShells", "Trident", "Sniffers", "ArmorTrims",
            }},
            { "All Advancements 1.20", new () {
                "AncientDebris", "WitherSkulls", "NautilusShells", "Trident", "EGap", "ArmorTrims",
            }},
            { "All Advancements 1.19", new () {
                "AncientDebris", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.18", new () {
                "AncientDebris", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.17", new () {
                "Cauldrons", "AncientDebris", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.16.5", new () {
                "AncientDebris", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.16", new () {
                "AncientDebris", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.15", new () {
                "GoldBlocks", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.14", new () {
                "Cats", "GoldBlocks", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.13", new () {
                "GoldBlocks", "WitherSkulls", "NautilusShells", "Trident", "EGap",
            }},
            { "All Advancements 1.12", new () {
                "GoldBlocks", "WitherSkulls", "Monsters", "Biomes", "EGap",
            }},
            { "All Achievements 1.11", new () {
                "GoldBlocks", "WitherSkulls", "Biomes", "EGap",
            }},
            { "All Blocks 1.21", new () {
                "DeepslateEmerald", "HeavyCore", "WitherSkulls", "ShulkerShells", "NautilusShells", "Trident",
            }},
            { "All Blocks 1.20", new () {
                "AncientDebris", "DeepslateEmerald", "WitherSkulls", "ShulkerShells", "NautilusShells", "Trident",
            }},
            { "All Blocks 1.19", new () {
                "AncientDebris", "DeepslateEmerald", "WitherSkulls", "ShulkerShells", "NautilusShells", "Trident",
            }},
            { "All Blocks 1.18", new () {
                "AncientDebris", "DeepslateEmerald", "WitherSkulls", "ShulkerShells", "NautilusShells", "Trident",
            }},
            { "All Blocks 1.16", new () {
                "AncientDebris", "Mycelium", "WitherSkulls", "ShulkerShells", "NautilusShells", "Trident",
            }},
        };

        public bool TryGetCurrentList(out List<string> list) =>
            this.TryGetCurrentList(Tracker.Category.Name, Tracker.Category.CurrentVersion, out list);

        public bool TryGetCurrentList(string category, string version, out List<string> list)
        {
            string key = this.GetKey(category, version);
            return this.Pinned.TryGetValue(key, out list);
        }
        
        public bool TrySetCurrentList(List<UIPinnedObjectiveFrame> frames)
        {
            var names = new List<string>();
            foreach (UIPinnedObjectiveFrame frame in frames)
            {
                if (!string.IsNullOrWhiteSpace(frame.Objective.Name))
                    names.Add(frame.Objective.Name);
            }

            string key = this.GetKey(Tracker.Category.Name, Tracker.CurrentVersion);

            List<string> previous = this.Pinned[key];
            if (!names.SequenceEqual(previous))
            {
                this.Pinned[key] = names;
                return true;
            }
            return false; 
        }

        private string GetKey(string category, string version)
        {
            string key = $"{category} {version}";

            //get the most recent revision of the default list if present
            int revision = 2;
            while (this.Pinned.ContainsKey($"{category} {version} v{revision}"))
            {
                key = $"{category} {version} v{revision}";
                revision++;
            }

            return key;
        }
    }
}
