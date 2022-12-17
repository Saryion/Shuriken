using System;
using System.Collections.Generic;

namespace Shuriken
{
    /// <summary>
    /// A bunch of useful methods for working with characters.
    /// </summary>
    public class Chars
    {
        private static List<Player> Players => Entities.Instance.PlayerList;
        private static List<NPC> NPCs => Entities.Instance.NpcList;

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
    }
}