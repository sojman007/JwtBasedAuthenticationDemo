using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthenticationAndIdentityDemo.DTOs
{
    public  class AuthModels
    {
        [Required]
        
        public virtual string Email { get; set; }
        
        [Required]
        [MaxLength(24)]
        public virtual string Password { get; set; }
    }




    public class LogInModel:AuthModels
    {
        [Required]
        public override string Password { get => base.Password; set => base.Password = value; }
    }


   public class SignInModel: AuthModels
    {
        [Required]
        public string UserName { get; set; } 
    }
}
