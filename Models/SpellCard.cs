using MonsterTradingCardsGame.Enums;
using System;

namespace MonsterTradingCardsGame.Models
{
    /// <summary>
    /// Represents a spell card in the game.
    /// </summary>
    public class SpellCard : Card
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCard"/> class with the specified name, damage and element type.
        /// Creates also an instance of the <see cref="Card"/> parent class.
        /// </summary>
        /// <param name="name">The name of the spell card.</param>
        /// <param name="damage">The damage value of the spell card.</param>
        /// <param name="elementType">The element type of the spell card.</param>
        public SpellCard(string name, float damage, ElementTypeEnum elementType)
            : base(name, damage, elementType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellCard"/> class with the specified name, damage and element type.
        /// Creates also an instance of the <see cref="Card"/> parent class.
        /// </summary>
        /// <param name="id">The unique identifier of the spell card.</param>
        /// <param name="name">The name of the spell card.</param>
        /// <param name="damage">The damage value of the spell card.</param>
        /// <param name="elementType">The element type of the spell card.</param>
        public SpellCard(Guid id, string name, int damage, ElementTypeEnum elementType)
            : base(id, name, damage, elementType)
        {
        }
    }
}