using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Network;

namespace TestMod
{
    public class ModEntry : Mod
    {
        private List<StardewValley.Object> stones;
        private Texture2D pixelTexture;

        public override void Entry(IModHelper helper)
        {
            pixelTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            var colorArray = Enumerable.Range(0, 1).Select(i => Color.White).ToArray();
            pixelTexture.SetData(colorArray);

            stones = new List<StardewValley.Object>();
            helper.Events.Display.RenderedWorld += OnRenderedWorld;
            helper.Events.Player.Warped += OnWarped;
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (!e.IsLocalPlayer)
            {
                return;
            }

            stones.Clear();

            if (e.NewLocation is MineShaft)
            {
                FindStones(e.NewLocation);
            }
        }

        private void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (!Context.IsWorldReady)
            {
                return;
            }

            if (stones.Count > 0)
            {
                foreach (var stone in stones)
                {
                    DrawRectangle(stone.GetBoundingBox());
                }
            }
        }

        private void FindStones(GameLocation mine)
        {
            foreach (var pair in mine.Objects.Pairs.Where(pair => pair.Value.Name.Equals("Stone")))
            {
                stones.Add(pair.Value);
            }
        }

        private void DrawRectangle(Rectangle rect)
        {
            rect = Game1.GlobalToLocal(Game1.viewport, rect);

            Game1.spriteBatch.Draw(pixelTexture, rect, Color.White);
        }
    }
}
