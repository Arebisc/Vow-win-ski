using System;
using System.Collections.Generic;

namespace Vow_win_ski.Processes
{

    public partial class PCB
    {

        /// <summary>
        /// Zamyka aktualnie wykonywany lub wskazany przez identyfikator proces
        /// (przejście na Terminated, przygotowanie do całkowitego usunięcia procesu)
        /// i usuwa go z listy planisty
        /// Jeśli proces zatrzymywany nie jest w stanie Running, procez zatrzymujący blokuje się pod semaforem StopperSemaphore
        /// </summary>
        /// <param name="Reason">Przyczyna zamknięcia procesu</param>
        /// <param name="ClosingProcess">Proces zamykający</param>
        /// <param name="ExitCode">Opcjonalny; kod wyjścia zwracany przez proces</param>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - zamknięto proces
        /// 1 - proces oczekuje na zamkniecie
        /// </returns>
        /// <remarks>Zatrzymanie procesu i zawiadomienie nadzorcy - XH, nienormowalne zatrzymanie realizacji zlecenia - XQUE</remarks>
        public int TerminateProcess(ReasonOfProcessTerminating Reason, PCB ClosingProcess, int ExitCode = 0)
        {
            //throw new NotImplementedException();

            string ReasonString = "(brak powodu)";

            switch (Reason)
            {
                case ReasonOfProcessTerminating.Ended:
                    ReasonString = "Proces sie zakonczyl.";
                    break;

                case ReasonOfProcessTerminating.ThrownError:
                    ReasonString = "Wystapil blad w procesie.";
                    break;

                case ReasonOfProcessTerminating.UserClosed:
                    ReasonString = "Proces zostal zakmniety przez uzytkownika.";
                    break;

                case ReasonOfProcessTerminating.CriticalError:
                    ReasonString = "Program spowodowal wystapienie bledu krytycznego i zostal zamkniety przez system.";
                    break;

                case ReasonOfProcessTerminating.KilledByOther:
                    ReasonString = "Proces zostal zamkniety przez inny proces.";
                    break;

                case ReasonOfProcessTerminating.ClosingSystem:
                    ReasonString = "Proces zostal zamkniety z powodu zamykania systemu.";
                    break;
            }


            if (State == ProcessState.Running)
            {
                State = ProcessState.Terminated;
                client.Disconnect();

                Console.WriteLine("Zakonczono proces " + this.ToString());
                Console.Write("Powod zamkniecia: " + ReasonString);
                return 0;

            }
            else
            {
                WaitingForStopping = true;
                Console.WriteLine("Oczekiwanie na zamkniecie procesu: " + this.ToString());
                Console.WriteLine("Proces zostanie zamkniety po przejsciu do stanu Running.");
                Console.Write("Powod zamkniecia: " + ReasonString);

                return 1;
            }

        }

