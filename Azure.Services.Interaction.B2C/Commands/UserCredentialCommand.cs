namespace Azure.Services.Interaction.B2C.Commands
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class UserCredentialCommand
    {
        [Required]
        public string NewPassword { get; set; }
    }
}
