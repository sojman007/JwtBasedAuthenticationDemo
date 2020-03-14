using JwtAuthenticationAndIdentityDemo.DatabaseEntities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace JwtAuthenticationAndIdentityDemo
{
    public class CustomUser:IdentityUser
    {

        //Constrain:  A User can have many tasks assigned to his name
        public IEnumerable<UserTask> Tasks { get; set; }

    }

}