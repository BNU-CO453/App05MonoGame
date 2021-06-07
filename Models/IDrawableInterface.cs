using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App05MonoGame.Models
{
    public interface IDrawableInterface
    {
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
