using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Shuriken.API;

using StatCurves;

namespace Shuriken
{
    public class Items
    {
        public static List<Item> Map;

        /// <summary>
        /// Caches a collection of Items from the API and deserializes
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
        /// Equips an item onto the entity by either ID or name of item.
        /// </summary>
        /// <param name="source">ID or Name</param>
        /// <param name="entity">Player or NPC</param>
        public static async void EquipItem(object source, Entity entity)
        {
            if (Map == null) await CacheItemsFromAPI();

            Item item = await Task.Run(() => GetItem(source));
            if (item == null) return;
            
            entity.baseAsset.equips[item.EquipSlot] = item;
            entity.UpdateAsset(entity.baseAsset);
        }

        /// <summary>
        /// Equips a list of items onto an entity, either by ID or name of item.
        /// </summary>
        /// <param name="source">List of ID's or Item names.</param>
        /// <param name="entity">Player or NPC</param>
        public static async void EquipItems(List<object> source, Entity entity)
        {
            if (Map == null) await CacheItemsFromAPI();
            
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
        /// Returns a list of all items of specified equip slot.
        /// </summary>
        /// <param name="equipSlot">The equip slot of the items you would like to retrieve.</param>
        /// <returns>List of Item</returns>
        public static async Task<List<Item>> GetItemsByEquipSlot(EquipItemSlot equipSlot)
        {
            if (Map == null) await CacheItemsFromAPI();

            var items = new List<Item>();
            foreach (Item item in Map)
            {
                if (item.EquipSlot == equipSlot) items.Add(item);
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
            return item.ColorR != "" || item.ColorR != null;
        }

        /// <summary>
        /// Returns a list of all items which are capable of custom color.
        /// </summary>
        /// <returns>List of Item</returns>
        public static async Task<List<Item>> GetCCItems()
        {
            if (Map == null) await CacheItemsFromAPI();

            var items = new List<Item>();
            foreach (Item item in Map)
            {
                if (IsCC(item)) items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Applies a custom color to the item specified in hex codes,
        /// R G B being separate color strips on the item.
        /// </summary>
        /// <param name="equipSlot">Which item slot to apply the color</param>
        /// <param name="r">Hex Code</param>
        /// <param name="g">Hex Code</param>
        /// <param name="b">Hex Code</param>
        /// <param name="entity">Player or NPC</param>
        public static async void CustomColor(EquipItemSlot equipSlot, string r, string g, string b, Entity entity)
        {
            Item item = await GetItem(entity.baseAsset.equips[equipSlot].ID);

            if (!IsCC(item)) return;

            item.ColorR = r;
            item.ColorG = g;
            item.ColorB = b;

            entity.baseAsset.equips[equipSlot] = item;
            entity.UpdateAsset(entity.baseAsset);
        }
    }
}