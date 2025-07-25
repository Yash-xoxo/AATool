﻿using AATool.Configuration;
using AATool.Data.Objectives.Complex;
using AATool.Data.Progress;
using AATool.Net;
using AATool.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace AATool.Data.Objectives
{
    public class CriteriaSet
    {
        public Uuid ClosestToCompletion { get; private set; }

        public readonly Dictionary<string, Criterion> All;
        public readonly Dictionary<Uuid, int> Progress;
        public readonly Advancement Owner;
        public readonly string Goal;

        public bool Any => this.All.Any();
        public int Count => this.All.Count;
        public int MostCompleted => this.NumberCompletedBy(this.ClosestToCompletion);

        public CriteriaSet(XmlNode node, Advancement owner)
        {
            this.All = new Dictionary<string, Criterion>();
            this.Progress = new Dictionary<Uuid, int>();
            this.Owner = owner;
            this.ClosestToCompletion = Uuid.Empty;
            this.Goal = XmlObject.Attribute(node, "goal", "Completed");

            if (node is null)
                return;
            
            if (this.Owner.Id is ArmorTrims.AdvancementId or ArmorTrims.CategoryId)
            {
                foreach (XmlNode criterionNode in node.ChildNodes)
                {
                    var criterion = new ArmorTrimCriterion(criterionNode, owner);
                    this.All[criterion.Id] = criterion;
                }
            }
            else
            {
                foreach (XmlNode criterionNode in node.ChildNodes)
                {
                    var criterion = new Criterion(criterionNode, owner);
                    this.All[criterion.Id] = criterion;
                }
            }
        }

        public bool Contains(string criterion) =>
            this.All.ContainsKey(criterion);

        public int NumberCompletedBy(Uuid player)
        {
            if (Config.Tracking.ManualChecklistMode)
            {
                return this.Progress.FirstOrDefault().Value;
            }
            else
            {
                return this.Progress.TryGetValue(player, out int completed) ? completed : 0;
            }
        }
            

        public int PercentCompletedBy(Uuid player)
        {
            if (this.Count is 0)
                return 0;

            float properFraction = (float)this.NumberCompletedBy(player) / this.Count;
            return (int)(properFraction * 100);
        }
        
        public void UpdateStates(WorldState progress)
        {
            if (!this.Any)
                return;

            //update all criteria in this group and count them
            this.Progress.Clear();
            foreach (Criterion criterion in this.All.Values)
            {
                criterion.UpdateState(progress);
                foreach (Completion completion in criterion.Completions)
                {
                    this.Progress.TryGetValue(completion.Player, out int current);
                    this.Progress[completion.Player] = current + 1;
                }
            }
            this.FindPlayerWithMost(progress);
        }

        public void FindPlayerWithMost(WorldState progress)
        {
            if (!this.Any)
                return;

            var mostCompleted = new KeyValuePair<Uuid, int>(Uuid.Empty, 0);
            foreach (KeyValuePair<Uuid, int> player in this.Progress)
            {
                if (player.Value >= mostCompleted.Value)
                    mostCompleted = player;
            }

            if (mostCompleted.Key == Uuid.Empty && progress.Players.Any())
                mostCompleted = new(progress.Players.First().Key, 0);

            this.ClosestToCompletion = mostCompleted.Key;
        }

        public void CloneCriteria(Dictionary<(string, string), Criterion> dictionary)
        {
            //copy criteria to passed list
            foreach (KeyValuePair<string, Criterion> criterion in this.All)
                dictionary[(this.Owner.Id, criterion.Key)] = criterion.Value;
        }
    }
}