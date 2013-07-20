﻿using CSharp.Coderbits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battler
{
    public class Battle
    {
        private ApiModel Fighter1;
        private ApiModel Fighter2;

        public async Task Init(string fighter1, string fighter2)
        {
            Api coderbitsApi = new Api();

            var fighterModel1Task = coderbitsApi.GetProfileAsync(fighter1);
            var fighterModel2Task = coderbitsApi.GetProfileAsync(fighter2);

            await Task.WhenAll(fighterModel1Task, fighterModel2Task);

            Fighter1 = fighterModel1Task.Result;
            Fighter2 = fighterModel2Task.Result;
        }

        public async Task<List<BoutResult>> FightLanguages()
        {
            await Task.Delay(1000);

            var result = new List<BoutResult>();

            result.AddRange(FightSkills(Fighter1.top_languages, Fighter2.top_languages, true));
            result.AddRange(FightSkills(Fighter2.top_languages, Fighter1.top_languages, false));

            return result;
        }

        public async Task<List<BoutResult>> FightEnvironments()
        {
            await Task.Delay(1000);

            var result = new List<BoutResult>();

            result.AddRange(FightSkills(Fighter1.top_environments, Fighter2.top_environments, true));
            result.AddRange(FightSkills(Fighter2.top_environments, Fighter1.top_environments, false));

            return result;
        }

        public async Task<List<BoutResult>> FightFrameworks()
        {
            await Task.Delay(1000);

            var result = new List<BoutResult>();

            result.AddRange(FightSkills(Fighter1.top_frameworks, Fighter2.top_frameworks, true));
            result.AddRange(FightSkills(Fighter2.top_frameworks, Fighter1.top_frameworks, false));

            return result;
        }

        public async Task<List<BoutResult>> FightTools()
        {
            await Task.Delay(1000);

            var result = new List<BoutResult>();

            result.AddRange(FightSkills(Fighter1.top_tools, Fighter2.top_tools, true));
            result.AddRange(FightSkills(Fighter2.top_tools, Fighter1.top_tools, false));

            return result;
        }

        public async Task<List<BoutResult>> FightInterests()
        {
            await Task.Delay(1000);

            var result = new List<BoutResult>();

            result.AddRange(FightSkills(Fighter1.top_interests, Fighter2.top_interests, true));
            result.AddRange(FightSkills(Fighter2.top_interests, Fighter1.top_interests, false));

            return result;
        }

        public async Task<List<BoutResult>> FightTraits()
        {
            await Task.Delay(1000);

            var result = new List<BoutResult>();

            result.AddRange(FightSkills(Fighter1.top_traits, Fighter2.top_traits, true));
            result.AddRange(FightSkills(Fighter2.top_traits, Fighter1.top_traits, false));

            return result;
        }

        public async Task<List<BoutResult>> FightAreas()
        {
            await Task.Delay(1000);

            var result = new List<BoutResult>();

            result.AddRange(FightSkills(Fighter1.top_areas, Fighter2.top_areas, true));
            result.AddRange(FightSkills(Fighter2.top_areas, Fighter1.top_areas, false));

            return result;
        }

        private List<BoutResult> FightSkills(List<ApiModel.Stat> FirstFighterSkills, List<ApiModel.Stat> SecondFighterSkills, bool Fighter1First)
        {
            var result = new List<BoutResult>();

            //Intentionally inefficient to slow down the fight
            foreach (var item1 in FirstFighterSkills)
            {
                var found = false;

                foreach (var item2 in SecondFighterSkills)
                {
                    if (item1.name.Equals(item2.name))
                    {
                        if (item1.count > item2.count)
                        {
                            if (Fighter1First)
                                result.Add(new BoutResult() { Message = item1.name, Fighter1Hits = item1.count - item2.count, Fighter2Hits = 0 });
                            else
                                result.Add(new BoutResult() { Message = item1.name, Fighter1Hits = 0, Fighter2Hits = item1.count - item2.count });
                        }

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (Fighter1First)
                        result.Add(new BoutResult() { Message = item1.name, Fighter1Hits = item1.count, Fighter2Hits = 0 });
                    else
                        result.Add(new BoutResult() { Message = item1.name, Fighter1Hits = 0, Fighter2Hits = item1.count });
                }
            }

            return result;
        }
    }
}
