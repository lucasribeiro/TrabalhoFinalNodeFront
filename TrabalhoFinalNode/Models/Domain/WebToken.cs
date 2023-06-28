using MvcCoreTutorial.Models.Domain;

namespace TrabalhoFinalNode.Models.Domain
{
    public class WebToken
    {
        public Login usuario { get; set; }
        public string token { get; set; }
    }
}
