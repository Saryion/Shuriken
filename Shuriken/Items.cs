using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using StatCurves;

using Shuriken.API;

namespace Shuriken
{
    /// <summary>
    /// A bunch of useful methods for working with items.
    /// </summary>
    public class Items
    {
        public static List<Item> Map;

        /// <summary>
        /// Caches a collection of items from the API and deserializes
        /// them into a list of Item. Saved in Items.Map.
        /// </summary>
        /// <returns>Boolean</returns>
        public static async Task<bool> CacheItemsFromAPI()
        {
            var data = await APIManager.SendReq(APITypes.ITEMS);
            if (data == null) return false;

            var items = JsonConvert.DeserializeObject<List<Item>>(data);
            Map = items;
            
            return true;
        }
        
        /// <summary>
        /// Equips an item onto the entity, either by ID or name of item.
        /// </summary>
        /// <param name="source">ID or Name</param>
        /// <param name="entity">Player or NPC, leave null to default to the player.</param>
        public static async void EquipItem(object source, Entity entity = null)
        {
            if (Map == null) await CacheItemsFromAPI();

            Item item = await Task.Run(() => GetItem(source));
            if (item == null) return;

            if (entity == null) entity = Entities.Instance.me;
            
            entity.baseAsset.equips[item.EquipSlot] = item;
            entity.UpdateAsset(entity.baseAsset);
        }

        /// <summary>
        /// Equips a list of items onto an entity, either by ID or name of item.
        /// </summary>
        /// <param name="source">List of ID's or Item names.</param>
        /// <param name="entity">Player or NPC, leave null to default to the player.</param>
        public static async void EquipItems(List<object> source, Entity entity = null)
        {
            if (Map == null) await CacheItemsFromAPI();
            
            if (entity == null) entity = Entities.Instance.me;
            
            foreach (var i in source)
            {
                Item item = await Task.Run(() => GetItem(i));
                if (item == null) continue;
                
                entity.baseAsset.equips[item.EquipSlot] = item;
            }
            
            entity.UpdateAsset(entity.baseAsset);
        }

        /// <summary>
        /// Returns the Item by ID or name.
        /// </summary>
        /// <param name="source">ID or Name</param>
        /// <returns>Item</returns>
        public static async Task<Item> GetItem(object source)
        {
            if (Map == null) await CacheItemsFromAPI();
            Item item;
            try
            {
                item = Map?.Find(i => i.ID == (int)source);
            }
            catch
            {
                var name = (string)source;
                item = Map?.Find(i => i.Name.ToLower() == name.ToLower());
            }
            
            return item;
        }

        /// <summary>
        /// Returns a list of all items by equip slot or none.
        /// </summary>
        /// <param name="equipSlot">The equip slot of the items you would like to retrieve.
        /// Leave null to return a list of all items.</param>
        /// <returns>List of Item</returns>
        public static async Task<List<Item>> GetItems(EquipItemSlot equipSlot = EquipItemSlot.None)
        {
            if (Map == null) await CacheItemsFromAPI();
            if (equipSlot == EquipItemSlot.None) return Map;
            
            var items = new List<Item>();

            try
            {
                foreach (Item item in Map)
                {
                    if (item.EquipSlot == equipSlot) items.Add(item);
                }
            }
            catch (NullReferenceException e)
            {
                return null;
            }

            return items;
        }
        
        /// <summary>
        /// Returns if the item is custom color compatible.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Boolean</returns>
        public static bool IsCC(Item item)
        {
            return item.ColorR != null;
        }

        /// <summary>
        /// Returns a list of all items which are capable of custom color.
        /// </summary>
        /// <returns>List of Item</returns>
        public static async Task<List<Item>> GetCustomColorItems()
        {
            if (Map == null) await CacheItemsFromAPI();

            var items = new List<Item>();

            try
            {
                foreach (Item item in Map)
                {
                    if (IsCC(item)) items.Add(item);
                }
            }
            catch (NullReferenceException e)
            {
                return null;
            }

            return items;
        }

        /// <summary>
        /// Applies a custom color to the item specified in hex codes,
        /// R G B being separate color strips on the item.
        /// </summary>
        /// <param name="item">The Item which you would like to custom color.</param>
        /// <param name="r">Hex code or leave null to use original color.</param>
        /// <param name="g">Hex code or leave null to use original color.</param>
        /// <param name="b">Hex code or leave null to use original color.</param>
        /// <returns>CC Item</returns>
        public static Item CustomColor(Item item, string r = null, string g = null, string b = null)
        {
            if (!IsCC(item)) return null;

            item.ColorR = r ?? item.ColorR;
            item.ColorG = g ?? item.ColorG;
            item.ColorB = b ?? item.ColorB;

            return item;
        }
    }
}