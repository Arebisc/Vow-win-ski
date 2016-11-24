using System;
using System.Collections.Generic;
using System.Linq;
using Vow_win_ski.Processes;

namespace Vow_win_ski.MemoryModule
{
    public class Memory
    {
        private const int FramesCount = 16;
        private const int FramesSize = 16;
        private ExchangeFile _exchangeFile;
        private FifoQueue _fifoQueue;
        private PhysicalMemory _physicalMemory;
        private FreeFramesList _freeFramesList;
        public List<ProcessPages> ProcessPages;

        public Memory()
        {
            _exchangeFile = new ExchangeFile();
            _fifoQueue = new FifoQueue();
            _physicalMemory = new PhysicalMemory(FramesCount, FramesSize);
            _freeFramesList = new FreeFramesList(FramesCount);
            ProcessPages = new List<ProcessPages>();
        }

        private char GetByte(int id, int index)
        {
            //Ustalenie numeru strony i offsetu
            int pages = index/FramesSize;
            int offset = index%FramesSize;
            //Poszukiwanie tablicy stron do danego procesu
            var process = ProcessPages.Select(x => x).SingleOrDefault(x => x.Id == id);
            //sprawdzenie czy dana strona znajduje sie w pamieci
            if (process.IsPageInMemory(pages))
            {
                //Zwrocenie danego bajtu
                return _physicalMemory.GetFrame(process.ReadFrameNumber(pages)).GetByte(offset);
            }
            else
            {
                if (_freeFramesList.FreeFramesCount == 0)
                {
                    //pobranie najstarszego numeru ramki
                    var indexprocessdata = _fifoQueue.RemoveFrame();
                    //przypisanie do tablicy stron z ktorego pobrano ramke ze juz nie ma jej w pamieci
                    foreach (ProcessPages processPages in ProcessPages)
                    {
                        if (processPages.Id == indexprocessdata.Id)
                        {
                            processPages.RemoveFrame(indexprocessdata.FrameNumber);
                        }
                    }
                    //dodanie ramki do list
                    _freeFramesList.AddToList(indexprocessdata.FrameNumber);
                    //pobranie z listy wolnych ramek najstarszej ramki
                    int newFrameIndex = _freeFramesList.RemoveFromList();
                    //dodanie do kolejki FIFO nowego numeru ramki
                    _fifoQueue.AddFrame(new FrameData()
                    {
                        FrameNumber = indexprocessdata.FrameNumber,
                        Id = id
                    });
                    //wpisanie do pamieci nowej strony
                    _physicalMemory.SetFrame(newFrameIndex, _exchangeFile.ReadFromExchangeFile(id, pages));
                    //wpisanie do tablicy stron,ze strona znajduje sie w pamieci
                    foreach (var processPage in ProcessPages)
                    {
                        if (processPage.Id == id)
                            processPage.AddFrame(pages, newFrameIndex);
                    }
                    //zwrocenie bajtu
                    return _physicalMemory.GetFrame(newFrameIndex).GetByte(offset);
                }
                else
                {
                    //pobranie nowej strony
                    var newFrameIndex = _freeFramesList.RemoveFromList();
                    //dodanie do kolejki FIFO nowego numeru ramki
                    _fifoQueue.AddFrame(new FrameData()
                    {
                        FrameNumber = newFrameIndex,
                        Id = id
                    });
                    //wpisanie do pamieci nowej strony
                    _physicalMemory.SetFrame(newFrameIndex, _exchangeFile.ReadFromExchangeFile(id, pages));
                    //wpisanie do tablicy stron,ze strona znajduje sie w pamieci
                    foreach (var processPage in ProcessPages)
                    {
                        if (processPage.Id == id)
                            processPage.AddFrame(pages, newFrameIndex);
                    }
                    //zwrocenie bajtu
                    return _physicalMemory.GetFrame(newFrameIndex).GetByte(offset);


                }
            }
        }

