using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Memory
{
    class ExchangeFile
    {
        private List<ExchangeFileProcess> TakenProcesses;

        public void PlaceIntoMemory(ExchangeFileProcess data)
        {
            TakenProcesses.Add(data);
        }

        public void RemoveFromMemory(int id)
        {
            TakenProcesses = TakenProcesses
                .Select(x => x)
                .Where(x => x.TakenProcessPages.Id != id)  
                .ToList();
        }
    }
}
