using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MonsterTradingCardsGame.Models.Base;

namespace MonsterTradingCardsGame.Models
{
    public class Deck : IEnumerable<Card>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        private List<Card> _cards;

        public Deck(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            _cards = new List<Card>();
        }

        public Deck(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
            _cards = new List<Card>();

        }
        public Deck()
        {
            _cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            if (_cards.Count > 4)
            {
                throw new Exception("Deck is full.");
            }
            _cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (_cards.Contains(card))
            {
                _cards.Remove(card);
            }
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Deck Id: {Id}");
            sb.AppendLine("Cards:");
            foreach (var card in _cards)
            { 
                sb.AppendLine(card.ToString());
            }
            
            
            return sb.ToString();
        }
    }
}
