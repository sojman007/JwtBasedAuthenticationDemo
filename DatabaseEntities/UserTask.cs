using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthenticationAndIdentityDemo.DatabaseEntities
{
    public class UserTask
    {
        //configure with fluent
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(30)]
        public string TaskName { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public bool IsDone { get; set; } = false;
        

        public DateTime CompletedDateTime { get; set; }

        // which should act as the foreign key relationship to the User that owns this task
        public CustomUser CustomUser { get; set; } 

    }
}
