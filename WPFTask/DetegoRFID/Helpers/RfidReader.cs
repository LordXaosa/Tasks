using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetegoRFID.Helpers
{
    public class RfidReader
    {
        private static readonly Rfid.RfidReader _reader = new Rfid.RfidReader();
        public static Rfid.RfidReader Instance => _reader;
    }
}
