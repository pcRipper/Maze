using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.MDI
{
    public interface iMdiChild
    {
        public void customizeParent(Form mdiParent);
    }
}
