using System;
using System.Collections.Generic;

namespace Vow_win_ski.Processes {

    public partial class PCB {

        /// <summary>
        /// Zamyka aktualnie wykonywany lub wskazany przez identyfikator proces
        /// (przejście na Terminated, przygotowanie do całkowitego usunięcia procesu)
        /// i usuwa go z listy planisty
        /// Jeśli proces zatrzymywany nie jest w stanie Running, procez zatrzymujący blokuje się pod semaforem StopperSemaphore
        /// </summary>
        /// <param name="Reason">Przyczyna zamknięcia procesu</param>
        /// <param name="ExitCode">Opcjonalny; kod wyjścia zwracany przez proces</param>
        /// <param name="Process">Opcjonalny; jeśli podano, zamyka dany proces</param>
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - zamknięto proces
        /// </returns>
        /// <remarks>Zatrzymanie procesu i zawiadomienie nadzorcy - XH, nienormowalne zatrzymanie realizacji zlecenia - XQUE</remarks>
        public int TerminateProcess(ReasonOfProcessTerminating Reason, int ExitCode = 0) {
            throw new NotImplementedException();
            client.Disconnect();
        }

        /// <summary>
        /// Uruchamia proces po utworzeniu go metodą CreateProcess (przejście New -> Ready) i dodaje go do listy planisty
        /// </summary>
        /// <remarks>Uruchomienie procesu - XY</remarks>  
        /// <returns>
        /// Kod odpowiedzi:
        /// 0 - uruchomiono proces
        /// 1 - nie znaleziono procesu
        /// 2 - błąd: proces ma stan inny niż New
        /// </returns>
        public int RunNewProcess() {

            if (this.State != ProcessState.New) {
                Console.WriteLine("Blad uruchamiania procesu: Proces musi miec stan New. " + this.ToString());
                return 2;
            }

            this.State = ProcessState.Ready;
            Console.WriteLine("Uruchomiono proces " + this.ToString());

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Ready -> Running</remarks>
        /// <returns></returns>
        public int RunReadyProcess() {
            if(State == ProcessState.Ready) {
                State = ProcessState.Running;
                client.Connect();

                if (ReceiverMessageSemaphore == 1) {
                    Receive();
                }

                return 0;
            } else {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Running -> Waiting</remarks>
        /// <returns></returns>
        public int WaitForSomething() {
            throw new NotImplementedException();
            client.Disconnect();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Waiting -> Ready</remarks>
        /// <returns></returns>
        public int StopWaiting() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Running -> Ready</returns>
        public int WaitForScheduling() {
            if (State == ProcessState.Running) {
                State = ProcessState.Ready;
                client.Disconnect();
                return 0;
            } else {
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
        public int RemoveProcess() {
            throw new NotImplementedException();

            if (this.State == ProcessState.Terminated) {
                //Usuń dzieci
                int ret;
                LinkedList<PCB>.Enumerator en = this.Children.GetEnumerator();

                while (en.MoveNext()) {
                    ret = en.Current.RemoveProcess();
                    if (ret != 0) return ret;
                }

                //Zwolnij pamiec


                _CreatedPCBs.Remove(this);

                Console.WriteLine("Usunieto proces " + this.ToString());
                return 0;

            } else {
                Console.WriteLine("Blad usuwania procesu: Proces nie zostal zatrzymany przed usunieciem. " + this.ToString());
                return 2;
            }

        }

        public override string ToString() {
            return "[" + PID.ToString() + "] " + Name + ", stan=" + State.ToString() + ", priorytet=" + CurrentPriority.ToString();
        }

        /// <summary>
        /// Drukuje w mkonsoli zawartosc wszystkich pol PCB
        /// </summary>
        public void PrintAllFields() {
            throw new NotImplementedException();
        }

        public void Send(string receivername, string message) {

            //byte id = receiver.PID;
            //if(receiver.Lock == 0) {
            //        client._send(id, message);
            //} else {
            //    receiver.Lock = 0;
            //    cliend._send(id, message);
            //}
        }

        void Receive() {
            //if(client._receive() == false) {
            //    Lock == 1;
            //}
        }
    }

}