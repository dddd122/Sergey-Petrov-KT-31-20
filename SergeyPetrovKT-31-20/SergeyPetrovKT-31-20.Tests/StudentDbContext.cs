using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SergeyPetrovKT_31_20.Tests
{
    internal class StudentDbContext
    {
        private object dbContextOptions;

        public StudentDbContext(object dbContextOptions)
        {
            this.dbContextOptions = dbContextOptions;
        }
    }
}
