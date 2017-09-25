using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Construction
{
    public interface IConstructionService
    {
        IList<Mine> GetMinesToBuild(int planetId);
    }
}
