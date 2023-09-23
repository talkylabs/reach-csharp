using System.Net;

namespace Reach.Types {

    public class EmptyUri : System.Uri 
    {
        public static readonly string Uri = "https://this.is.empty.url";

        public EmptyUri() : base(Uri) {}
    }

}
