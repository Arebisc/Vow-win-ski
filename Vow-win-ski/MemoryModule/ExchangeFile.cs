using System.Collections.Generic;
using System.Linq;

namespace Vow_win_ski.MemoryModule
{
    class ExchangeFile
    {
        private List<ExchangeFileProcess> _takenProcesses;

        public void PlaceIntoMemory(ExchangeFileProcess data)
        {
            _takenProcesses.Add(data);
        }

        public void RemoveFromMemory(int id)
        {
            _takenProcesses = _takenProcesses
                .Select(x => x)
                .Where(x => x.TakenProcessPages.Id != id)  
                .ToList();
        }
    }
}
