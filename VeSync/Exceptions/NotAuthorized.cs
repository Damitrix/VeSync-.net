using System;
using System.Collections.Generic;
using System.Text;


    [Serializable]
    class NotAuthorized : Exception
    {
        public NotAuthorized() : base("The credenitals you have provided have been declined. Please check your credentials and try again.")
        {

        }

        public NotAuthorized(string additional) : base(additional)
        {

        }
    }

