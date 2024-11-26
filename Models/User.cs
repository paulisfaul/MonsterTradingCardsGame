using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Enums;
using MonsterTradingCardsGame.Models.Base;

namespace MonsterTradingCardsGame.Models
{
    public class User : BaseModel
    {
        public UserCredentials UserCredentials { get; private set; }

        public UserData UserData { get; private set; }


        ///// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with username and the role.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="role">The role of the new user.</param>
        public User(string username,RoleEnum role)
        {
            Id = Guid.NewGuid();
            UserCredentials = new UserCredentials(username, role);
            UserData = new UserData();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified id, username, role, lastLoginAt, createdAt, name bio and image.
        /// This constructor is used when the user already exists in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="lastLoginAt">The last login date and time of the user, if available.</param>
        /// <param name="createdAt">The creation date and time of the user.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="bio">The biography of the user.</param>
        /// <param name="image">The image URL of the user.</param>
        public User(Guid id, string username, RoleEnum role, DateTime? lastLoginAt, DateTime createdAt, string name, string bio, string image)
        {
            Id = id;
            UserCredentials = new UserCredentials(username, role);
            UserData = new UserData(name, bio, image, lastLoginAt, createdAt);
        }

        public override string ToString()
        {
            return $@"
User Details:
---------------
Id: {Id}
Credentials:{UserCredentials}
Data:{UserData}";
        }
    }
}