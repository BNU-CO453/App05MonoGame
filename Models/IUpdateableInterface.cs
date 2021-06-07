using Microsoft.Xna.Framework;

namespace App05MonoGame.Models
{
    public interface IUpdateableInterface
    {
        public void Update(GameTime gametTime);

        public bool HasCollided(Sprite other);
    }
}
