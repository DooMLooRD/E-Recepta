using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    public class WrongDataHandler : Exception
    {
        public bool WrongDataNotification(List<Prescription> list)
        {
            if (list.Capacity == 0)
                return true;
            return false;
        }
        public WrongDataHandler()
        { }

        public WrongDataHandler(string message)
            : base(message)
        { }

        public WrongDataHandler(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
