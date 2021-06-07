using Microsoft.Xna.Framework;

namespace App05MonoGame.Models
{
    /// <summary>
    /// This method will update any game object that
    /// is updatable
    /// </summary>
    public interface IUpdateableInterface
    {
        public void Update(GameTime gametTime);

    }
}