        public void AllocateMemory(PCB processData, char[] program)
        {
            //obliczenie ilosci stron 
            int pages = (int) Math.Ceiling((double) program.Length/FramesSize);

            //przypisnie Stron procesu i delegatow do Listy stron i do PCB
            ProcessPages temp = new ProcessPages(processData.PID, pages);
            temp.GetChar = GetByte;
            temp.ChangeByteDel = ChangeByte;
            ProcessPages.Add(temp);
            processData.MemoryBlocks = temp;

            //uzupelnienie stron
            var frames = new List<Frame>();
            for (int i = 0; i < pages; i++)
            {
                frames.Add(new Frame(FramesSize));
                frames[i].WriteFrame(program.Select(x => x)
                    .Skip(FramesSize*i)
                    .Take((program.Length - FramesSize*i < FramesSize) ? program.Length - FramesSize*i : FramesSize)
                    .ToArray());
            }

            //Dodanie danych do pliku wymiany
            ExchangeFileProcess newProcess = new ExchangeFileProcess()
            {
                TakenProcessPages = temp,
                TakenFrames = frames
            };
            _exchangeFile.PlaceIntoMemory(newProcess);

            //sprawdzenie czy mozna dodac pierwsza strone do pamieci
            if (_freeFramesList.FreeFramesCount >= 1)
            {
                int index = _freeFramesList.RemoveFromList();
                _fifoQueue.AddFrame(new FrameData()
                {
                    FrameNumber = index,
                    Id = processData.PID
                });
                _physicalMemory.SetFrame(index, frames[0].ReadFrame());
                foreach (ProcessPages process in ProcessPages)
                {
                    if (process.Id == processData.PID)
                        process.AddFrame(0, index);
                }
            }
            else
            {
                var indexprocessdata = _fifoQueue.RemoveFrame();
                foreach (ProcessPages processPages in ProcessPages)
                {
                    if (processPages.Id == indexprocessdata.Id)
                    {
                        processPages.RemoveFrame(indexprocessdata.FrameNumber);
                    }
                }
                _freeFramesList.AddToList(indexprocessdata.FrameNumber);
                int index = _freeFramesList.RemoveFromList();
                _fifoQueue.AddFrame(new FrameData()
                {
                    FrameNumber = index,
                    Id = processData.PID
                });
                _physicalMemory.SetFrame(indexprocessdata.FrameNumber, frames[0].ReadFrame());
                foreach (var processPage in ProcessPages)
                {
                    if (processPage.Id == processData.PID)
                        processPage.AddFrame(0, indexprocessdata.FrameNumber);
                }
            }
        }

        public void RemoveFromMemory(PCB processData)
        {
            int[] frames = null;
            for (int i = 0; i < ProcessPages.Count; i++)
            {
                if (ProcessPages[i].Id == processData.PID)
                {
                    frames = ProcessPages[i].ReadFrameNumbers();
                    ProcessPages.RemoveAt(i);
                    break;
                }
            }
            if (frames != null)
            {
                foreach (var frame in frames)
                {
                    _freeFramesList.AddToList(frame);
                    _physicalMemory.GetFrame(frame).ClearFrame();
                }
            }
            _fifoQueue.RemoveChoosenProcess(processData.PID);
            _exchangeFile.RemoveFromMemory(processData.PID);
        }

        public void ChangeByte(int id, int index, char data)
        {
            //Ustalenie numeru strony i offsetu
            int pages = index/FramesSize;
            int offset = index%FramesSize;
            //Poszukiwanie tablicy stron do danego procesu
            var process = ProcessPages.Select(x => x).SingleOrDefault(x => x.Id == id);
            //sprawdzenie czy dana strona znajduje sie w pamieci
            if (process.IsPageInMemory(pages))
            {
                //Zwrocenie danego bajtu
                _physicalMemory.GetFrame(process.ReadFrameNumber(pages)).ChangeByte(offset, data);
            }
            else
            {
                if (_freeFramesList.FreeFramesCount == 0)
                {
                    //pobranie najstarszego numeru ramki
                    var indexprocessdata = _fifoQueue.RemoveFrame();
                    //przypisanie do tablicy stron z ktorego pobrano ramke ze juz nie ma jej w pamieci
                    foreach (ProcessPages processPages in ProcessPages)
                    {
                        if (processPages.Id == indexprocessdata.Id)
                        {
                            processPages.RemoveFrame(indexprocessdata.FrameNumber);
                        }
                    }
                    //dodanie ramki do list
                    _freeFramesList.AddToList(indexprocessdata.FrameNumber);
                    //pobranie z listy wolnych ramek najstarszej ramki
                    int newFrameIndex = _freeFramesList.RemoveFromList();
                    //dodanie do kolejki FIFO nowego numeru ramki
                    _fifoQueue.AddFrame(new FrameData()
                    {
                        FrameNumber = indexprocessdata.FrameNumber,
                        Id = id
                    });
                    //wpisanie do pamieci nowej strony
                    _physicalMemory.SetFrame(newFrameIndex, _exchangeFile.ReadFromExchangeFile(id, pages));
                    //wpisanie do tablicy stron,ze strona znajduje sie w pamieci
                    foreach (var processPage in ProcessPages)
                    {
                        if (processPage.Id == id)
                            processPage.AddFrame(pages, newFrameIndex);
                    }
                    //zwrocenie bajtu
                    _physicalMemory.GetFrame(newFrameIndex).ChangeByte(offset, data);
                }
                else
                {
                    //pobranie nowej strony
                    var newFrameIndex = _freeFramesList.RemoveFromList();
                    //dodanie do kolejki FIFO nowego numeru ramki
                    _fifoQueue.AddFrame(new FrameData()
                    {
                        FrameNumber = newFrameIndex,
                        Id = id
                    });
                    //wpisanie do pamieci nowej strony
                    _physicalMemory.SetFrame(newFrameIndex, _exchangeFile.ReadFromExchangeFile(id, pages));
                    //wpisanie do tablicy stron,ze strona znajduje sie w pamieci
                    foreach (var processPage in ProcessPages)
                    {
                        if (processPage.Id == id)
                            processPage.AddFrame(pages, newFrameIndex);
                    }
                    //zwrocenie bajtu
                    _physicalMemory.GetFrame(newFrameIndex).ChangeByte(offset, data);


                }
            }
        }
    }
}
