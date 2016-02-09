using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmoteEnercitiesMessages;

namespace GameplayManager
{
    public class Player
    {
        public string Name;
        public EnercitiesRole Role;
        public Gender Gender;

        public Player(string name, EnercitiesRole role)
        {
            this.Name = name;
            this.Role = role;
            this.Gender = EmoteEnercitiesMessages.Gender.Male;
        }

        public bool IsAI()
        {
            return Role == EnercitiesRole.Mayor;
        }

        public override string ToString()
        {
            return string.Format("'{0}':{1}", Name, Role.ToString());
        }
    }
}
