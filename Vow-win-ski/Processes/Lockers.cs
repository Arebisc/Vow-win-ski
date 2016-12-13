using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vow_win_ski.Processes
{
    public class Lockers
    {
        private byte open = 0;
        List<Vow_win_ski.Processes.PCB> waiting;
        private string Name;
        PCB proces;

        public Lockers()
        {
            waiting = new List<PCB>();
        }

        public void Lock(PCB Proces)
        {
            if (Check())
            {
                proces = Proces;
                this.Name = proces.Name;
                open = 1;
                proces.WaitForScheduling();
            }
            else
            {
                waiting.Add(proces);
                proces.WaitForScheduling();
            }
        }

        public void Unlock(string name)
        {
            if (!Check())
            {
                if (waiting.Count() > 1)
                {
                    if (Check(name))
                    {
                        foreach(var i in waiting)
                        {
                            if (name == i.Name)
                            {
                                proces = i;
                                break;
                            }
                        }
                        proces.StopWaiting();
                        this.Name = proces.Name;
                    }
                }
                else if (waiting.Count() == 1)
                {
                    if (Check(name))
                    {
                        foreach (var i in waiting)
                        {
                            if (name == i.Name)
                            {
                                proces = i;
                                break;
                            }
                        }
                        proces.StopWaiting();
                        open = 0;
                    }
                }
            }
        }

        public void Show()
        {
            foreach (var i in waiting)
            {
                Console.WriteLine(i.PID + "\t" + i.Name);
            }
        }

        public bool Check()
        {
            if (open == 0)
                return true;
            else
                return false;
        }

        public bool Check(string name)
        {
            if (this.Name == name)
                return true;
            else
                return false;
        }
    }
}
