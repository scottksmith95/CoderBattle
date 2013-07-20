using CSharp.Coderbits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battler
{
    public class Battle
    {
        private const int DELAY = 1500;

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

        public async Task<BoutResult> FightPersonal()
        {
            await Task.Delay(DELAY);

            var result = new BoutResult() { Category = "Personal" };

            result.Results.Add(FightString("Headline", Fighter1.title, Fighter2.title));
            result.Results.Add(FightString("Location", Fighter1.location, Fighter2.location));
            result.Results.Add(FightString("Website", Fighter1.website_link, Fighter2.website_link));
            result.Results.Add(FightString("Bio", Fighter1.bio, Fighter2.bio));

            result = CalculateWinner(result);

            return result;
        }

        public async Task<BoutResult> FightLanguages()
        {
            return await FightApiModelStat("Languages", Fighter1.top_languages, Fighter2.top_languages);
        }

        public async Task<BoutResult> FightEnvironments()
        {
            return await FightApiModelStat("Environments", Fighter1.top_environments, Fighter2.top_environments);
        }

        public async Task<BoutResult> FightFrameworks()
        {
            return await FightApiModelStat("Frameworks", Fighter1.top_frameworks, Fighter2.top_frameworks);
        }

        public async Task<BoutResult> FightTools()
        {
            return await FightApiModelStat("Tools", Fighter1.top_tools, Fighter2.top_tools);
        }

        public async Task<BoutResult> FightInterests()
        {
            return await FightApiModelStat("Interests", Fighter1.top_interests, Fighter2.top_interests);
        }

        public async Task<BoutResult> FightTraits()
        {
            return await FightApiModelStat("Traits", Fighter1.top_traits, Fighter2.top_traits);
        }

        public async Task<BoutResult> FightAreas()
        {
            return await FightApiModelStat("Areas", Fighter1.top_areas, Fighter2.top_areas);
        }

        public async Task<BoutResult> FightGeneralCounts()
        {
            await Task.Delay(DELAY);

            var result = new BoutResult() { Category = "Statistics" };

            result.Results.Add(FightInt("Views", Fighter1.views, Fighter2.views));
            result.Results.Add(FightInt("Followers", Fighter1.follower_count, Fighter2.follower_count));
            result.Results.Add(FightInt("Following", Fighter1.following_count, Fighter2.following_count));
            result.Results.Add(FightInt("1 bit badges", Fighter1.one_bit_badges, Fighter2.one_bit_badges));
            result.Results.Add(FightInt("8 bit badges", Fighter1.eight_bit_badges, Fighter2.eight_bit_badges));
            result.Results.Add(FightInt("16 bit badges", Fighter1.sixteen_bit_badges, Fighter2.sixteen_bit_badges));
            result.Results.Add(FightInt("32 bit badges", Fighter1.thirty_two_bit_badges, Fighter2.thirty_two_bit_badges));
            result.Results.Add(FightInt("64 bit badges", Fighter1.sixty_four_bit_badges, Fighter2.sixty_four_bit_badges));

            result = CalculateWinner(result);

            return result;
        }

        private BoutResult.BoutMiniResult FightString(string value, string firstValue, string secondValue)
        {
            //TODO - CHANGE?
            var result = new BoutResult.BoutMiniResult()
            {
                Message = value
            };
            if (!string.IsNullOrEmpty(firstValue))
                result.Fighter1Hits = 1;
            if (!string.IsNullOrEmpty(secondValue))
                result.Fighter2Hits = 1;
            return result;

            //if (!string.IsNullOrEmpty(firstValue) && string.IsNullOrEmpty(secondValue))
            //    return new BoutResult.BoutMiniResult() { Message = value, Fighter1Hits = 1, Fighter2Hits = 0 };
            //else if (string.IsNullOrEmpty(firstValue) && !string.IsNullOrEmpty(secondValue))
            //    return new BoutResult.BoutMiniResult() { Message = value, Fighter1Hits = 0, Fighter2Hits = 1 };
            //else
            //    return new BoutResult.BoutMiniResult() { Message = value, Fighter1Hits = 0, Fighter2Hits = 0 };
        }

        private BoutResult.BoutMiniResult FightInt(string value, int firstValue, int secondValue)
        {
            //TODO - CHANGE?
            return new BoutResult.BoutMiniResult() { Message = value, Fighter1Hits = firstValue, Fighter2Hits = secondValue };

            //if (firstValue > secondValue)
            //    return new BoutResult.BoutMiniResult() { Message = value, Fighter1Hits = firstValue - secondValue, Fighter2Hits = 0 };
            //else if (secondValue > firstValue)
            //    return new BoutResult.BoutMiniResult() { Message = value, Fighter1Hits = 0, Fighter2Hits = secondValue - firstValue };
            //else
            //    return new BoutResult.BoutMiniResult() { Message = value, Fighter1Hits = 0, Fighter2Hits = 0 };
        }

        private async Task<BoutResult> FightApiModelStat(string category, List<ApiModel.Stat> firstStats, List<ApiModel.Stat> secondStats)
        {
            await Task.Delay(DELAY);

            var result = new BoutResult() { Category = category };

            result.Results.AddRange(FightSkills(firstStats, secondStats, true));
            result.Results.AddRange(FightSkills(secondStats, firstStats, false));

            result = CalculateWinner(result);

            return result;
        }

        private List<BoutResult.BoutMiniResult> FightSkills(List<ApiModel.Stat> firstFighterSkills, List<ApiModel.Stat> secondFighterSkills, bool fighter1First)
        {
            var result = new List<BoutResult.BoutMiniResult>();

            //Intentionally inefficient to slow down the fight
            foreach (var item1 in firstFighterSkills)
            {
                var found = false;

                foreach (var item2 in secondFighterSkills)
                {
                    if (item1.name.Equals(item2.name))
                    {
                        //TODO - CHANGE?
                        if (fighter1First)
                            result.Add(new BoutResult.BoutMiniResult() { Message = item1.name, Fighter1Hits = item1.count, Fighter2Hits = item2.count });

                        //if (item1.count > item2.count)
                        //{
                        //    if (fighter1First)
                        //        result.Add(new BoutResult.BoutMiniResult() { Message = item1.name, Fighter1Hits = item1.count - item2.count, Fighter2Hits = 0 });
                        //    else
                        //        result.Add(new BoutResult.BoutMiniResult() { Message = item1.name, Fighter1Hits = 0, Fighter2Hits = item1.count - item2.count });
                        //}
                        //else if (item1.count == item2.count)
                        //{
                        //    result.Add(new BoutResult.BoutMiniResult() { Message = item1.name, Fighter1Hits = 0, Fighter2Hits = 0 });
                        //}

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (fighter1First)
                        result.Add(new BoutResult.BoutMiniResult() { Message = item1.name, Fighter1Hits = item1.count, Fighter2Hits = 0 });
                    else
                        result.Add(new BoutResult.BoutMiniResult() { Message = item1.name, Fighter1Hits = 0, Fighter2Hits = item1.count });
                }
            }

            return result;
        }

        private BoutResult CalculateWinner(BoutResult boutResult)
        {
            var Fighter1Count = 0;
            var Fighter2Count = 0;
            foreach (var item in boutResult.Results)
            {
                if (item.Fighter1Hits > item.Fighter2Hits)
                    Fighter1Count += item.Fighter1Hits;
                else if (item.Fighter2Hits > item.Fighter1Hits)
                    Fighter2Count += item.Fighter2Hits;
            }

            if (Fighter1Count > Fighter2Count)
                boutResult.Fighter1Won = true;
            else if (Fighter2Count > Fighter1Count)
                boutResult.Fighter2Won = true;
            else
            {
                boutResult.Fighter1Won = true;
            }

            return boutResult;
        }
    }
}
