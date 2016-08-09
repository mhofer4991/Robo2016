using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLibrary
{
    public class TravelResponse
    {
        public TravelResponse()
        {
        }

        public int ID
        {
            get;
            set;
        }

        public bool Accepted
        {
            get;
            set;
        }

        public Route CreatedRoute
        {
            get;
            set;
        }
    }
}
