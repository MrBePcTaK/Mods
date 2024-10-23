using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

[assembly: ModInfo("Death Waypoints", Version = "1.0.3", Side = "Client",
    Description = "Automatically creates a waypoint at the place of death",
    Website = "https://mods.vintagestory.at/deathwaypoints",
    Authors = new[] { "DArkHekRoMaNT" })]
[assembly: ModDependency("game")] //tested on 1.14.10, but theoretically can work on any version, including below


namespace DeathWaypoints
{
    public class Core : ModSystem
    {
        private int lastEntityDead = 0;
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Event.PlayerEntitySpawn += (IClientPlayer byPlayer) =>
            {
                Entity entity = api.World.Player.Entity;
                entity.WatchedAttributes.RegisterModifiedListener("entityDead", () =>
                {
                    if (lastEntityDead == 1)
                    {
                        lastEntityDead = entity.WatchedAttributes.GetInt("entityDead");
                    }
                    else if (entity.WatchedAttributes.GetInt("entityDead") == 1)
                    {
                        api.SendChatMessage(string.Format(
                            "/waypoint addati {0} ={1} ={2} ={3} {4} {5} Death: {6}",

                            //icon - circle, bee, cave, home, ladder, pick, rocks, ruins, spiral, star1, star2, trader, vessel
                            "bee", 

                            (int)entity.SidedPos.X, (int)entity.SidedPos.Y, (int)entity.SidedPos.Z,

                            //pinned - true, false
                            true,

                            //color - https://www.99colors.net/dot-net-colors"
                            "crimson",

                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        ));
                        lastEntityDead = 1;
                    }
                });
            };
        }
    }
}