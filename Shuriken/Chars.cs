using System;
using System.Collections.Generic;

namespace Shuriken
{
    /// <summary>
    /// A bunch of useful methods for working with characters.
    /// </summary>
    public class Chars
    {
        public static List<Player> Players => Entities.Instance.PlayerList;
        public static List<NPC> NPCs => Entities.Instance.NpcList;

        /// <summary>
        /// Gets a Player by ID or Name if exists.
        /// </summary>
        /// <param name="source">Player ID or Name</param>
        /// <returns>Player</returns>
        public static Player GetPlayer(object source)
        {
            Player player;
            try
            {
                player = Players.Find(p => p.ID == (int)source);
            }
            catch
            {
                player = Players.Find(p => p.name == (string)source);
            }

            return player;
        }
        
        /// <summary>
        /// Gets a NPC by NPCID or Name if exists.
        /// </summary>
        /// <param name="source">NPC NPCID or Name</param>
        /// <returns>NPC</returns>
        public static NPC GetNPC(object source)
        {
            NPC npc;
            try
            {
                npc = NPCs.Find(n => n.NPCID == (int)source);
            }
            catch
            {
                npc = NPCs.Find(n => n.name == (string)source);
            }

            return npc;
        }
        
        /// <summary>
        /// Checks if the Player is in your instance.
        /// </summary>
        /// <param name="name">Player name</param>
        /// <returns>Boolean</returns>
        public static bool IsPlayerInInstance(string name)
        {
            return Players.Find(p => p.name == name) != null;
        }
        
        /// <summary>
        /// Checks if the NPC is in your instance.
        /// </summary>
        /// <param name="name">NPC name</param>
        /// <returns>Boolean</returns>
        public static bool IsNPCInInstance(string name)
        {
            // Needs to trim because some NPCs have trailing whitespace.
            return NPCs.Find(p => p.name.Trim() == name) != null;
        }

        /// <summary>
        /// Hides the players wrapper and optionally their pets from your screen.
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="hidePets">Boolean</param>
        public static void HidePlayer(Player player, bool hidePets = true)
        {
            player.wrapper.SetActive(false);
            player.namePlate.gameObject.SetActive(false);
            
            if (hidePets && player.petGO != null) player.petGO.SetActive(false);
        }
        
        /// <summary>
        /// Shows the players wrapper if they have previously been hidden..
        /// </summary>
        /// <param name="player">Player</param>
        public static void ShowPlayer(Player player)
        {
            player.wrapper.SetActive(true);
            player.namePlate.gameObject.SetActive(true);
            
            if (player.petGO != null && !player.petGO.activeSelf) player.petGO.SetActive(true);
        }

        /// <summary>
        /// Checks if the player is Staff.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Boolean</returns>
        public static bool IsStaff(Player player)
        {
            return player.AccessLevel >= AccessLevels.Tester;
        }
        
        /// <summary>
        /// Checks if the player is a Tester member.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Boolean</returns>
        public static bool IsTester(Player player)
        {
            return player.AccessLevel == AccessLevels.Tester || player.AccessLevel == AccessLevels.WhiteHat;
        }
        
        /// <summary>
        /// Checks if the player is a Moderator member.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Boolean</returns>
        public static bool IsMod(Player player)
        {
            return player.AccessLevel == AccessLevels.Moderator;
        }
        
        /// <summary>
        /// Checks if the player is a Game Developer member.
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Boolean</returns>
        public static bool IsDev(Player player)
        {
            return player.AccessLevel >= AccessLevels.Admin;
        }
    }
}