        /// <summary>
        /// Uruchamia proces po utworzeniu go metodą CreateProcess (przejście New -> Ready)
        /// </summary>
        /// <remarks>Uruchomienie procesu - XY</remarks>  
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - uruchomiono proces
        /// 1 - nie znaleziono procesu
        /// 2 - błąd: proces ma stan inny niż New
        /// </returns>
        public int RunNewProcess()
        {

            if (State == ProcessState.New)
            {
                State = ProcessState.Ready;
                Console.WriteLine("Uruchomiono proces " + this.ToString());

                CPU.Scheduler.GetInstance.AddProcess(this);
                return 0;

            }
            else
            {
                Console.WriteLine("Blad uruchamiania procesu: Proces musi miec stan New. " + this.ToString());
                return 2;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Ready -> Running</remarks>
        /// <returns>
        /// 0 - uruchomiono     1 - stan inny niż Ready     2 - proces został zakończony
        /// </returns>
        public int RunReadyProcess()
        {
            if (State == ProcessState.Ready)
            {

                if (WaitingForStopping)
                {
                    //odblokuj proces zamykający
                    Console.WriteLine("Odblokowano proces [oczekujacy na zamkniecie innego procesu]: ");

                    Console.WriteLine("Zamknieto czekajacy na zamkniecie proces wchodzacy do stanu Running.");
                    State = ProcessState.Terminated;
                    return 2;

                }
                else
                {
                    State = ProcessState.Running;
                    Console.WriteLine("Uruchomiono proces czekajacy na procesor: " + this.ToString());

                    client.Connect();

                    if (ReceiverMessageLock == 1)
                    {
                        Receive();
                    }

                    return 0;
                }

            }
            else
            {
                Console.WriteLine("Blad uruchamiania czekajacego procesu: Proces ma stan inny niz Ready. " + this.ToString());
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Running -> Waiting</remarks>
        /// <returns>0 - proces przeszedł w stan oczekiwania, 1 - proces ma stan inny niż Running</returns>
        public int WaitForSomething()
        {

            if (State == ProcessState.Running)
            {
                State = ProcessState.Waiting;
                client.Disconnect();
                CPU.Scheduler.GetInstance.RemoveProcess(this);

                Console.WriteLine("Proces " + this.ToString() + " przeszedl w stan oczekiwania na odblokowanie.");
                return 0;

            }
            else
            {
                Console.WriteLine("Nie udalo sie wstrzymac procesu. Proces ma stan inny niz Running. [" + this.ToString() + "]");
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Waiting -> Ready</remarks>
        /// <returns>0 - proces przeszedł na Ready, 1 - stan inny niż Waiting</returns>
        public int StopWaiting()
        {

            if (State == ProcessState.Waiting)
            {
                State = ProcessState.Ready;
                CPU.Scheduler.GetInstance.AddProcess(this);

                Console.WriteLine("Proces " + this.ToString() + " przeszedl w stan oczekiwania na przydzial procesora.");
                return 0;
            }
            else
            {

                Console.WriteLine("Nie udalo sie odblokowac procesu. Proces ma stan inny niz Waiting. [" + this.ToString() + "]");
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Running -> Ready</remarks>
        /// <returns>0 - proces przeszedł na Ready, 1 - proces ma stan inny niż Running</returns>
        public int WaitForScheduling()
        {
            if (State == ProcessState.Running)
            {
                State = ProcessState.Ready;
                client.Disconnect();

                Console.WriteLine("Przerwano realizacje przez procesor procesu: " + this.ToString());
                return 0;

            }
            else
            {

                Console.WriteLine("Blad przerywania procesu: Proces ma stan inny niz Running. [" + this.ToString() + "]");
                return 1;
            }
        }

        /// <summary>
        /// Usuwa proces o podanym identyfikatorze oraz jego wszystkie dzieci. Proces musi najpierw zostać zatrzymany przez TerminateProcess
        /// (zwalnia pamięć, coś jak destruktor)
        /// </summary>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - usunięto proces
        /// 1 - nie znaleziono procesu
        /// 2 - błąd: przed usunięciem procesu należy go zatrzymać
        /// </returns>
        /// <remarks>Usunięcie procesu - XD</remarks>
        public int RemoveProcess()
        {
            throw new NotImplementedException();

            if (this.State == ProcessState.Terminated)
            {
                //Usuń dzieci
                //int ret;
                //LinkedList<PCB>.Enumerator en = this.Children.GetEnumerator();

                //while (en.MoveNext()) {
                //    ret = en.Current.RemoveProcess();
                //    if (ret != 0) return ret;
                //}



                //Zwolnij pamiec


                _CreatedPCBs.Remove(this);

                Console.WriteLine("Usunieto proces " + this.ToString());
                return 0;

            }
            else
            {
                Console.WriteLine("Blad usuwania procesu: Proces nie zostal zatrzymany przed usunieciem. " + this.ToString());
                return 2;
            }

        }

        public override string ToString()
        {
            return "[" + PID.ToString() + "] " + Name + ", stan=" + State.ToString() + ", priorytet=" + CurrentPriority.ToString();
        }

        /// <summary>
        /// Drukuje w mkonsoli zawartosc wszystkich pol PCB
        /// </summary>
        public void PrintAllFields()
        {
            //throw new NotImplementedException();
            Console.WriteLine("PID: " + PID.ToString());
            Console.WriteLine("Nazwa: " + Name);
            Console.WriteLine("Priorytet: " + CurrentPriority.ToString());
            Console.WriteLine("Poczatkowy priorytet: " + StartPriority);
            Console.WriteLine("Czas posiadania obecnego priorytetu: " + PriorityTime);
            Console.WriteLine("Rejestry: " + Registers.ToString());
            Console.WriteLine("Stan: " + State.ToString());
            Console.WriteLine("Licznik instrukcji: " + InstructionCounter);
            //Console.WriteLine("Rodzic: " + ((Parent == null) ? "[brak]" : Parent.ToString()));
            //Console.WriteLine("Ilosc dzieci: " + Children.Count.ToString());
            Console.WriteLine("Strony pamieci: " + MemoryBlocks.ToString());
            Console.WriteLine("Zamek odbioru wiadomsci: " + ReceiverMessageLock);
        }

        public void Send(string receivername, string message)
        {

            //byte id = receiver.PID;
            //if(receiver.Lock == 0) {
            //        client._send(id, message);
            //} else {
            //    Unlock(receiver);
            //    cliend._send(id, message);
            //}
        }

        void Receive()
        {
            if (client._receive() == false)
            {
                //Lock(this);
            }
        }
    }

}