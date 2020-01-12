using System;
using System.Threading.Tasks;
using BitLab.Message;

namespace BitLab.Node
{
    public class Node : IDisposable
    {
        private readonly MessageCreator<IncomingMessage> _messageCreator = new MessageCreator<IncomingMessage>();
        private NodeStatus _status = NodeStatus.Offline;
        
        
        public NodeStatus Status
        {
            get
            {
                return this._status;
            }
            private set
            {
                Console.WriteLine($"Status się zmienił z {_status} na {value}");

                NodeStatus status = this._status;
                this._status = value;
                
                if (status == this._status)
                {
                    return;
                }
                
                if (value != NodeStatus.Failed && value != NodeStatus.Offline)
                {
                    return;
                }

                Console.WriteLine("POLACZENIE ZERWANE !!!!");
            }
        }

        /// <summary>
        /// Metoda która wyświetla komunikat podczas usunięcia node'a
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("Node został zniszczony");
        }
        
        
        /// <summary>Wyślij wiadomość do peera asynchronicznie</summary>
        /// <param name="payload">Payload do wysłánia</param>
        /// <param name="System.OperationCanceledException.OperationCanceledException">Połączenie z nodem zostało zerwane</param>
        public Task SendMessage(Payload payload)
        {
            if (payload == null)
            {
                throw new Exception(nameof (payload));
            }
            
            TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            if (!this.IsConnected)
            {
                completion.SetException((Exception) new OperationCanceledException("Połączenie z peerem zostało zerwane"));
                return (Task) completion.Task;
            }
            
            return (Task) completion.Task;
        }
        
        /// <summary>
        /// Tworzymy obiekt który będzie kolejkował wiadomości póki nie zniknie
        /// </summary>
        /// <returns>Zwraca obiekt który nasłuchuje danego node'a</returns>
        public NodeListener CreateListener()
        {
            return new NodeListener(this);
        }
        
        /// <summary>
        /// Property o statusie połączenia z nodem
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (this.Status != NodeStatus.Connected)
                {
                    Console.WriteLine("Udało się połączyć z nodem");
                    return this.Status == NodeStatus.HandShaked;
                }
                
                return true;
            }
        }
        
        public MessageCreator<IncomingMessage> MessageCreator
        {
            get
            {
                return this._messageCreator;
            }
        }
    }
}