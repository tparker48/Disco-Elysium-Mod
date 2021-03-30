﻿
namespace Disco_Explorer_Mod
{
    public static class GeneralUtilities
    {
        // Set abilityType to given value [1,6]
        public static void SetAbilityLevel(Sunshine.Metric.AbilityType abilityType, int value)

            Sunshine.Metric.CharacterSheet character = Voidforge.SingletonComponent<World>.Singleton.you;
            Sunshine.Metric.Ability ability = character.GetAbility(abilityType);
            Sunshine.Metric.Modifier modifier = ability.GetModifierOfType(Sunshine.Metric.ModifierType.INITIAL_DICE);
            if (modifier == null)
            {
                modifier = new Sunshine.Metric.Modifier(Sunshine.Metric.ModifierType.INITIAL_DICE, 1, null, Sunshine.Metric.SkillType.NONE);
                ability.Add(modifier);
            }

            Sunshine.Metric.Modifier modifier2 = modifier;
            modifier2.Amount = value;
            character.Recalc();
        }

        // adds givent amount of skill points
        public static void SetSkillPoints(int amount)
        {
            if (amount >= 0 && amount <= 100)
            {
                LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.SkillPoints = amount;
            }
        }

        public static void SetMoney(int amount)
        {
            if (amount >= 0 && amount <= 999)
            {
                amount *= 100;
                int currentBalance = LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.Money;
                LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.Money = amount;
                NotificationSystem.NotificationUtil.ShowMoney(amount - currentBalance);
            }
        }

        // adds every piece of clothing to the player's inventory
        public static void AddAllClothes()
        {
            new ThoughtsAndItemsTests().AddAllClothes();
        }

        // makes every thought researchable
        public static void AddAllThoughts()
        {
            new ThoughtsAndItemsTests().AddAllThoughts();
        }
        public static void ToggleHud()
        {
            Sunshine.Views.HudToggle.Singleton.ToggleVisibility();
        }

        // additions suggested and provided by : T1eru @ Nexus Mods -----------------------------------------
        public static void FinishAllThoughts()
        {
            new ThoughtsAndItemsTests().FinishThoughts();
        }

        // requires a rep string designating one of the following repuation types:
        // [apocalypse_cop, boring_cop, superstar_cop, sorry_cop, communist, revacholian_nationhood, ultraliberal, moralist,
        // honour, the_destroyer, torque_dork, art_cop, remote_viewer, suicide_cop]
        //public static void ReputationGrows(string rep)
        //{
        //    Sunshine.Dialogue.KarmaLuaFunctions.ReputationGrows(rep);
        //}

        public static void UnlockAllWhiteChecks()
        {
            new DialogueTests().UnlockAllWhiteChecks();
        }
        // ---------------------------------------------------------------------------------------------------
    }
}
